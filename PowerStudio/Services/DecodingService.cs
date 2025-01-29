using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PowerStudio.Services
{
    public static class DecodingService
    {
        public static byte[] DecryptData(byte[] encryptedData)
        {
            string key = "your_encryption_key"; // maybe to save in register
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            return PerformCryptography(encryptedData, decryptor);
        }
        private static byte[] GenerateRandomKey(int keySize = 256)
        {
            using var aes = Aes.Create();
            aes.KeySize = keySize;
            aes.GenerateKey();
            return aes.Key;
        }

        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
            }
            return memoryStream.ToArray();
        }
    }
}
