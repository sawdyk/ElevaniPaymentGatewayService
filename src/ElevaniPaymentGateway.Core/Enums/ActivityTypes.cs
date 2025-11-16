using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ElevaniPaymentGateway.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActivityTypes
    {
        [Display(Description = "Login")]
        Login = 1,
        [Display(Description = "Password")]
        Password,
        [Display(Description = "Merchant")]
        Merchant,
        [Display(Description = "Merchant IP Address")]
        MerchantIPAddress,
    }
}
