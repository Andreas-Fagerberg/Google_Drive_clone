public interface IFileRepository
{
    public Task<FileEntity> UploadFileAsync(FileEntity entity);
    public Task<FileEntity?> GetFileAsync(int id, string userId);

    // public Task<FileEntity> UpdateFileAsync();
    public Task DeleteFileAsync(FileEntity entity);
    public Task<bool> IsFileOwnedByUserAsync(int id, string userId);
}
