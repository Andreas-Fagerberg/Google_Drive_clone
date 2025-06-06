using System.ComponentModel.DataAnnotations;
using Google_Drive_clone.Models.Entities;

public class FolderEntity
{
    public int Id { get; set; }

    [Required]
    public required string FolderName { get; set; }

    [Required]
    public required string UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<FileEntity>? Files { get; set; } = new List<FileEntity>();
}
