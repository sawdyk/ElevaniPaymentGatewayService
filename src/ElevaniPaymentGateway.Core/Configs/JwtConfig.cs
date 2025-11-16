namespace ElevaniPaymentGateway.Core.Configs
{
    public class JwtConfig
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double AccessExpiration { get; set; }
        public double RefreshExpiration { get; set; }
    }
}
