namespace VideoProjectManager.Models;

public class Project
{
    public uint Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<VideoFile> VideoFiles { get; set; } = new List<VideoFile>();
}