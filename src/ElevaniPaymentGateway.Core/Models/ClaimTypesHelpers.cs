namespace ElevaniPaymentGateway.Core.Models
{
    public class ClaimTypesHelpers
    {
        public static string UserId { get; set; } = "UserId";
        public static string FirstName { get; set; } = "FirstName";
        public static string LastName { get; set; } = "LastName";
        public static string UserName { get; set; } = "UserName";
        public static string EmailAddress { get; set; } = "EmailAddress";
        public static string PhoneNumber { get; set; } = "PhoneNumber";
        public static string Role { get; set; } = "Role";
        public static string Jti { get; set; } = "Jti";
    }

    public class PaymentClaimTypesHelpers
    {
        public static string MerchantId { get; set; } = "MerchantId";
        public static string Name { get; set; } = "FirstName";
        public static string Slug { get; set; } = "Slug";
        public static string PaymentGateway { get; set; } = "PaymentGateway";
    }
}
