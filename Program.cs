using ProjectFlow.Models;
using ProjectFlow.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ProjectFlow.Dtos;
using ProjectFlow.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=projectflow.db"));

builder.Services.AddScoped<ProjectService>();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/projects", async (ProjectService service) =>
{
  return await service.GetAllProjectsAsync();
});

app.MapGet("/projects/{id}", async (int id, ProjectService service) =>
{
  return await service.GetProjectByIdAsync(id);
});

app.MapPost("/projects", async (Project project, AppDbContext db) =>
{
  var validationResults = new List<ValidationResult>();
  var isValid = Validator.TryValidateObject(
    project,
    new ValidationContext(project),
    validationResults,
    true
  );

  if (!isValid)
  {
    return Results.BadRequest(validationResults);
  }

  project.CreatedAt = DateTime.Now;

  db.Projects.Add(project);

  await db.SaveChangesAsync();

  return Results.Created($"/projects/{project.Id}", project);
});

app.MapPut("/projects/{id}", async (int id, Project project, AppDbContext db) =>
{
  var validationResults = new List<ValidationResult>();

  var isValid = Validator.TryValidateObject(
    project,
    new ValidationContext(project),
    validationResults,
    true
  );

  if (!isValid)
  {
    return Results.BadRequest(validationResults);
  }

  var existingProject = await db.Projects.FindAsync(id);

  if (existingProject == null)
  {
    return Results.NotFound();
  }

  existingProject.Name = project.Name;
  existingProject.Description = project.Description;

  await db.SaveChangesAsync();

  return Results.Ok(existingProject);
});

app.MapDelete("/projects/{id}", async (int id, AppDbContext db) =>
{
  var existingProject = await db.Projects.FindAsync(id);

  if (existingProject == null)
  {
    return Results.NotFound();
  }

  db.Projects.Remove(existingProject);

  await db.SaveChangesAsync();

  return Results.NoContent();
});

app.MapGet("/projects/dto", async (AppDbContext db) =>
{
  return await db.Projects
      .Select(p => new ProjectDto
      {
          Name = p.Name,
          Description = p.Description
      })
      .ToListAsync();
});

app.Run();