using System.Text;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;

namespace Instead
{
    public class Crypto
    {

        private static byte[] hkdfSha256(string ikm, int length, string salt, string info)
        {
            var hkdf = new HkdfBytesGenerator(new Sha256Digest());
            hkdf.Init(new HkdfParameters(ikm.UTF8ToBytes(), salt.UTF8ToBytes(), info.UTF8ToBytes()));
            var result = new byte[length];
            hkdf.GenerateBytes(result, 0, length);
            return result;
        }

        public static string DerivePrivateKey(string salt, string password, string secretKey, string username)
        {
            var keyParts = secretKey.Split('-');
            var version = keyParts[0];
            var key = keyParts[1];

            var passwordBin = password.Trim().Normalize(System.Text.NormalizationForm.FormKC).UTF8ToBytes();

            var generator = new HkdfBytesGenerator(new Sha256Digest());
            var saltedSalt = hkdfSha256(salt, 32, username, version);
            var hashedPassword = SCrypt.Generate(passwordBin, saltedSalt, 16384, 8, 1, 32);
            var saltedKey = hkdfSha256(key, 32, username, version);
            return hashedPassword.Xor(saltedKey).ToHex();
        }
    }
}
