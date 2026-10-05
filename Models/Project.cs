using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Models;

public class Project
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";
    
    public string Description { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}