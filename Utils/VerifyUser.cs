public class UserValidation
{
    public static string GetRequiredUserId(string? userId)
    {
        if (userId == null)
            throw new UnauthorizedAccessException($"User was not found");

        return userId;
    }
}
