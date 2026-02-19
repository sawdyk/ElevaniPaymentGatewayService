using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Models.Response.Gratip;
using ElevaniPaymentGateway.Core.Models.Response.TransactionService;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ElevaniPaymentGateway.Core.Helpers
{
    public static class StringHelpers
    {
        public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => input[0].ToString().ToUpper() + input.Substring(1)
        };

        public static string GenerateTransferReference(string amount)
        {
            string reference = $"walwd_{amount}{DateTime.Now.ToString("yyMMddhhmmss")}";
            return reference;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
                return attributes.First().Description;

            return value.ToString();
        }

        public static string MaskCardNumber(string cardNumber)
        {
            if (cardNumber.Length == 14)
                cardNumber = cardNumber.Remove(6, 4).Insert(4, "****");
            if (cardNumber.Length == 15)
                cardNumber = cardNumber.Remove(6, 5).Insert(4, "*****");
            if (cardNumber.Length == 16)
                cardNumber = cardNumber.Remove(6, 6).Insert(4, "******");
            if (cardNumber.Length == 17)
                cardNumber = cardNumber.Remove(6, 7).Insert(4, "*******");
            if (cardNumber.Length == 18)
                cardNumber = cardNumber.Remove(6, 8).Insert(4, "********");
            if (cardNumber.Length == 19)
                cardNumber = cardNumber.Remove(6, 9).Insert(4, "*********"); 
            if (cardNumber.Length == 20)
                cardNumber = cardNumber.Remove(6, 10).Insert(4, "**********");
            return cardNumber;
        }

        public static TransactionStatus FormatPayAgencyStatus(string status)
        {
            if (status.ToLower().Equals("failed"))
                return TransactionStatus.Failed;
            else if (status.ToLower().Equals("success"))
                return TransactionStatus.Completed;
            else if (status.ToLower().Equals("init"))
                return TransactionStatus.Init;
            else if (status.ToLower().Equals("pending"))
                return TransactionStatus.Pending;
            else if (status.ToLower().Equals("redirect"))
                return TransactionStatus.Redirect;
            else if (status.ToLower().Equals("blocked"))
                return TransactionStatus.Blocked;
            else if (status.ToLower().Equals("abandoned"))
                return TransactionStatus.Abandoned;
            else
                return default;
        }
    }
}
