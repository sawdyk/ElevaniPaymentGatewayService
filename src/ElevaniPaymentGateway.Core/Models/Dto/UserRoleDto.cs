namespace ElevaniPaymentGateway.Core.Models.Dto
{
    public class UserRoleDto
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public UserDto User { get; set; }
        public RoleDto Role { get; set; }
    }
}
