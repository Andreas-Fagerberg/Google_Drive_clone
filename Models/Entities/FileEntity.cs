using System.ComponentModel.DataAnnotations;

namespace Google_Drive_clone.Models.Entities;

public class FileEntity
{
    public int Id { get; set; }

    [Required]
    public required string FileName { get; set; }

    [Required]
    public required byte[] Content { get; set; } = new byte[0];
    public DateTime UploadedAt { get; set; }

    // Navigation props
    public int? FolderId { get; set; }

    // Navigation Props
    public FolderEntity? Folder { get; set; }
}
