namespace NSubstituteTest.Services;

public interface ITaskService
{
    Task<IEnumerable<Models.Task>> GetAllTasksAsync(CancellationToken ct);
}
