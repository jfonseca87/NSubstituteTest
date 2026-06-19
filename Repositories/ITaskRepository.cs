namespace NSubstituteTest.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<Models.Task>> GetAllTasksAsync(CancellationToken ct);
}
