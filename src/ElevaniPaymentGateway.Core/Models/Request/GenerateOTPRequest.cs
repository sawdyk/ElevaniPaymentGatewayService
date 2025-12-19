using ElevaniPaymentGateway.Core.Enums;
using System.Text.Json.Serialization;

namespace ElevaniPaymentGateway.Core.Models.Request
{
    public class GenerateOTPRequest
    {
        public string EmailAddress { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OTPTypes OTPType { get; set; }
    }
}
