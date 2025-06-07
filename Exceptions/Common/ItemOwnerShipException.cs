public class ItemOwnershipException : Exception
{
    public ItemOwnershipException()
        : base($"The user does not own this item") { }
}
