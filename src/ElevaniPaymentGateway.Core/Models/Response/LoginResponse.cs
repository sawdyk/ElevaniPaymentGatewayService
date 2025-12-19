using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Models.Dto;

namespace ElevaniPaymentGateway.Core.Models.Response
{
    public class LoginResponse
    {
        public UserDto UserDetails { get; set; }
        //public List<RolePermissionResponse> RolePermissions { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }

    public class UserLoginResponse : LoginResponse
    {
        public Merchant Merchant { get; set; }
    }

    //public class RolePermissionResponse
    //{
    //    public string PermissionName { get; set; }
    //    public string PermissionGroup { get; set; }
    //}
}
