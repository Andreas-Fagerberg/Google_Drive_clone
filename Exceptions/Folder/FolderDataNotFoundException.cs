public class FolderDataNotFoundException : Exception
{
    public FolderDataNotFoundException(int id)
        : base($"Folder with ID: '{id}' was not found.") { }
}
