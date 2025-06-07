using Microsoft.EntityFrameworkCore;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;
    private readonly IFolderRepository _folderRepository;
    private readonly OwnershipValidator _ownershipValidator;

    public FileService(
        IFileRepository fileRepository,
        IFolderRepository folderRepository,
        OwnershipValidator ownershipValidator
    )
    {
        _fileRepository = fileRepository;
        _folderRepository = folderRepository;
        _ownershipValidator = ownershipValidator;
    }

    /// <summary>
    /// Uploads a new file to the specified folder for the given user.
    /// </summary>
    /// <param name="request">The file upload request containing file data and folder name.</param>
    /// <param name="userId">The ID of the user uploading the file.</param>
    /// <returns>The uploaded <see cref="FileEntity"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the request is null.</exception>
    /// <exception cref="DuplicateItemException">Thrown if a file with the same name already exists in the folder.</exception>
    public async Task<FileEntity> UploadFileAsync(FileUploadRequest request, string userId)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var folder = await _folderRepository.GetOrCreateFolderAsync(request.FolderName, userId);

        var entity = await FileUploadRequest.ToEntity(request, userId, folder.Id);

        try
        {
            return await _fileRepository.UploadFileAsync(entity);
        }
        catch (DbUpdateException ex) when (DbExceptionHelper.IsUniqueConstraintViolation(ex))
        {
            throw new DuplicateItemException(
                "A file with the same name already exists in this folder."
            );
        }
    }

    /// <summary>
    /// Retrieves a file by its ID for the specified user.
    /// </summary>
    /// <param name="id">The ID of the file to retrieve.</param>
    /// <param name="userId">The ID of the user requesting the file.</param>
    /// <returns>The retrieved <see cref="FileEntity"/>.</returns>
    /// <exception cref="ValidationException">Thrown if the file ID is not valid.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user does not own the file.</exception>
    /// <exception cref="FileDataNotFoundException">Thrown if the file does not exist.</exception>
    public async Task<FileEntity> GetFileAsync(int id, string userId)
    {
        ValidateFileId(id);
        
        await _ownershipValidator.EnsureUserOwnsFileAsync(id, userId);

        var file = await _fileRepository.GetFileAsync(id, userId);

        if (file == null)
        {
            throw new FileDataNotFoundException(id);
        }

        return file;
    }

    // public Task<FileEntity> UpdateFileAsync()
    // {
    //     throw new NotImplementedException();
    // }

    /// <summary>
    /// Deletes a file by its ID for the specified user.
    /// </summary>
    /// <param name="id">The ID of the file to delete.</param>
    /// <param name="userId">The ID of the user requesting deletion.</param>
    /// <returns>The deleted <see cref="FileEntity"/>.</returns>
    /// <exception cref="ValidationException">Thrown if the file ID is invalid.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user does not own the file.</exception>
    /// <exception cref="FileDataNotFoundException">Thrown if the file does not exist.</exception>
    public async Task<FileEntity> DeleteFileAsync(int id, string userId)
    {
        ValidateFileId(id);

        await _ownershipValidator.EnsureUserOwnsFileAsync(id, userId);

        var existingFile = await _fileRepository.GetFileAsync(id, userId);

        if (existingFile == null)
        {
            throw new FileDataNotFoundException(id);
        }

        await _fileRepository.DeleteFileAsync(existingFile);

        return existingFile;
    }

    #region Helper Methods

    /// <summary>
    /// Checks if the given id is a valid positive number.
    /// </summary>
    /// <param name="id">The file id to be validated</param>
    /// <exception cref="ValidationException">Thrown if id is a non-positiv number</exception>
    private void ValidateFileId(int id)
    {
        if (id <= 0)
        {
            throw new ValidationException("File ID must be a positive number");
        }
    }

    #endregion
}
