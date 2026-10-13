namespace TaskManagementApi.Models;

public class BatchUpdateResult
{
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public double TotalExecutionTimeMs { get; set; }
}