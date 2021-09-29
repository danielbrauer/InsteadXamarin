using System.Text;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using System;
using Instead.Models;

namespace Instead.Services
{
    public class Crypto
    {

        private static byte[] HkdfSha256(string ikm, int length, string salt, string info)
        {
            var hkdf = new HkdfBytesGenerator(new Sha256Digest());
            hkdf.Init(new HkdfParameters(ikm.UTF8ToBytes(), salt.UTF8ToBytes(), info.UTF8ToBytes()));
            var result = new byte[length];
            hkdf.GenerateBytes(result, 0, length);
            return result;
        }

        public static string DeriveUserKey(string salt, Credentials creds)
        {
            var keyParts = creds.SecretKey.Split('-');
            var version = keyParts[0];
            var key = keyParts[1];

            var passwordBin = creds.Password.Trim().Normalize(NormalizationForm.FormKC).UTF8ToBytes();

            var saltedSalt = HkdfSha256(salt, 32, creds.Username, version);
            var hashedPassword = SCrypt.Generate(passwordBin, saltedSalt, 16384, 8, 1, 32);
            var saltedKey = HkdfSha256(key, 32, creds.Username, version);
            return hashedPassword.Xor(saltedKey).ToHex();
        }

        public static byte[] DecryptAesGcm(byte[] cipherText, byte[] key, byte[] iv)
        {
            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), 128, iv);
            cipher.Init(false, parameters);

            var plainText = new byte[cipherText.Length];

            var offset = cipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);

            cipher.DoFinal(plainText, offset);

            return plainText;
        }
    }
}
