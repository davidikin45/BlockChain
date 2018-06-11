using BlockChainCourse.Cryptography.Hashing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace BlockChainCourse.Cryptography
{
    public static class BlockChainAddress
    {
        public static string CreateAddress(string publicKey)
        {
            byte[] PreHashQ = AppendBitcoinNetwork(RipeMD160(Sha256(HexToByte(publicKey))), 0);
            return Base58Encode(ConcatAddress(PreHashQ, Sha256(Sha256(PreHashQ))));
        }

        public static string CreateTestAddress(string publicKey)
        {
            byte[] PreHashQ = AppendBitcoinNetwork(RipeMD160(Sha256(HexToByte(publicKey))), 111);
            return Base58Encode(ConcatAddress(PreHashQ, Sha256(Sha256(PreHashQ))));
        }

        public static byte[] HexToByte(string HexString)
        {
            if (HexString.Length % 2 != 0)
                throw new Exception("Invalid HEX");
            byte[] retArray = new byte[HexString.Length / 2];
            for (int i = 0; i < retArray.Length; ++i)
            {
                retArray[i] = byte.Parse(HexString.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return retArray;
        }

        public static byte[] Sha256(byte[] array)
        {
            SHA256Managed hashstring = new SHA256Managed();
            return hashstring.ComputeHash(array);
        }

        public static byte[] RipeMD160(byte[] array)
        {
            RIPEMD160Managed hashstring = new RIPEMD160Managed();
            return hashstring.ComputeHash(array);
        }

        public static byte[] AppendBitcoinNetwork(byte[] RipeHash, byte Network)
        {
            byte[] extended = new byte[RipeHash.Length + 1];
            extended[0] = (byte)Network;
            Array.Copy(RipeHash, 0, extended, 1, RipeHash.Length);
            return extended;
        }

        public static byte[] ConcatAddress(byte[] RipeHash, byte[] Checksum)
        {
            byte[] ret = new byte[RipeHash.Length + 4];
            Array.Copy(RipeHash, ret, RipeHash.Length);
            Array.Copy(Checksum, 0, ret, RipeHash.Length, 4);
            return ret;
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
    }
}
