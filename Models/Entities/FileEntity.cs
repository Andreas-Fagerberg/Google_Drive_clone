using System.ComponentModel.DataAnnotations;



public class FileEntity
{
    public int Id { get; set; }

    [Required]
    public required string FileName { get; set; }

    [Required]
    public required string ContentType { get; set; }

    [Required]
    public required byte[] Content { get; set; } = new byte[0];
    public DateTime UploadedAt { get; set; }

    [Required]
    public required int FolderId { get; set; }

    // Navigation props
    public FolderEntity? Folder { get; set; }
}
