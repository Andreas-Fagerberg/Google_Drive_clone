using System.ComponentModel.DataAnnotations;

public class FileUploadRequest
{
    [Required]
    public required string FileName { get; set; }

    [Required]
    public required string FilePath { get; set; }

    public int FolderId { get; set; }
}
