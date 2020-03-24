using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LostAndFound.Services
{
    class EncryptionManager
    {
        public static byte[] Encrypt(byte[] information, byte[] key)
        {
            // Make sure parameters are valid
            CheckParams(information, key);
            byte[] encrypted;
            string infoString = Convert.ToBase64String(information);

            // Create AES object using key
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;

                // Create encryptor
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Create streams
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(infoString);
                    }

                    encrypted = memoryStream.ToArray();
                }
            }

            // Return encrypted bytes
            return encrypted;
        }

        public static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            // Make sure parameters are valid
            CheckParams(encrypted, key);

            string decrypted;

            // Create AES object using key
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;

                // Create streams
                using (MemoryStream memoryStream = new MemoryStream(encrypted))
                {
                    byte[] iv = new byte[16];
                    memoryStream.Read(iv, 0, iv.Length);

                    // Create decryptor
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, iv);

                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        decrypted = streamReader.ReadToEnd();
                    }
                }
            }

            var decryptedBytes = Convert.FromBase64String(decrypted);
            // Return decrypted bytes
            return decryptedBytes;
        }

        private static void CheckParams(byte[] information, byte[] key)
        {
            // Null check
            if (information == null | key == null)
            {
                throw new ArgumentNullException("Parameter cannot be null");
            }

            // AES 256 requires a 32 byte key
            if (key.Length != 32)
            {
                throw new ArgumentException("Key must be exactly 32 bytes");
            }
        }
    }
}
