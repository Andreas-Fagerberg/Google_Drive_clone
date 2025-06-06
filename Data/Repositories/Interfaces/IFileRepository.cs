public interface IFileRepository
{
    public Task<FileEntity> CreateFileAsync(FileEntity fileEntity);
    public Task<FileEntity> GetFileAsync(int fileId);
    public Task<List<FileEntity>> GetAllFilesAsync();
    public Task<FileEntity> UpdateFileAsync();
    public Task DeleteFileAsync();
}
