namespace ElevaniPaymentGateway.Core.Configs
{
    public class AppSettingsConfig
    {
        public bool IsAppLive { get; set; }
        public string APISecretHeader { get; set; }
        public string APIKeyHeader { get; set; }
        public int MerchantCredentialExpiration { get; set; }
        public bool EnableAPIEncryption { get; set; }
    }
}
