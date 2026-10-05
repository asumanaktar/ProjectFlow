using Microsoft.EntityFrameworkCore;
using ProjectFlow.Models;

namespace ProjectFlow.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
}