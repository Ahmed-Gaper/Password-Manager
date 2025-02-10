using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Encryption
{

    public class XorEncryptionService : IEncryptionService
    {
        private const int IvSize = 16; 
        private const int KeystreamBlockSize = 32;

        public string Encrypt(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            byte[] keyBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));

            byte[] iv = new byte[IvSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = new byte[IvSize + plainBytes.Length];

            Buffer.BlockCopy(iv, 0, cipherBytes, 0, IvSize);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                int numBlocks = (plainBytes.Length + KeystreamBlockSize - 1) / KeystreamBlockSize;

                for (int block = 0; block < numBlocks; block++)
                {
                    byte[] counterBytes = BitConverter.GetBytes(block);
                    byte[] hmacInput = new byte[IvSize + counterBytes.Length];
                    Buffer.BlockCopy(iv, 0, hmacInput, 0, IvSize);
                    Buffer.BlockCopy(counterBytes, 0, hmacInput, IvSize, counterBytes.Length);

                    byte[] keystreamBlock = hmac.ComputeHash(hmacInput);

                    int offset = block * KeystreamBlockSize;
                    int bytesToProcess = Math.Min(KeystreamBlockSize, plainBytes.Length - offset);

                    for (int i = 0; i < bytesToProcess; i++)
                    {
                        cipherBytes[IvSize + offset + i] = (byte)(plainBytes[offset + i] ^ keystreamBlock[i]);
                    }
                }
            }

            return Convert.ToBase64String(cipherBytes);
        }


        public string Decrypt(string cipherText, string key)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            if (cipherBytes.Length < IvSize)
                throw new ArgumentException("Invalid cipher text.", nameof(cipherText));

            byte[] keyBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));

            byte[] iv = new byte[IvSize];
            Buffer.BlockCopy(cipherBytes, 0, iv, 0, IvSize);

            int cipherDataLength = cipherBytes.Length - IvSize;
            byte[] plainBytes = new byte[cipherDataLength];

            using (var hmac = new HMACSHA256(keyBytes))
            {
                int numBlocks = (cipherDataLength + KeystreamBlockSize - 1) / KeystreamBlockSize;
                for (int block = 0; block < numBlocks; block++)
                {
                    byte[] counterBytes = BitConverter.GetBytes(block);
                    byte[] hmacInput = new byte[IvSize + counterBytes.Length];
                    Buffer.BlockCopy(iv, 0, hmacInput, 0, IvSize);
                    Buffer.BlockCopy(counterBytes, 0, hmacInput, IvSize, counterBytes.Length);

                    byte[] keystreamBlock = hmac.ComputeHash(hmacInput);

                    int offset = block * KeystreamBlockSize;
                    int bytesToProcess = Math.Min(KeystreamBlockSize, cipherDataLength - offset);

                    for (int i = 0; i < bytesToProcess; i++)
                    {
                        plainBytes[offset + i] = (byte)(cipherBytes[IvSize + offset + i] ^ keystreamBlock[i]);
                    }
                }
            }

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
