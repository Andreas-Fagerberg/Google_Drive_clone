public interface IFolderRepository
{
    public Task<FolderEntity> AddFolderAsync(FolderEntity entity);
    public Task<FolderEntity?> GetFolderAsync(int id, string userId);
    public Task<List<FolderEntity>> GetAllUserFoldersAsync(string userId);
    public Task UpdateFolderAsync(FolderEntity entity);
    public Task DeleteFolderAsync(FolderEntity entity);
    public Task<bool> FolderExistsAsync(string folderName, string userId);
}
