using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VideoProjectManager.Models;
using VideoProjectManager.Services;

namespace VideoProjectManager.ViewModels;

public partial class ProjectViewModel : ObservableObject
{
    private readonly IProjectService _projectService;
    private readonly IVideoFileService _videoFileService;

    [ObservableProperty]
    private ObservableCollection<Project> projects = new();

    [ObservableProperty]
    private Project? selectedProject;

    [ObservableProperty]
    private string projectTitle = string.Empty;

    [ObservableProperty]
    private string projectDescription = string.Empty;

    [ObservableProperty]
    private bool isLoading = false;

    public ProjectViewModel(IProjectService projectService, IVideoFileService videoFileService)
    {
        _projectService = projectService;
        _videoFileService = videoFileService;
    }

    [RelayCommand]
    public async Task LoadProjects()
    {
        IsLoading = true;
        try
        {
            var projects = await _projectService.GetAllProjectsAsync();
            Projects = new ObservableCollection<Project>(projects);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task CreateProject()
    {
        if (string.IsNullOrWhiteSpace(ProjectTitle))
            return;

        var project = new Project
        {
            Title = ProjectTitle,
            Description = ProjectDescription
        };

        await _projectService.CreateProjectAsync(project);
        Projects.Add(project);
        ClearForm();
    }

    [RelayCommand]
    public async Task UpdateProject()
    {
        if (SelectedProject == null || string.IsNullOrWhiteSpace(ProjectTitle))
            return;

        SelectedProject.Title = ProjectTitle;
        SelectedProject.Description = ProjectDescription;
        await _projectService.UpdateProjectAsync(SelectedProject);
        ClearForm();
    }

    [RelayCommand]
    public async Task DeleteProject()
    {
        if (SelectedProject == null)
            return;

        await _projectService.DeleteProjectAsync(SelectedProject.Id);
        Projects.Remove(SelectedProject);
        ClearForm();
    }

    [RelayCommand]
    public void SelectProject(Project project)
    {
        SelectedProject = project;
        ProjectTitle = project.Title;
        ProjectDescription = project.Description ?? string.Empty;
    }

    private void ClearForm()
    {
        ProjectTitle = string.Empty;
        ProjectDescription = string.Empty;
        SelectedProject = null;
    }
}