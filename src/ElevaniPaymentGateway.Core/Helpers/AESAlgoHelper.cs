using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace ElevaniPaymentGateway.Core.Helpers
{
    public class AESAlgoHelper
    {
        private Random random = new Random();
        private readonly ILogger<AESAlgoHelper> _logger;
        public AESAlgoHelper(ILogger<AESAlgoHelper> logger)
        {
            _logger = logger;
        }
        private string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private string DecryptStringFromBytes_Aes(string cipherText, byte[] Key, byte[] IV)
        {
            try
            {
                // Check arguments.
                if (cipherText == null || cipherText.Length <= 0)
                    throw new ArgumentNullException("cipherText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");
                // Declare the string used to hold
                // the decrypted text.
                string plaintext = null;
                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key,
                    aesAlg.IV);
                    byte[] cipherbytes = HexadecimalStringToByteArray(cipherText);
                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(cipherbytes))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                        decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                return plaintext;
            }
            catch (ArgumentNullException e)
            {
                throw new ArgumentException($"{e}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured decrypting from Byte_Aes>> {ex.Message}--->> " +
                    $"StackTrace>> {ex.StackTrace} --->> Inner Exception>>{ex.InnerException}--->> Source>> {ex.Source}");
                throw;
            }
        }

        private byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            try
            {
                // Check arguments.
                if (plainText == null || plainText.Length <= 0)
                    throw new ArgumentNullException("plainText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");
                byte[] encrypted;
                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;
                    // Create an encryptor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key,
                    aesAlg.IV);
                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt,
                        encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.
                                swEncrypt.Write(plainText);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }
                // Return the encrypted bytes from the memory stream.
                return encrypted;
            }
            catch (ArgumentNullException e)
            {
                throw new ArgumentException($"{e}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured decrypting from Byte_Aes>> {ex.Message}--->> " +
                   $"StackTrace>> {ex.StackTrace} --->> Inner Exception>>{ex.InnerException}--->> Source>> {ex.Source}");
                throw;
            }
        }

        public string GenerateIV()
        {
            var iv = RandomString(16);
            return iv;
        }

        public string GenerateKey()
        {
            var iv = RandomString(16);
            return iv;
        }

        private byte[] HexadecimalStringToByteArray(string input)
        {
            var outputLength = input.Length / 2;
            var output = new byte[outputLength];
            using (var sr = new StringReader(input))
            {
                for (var i = 0; i < outputLength; i++)
                    output[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(),
                    (char)sr.Read() }), 16);
            }
            return output;
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string Encrypt(string PlainText, string key, string iv)
        {
            try
            {
                using (Aes myAes = Aes.Create())
                {
                    myAes.Key = System.Text.Encoding.UTF8.GetBytes(key);
                    myAes.IV = System.Text.Encoding.UTF8.GetBytes(iv);
                    // Encrypt the string to an array of bytes.
                    byte[] encrypted = EncryptStringToBytes_Aes(PlainText, myAes.Key,
                    myAes.IV);
                    string ciphertext = ByteArrayToString(encrypted);
                    return ciphertext;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured decrypting from Byte_Aes>> {ex.Message}--->> " +
                   $"StackTrace>> {ex.StackTrace} --->> Inner Exception>>{ex.InnerException}--->> Source>> {ex.Source}");
                throw;
            }
        }

        public string Decrypt(string EncryptedText, string key, string iv)
        {
            try
            {
                using (Aes myAes = Aes.Create())
                {
                    myAes.Key = System.Text.Encoding.UTF8.GetBytes(key);
                    myAes.IV = System.Text.Encoding.UTF8.GetBytes(iv);
                    // Decrypt the bytes to a string.
                    string roundtrip = DecryptStringFromBytes_Aes(EncryptedText, myAes.Key,
                    myAes.IV);
                    return roundtrip;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured decrypting from Byte_Aes>> {ex.Message}--->> " +
                   $"StackTrace>> {ex.StackTrace} --->> Inner Exception>>{ex.InnerException}--->> Source>> {ex.Source}");
                throw;
            }
        }
    }
}
