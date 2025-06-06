public class DuplicateItemException : Exception
{
    public DuplicateItemException(string name)
        : base($"An item already exists with the name '{name}'.") { }
}
