public class FolderUpdateResponse
{
    public required int Id { get; set; }
    public required string FolderName { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public static FolderUpdateResponse FromEntity(FolderEntity entity)
    {
        return new FolderUpdateResponse { Id = entity.Id, FolderName = entity.FolderName };
    }
}
