using System.ComponentModel.DataAnnotations;

namespace Google_Drive_clone.Models.Entities;

public class FileEntity
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string ContentType { get; set; }

    [Required]
    public required byte[] Content { get; set; }
    public DateTime UploadedAt { get; set; }

    // Navigation props
    public int? FolderId { get; set; }
    public FolderEntity? Folder { get; set; }
}
