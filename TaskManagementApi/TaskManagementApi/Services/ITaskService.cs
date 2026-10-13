using TaskManagementApi.Models;

namespace TaskManagementApi.Services;

public interface ITaskService
{
    Task<IEnumerable<WorkTask>> GetAllTasksAsync();
    Task<WorkTask?> GetTaskByIdAsync(int id);
    Task<WorkTask> CreateTaskAsync(WorkTask task);
    Task<bool> UpdateTaskAsync(int id, WorkTask updatedTask);
    Task<bool> DeleteTaskAsync(int id);
}