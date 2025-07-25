namespace BKCordServer.IdentityModule.UseCases.Auth.ValidationHelpers;
public static class PasswordValidationHelper
{
    public static bool ContainLowercase(string password)
    {
        return password.Any(char.IsLower);
    }

    public static bool ContainUppercase(string password)
    {
        return password.Any(char.IsUpper);
    }

    public static bool ContainDigit(string password)
    {
        return password.Any(char.IsDigit);
    }

    public static bool ContainNonAlphanumeric(string password)
    {
        return password.Any(c => !char.IsLetterOrDigit(c));
    }
}
