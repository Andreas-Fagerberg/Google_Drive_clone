using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class FolderEntity
{
    public int Id { get; set; }

    [Required]
    public required string FolderName { get; set; }

    [Required]
    public required string FolderNameNormalized { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public required string UserId { get; set; }

    // Navigation props
    public IdentityUser? User { get; set; }
    public ICollection<FileEntity> Files { get; set; } = new List<FileEntity>();
}
