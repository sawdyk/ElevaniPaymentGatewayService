using System.Security.Cryptography;
using System.Text;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.PaymentGateway.PayAgency
{
    public static class PayAgencyEncryptionService
    {
        // Convert byte array to hex string (matches Node.js output)
        //private static string BytesToHex(byte[] bytes)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (byte b in bytes)
        //    {
        //        sb.Append(string.Format("%02x", b));
        //    }
        //    return sb.ToString();
        //}

        private static string BytesToHex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        private static byte[] HexToBytes(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Hex string must have an even number of characters.");
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        public static string EncryptData(string data, string key)
        {
            // Generate random 16-byte IV (same as Node.js randomBytes(16))
            byte[] iv = new byte[16];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(iv);
            }

            // Use key as UTF-8 bytes (NOW MATCHES Node.js Buffer.from(key, "utf-8"))
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length != 32)
            {
                throw new ArgumentException("Key must be 32 bytes (32 UTF-8 characters).");
            }

            byte[] encryptedBytes;
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7; // PKCS7 is equivalent to PKCS5Padding in Java

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    encryptedBytes = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                }
            }

            // Return hex:hex format (NOW MATCHES Node.js .toString("hex"))
            return BytesToHex(iv) + ":" + BytesToHex(encryptedBytes);
        }

        public static string DecryptData(string encryptedData, string key)
        {
            // Split the encrypted data into IV and ciphertext
            string[] parts = encryptedData.Split(':');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid encrypted data format. Expected 'iv:ciphertext' in hex format.");
            }

            // Convert hex strings back to bytes
            byte[] iv = HexToBytes(parts[0]);
            byte[] encryptedBytes = HexToBytes(parts[1]);

            // Use key as UTF-8 bytes
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length != 32)
            {
                throw new ArgumentException("Key must be 32 bytes (32 UTF-8 characters).");
            }

            byte[] decryptedBytes;
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                }
            }

            // Convert decrypted bytes back to string
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
