using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(GratipCredential))]
    public class GratipCredential : BaseEntity
    {
        public string APIKey { get; set; }
        public string APISecret { get; set; }
    }
}
