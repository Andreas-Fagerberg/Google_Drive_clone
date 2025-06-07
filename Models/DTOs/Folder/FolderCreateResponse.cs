public class FolderCreateResponse
{
    public required int Id { get; set; }
    public required string FolderName { get; set; }
    public required DateTime CreatedAt { get; set; }

    public static FolderCreateResponse FromEntity(FolderEntity entity)
    {
        return new FolderCreateResponse
        {
            Id = entity.Id,
            FolderName = entity.FolderName,
            CreatedAt = entity.CreatedAt,
        };
    }
}
