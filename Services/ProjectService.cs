using Microsoft.EntityFrameworkCore;
using VideoProjectManager.Data;
using VideoProjectManager.Models;

namespace VideoProjectManager.Services;

public class ProjectService : IProjectService
{
    private readonly ProjectDbContext _context;

    public ProjectService(ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await _context.Projects.Include(p => p.VideoFiles).ToListAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(uint id)
    {
        return await _context.Projects.Include(p => p.VideoFiles).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Project> CreateProjectAsync(Project project)
    {
        project.CreatedAt = DateTime.Now;
        project.UpdatedAt = DateTime.Now;
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> UpdateProjectAsync(Project project)
    {
        project.UpdatedAt = DateTime.Now;
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task DeleteProjectAsync(uint id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}