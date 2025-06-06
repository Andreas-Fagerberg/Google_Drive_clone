public interface IFolderService
{
    public Task<FolderEntity> CreateFolderAsync(FolderCreateRequest request, string userId);
    public Task<FolderEntity> GetFolderAsync(int id, string userId);
    public Task<List<FolderEntity>> GetAllFoldersAsync(string userId);
    public Task<FolderEntity> UpdateFolderAsync(FolderUpdateRequest request, int id, string userId);
    public Task DeleteFolderAsync(int id, string userId);
}
