using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace ElevaniPaymentGateway.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentGateways
    {
        [Display(Description = "GRATIP")]
        GRATIP = 1,
        [Display(Description = "PAY AGENCY")]
        PAYAGENCY
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentGatewayTypes
    {
        [Display(Description = "API")]
        API = 1,
        [Display(Description = "MPGS")]
        MPGS
    }
}
