public class FileDataNotFoundException : Exception
{
    public FileDataNotFoundException(int id)
        : base($"File with ID: '{id}' was not found.") { }
}
