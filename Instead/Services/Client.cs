using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using RestEase;
using SecureRemotePassword;

namespace Instead.Services
{

    public class Client
    {

        JsonWebKey privateKey;
        JsonWebKey publicKey;

        Client() { }

        public async static Task<Client> Login(string username, string password, string secretKey)
        {
            Console.WriteLine("Starting login");

            var srpClient = new SrpClient();
            var clientEphemeral = srpClient.GenerateEphemeral();
            var auth = RestClient.For<AuthService>("http://10.0.2.2:3001");

            var startResponse = await auth.StartLogin(
                new StartLoginInput
                {
                    username = username,
                    clientEphemeralPublic = clientEphemeral.Public
                }
                );

            var srpKey = Crypto.DeriveUserKey(startResponse.srpSalt, password, secretKey, username);

            var clientSession = srpClient.DeriveSession(
                clientEphemeral.Secret,
                startResponse.serverEphemeralPublic,
                startResponse.srpSalt,
                username,
                srpKey
                );
                
            var finishResponse = await auth.FinishLogin(
                new FinishLoginInput { clientSessionProof = clientSession.Proof }
                );

            srpClient.VerifySession(
                clientEphemeral.Public,
                clientSession,
                finishResponse.serverSessionProof
                );

            var mukHex = Crypto.DeriveUserKey(finishResponse.mukSalt, password, secretKey, username);

            var privateKeyJson = Crypto.DecryptAesGcm(
                Convert.FromBase64String(finishResponse.privateKey),
                mukHex.HexToBytes(),
                Convert.FromBase64String(finishResponse.privateKeyIv)
                );

            var client = new Client() {
                privateKey = new JsonWebKey(Encoding.UTF8.GetString(privateKeyJson)),
                publicKey = finishResponse.publicKey
            };

            return client;
        }
    }
}
