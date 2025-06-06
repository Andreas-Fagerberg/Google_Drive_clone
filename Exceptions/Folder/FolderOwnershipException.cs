public class FolderOwnershipException : Exception
{
    public FolderOwnershipException(int id)
        : base($"User does not have access to folder with ID '{id}'") { }
}
