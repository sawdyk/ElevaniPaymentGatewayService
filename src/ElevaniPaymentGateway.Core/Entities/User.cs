using ElevaniPaymentGateway.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace ElevaniPaymentGateway.Core.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserStatus Status { get; set; } = UserStatus.InActive;
        [JsonIgnore]
        public override string? PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
        public DateTime? LastPasswordResetDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? RefreshToken { get; set; } // for user refresh token
        public DateTime? RefreshTokenExpiration { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
