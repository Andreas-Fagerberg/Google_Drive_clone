using System.ComponentModel.DataAnnotations;

public class FileUploadRequest
{
    [Required]
    public required IFormFile File { get; set; }

    [Required]
    public required int FolderId { get; set; }

    public static async Task<FileEntity> ToEntity(FileUploadRequest request, string userId)
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
            Content = fileContent,
            ContentType = contentType,
            FolderId = request.FolderId,
            UserId = userId,
        };
    }
}
