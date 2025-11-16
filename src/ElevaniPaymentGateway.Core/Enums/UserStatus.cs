using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace ElevaniPaymentGateway.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserStatus
    {
        [Display(Description = "InActive")]
        Active = 0,
        [Display(Description = "Active")]
        Inactive = 1,
    }
}
