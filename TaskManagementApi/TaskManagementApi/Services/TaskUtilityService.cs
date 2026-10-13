using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services;

public class TaskUtilityService : ITaskUtilityService
{
    private readonly AppDbContext _context;

    public TaskUtilityService(AppDbContext context)
    {
        _context = context;
    }

    public AllocateTasksResponse AllocateWorkload(AllocateTasksRequest request)
    {
        var tasks = request.Tasks ?? new();
        var maxCapacity = request.MaxCapacityHours;

        List<WorkTask> bestCombination = new();
        int maxCount = -1;
        decimal bestSum = -1;

        int totalCombinations = 1 << tasks.Count;
        for (int i = 0; i < totalCombinations; i++)
        {
            var currentCombo = new List<WorkTask>();
            decimal currentSum = 0;

            for (int j = 0; j < tasks.Count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    currentSum += tasks[j].EstimatedHours;
                    currentCombo.Add(tasks[j]);
                }
            }

            if (currentSum <= maxCapacity)
            {
                if (currentCombo.Count > maxCount)
                {
                    maxCount = currentCombo.Count;
                    bestSum = currentSum;
                    bestCombination = currentCombo;
                }
                else if (currentCombo.Count == maxCount && currentSum > bestSum)
                {
                    bestSum = currentSum;
                    bestCombination = currentCombo;
                }
            }
        }

        return new AllocateTasksResponse
        {
            SelectedTasks = bestCombination,
            RemainingHours = maxCapacity - bestSum
        };
    }

    public async Task<BatchUpdateResult> BulkUpdateStatusAsync(List<int> taskIds, Models.TaskStatus newStatus)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var semaphore = new SemaphoreSlim(3, 3);
        int successCount = 0;
        int failureCount = 0;

        var tasks = taskIds.Select(async id =>
        {
            await semaphore.WaitAsync();
            try
            {
                // Simulate 500ms delay for external webhook/notification service
                await Task.Delay(500);

                var taskToUpdate = await _context.Tasks.FindAsync(id);
                if (taskToUpdate != null)
                {
                    taskToUpdate.Status = newStatus;
                    Interlocked.Increment(ref successCount);
                }
                else
                {
                    Interlocked.Increment(ref failureCount);
                }
            }
            catch
            {
                Interlocked.Increment(ref failureCount);
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
        await _context.SaveChangesAsync();
        stopwatch.Stop();

        return new BatchUpdateResult
        {
            SuccessCount = successCount,
            FailureCount = failureCount,
            TotalExecutionTimeMs = stopwatch.ElapsedMilliseconds
        };
    }
}