public class FoldersSummary
{
    public required List<FolderSummary> Folders { get; set; }

    public static FoldersSummary FromEntities(List<FolderEntity> entities)
    {
        return new FoldersSummary
        {
            Folders = entities?.Select(FolderSummary.FromEntity).ToList() ?? [],
        };
    }
}
