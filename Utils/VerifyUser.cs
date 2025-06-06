public class UserValidation
{
    public static string ValidateUser(string? userId)
    {
        if (userId == null)
            throw new UnauthorizedAccessException();

        return userId;
    }
}
