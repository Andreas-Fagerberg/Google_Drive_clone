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

    public Task DeleteFileAsync(int id)
    {
        throw new NotImplementedException();
    }

}
