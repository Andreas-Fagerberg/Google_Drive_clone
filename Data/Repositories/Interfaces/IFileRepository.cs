public interface IFileRepository
{
    public Task<FileEntity> UploadFileAsync(FileEntity entity);
    public Task<FileEntity?> GetFileAsync(int id, string userId);
    public Task<FileEntity> UpdateFileAsync();
    public Task DeleteFileAsync(int id, string userId);
}
