using System.Net;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;
using WebApi_app.Models;

namespace WebApi_app.Services
{
    public class EncryptionService
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("YourStaticKey1234567890");
        public string HashPassword(string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha512.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }
        public static string EncryptPersonalData(string data, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GetKeyFromPassword(key);
                aesAlg.GenerateIV();

                byte[] encryptedData;

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    encryptedData = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                }

                byte[] combinedData = new byte[aesAlg.IV.Length + encryptedData.Length];
                Array.Copy(aesAlg.IV, combinedData, aesAlg.IV.Length);
                Array.Copy(encryptedData, 0, combinedData, aesAlg.IV.Length, encryptedData.Length);

                return Convert.ToBase64String(combinedData);
            }
        }
        public static string DecryptPersonalData(string encryptedData, string key)
        {
            byte[] combinedData = Convert.FromBase64String(encryptedData);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GetKeyFromPassword(key);

                byte[] iv = new byte[aesAlg.BlockSize / 8];
                byte[] encryptedBytes = new byte[combinedData.Length - iv.Length];

                Array.Copy(combinedData, iv, iv.Length);
                Array.Copy(combinedData, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                aesAlg.IV = iv;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    string decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                    return decryptedData;
                }
            }
        }

        private static byte[] GetKeyFromPassword(string password)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] hash = sha256.ComputeHash(passwordBytes);

                    byte[] key = new byte[32];
                    Array.Copy(hash, key, key.Length);

                    return key;
                }
            }
        }
    }
