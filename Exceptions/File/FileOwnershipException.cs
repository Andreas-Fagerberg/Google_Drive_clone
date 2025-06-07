public class FileOwnershipException : Exception
{
    public FileOwnershipException(int id)
        : base($"User does not have access to file with ID '{id}'") { }
}
