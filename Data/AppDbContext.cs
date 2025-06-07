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

        // Folder configs
        modelBuilder
            .Entity<FolderEntity>()
            .HasIndex(f => new { f.FolderNameNormalized, f.UserId })
            .IsUnique();

        modelBuilder
            .Entity<FolderEntity>()
            .Property(f => f.FolderName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder
            .Entity<FolderEntity>()
            .Property(f => f.FolderNameNormalized)
            .HasMaxLength(100)
            .IsRequired();

        // File configs
        modelBuilder
            .Entity<FileEntity>()
            .HasIndex(f => new { f.FileNameNormalized, f.UserId })
            .IsUnique();

        modelBuilder.Entity<FileEntity>().Property(f => f.FileName).HasMaxLength(255).IsRequired();

        modelBuilder
            .Entity<FileEntity>()
            .Property(f => f.FileNameNormalized)
            .HasMaxLength(255)
            .IsRequired();

        // Configure cascade delete
        modelBuilder
            .Entity<FileEntity>()
            .HasOne(f => f.Folder)
            .WithMany(folder => folder.Files)
            .HasForeignKey(f => f.FolderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
