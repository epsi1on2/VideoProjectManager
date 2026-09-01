using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VideoProjectManager.Models;
using VideoProjectManager.Services;

namespace VideoProjectManager.ViewModels;

public partial class VideoFileViewModel : ObservableObject
{
    private readonly IVideoFileService _videoFileService;

    [ObservableProperty]
    private uint? currentProjectId;

    [ObservableProperty]
    private ObservableCollection<VideoFile> videoFiles = new();

    [ObservableProperty]
    private VideoFile? selectedVideoFile;

    [ObservableProperty]
    private string videoDescription = string.Empty;

    [ObservableProperty]
    private bool isLoading = false;

    public VideoFileViewModel(IVideoFileService videoFileService)
    {
        _videoFileService = videoFileService;
    }

    [RelayCommand]
    public async Task LoadVideoFiles(uint projectId)
    {
        CurrentProjectId = projectId;
        IsLoading = true;
        try
        {
            var files = await _videoFileService.GetVideoFilesByProjectAsync(projectId);
            VideoFiles = new ObservableCollection<VideoFile>(files);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task AddVideoFile(string filePath)
    {
        if (CurrentProjectId == null || !File.Exists(filePath))
            return;

        var videoFile = new VideoFile
        {
            ProjectId = CurrentProjectId.Value,
            FullPath = filePath,
            Description = VideoDescription
        };

        await _videoFileService.CreateVideoFileAsync(videoFile);
        VideoFiles.Add(videoFile);
        VideoDescription = string.Empty;
    }

    [RelayCommand]
    public async Task BatchAddVideoFiles(string[] filePaths)
    {
        if (CurrentProjectId == null || !filePaths.Any())
            return;

        var files = await _videoFileService.BatchAddVideoFilesAsync(CurrentProjectId.Value, filePaths.ToList());
        foreach (var file in files)
        {
            VideoFiles.Add(file);
        }
    }

    [RelayCommand]
    public async Task UpdateVideoFile()
    {
        if (SelectedVideoFile == null)
            return;

        SelectedVideoFile.Description = VideoDescription;
        await _videoFileService.UpdateVideoFileAsync(SelectedVideoFile);
    }

    [RelayCommand]
    public async Task DeleteVideoFile()
    {
        if (SelectedVideoFile == null)
            return;

        await _videoFileService.DeleteVideoFileAsync(SelectedVideoFile.Id);
        VideoFiles.Remove(SelectedVideoFile);
        ClearForm();
    }

    [RelayCommand]
    public void SelectVideoFile(VideoFile videoFile)
    {
        SelectedVideoFile = videoFile;
        VideoDescription = videoFile.Description;
    }

    private void ClearForm()
    {
        SelectedVideoFile = null;
        VideoDescription = string.Empty;
    }
}