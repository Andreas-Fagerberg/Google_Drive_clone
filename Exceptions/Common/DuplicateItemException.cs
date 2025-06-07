public class DuplicateItemException : Exception
{
    public DuplicateItemException(string message = "An item already exists with that name" )
        : base(message) { }
}
