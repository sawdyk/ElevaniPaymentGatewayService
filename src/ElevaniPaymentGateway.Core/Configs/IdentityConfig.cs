namespace ElevaniPaymentGateway.Core.Configs
{
    public class IdentityConfig
    {
        public int MaxFailedAccessAttempts { get; set; }
        public int DefaultLockoutTimeSpan { get; set; }
        public bool AllowedForNewUsers { get; set; }
    }
}
