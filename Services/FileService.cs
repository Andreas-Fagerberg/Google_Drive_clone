using Google_Drive_clone.Models.Entities;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    // TODO: Consider only taking in a filepath and extracting the needed data from it instead 
    // of name and so on.
    public async Task<FileEntity> UploadFileAsync(FileUploadRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request) + ": cannot be null", nameof(request));

        // TODO: Implement a validation check for if file already exists in folder.


        // Gets all invalid path chars and stores them in an array for easy comparison.
        char[] invalidPathChars = Path.GetInvalidPathChars();

        if (request.FilePath.IndexOfAny(invalidPathChars) >= 0)
        {
            throw new ArgumentException(
                nameof(request.FilePath) + ": contains invalid path characters!",
                nameof(request.FilePath)
            );
        }
        byte[] fileBytes = await File.ReadAllBytesAsync(request.FilePath);

        var fileEntity = new FileEntity
        {
            FileName = request.FileName,
            Content = fileBytes,
            FolderId = request.FolderId,
        };

        var response = _fileRepository.CreateFileAsync(fileEntity);

        return fileEntity;
    }
}
