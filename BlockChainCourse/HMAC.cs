﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace BlockChainCourse.Cryptography
{
	public class Hmac
	{
		private const int KeySize = 32;

		public static byte[] GenerateKey()
		{
			using (var randomNumberGenerator = new RNGCryptoServiceProvider())
			{
				var randomNumber = new byte[KeySize];
				randomNumberGenerator.GetBytes(randomNumber);

				return randomNumber;
			}
		}

        public static string ComputeHashSha256Base58StringWithHMACKey(string toBeHashed, string key)
        {
            var keyToUse = Encoding.UTF8.GetBytes(key);
            var message = Encoding.UTF8.GetBytes(toBeHashed);
            return Base58Encode(ComputeHmacSha256(message, keyToUse));
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

        public static string ComputeHashSha256Base64StringWithHMACKey(string toBeHashed, string key)
        {
            var keyToUse = Encoding.UTF8.GetBytes(key);
            var message = Encoding.UTF8.GetBytes(toBeHashed);

            return Convert.ToBase64String(ComputeHmacSha256(message, keyToUse));
        }

        public static byte[] ComputeHmacSha256(byte[] toBeHashed, byte[] key)
		{
			using (var hmac = new HMACSHA256(key))
			{
				return hmac.ComputeHash(toBeHashed);
			}
		}
	}
}
