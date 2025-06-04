public interface IFolderService
{
    public Task<FolderCreateResponse> CreateFolderAsync(FolderCreateRequest request, string userId);
    public Task<string> DeleteFolderAsync(int id);
}
