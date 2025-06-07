using System.ComponentModel.DataAnnotations;

public class FolderCreateRequest
{
    [Required]
    [StringLength(
        100,
        MinimumLength = 1,
        ErrorMessage = "Foldername must be between 1 and 100 characters."
    )]
    public required string FolderName { get; set; }

    public static FolderEntity ToEntity(FolderCreateRequest request, string userId)
    {
        return new FolderEntity
        {
            FolderName = request.FolderName,
            FolderNameNormalized = request.FolderName.ToLowerInvariant(),
            UserId = userId,
        };
    }
}
