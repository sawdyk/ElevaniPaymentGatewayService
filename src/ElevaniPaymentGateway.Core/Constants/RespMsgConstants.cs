namespace ElevaniPaymentGateway.Core.Constants
{
    public static class RespMsgConstants
    {
        public const string Successful = "Successful";
        public const string Failure = "Failed";
        public const string NoContent = "NoContent";
        public const string Exists = "Record/Item Exists";
        public const string NotFound = "Item with specified Id does not exists";
        public const string UnhandledException = "An error occurred";
        public const string InvalidUser = "No user with the specified email address";
        public const string ActiveUser = "User account has already been activated";
        public const string InActiveUser = "User account is not active";
        public const string InValidOTP = "Invalid OTP";
        public const string ExpiredOTP = "OTP has expired, Kindly request for a new OTP";
        public const string PasswordMismatch = "Confirm password and new password mismatch";
        public const string InvalidUsernamePassword = "Invalid Username/Password";
        public const string EmailConfirmationError = "Email is not confirmed";
        public const string UnAuthorizedAccess = "You have an unauthorized access to this application";
        public const string SignInLockedOut = "You account has been locked out for {minutes} minutes";
        public const string TransactionInitiationError = "Unable to initiate transaction. Please try again later";
    }
}
