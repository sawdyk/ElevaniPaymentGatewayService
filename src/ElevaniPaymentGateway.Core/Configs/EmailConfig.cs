namespace ElevaniPaymentGateway.Core.Configs
{
    public class EmailConfig
    {
        public bool EnableSSL { get; set; }
        public string? From { get; set; }
        public string? ElevaniEmail { get; set; } //sender's mail
        public string? SMTPAddress { get; set; }
        public int SMTPPort { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
