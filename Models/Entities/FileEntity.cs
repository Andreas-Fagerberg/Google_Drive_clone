using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class FileEntity
{
    public int Id { get; set; }

    [Required]
    public required string FileName { get; set; }

    [Required]
    public required string FileNameNormalized { get; set; }

    [Required]
    public required byte[] Content { get; set; }

    [Required]
    public required string ContentType { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public required int FolderId { get; set; }

    [Required]
    public required string UserId { get; set; }

    // Navigation props
    public FolderEntity? Folder { get; set; }
    public IdentityUser? User { get; set; }  // Add this line
}
