using System.Data;
using Google_Drive_clone.Data.Repositories;

public class FolderService : IFolderService
{
    private readonly FolderRepository _folderRepository;

    public FolderService(FolderRepository folderRepository)
    {
        _folderRepository = folderRepository;
    }

    public async Task<FolderCreateResponse> CreateFolderAsync(
        FolderCreateRequest request,
        string userId
    )
    {
        ValidateFolderCreateRequest(request);
        await CheckForDuplicateFolderAsync(request.FolderName, userId);

        FolderEntity folderEntity = new FolderEntity
        {
            UserId = userId,
            FolderName = request.FolderName,
            CreatedAt = DateTime.UtcNow,
        };

        var response = await _folderRepository.AddFolderAsync(folderEntity);

        return new FolderCreateResponse
        {
            Id = response.Id,
            FolderName = response.FolderName,
            CreatedAt = response.CreatedAt,
        };
    }

    public async Task<FolderEntity> GetFolderAsync(int folderId)
    {
        if (folderId <= 0)
        {
            throw new ArgumentException("Folder ID must be a positive number");
        }

        var folder = await _folderRepository.GetFolderAsync(folderId);

        if (folder == null)
        {
            throw new KeyNotFoundException();
        }

        return folder;
    }

    public async Task<string> DeleteFolderAsync(int id)
    {
        throw new NotImplementedException();
    }

    #region Helper methods

    public void ValidateFolderCreateRequest(FolderCreateRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Request cannot be null.");
        }
        if (string.IsNullOrWhiteSpace(request.FolderName))
        {
            throw new ValidationException("Foldername cannot be null or whitespace.");
        }
        if (request.FolderName.Length > 100)
        {
            throw new ValidationException("Foldername must be at most 100 characters.");
        }
    }

    public async Task CheckForDuplicateFolderAsync(string folderName, string userId)
    {
        var existingFolder = await _folderRepository.GetFolderByNameAsync(folderName, userId);

        if (existingFolder != null)
        {
            throw new NameAlreadyExistsException(folderName);
        }
    }

    #endregion
}
