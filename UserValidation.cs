using System.Net.Mail;

namespace UserManagementApi.Services;

public static class UserValidation
{
    public static Dictionary<string, string[]> Validate(string? name, string? email, int age)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 2 || name.Trim().Length > 80)
            errors["name"] = new[] { "Name is required and must contain between 2 and 80 characters." };

        if (string.IsNullOrWhiteSpace(email) || email.Length > 254 || !IsValidEmail(email))
            errors["email"] = new[] { "A valid email address is required (maximum 254 characters)." };

        if (age < 13 || age > 120)
            errors["age"] = new[] { "Age must be between 13 and 120." };

        return errors;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var parsed = new MailAddress(email.Trim());
            return parsed.Address == email.Trim();
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
