using Dapper;
using NSubstituteTest.Data;

namespace NSubstituteTest.Repositories;

public class TaskRepository(IConnectionFactory connectionFactory) : ITaskRepository
{
    public async Task<IEnumerable<Models.Task>> GetAllTasksAsync(CancellationToken ct)
    {
        using var conn = connectionFactory.CreateConnection();
        string query = "SELECT TOP 1000 Id, ProjectId, ExternalId, IntegrationId, Name, StartDate, DueDate, State, SynchronizedDate, TaskNotes FROM Tasks";
        var tasks = await conn.QueryAsync<Models.Task>(query);
        return tasks;
    }
}
