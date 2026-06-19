
using NSubstituteTest.Repositories;

namespace NSubstituteTest.Services;

public class TaskService(ICacheService cacheService, ITaskRepository taskRepository) : ITaskService
{
    public async Task<IEnumerable<Models.Task>> GetAllTasksAsync(CancellationToken ct)
    {
        var result = await cacheService.GetOrSetAsync(
            "all_tasks",
            async (cancellationToken) => await taskRepository.GetAllTasksAsync(cancellationToken),
            TimeSpan.FromMinutes(5),
            ct
        );

        return result ?? [];
    }
}
