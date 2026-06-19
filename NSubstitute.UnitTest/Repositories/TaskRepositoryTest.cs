using NSubstituteTest.Repositories;

namespace NSubstitute.UnitTest.Repositories;

public class TaskRepositoryTest
{
    private readonly ITaskRepository _mockTaskRepository;

    public TaskRepositoryTest()
    {
        _mockTaskRepository = Substitute.For<ITaskRepository>();
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnAllTasks()
    {
        // Arrange
        _mockTaskRepository.GetAllTasksAsync(Arg.Any<CancellationToken>())
            .Returns(
            [
                new() 
                { 
                    Id = 1, 
                    Name = "Task 1", 
                    ExternalId = "aaa-111", 
                    IntegrationId = "bbb-111", 
                    ProjectId = 1, 
                    State = 1, 
                    DueDate = DateTime.Now, 
                    StartDate = DateTime.Now.AddMonths(-6), 
                    SynchronizedDate = DateTime.Now.AddDays(-10),
                    TaskNotes = "Notes",
                },
                new()
                { 
                    Id = 2, 
                    Name = "Task 2", 
                    ExternalId = "aaa-222", 
                    IntegrationId = "bbb-222", 
                    ProjectId = 1, 
                    State = 1, 
                    DueDate = DateTime.Now, 
                    StartDate = DateTime.Now.AddMonths(-6), 
                    SynchronizedDate = DateTime.Now.AddDays(-10),
                    TaskNotes = "Notes",
                }
            ]);

        // Act
        var result = await _mockTaskRepository.GetAllTasksAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}
