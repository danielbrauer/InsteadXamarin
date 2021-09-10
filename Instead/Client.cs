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
            //var clientEphemeral = client.GenerateEphemeral();
            var clientEphemeral = new SrpEphemeral()
            {
                Public = "9d7b2dfa81c6bd65dba363af2dcaeb2410a8a3877963c40c3222b1989ccfd505d7f376a031bcb73b92d9fac59b287906e30da147fac55d361477f42fd38431e6886f796a357369b96c757297efb9b6164fe3e28501392f1603c8bade2855749b9422e217e9216e30571e0d926ea687e35124039cb211f90cfa0fe531070ed343e7fa30a9d8067cd02279e6639af9cae1cd8234c35cde566789e5ccd06059c20052d3e39a9c9a214bc712bed0d9f8c3bc537f699404116317a04a42cfcf364fac9b8627ff6718a0284c8d078bad946239c0f7fa5e71ad468cd47fb0c0f151ab015881eba894e304329ed895b927f263025cca5592b09a0b26d695d71d571b3a18",
                Secret = "601d3889afb3646d5a5ca45085ee6910f8d1c8b164be6d898d95f961bf0bf017"
            };

            var startResponse = await auth.startLogin(new StartLoginInput { username = username, clientEphemeralPublic = clientEphemeral.Public });

            var srpKey = Crypto.DerivePrivateKey(startResponse.srpSalt, password, secretKey, username);

            var clientSession = client.DeriveSession(clientEphemeral.Secret, startResponse.serverEphemeralPublic, startResponse.srpSalt, username, srpKey);

            var finishResponse = await auth.finishLogin(new FinishLoginInput { clientSessionProof = clientSession.Proof });

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
