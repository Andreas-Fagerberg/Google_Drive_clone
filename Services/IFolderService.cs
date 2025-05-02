public interface IFolderService
{
    public Task<FolderCreateResponse> CreateFolderAsync(FolderCreateRequest request);
}