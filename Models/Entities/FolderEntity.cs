using System.ComponentModel.DataAnnotations;
using Google_Drive_clone.Models.Entities;

public class FolderEntity
{
    public int Id { get; set; }

    [Required]
    public required string FolderName { get; set; }
    public string Path { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? ParentFolderId { get; set; }
    public FolderEntity? ParentFolder { get; set; }
    public ICollection<FolderEntity>? ChildFolders { get; set; } = new List<FolderEntity>();
    public ICollection<FileEntity>? Files { get; set; } = new List<FileEntity>();
}
