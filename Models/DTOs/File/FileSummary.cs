public class FileSummary
{
    public required int Id { get; set; }
    public required string FileName { get; set; }
    public required string ContentType { get; set; }

    public static FileSummary FromEntity(FileEntity entity)
    {
        return new FileSummary
        {
            Id = entity.Id,
            FileName = entity.FileNameNormalized,
            ContentType = entity.ContentType,
        };
    }
}
