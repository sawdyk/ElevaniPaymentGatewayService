using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace ElevaniPaymentGateway.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserRoles
    {
        [Display(Description = "Super Admin")]
        SuperAdmin = 1,
        [Display(Description = "Admin")]
        Admin,
        [Display(Description = "Merchant Admin")]
        MerchantAdmin,
        [Display(Description = "Merchant User")]
        MerchantUser,
    }
}
