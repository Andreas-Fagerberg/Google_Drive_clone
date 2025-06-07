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

        await ValidateUniqueNameAsync(request.FolderName, userId);

        FolderEntity folderEntity = new FolderEntity
        {
            FolderName = request.FolderName.Trim(),
            UserId = userId,
        };

        var response = await _folderRepository.AddFolderAsync(folderEntity);

        return response;
    }

    public async Task<FolderEntity> GetFolderAsync(int id, string userId)
    {
        ValidateFolderId(id);

        var response = await _folderRepository.GetFolderAsync(id, userId);

        if (response == null)
        {
            throw new FolderDataNotFoundException(id);
        }

        return response;
    }

    public async Task<List<FolderEntity>> GetAllUserFoldersAsync(string userId)
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
        ValidateFolderUpdateRequest(request);

        var existingFolder = await _folderRepository.GetFolderAsync(id, userId);

        if (existingFolder == null)
        {
            throw new FolderDataNotFoundException(id);
        }

        if (
            !existingFolder.FolderName.Equals(
                request.FolderName,
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            await ValidateUniqueNameAsync(request.FolderName, userId);
        }

        existingFolder.FolderName = request.FolderName;

        await _folderRepository.UpdateFolderAsync(existingFolder);

        return existingFolder;
    }

    public async Task DeleteFolderAsync(int id, string userId)
    {
        ValidateFolderId(id);

        var existingFolder = await _folderRepository.GetFolderAsync(id, userId);

        if (existingFolder == null)
        {
            throw new FolderDataNotFoundException(id);
        }

        await _folderRepository.DeleteFolderAsync(existingFolder);
    }

    #region Helper methods

    /// <summary>
    /// Checks if the given id is a valid positive number.
    /// </summary>
    /// <param name="id">The folder id to be validated</param>
    /// <exception cref="ValidationException">Thrown if id is a non-positiv number</exception>
    private void ValidateFolderId(int id)
    {
        if (id <= 0)
        {
            throw new ValidationException("Folder ID must be a positive number");
        }
    }

    private void ValidateFolderCreateRequest(FolderCreateRequest request)
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
    /// Validates the folder update request.
    /// </summary>
    /// <param name="request">The folder update request to validate</param>
    /// <exception cref="ArgumentNullException">Thrown if request is null</exception>
    /// <exception cref="ValidationException">Thrown if folder name is invalid</exception>
    private void ValidateFolderUpdateRequest(FolderUpdateRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Request cannot be null.");
        }
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

    private async Task ValidateUniqueNameAsync(string folderName, string userId)
    {
        if (await _folderRepository.FolderExistsAsync(folderName, userId))
        {
            throw new DuplicateItemException(folderName);
        }
    }

    #endregion
}
