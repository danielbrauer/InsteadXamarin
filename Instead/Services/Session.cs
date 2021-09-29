using System;
using System.Text;
using System.Threading.Tasks;
using Instead.Models;
using Microsoft.IdentityModel.Tokens;
using RestEase;
using SecureRemotePassword;

namespace Instead.Services
{

    public class Session
    {
        readonly IInsteadApi api;

        public Session(string url)
        {
            api = RestClient.For<IInsteadApi>(url);
        }

        public class KeyPair
        {
            public JsonWebKey Private;
            public JsonWebKey Public;
        }

        public async Task<KeyPair> Login(Credentials creds)
        {
            Console.WriteLine("Starting login");

            var srpClient = new SrpClient();

            var clientEphemeral = srpClient.GenerateEphemeral();

            var startResponse = await api.StartLogin(
                new StartLoginInput
                {
                    username = creds.Username,
                    clientEphemeralPublic = clientEphemeral.Public
                }
                );

            var srpKey = Crypto.DeriveUserKey(startResponse.srpSalt, creds);

            var clientSession = srpClient.DeriveSession(
                clientEphemeral.Secret,
                startResponse.serverEphemeralPublic,
                startResponse.srpSalt,
                creds.Username,
                srpKey
                );
                
            var finishResponse = await api.FinishLogin(
                new FinishLoginInput { clientSessionProof = clientSession.Proof }
                );

            srpClient.VerifySession(
                clientEphemeral.Public,
                clientSession,
                finishResponse.serverSessionProof
                );

            var mukHex = Crypto.DeriveUserKey(finishResponse.mukSalt, creds);

            var privateKeyJson = Crypto.DecryptAesGcm(
                Convert.FromBase64String(finishResponse.privateKey),
                mukHex.HexToBytes(),
                Convert.FromBase64String(finishResponse.privateKeyIv)
                );

            return new KeyPair
            {
                Private = new JsonWebKey(Encoding.UTF8.GetString(privateKeyJson)),
                Public = finishResponse.publicKey
            };
        }

        public async Task Logout()
        {
            await api.Logout();
        }
    }
}
