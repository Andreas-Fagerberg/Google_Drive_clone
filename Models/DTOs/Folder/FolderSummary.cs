public class FolderSummary
{
    public required int Id { get; set; }
    public required string FolderName { get; set; }
    public required List<FileSummary> Files { get; set; }

    public static FolderSummary FromEntity(FolderEntity entity)
    {
        return new FolderSummary
        {
            Id = entity.Id,
            FolderName = entity.FolderName,
            Files = entity.Files?.Select(FileSummary.FromEntity).ToList() ?? [],
        };
    }
}
