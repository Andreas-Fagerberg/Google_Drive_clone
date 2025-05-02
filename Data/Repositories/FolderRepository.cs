namespace Google_Drive_clone.Data.Repositories;

public class FolderRepository
{
    private readonly AppDbContext _context;

    public FolderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddFolderToDbAsync(FolderEntity folderEntity)
    {
        await _context.AddAsync(folderEntity);

        FolderCreateResponse folderCreateResponse = new FolderCreateResponse
        {
            FolderPath = folderEntity.Path,
        };

        return await _context.SaveChangesAsync();
    }
}
