namespace TaskManagementApi.Models;

public class AllocateTasksRequest
{
    public decimal MaxCapacityHours { get; set; }
    public List<WorkTask> Tasks { get; set; } = new();
}

public class AllocateTasksResponse
{
    public List<WorkTask> SelectedTasks { get; set; } = new();
    public decimal RemainingHours { get; set; }
}

public class BulkUpdateRequest
{
    public List<int> TaskIds { get; set; } = new();
    public TaskStatus NewStatus { get; set; }
}