public class OwnershipValidator
{
    private readonly IFileRepository _fileRepository;
    private readonly IFolderRepository _folderRepository;

    public OwnershipValidator(IFileRepository fileRepository, IFolderRepository folderRepository)
    {
        _fileRepository = fileRepository;
        _folderRepository = folderRepository;
    }

    public static void CheckOwnership(bool ownsItem)
    {
        if (!ownsItem)
            throw new ItemOwnershipException();
    }

    public async Task EnsureUserOwnsFileAsync(int id, string userId)
    {
        var isOwned = await _fileRepository.IsFileOwnedByUserAsync(id, userId);
        CheckOwnership(isOwned);
    }

    public async Task EnsureUserOwnsFolderAsync(int id, string userId)
    {
        var isOwned = await _folderRepository.IsFolderOwnedByUserAsync(id, userId);
        CheckOwnership(isOwned);
    }

    public async Task EnsureUserOwnsItemsAsync(int folderId, int fileId, string userId)
    {
        var isOwnedFolder = await _folderRepository.IsFolderOwnedByUserAsync(folderId, userId);
        var isOwnedFile = await _fileRepository.IsFileOwnedByUserAsync(fileId, userId);
        CheckOwnership(isOwnedFolder && isOwnedFile);
    }
}
