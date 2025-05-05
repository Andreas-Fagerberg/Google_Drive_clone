public interface IFolderService
{
    public Task<string> CreateFolderAsync(FolderCreateRequest request);
    public Task<string> DeleteFolderAsync(string id);
}