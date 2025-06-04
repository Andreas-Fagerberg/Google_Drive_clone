using Google_Drive_clone.Models.Entities;

public interface IFileService
{
    public Task<FileEntity> UploadFileAsync(FileUploadRequest request);
}
