using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace ElevaniPaymentGateway.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserStatus
    {
        [Display(Description = "Active")]
        Active = 0,
        [Display(Description = "InActive")]
        InActive = 1,
    }
}
