namespace ElevaniPaymentGateway.Core.Models.Dto
{
    public class UserContextDto
    {
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? Role { get; set; }
        public string? IPAddress { get; set; }
    }
}
