namespace ElevaniPaymentGateway.Core.Helpers
{
    public class RandomGeneratorHelper
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%&0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateTransactionReference(string merchantSlug, string currencyCode, string amount)
        {
           return $"{merchantSlug.ToUpper()}{DateTime.Now.ToString("MMddyyyyhhmmss")}{currencyCode}{amount}";
        }
    }
}
