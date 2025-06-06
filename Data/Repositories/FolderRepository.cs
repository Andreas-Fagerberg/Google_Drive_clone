using Microsoft.EntityFrameworkCore;

public class FolderRepository : IFolderRepository
{
    private readonly AppDbContext _context;

    public FolderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FolderEntity> AddFolderAsync(FolderEntity entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<FolderEntity?> GetFolderAsync(int folderId)
    {
        return await _context
            .Folders.Where(folder => folder.Id == folderId)
            .Select(folder => new FolderEntity
            {
                Id = folder.Id,
                FolderName = folder.FolderName,
                CreatedAt = folder.CreatedAt,
                UserId = folder.UserId,
                Files = folder
                    .Files.Select(file => new FileEntity
                    {
                        Id = file.Id,
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        Content = Array.Empty<byte>(),
                        FolderId = file.FolderId,
                    })
                    .ToList(),
            })
            .FirstOrDefaultAsync();
    }

    public async Task<FolderEntity?> GetFolderByNameAsync(string folderName, string userId)
    {
        return await _context.Folders.FirstOrDefaultAsync(f =>
            f.FolderName == folderName && f.UserId == userId
        );
    }

    public async Task<List<FolderEntity>> GetAllUserFoldersAsync(string userId)
    {
        return await _context
            .Folders.Where(f => f.UserId == userId)
            .OrderBy(f => f.FolderName)
            .ToListAsync();
    }

    public async Task UpdateFolderAsync(FolderEntity entity)
    {
        _context.Folders.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFolderAsync(FolderEntity entity)
    {
        _context.Folders.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
