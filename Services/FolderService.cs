using Google_Drive_clone.Data.Repositories;

public class FolderService : IFolderService
{
    private readonly FolderRepository _folderRepository;

    public FolderService(FolderRepository folderRepository)
    {
        _folderRepository = folderRepository;
    }

    public async Task<string?> CreateFolderAsync(FolderCreateRequest request)
    {
        FolderEntity folderEntity = new FolderEntity
        {
            FolderName = request.FolderName,
            Path = "placeholder",
            ParentFolderId = request.ParentFolderId,
        };

        if (await _folderRepository.AddFolderToDbAsync(folderEntity) == 0)
        {
            return null;
        }

        return folderEntity.Path;
    }

    public async Task<string> DeleteFolderAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<string>
}
