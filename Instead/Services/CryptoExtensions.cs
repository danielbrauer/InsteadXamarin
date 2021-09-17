using System;
using System.Text;

namespace Instead.Services
{
    public static class CryptoExtensions
    {
        public static string ToHex(this byte[] b)
        {
            return BitConverter.ToString(b).Replace("-", "").ToLower();
        }

        public static byte[] UTF8ToBytes(this string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        // Adapted from https://stackoverflow.com/a/9995303
        public static byte[] HexToBytes(this string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        static int GetHexVal(char hex)
        {
            int val = hex;
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static byte[] Xor(this byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Argument lengths must match");
            }
            var result = new byte[a.Length];
            for (var i = 0; i < result.Length; ++i)
            {
                result[i] = (byte)(a[i] ^ b[i]);
            }
            return result;
        }
    }
}
