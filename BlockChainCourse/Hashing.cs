using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace BlockChainCourse.Cryptography
{
	public class HashData
	{
        public static string ComputeHashSha256Base58String(string toBeHashed)
        {
            var message = Encoding.UTF8.GetBytes(toBeHashed);
            return Base58Encode(ComputeHashSha256(message));
        }

        public static string Base58Encode(byte[] array)
        {
            const string ALPHABET = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            string retString = string.Empty;
            BigInteger encodeSize = ALPHABET.Length;
            BigInteger arrayToInt = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                arrayToInt = arrayToInt * 256 + (long)array[i];
            }
            while (arrayToInt > 0)
            {
                int rem = (int)(arrayToInt % encodeSize).IntValue();
                arrayToInt /= encodeSize;
                retString = ALPHABET[rem] + retString;
            }
            for (int i = 0; i < array.Length && array[i] == 0; ++i)
                retString = ALPHABET[0] + retString;

            return retString;
        }

        public static string ComputeHashSha256Base64StringWithHMACKey(string toBeHashed)
        {
            var message = Encoding.UTF8.GetBytes(toBeHashed);

            return Convert.ToBase64String(ComputeHashSha256(message));
        }

        public static byte[] ComputeHashSha256(byte[] toBeHashed)
		{
			using (var sha256 = SHA256.Create())
			{
				return sha256.ComputeHash(toBeHashed);
			}
		}
	}
}
