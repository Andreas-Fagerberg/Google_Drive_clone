public class FolderService : IFolderService
{
    private readonly IFolderRepository _folderRepository;

    public FolderService(IFolderRepository folderRepository)
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

    public async Task<FolderEntity> GetFolderAsync(int id, string userId)
    {
        ValidateFolderId(id);

        var response = await _folderRepository.GetFolderAsync(id);

        if (response == null)
        {
            throw new FolderDataNotFoundException(id);
        }

        ValidateOwnership(response, userId);

        return response;
    }

    public async Task<List<FolderEntity>> GetAllFoldersAsync(string userId)
    {
        var response = await _folderRepository.GetAllUserFoldersAsync(userId);

        if (response == null)
        {
            throw new FoldersDataNotFoundException(userId);
        }

        return response;
    }

    public async Task<FolderEntity> UpdateFolderAsync(
        FolderUpdateRequest request,
        int id,
        string userId
    )
    {
        ValidateFolderId(id);

        var existingFolder = await _folderRepository.GetFolderAsync(id);

        if (existingFolder == null)
        {
            throw new FolderDataNotFoundException(id);
        }

        ValidateOwnership(existingFolder, userId);

        existingFolder.FolderName = request.FolderName;

        await _folderRepository.UpdateFolderAsync(existingFolder);

        return existingFolder;
    }

    public async Task DeleteFolderAsync(int id, string userId)
    {
        ValidateFolderId(id);
        var existingFolder = await _folderRepository.GetFolderAsync(id);

        if (existingFolder == null)
        {
            throw new FolderDataNotFoundException(id);
        }

        ValidateOwnership(existingFolder, userId);

        await _folderRepository.DeleteFolderAsync(existingFolder);
    }

    #region Helper methods

    /// <summary>
    /// Checks if the given id is a valid positive number.
    /// </summary>
    /// <param name="id">The folder id to be validated</param>
    /// <exception cref="ValidationException">Thrown if id is a non-positiv number</exception>
    public void ValidateFolderId(int id)
    {
        if (id <= 0)
        {
            throw new ValidationException("Folder ID must be a positive number");
        }
    }

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

    public static void ValidateOwnership(FolderEntity entity, string userId)
    {
        if (!entity.UserId.Equals(userId))
        {
            throw new FolderOwnershipException(entity.Id);
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
