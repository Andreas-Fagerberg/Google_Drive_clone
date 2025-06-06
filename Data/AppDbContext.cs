
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<FileEntity> Files { get; set; }
    public DbSet<FolderEntity> Folders { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<FolderEntity>()
            .HasIndex(f => new { f.FolderName, f.UserId })
            .IsUnique();

        modelBuilder
            .Entity<FolderEntity>()
            .Property(f => f.FolderName)
            .HasMaxLength(100)
            .IsRequired();
    }
}
