using System;
using System.Text;
using RestEase;
using SecureRemotePassword;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Instead
{
    public class Client
    {

        public async static void Login(string username, string password, string secretKey)
        {
            Console.WriteLine("Starting login");
            //var auth = RestClient.For<AuthService>("https://instead-dev.herokuapp.com");
            var auth = RestClient.For<AuthService>("http://10.0.2.2:3001");

            var client = new SrpClient(new SrpParameters { PaddedLength = 0 });
            var clientEphemeral = client.GenerateEphemeral();

            var startResponse = await auth.startLogin(
				new StartLoginInput { username = username, clientEphemeralPublic = clientEphemeral.Public }
				);

            var srpKey = Crypto.DerivePrivateKey(startResponse.srpSalt, password, secretKey, username);

			var clientSession = client.DeriveSession(
				clientEphemeral.Secret,
				startResponse.serverEphemeralPublic,
				startResponse.srpSalt,
				username,
				srpKey
				);
			Console.WriteLine(clientSession.Proof);
			var finishResponse = await auth.finishLogin(
				new FinishLoginInput { clientSessionProof = clientSession.Proof }
				);

            client.VerifySession(clientEphemeral.Public, clientSession, finishResponse.serverSessionProof);

            var mukHex = Crypto.DerivePrivateKey(finishResponse.mukSalt, password, secretKey, username);

            //var muk = new SymmetricSecurityKey(mukHex.HexToBytes());

            //var keyWrapProvider = new SymmetricKeyWrapProvider(muk, SecurityAlgorithms.Aes256Gcm);

            //var privateKey = keyWrapProvider.UnwrapKey(Convert.FromBase64String(finishResponse.privateKey));

            var muk = new AesGcm(mukHex.HexToBytes());

            var tag = new byte[16];
            var privateKeyEnc = Convert.FromBase64String(finishResponse.privateKey);
            var privateKeyJwk = new byte[privateKeyEnc.Length];

            muk.Decrypt(
                Convert.FromBase64String(finishResponse.privateKeyIv),
                privateKeyEnc,
                tag,
                privateKeyJwk);

            Console.WriteLine(Encoding.UTF8.GetString(privateKeyJwk));
        }
    }
}
