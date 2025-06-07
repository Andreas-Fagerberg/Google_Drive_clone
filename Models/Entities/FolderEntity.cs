using System.ComponentModel.DataAnnotations;

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
    public ICollection<FileEntity> Files { get; set; } = new List<FileEntity>();
}
