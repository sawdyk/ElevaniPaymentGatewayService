namespace ElevaniPaymentGateway.Core.Configs
{
    public class PayAgencyConfig
    {
        public string BaseUrl { get; set; }
        public string EncryptionKey { get; set; }
        public string SecretKey { get; set; }
        public string Initiate { get; set; }
        public string Status { get; set; }
        public string MerchantEncryptionKey { get; set; }
        public string IPAddress { get; set; }
    }
}
