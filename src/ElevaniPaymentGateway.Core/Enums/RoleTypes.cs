using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace ElevaniPaymentGateway.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RoleTypes
    {
        [Description("Admin User")]
        AdminUser = 1,
        [Description("Merchant")]
        Merchant
    }
}
