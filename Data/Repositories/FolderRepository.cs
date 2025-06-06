using Google_Drive_clone.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Google_Drive_clone.Data.Repositories;

public class FolderRepository
{
    private readonly AppDbContext _context;

    public FolderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FolderEntity> AddFolderAsync(FolderEntity folderEntity)
    {
        await _context.AddAsync(folderEntity);
        await _context.SaveChangesAsync();
        return folderEntity;
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
                        Content = new byte[0],
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
}
