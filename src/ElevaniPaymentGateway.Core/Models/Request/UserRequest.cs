using ElevaniPaymentGateway.Core.Enums;

namespace ElevaniPaymentGateway.Core.Models.Request
{
    public class UserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public UserRoles Role { get; set; }
    }

    public class MerchantUserRequest : UserRequest
    {
        public string EmailAddress { get; set; }
        public string MerchantId { get; set; }
    }

    public class UpdateMerchantUserRequest : UserRequest
    {
        public Guid Id { get; set; }
        public string MerchantId { get; set; }
    }

    public class AdminUserRequest : UserRequest
    {
        public string EmailAddress { get; set; }
    }

    public class UserStatusRequest
    {
        public Guid Id { get; set; }
        public UserStatus Status { get; set; }
    }

    public class UpdateUserRequest : UserRequest
    {
        public Guid Id { get; set; }
    }
}
