using System.Data;
using Google_Drive_clone.Data.Repositories;

public class FolderService : IFolderService
{
    private readonly FolderRepository _folderRepository;

    public FolderService(FolderRepository folderRepository)
    {
        _folderRepository = folderRepository;
    }

    public async Task<FolderEntity> CreateFolderAsync(FolderCreateRequest request, string userId)
    {
        ValidateFolderCreateRequest(request);
        await CheckForDuplicateFolderAsync(request.FolderName, userId);

        FolderEntity folderEntity = new FolderEntity
        {
            FolderName = request.FolderName,
            UserId = userId,
        };

        var response = await _folderRepository.AddFolderAsync(folderEntity);

        return response;
    }

    public async Task<FolderEntity> GetFolderAsync(int folderId, string userId)
    {
        if (folderId <= 0)
        {
            throw new ValidationException("Folder ID must be a positive number");
        }

        var response = await _folderRepository.GetFolderAsync(folderId);

        if (response == null)
        {
            throw new FolderDataNotFoundException(folderId);
        }

        if (response.UserId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        return response;
    }

    public async Task<FolderEntity> GetAllFoldersAsync(string userId)
    {
        
    }

    public Task<FolderEntity> UpdateFolderAsync(FolderUpdateRequest request, string userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFolderAsync(int id, string userId)
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

    /// <summary>
    /// Checks if a duplicate folder already exists with the given folder name and user id.
    /// </summary>
    /// <param name="folderName">The folder name to check for</param>
    /// <param name="userId">The id of the user to check for</param>
    /// <returns></returns>
    /// <exception cref="DuplicateItemException">Thrown if a duplicate folder is found</exception>
    public async Task CheckForDuplicateFolderAsync(string folderName, string userId)
    {
        var existingFolder = await _folderRepository.GetFolderByNameAsync(folderName, userId);

        if (existingFolder != null)
        {
            throw new DuplicateItemException(folderName);
        }
    }

    #endregion
}
