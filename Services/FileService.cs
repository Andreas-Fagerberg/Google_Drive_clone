using System.Threading.Tasks;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;
    private readonly IFolderRepository _folderRepository;

    public FileService(IFileRepository fileRepository, IFolderRepository folderRepository)
    {
        _fileRepository = fileRepository;
        _folderRepository = folderRepository;
    }

    public async Task<FileEntity> UploadFileAsync(FileUploadRequest request, string userId)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request) + ": cannot be null", nameof(request));

        var response = await _fileRepository.UploadFileAsync(
            await FileUploadRequest.ToEntity(request, userId)
        );

        return response;
    }

    public async Task<FileEntity> GetFileAsync(int id, string userId)
    {
        ValidateFileId(id);

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

    public Task<FileEntity> DeleteFileAsync(int id) { }

    #region HelperMethods

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
