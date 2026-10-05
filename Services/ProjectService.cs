using Microsoft.EntityFrameworkCore;
using ProjectFlow.Data;
using ProjectFlow.Models;

namespace ProjectFlow.Services;

public class ProjectService
{
    private readonly AppDbContext _db;
    
    public ProjectService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await _db.Projects.ToListAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        return await _db.Projects.FindAsync(id);
    }
}