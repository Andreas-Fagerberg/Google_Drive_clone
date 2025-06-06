using System.ComponentModel.DataAnnotations;

public class FolderUpdateRequest
{
    [Required]
    [StringLength(
        100,
        MinimumLength = 1,
        ErrorMessage = "Foldername must be between 1 and 100 characters."
    )]
    public required string FolderName { get; set; }
}