namespace ElevaniPaymentGateway.Core.Models.Request
{
    public class PasswordRequest
    {
        public string EmailAddress { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordRequest : PasswordRequest
    {
        public string OTP { get; set; }
    }

    public class ChangePasswordRequest : PasswordRequest
    {
        public string OldPassword { get; set; }
    }
}
