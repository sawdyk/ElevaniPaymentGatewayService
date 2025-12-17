using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace ElevaniPaymentGateway.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OTPTypes
    {
        [Description("Forgot Password")]
        ForgotPassword = 1,
    }
}
