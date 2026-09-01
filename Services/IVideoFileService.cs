using VideoProjectManager.Models;

namespace VideoProjectManager.Services;

public interface IVideoFileService
{
    Task<List<VideoFile>> GetVideoFilesByProjectAsync(uint projectId);
    Task<VideoFile?> GetVideoFileByIdAsync(uint id);
    Task<VideoFile> CreateVideoFileAsync(VideoFile videoFile);
    Task<VideoFile> UpdateVideoFileAsync(VideoFile videoFile);
    Task DeleteVideoFileAsync(uint id);
    Task<List<VideoFile>> BatchAddVideoFilesAsync(uint projectId, List<string> filePaths);
    Task<byte[]> CalculateMd5HashAsync(string filePath);
}