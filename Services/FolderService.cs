using Microsoft.EntityFrameworkCore;

public class FolderService : IFolderService
{
    private readonly IFolderRepository _folderRepository;
    private readonly OwnershipValidator _ownershipValidator;

    public FolderService(IFolderRepository folderRepository, OwnershipValidator ownershipValidator)
    {
        _folderRepository = folderRepository;
        _ownershipValidator = ownershipValidator;
    }

    /// <summary>
    /// Creates a new folder for the specified user.
    /// </summary>
    /// <param name="request">The folder creation request.</param>
    /// <param name="userId">The ID of the user creating the folder.</param>
    /// <returns>The created <see cref="FolderEntity"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the request is null.</exception>
    /// <exception cref="ValidationException">Thrown if the folder name is invalid.</exception>
    /// <exception cref="DuplicateItemException">Thrown if a folder with the same name already exists.</exception>
    public async Task<FolderEntity> CreateFolderAsync(FolderCreateRequest request, string userId)
    {
        ValidateFolderCreateRequest(request);

        try
        {
            var folder = await _folderRepository.AddFolderAsync(
                FolderCreateRequest.ToEntity(request, userId)
            );
            return folder;
        }
        catch (DbUpdateException ex) when (DbExceptionHelper.IsUniqueConstraintViolation(ex))
        {
            throw new DuplicateItemException("A folder with the same name already exists.");
        }
    }

    /// <summary>
    /// Retrieves a folder by its ID for the specified user.
    /// </summary>
    /// <param name="id">The ID of the folder to retrieve.</param>
    /// <param name="userId">The ID of the user requesting the folder.</param>
    /// <returns>The retrieved <see cref="FolderEntity"/>.</returns>
    /// <exception cref="ValidationException">Thrown if the folder ID is invalid.</exception>
    /// <exception cref="FolderDataNotFoundException">Thrown if the folder does not exist.</exception>
    public async Task<FolderEntity> GetFolderAsync(int id, string userId)
    {
        ValidateFolderId(id);

        var folder = await _folderRepository.GetFolderAsync(id, userId);

        if (folder == null)
        {
            throw new FolderDataNotFoundException(id);
        }

        return folder;
    }

    /// <summary>
    /// Retrieves all folders belonging to a specified user.
    /// </summary>
    /// <param name="userId">The ID of the user whose folders are being retrieved.</param>
    /// <returns>A list of <see cref="FolderEntity"/> representing the user's folders.</returns>
    public async Task<List<FolderEntity>> GetAllUserFoldersAsync(string userId)
    {
        var folder = await _folderRepository.GetAllUserFoldersAsync(userId);

        return folder;
    }

    /// <summary>
    /// Updates the name of an existing folder for the specified user.
    /// </summary>
    /// <param name="request">The update request containing the new folder name.</param>
    /// <param name="id">The ID of the folder to update.</param>
    /// <param name="userId">The ID of the user updating the folder.</param>
    /// <returns>The updated <see cref="FolderEntity"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the request is null.</exception>
    /// <exception cref="ValidationException">Thrown if the folder name is invalid.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user does not own the folder.</exception>
    /// <exception cref="FolderDataNotFoundException">Thrown if the folder does not exist.</exception>
    /// <exception cref="DuplicateItemException">Thrown if another folder with the same name exists.</exception>
    public async Task<FolderEntity> UpdateFolderAsync(
        FolderUpdateRequest request,
        int id,
        string userId
    )
    {
        ValidateFolderId(id);
        ValidateFolderUpdateRequest(request);

        FolderEntity? existingFolder;
        try
        {
            await _ownershipValidator.EnsureUserOwnsFolderAsync(id, userId);
            existingFolder = await _folderRepository.GetFolderAsync(id, userId);

            if (existingFolder == null)
            {
                throw new FolderDataNotFoundException(id);
            }

            existingFolder.FolderName = request.FolderName;
            existingFolder.FolderNameNormalized = request.FolderName.ToLowerInvariant();
            await _folderRepository.UpdateFolderAsync(existingFolder);

            return existingFolder;
        }
        catch (DbUpdateException ex) when (DbExceptionHelper.IsUniqueConstraintViolation(ex))
        {
            throw new DuplicateItemException("A folder with the same name already exists.");
        }
    }

    /// <summary>
    /// Deletes a folder by its ID for the specified user.
    /// </summary>
    /// <param name="id">The ID of the folder to delete.</param>
    /// <param name="userId">The ID of the user requesting deletion.</param>
    /// <exception cref="ValidationException">Thrown if the folder ID is invalid.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user does not own the folder.</exception>
    /// <exception cref="FolderDataNotFoundException">Thrown if the folder does not exist.</exception>
    public async Task DeleteFolderAsync(int id, string userId)
    {
        ValidateFolderId(id);

        await _ownershipValidator.EnsureUserOwnsFolderAsync(id, userId);

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
            throw new ArgumentNullException(nameof(request));
        if (string.IsNullOrWhiteSpace(request.FolderName))
        {
            throw new ValidationException("Foldername cannot be null or whitespace.");
        }
        if (request.FolderName.Length > 100)
        {
            throw new ValidationException("Foldername must be at most 100 characters.");
        }
    }

    #endregion
}
