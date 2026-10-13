using TaskManagementApi.Models;

namespace TaskManagementApi.Services;

public interface ITaskUtilityService
{
    AllocateTasksResponse AllocateWorkload(AllocateTasksRequest request);
    Task<BatchUpdateResult> BulkUpdateStatusAsync(List<int> taskIds, Models.TaskStatus newStatus);
}