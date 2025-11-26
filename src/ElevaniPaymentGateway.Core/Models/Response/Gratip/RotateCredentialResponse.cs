namespace ElevaniPaymentGateway.Core.Models.Response.Gratip
{
    public class RotateCredentialResponse
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public RotateCredentialResponseData? data { get; set; }
    }

    public class RotateCredentialResponseData
    {
        public string? api_key { get; set; }
        public string? api_secret { get; set; }
        public DateTime? rotated_at { get; set; }
    }
}
