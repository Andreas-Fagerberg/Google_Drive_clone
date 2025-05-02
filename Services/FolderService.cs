using Google_Drive_clone.Data.Repositories;

public class FolderService : IFolderService
{
    private readonly FolderRepository _folderRepository;

    public FolderService(FolderRepository folderRepository)
    {
        _folderRepository = folderRepository;
    }

    public async Task<FolderCreateResponse?> CreateFolderAsync(FolderCreateRequest request)
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
        
        return new FolderCreateResponse { FolderPath = folderEntity.Path };
    }
}
