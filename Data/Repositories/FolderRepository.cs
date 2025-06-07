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

    public async Task<FolderEntity> GetOrCreateFolderAsync(string folderName, string userId)
    {
        var normalizedFolderName = folderName.ToLowerInvariant();

        var folder = await _context.Folders.FirstOrDefaultAsync(f =>
            f.FolderNameNormalized == normalizedFolderName && f.UserId == userId
        );

        if (folder != null)
            return folder;

        var newFolder = new FolderEntity
        {
            FolderName = folderName,
            FolderNameNormalized = normalizedFolderName,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Folders.Add(newFolder);
        await _context.SaveChangesAsync();

        return newFolder;
    }

    public async Task<FolderEntity?> GetFolderAsync(int folderId, string userId)
    {
        return await _context
            .Folders.Include(folder => folder.Files)
            .Where(folder => folder.Id == folderId && folder.UserId == userId)
            .Select(folder => new FolderEntity
            {
                Id = folder.Id,
                FolderName = folder.FolderName,
                FolderNameNormalized = folder.FolderNameNormalized,
                CreatedAt = folder.CreatedAt,
                UserId = folder.UserId,
                Files = folder
                    .Files.Select(file => new FileEntity
                    {
                        Id = file.Id,
                        FileName = file.FileName,
                        FileNameNormalized = file.FileNameNormalized,
                        ContentType = file.ContentType,
                        Content = Array.Empty<byte>(),
                        FolderId = file.FolderId,
                        UserId = file.UserId,
                    })
                    .ToList(),
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<FolderEntity>> GetAllUserFoldersAsync(string userId)
    {
        var folders = await _context
            .Folders.Include(f => f.Files)
            .Where(folder => folder.UserId == userId)
            .OrderBy(folder => folder.FolderName)
            .Select(folder => new FolderEntity
            {
                Id = folder.Id,
                FolderName = folder.FolderName,
                FolderNameNormalized = folder.FolderNameNormalized,
                CreatedAt = folder.CreatedAt,
                UserId = folder.UserId,
                Files = folder
                    .Files.Select(file => new FileEntity
                    {
                        Id = file.Id,
                        FileName = file.FileName,
                        FileNameNormalized = file.FileNameNormalized,
                        ContentType = file.ContentType,
                        Content = Array.Empty<byte>(),
                        FolderId = file.FolderId,
                        UserId = file.UserId,
                    })
                    .ToList(),
            })
            .ToListAsync();

        return folders ?? new List<FolderEntity>();
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

    public async Task<bool> IsFolderOwnedByUserAsync(int id, string userId)
    {
        return await _context.Folders.AnyAsync(f => f.Id == id && f.UserId == userId);
    }
}
