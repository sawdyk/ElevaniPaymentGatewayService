namespace ElevaniPaymentGateway.Core.Models.Response
{
    public class JwtResponse
    {
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
