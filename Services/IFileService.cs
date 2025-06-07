
public interface IFileService
{
    public Task<FileEntity> UploadFileAsync(FileUploadRequest request, string userId);
    public Task<FileEntity> GetFileAsync(int id);
    public Task<FileEntity> UpdateFileAsync();
    public Task<FileEntity> DeleteFileAsync(int id);
}
