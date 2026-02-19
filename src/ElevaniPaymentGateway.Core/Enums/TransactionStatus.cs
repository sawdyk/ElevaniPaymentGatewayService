using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace ElevaniPaymentGateway.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionStatus
    {
        [Display(Description = "Pending")]
        Pending = 0,
        [Display(Description = "Completed")]
        Completed,
        [Display(Description = "Failed")]
        Failed,
        [Display(Description = "Declined")]
        Declined,
        [Display(Description = "Cancelled")]
        Cancelled,
        [Display(Description = "Init")]
        Init,
        [Display(Description = "Redirect")]
        Redirect,
        [Display(Description = "Blocked")]
        Blocked,
        [Display(Description = "Abandoned")]
        Abandoned,
    }
}
