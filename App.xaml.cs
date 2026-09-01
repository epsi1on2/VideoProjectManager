using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VideoProjectManager.Data;
using VideoProjectManager.Services;
using VideoProjectManager.ViewModels;
using VideoProjectManager.Views;

namespace VideoProjectManager;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ConfigureServices();
    }

    private void ConfigureServices()
    {
        var services = new ServiceCollection();

        // Database
        const string connectionString = "Server=localhost;Port=3306;Database=projects;Uid=root;Pwd=your_password;";
        services.AddDbContext<ProjectDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        // Services
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IVideoFileService, VideoFileService>();

        // ViewModels
        services.AddSingleton<ProjectViewModel>();
        services.AddSingleton<VideoFileViewModel>();

        // Views
        services.AddSingleton<MainWindow>();

        _serviceProvider = services.BuildServiceProvider();

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.DataContext = _serviceProvider.GetRequiredService<ProjectViewModel>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        _serviceProvider?.Dispose();
    }
}