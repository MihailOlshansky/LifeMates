namespace LifeMates.Domain.Extensions;

public static class EmailExtensions
{
    public static bool IsValid(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith(".")) {
            return false;
        }
        try {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch {
            return false;
        }
    }
}