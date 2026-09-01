namespace VideoProjectManager.Models;

public class VideoFile
{
    public uint Id { get; set; }
    public uint ProjectId { get; set; }
    public string FullPath { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ExtractedAudioFileFullPath { get; set; }
    public byte[] Md5Hash { get; set; } = new byte[16];
    public long? FileLength { get; set; }
    public long? FrameCount { get; set; }
    public decimal? FramePerSecond { get; set; }

    public virtual Project? Project { get; set; }
}