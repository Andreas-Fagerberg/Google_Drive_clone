public interface IFolderRepository
{
    public Task<FolderEntity> AddFolderAsync(FolderEntity entity);
    public Task<FolderEntity?> GetFolderAsync(int folderId);
    public Task<FolderEntity?> GetFolderByNameAsync(string folderName, string userId);
    public Task<List<FolderEntity>> GetAllUserFoldersAsync(string userId);
    public Task UpdateFolderAsync(FolderEntity entity);
    public Task DeleteFolderAsync(FolderEntity entity);
}
