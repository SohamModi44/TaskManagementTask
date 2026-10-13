using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models;

public class WorkTask
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    [Range(typeof(decimal), "0.01", "999999",
        ErrorMessage = "Estimated hours must be greater than 0.")]
    public decimal EstimatedHours { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}