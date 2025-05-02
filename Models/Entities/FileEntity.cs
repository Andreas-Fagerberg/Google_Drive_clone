using System.ComponentModel.DataAnnotations;

namespace Google_Drive_clone.Models.Entities;

public class FileEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }
    public DateTime UploadedAt { get; set; }
    public int? FolderId { get; set; }
    public FolderEntity? Folder { get; set; }
}
