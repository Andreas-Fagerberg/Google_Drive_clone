using Microsoft.EntityFrameworkCore;

public class FileRepository : IFileRepository
{
    private readonly AppDbContext _context;

    public FileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FileEntity> UploadFileAsync(FileEntity entity)
    {
        await _context.Files.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<FileEntity?> GetFileAsync(int id, string userId)
    {
        return await _context.Files.FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
    }

    // public Task<FileEntity> UpdateFileAsync()
    // {
    //     throw new NotImplementedException();
    // }

    public async Task DeleteFileAsync(FileEntity entity)
    {
        _context.Files.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsFileOwnedByUserAsync(int id, string userId)
    {
        return await _context.Files.AnyAsync(f => f.Id == id && f.UserId == userId);
    }
}
