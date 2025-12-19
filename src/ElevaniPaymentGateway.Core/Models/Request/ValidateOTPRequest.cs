using ElevaniPaymentGateway.Core.Enums;
using System.Text.Json.Serialization;

namespace ElevaniPaymentGateway.Core.Models.Request
{
    public class ValidateOTPRequest
    {
        public string EmailAddress { get; set; }
        public string OTP { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OTPTypes OTPType { get; set; }
    }
}
