using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using VideoProjectManager.Data;
using VideoProjectManager.Models;

namespace VideoProjectManager.Services;

public class VideoFileService : IVideoFileService
{
    private readonly ProjectDbContext _context;
    private static readonly string[] VideoExtensions = { ".mp4", ".avi", ".mkv", ".mov", ".flv", ".wmv", ".webm" };

    public VideoFileService(ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<List<VideoFile>> GetVideoFilesByProjectAsync(uint projectId)
    {
        return await _context.VideoFiles
            .Where(vf => vf.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<VideoFile?> GetVideoFileByIdAsync(uint id)
    {
        return await _context.VideoFiles.FindAsync(id);
    }

    public async Task<VideoFile> CreateVideoFileAsync(VideoFile videoFile)
    {
        if (videoFile.Md5Hash.All(b => b == 0))
        {
            videoFile.Md5Hash = await CalculateMd5HashAsync(videoFile.FullPath);
        }
        
        _context.VideoFiles.Add(videoFile);
        await _context.SaveChangesAsync();
        return videoFile;
    }

    public async Task<VideoFile> UpdateVideoFileAsync(VideoFile videoFile)
    {
        _context.VideoFiles.Update(videoFile);
        await _context.SaveChangesAsync();
        return videoFile;
    }

    public async Task DeleteVideoFileAsync(uint id)
    {
        var videoFile = await _context.VideoFiles.FindAsync(id);
        if (videoFile != null)
        {
            _context.VideoFiles.Remove(videoFile);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<VideoFile>> BatchAddVideoFilesAsync(uint projectId, List<string> filePaths)
    {
        var videoFiles = new List<VideoFile>();
        
        foreach (var filePath in filePaths)
        {
            if (File.Exists(filePath) && IsVideoFile(filePath))
            {
                var videoFile = new VideoFile
                {
                    ProjectId = projectId,
                    FullPath = filePath,
                    Description = Path.GetFileName(filePath),
                    Md5Hash = await CalculateMd5HashAsync(filePath),
                    FileLength = new FileInfo(filePath).Length
                };
                
                _context.VideoFiles.Add(videoFile);
                videoFiles.Add(videoFile);
            }
        }
        
        await _context.SaveChangesAsync();
        return videoFiles;
    }

    public async Task<byte[]> CalculateMd5HashAsync(string filePath)
    {
        return await Task.Run(() =>
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                return md5.ComputeHash(stream);
            }
        });
    }

    private static bool IsVideoFile(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return VideoExtensions.Contains(extension);
    }
}