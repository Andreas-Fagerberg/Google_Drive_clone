public class FoldersDataNotFoundException : Exception
{
    public FoldersDataNotFoundException(string id)
        : base($"No folders found for user with ID: '{id}'.") { }
}
