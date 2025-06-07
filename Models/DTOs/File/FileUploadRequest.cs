using System.ComponentModel.DataAnnotations;

public class FileUploadRequest
{
    [Required]
    public required IFormFile File { get; set; }

    [Required]
    public required string FolderName { get; set; }

    public static async Task<FileEntity> ToEntity(
        FileUploadRequest request,
        string userId,
        int folderId
    )
    {
        var contentType = request.File.ContentType;

        byte[] fileContent;
        using (var memoryStream = new MemoryStream())
        {
            await request.File.CopyToAsync(memoryStream);
            fileContent = memoryStream.ToArray();
        }

        return new FileEntity
        {
            FileName = request.File.FileName,
            FileNameNormalized = request.File.FileName.ToLowerInvariant(),
            Content = fileContent,
            ContentType = contentType,
            FolderId = folderId,
            UserId = userId,
        };
    }
}
