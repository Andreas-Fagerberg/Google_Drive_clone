public class NameAlreadyExistsException : Exception
{
    public NameAlreadyExistsException(string name)
        : base($"An item already exists with the name '{name}'.") { }
}
