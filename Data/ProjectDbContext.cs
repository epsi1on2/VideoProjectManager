using Microsoft.EntityFrameworkCore;
using VideoProjectManager.Models;

namespace VideoProjectManager.Data;

public class ProjectDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<VideoFile> VideoFiles { get; set; }

    public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnType("longtext");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<VideoFile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullPath).IsRequired().HasMaxLength(2048);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2048);
            entity.Property(e => e.ExtractedAudioFileFullPath).HasMaxLength(2048);
            entity.Property(e => e.Md5Hash).HasColumnType("binary(16)").IsRequired();
            entity.Property(e => e.FramePerSecond).HasPrecision(10, 4);
            entity.Property(e => e.FramePerSecond).HasConversion<decimal>();
            
            entity.HasOne(e => e.Project)
                .WithMany(p => p.VideoFiles)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}