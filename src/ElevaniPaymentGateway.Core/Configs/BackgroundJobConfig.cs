namespace ElevaniPaymentGateway.Core.Configs
{
    public class BackgroundJobConfig
    {
        public int GratipTransactionVerificationTaskDelay { get; set; }
        public int PayAgencyTransactionVerificationTaskDelay { get; set; }
    }
}
