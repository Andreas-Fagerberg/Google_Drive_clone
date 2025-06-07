using Microsoft.EntityFrameworkCore;
using Npgsql;

public static class DbExceptionHelper
{
    public static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        if (ex.InnerException is PostgresException pgEx)
        {
            // 23505 = unique_violation
            return pgEx.SqlState == "23505";
        }

        return false;
    }
}