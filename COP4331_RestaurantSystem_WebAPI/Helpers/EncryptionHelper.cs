using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace COP4331_RestaurantSystem_WebAPI.Helpers
{
    public static class EncryptionHelper
    {

        // Private Key used for token ciphering
        private static string privateKey = "hashslingingslasher";

        // Encrypts input string using sha256 to check against similarly encrypted strings in the database
        public static string sha256(string inputString)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] toHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString));
                StringBuilder buildHash = new StringBuilder();
                foreach(byte b in toHash)
                {
                    buildHash.Append(b.ToString("X2"));
                }

                return buildHash.ToString();
            }
        }

        // AES encryption code sourced from Microsoft System.Security.Cryptography documentation
        public static string aesCipher(string value, string key)
        {
            using(Aes aes = Aes.Create())
            {
                // Combine the private key with the key passed in
                aes.Key = Encoding.UTF8.GetBytes((privateKey + key).Length > 32 ? (privateKey + key).Substring(0, 32) : (privateKey + key));
                aes.IV = new byte[16];
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using(var msEncrypt = new MemoryStream())
                {
                    using(var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using(StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(value);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }

            }
        }

        // AES decryption code sourced from Microsoft System.Security.Cryptography documentation
        public static string aesDecipher(string value, string key)
        {
            using (Aes aes = Aes.Create())
            {
                // Combine the private key with the key passed in
                aes.Key = Encoding.UTF8.GetBytes((privateKey + key).Length > 32 ? (privateKey + key).Substring(0, 32) : (privateKey + key));
                aes.IV = new byte[16];
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(value)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader swDecrypt = new StreamReader(csDecrypt))
                        {
                            return swDecrypt.ReadToEnd();
                        }
                    }
                }

            }
        }
    }
}