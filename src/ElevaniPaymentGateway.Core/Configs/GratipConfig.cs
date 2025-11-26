namespace ElevaniPaymentGateway.Core.Configs
{
    public class GratipConfig
    {
        public string? BaseUrl { get; set; }
        public string? APIKey { get; set; }
        public string? APISecret { get; set; }
        public string? RotateCredential { get; set; }
        public TransactionsConfig? TransactionsConfig { get; set; }
    }

    public class TransactionsConfig
    {
        public string? Initiate { get; set; }
        public string? Status { get; set; }
        public string? Finalize { get; set; }
        public string? List { get; set; }
    }
}
