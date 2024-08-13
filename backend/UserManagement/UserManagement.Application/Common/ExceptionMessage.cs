namespace UserManagement.Application.Common
{
    public class ExceptionMessage
    {
        public const string UnregisteredEmail = "Email hasn't been registered";
        public const string WrongPassword = "Wrong password";
        public const string RegisteredEmail = "Email has been registered";
        public const string UserNotFound = "User not found";
        public const string InvalidPassword = "Invalid password. Password contains at least 16 characters including at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character.";
    }
}
