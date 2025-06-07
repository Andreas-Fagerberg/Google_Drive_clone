public class FileUploadResponse
{
    public required int Id { get; set; }
    public required string FileName { get; set; }

    public static FileUploadResponse FromEntity(FileEntity entity)
    {
        return new FileUploadResponse { Id = entity.Id, FileName = entity.FileName };
    }
}
