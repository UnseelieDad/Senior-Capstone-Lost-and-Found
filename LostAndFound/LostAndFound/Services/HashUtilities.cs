using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LostAndFound.Services
{
    public class HashUtilities
    {
        public static string GetHashFromString(string input)
        {
            SHA512 SHAmHash = new SHA512Managed();

            byte[] data = SHAmHash.ComputeHash(Encoding.UTF8.GetBytes(input));


            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static bool VerifyHash(string input, string hash)
        {
            string hashOfInput = GetHashFromString(input);
            return StringComparer.OrdinalIgnoreCase.Equals(hashOfInput, hash);
        }
    }
}
