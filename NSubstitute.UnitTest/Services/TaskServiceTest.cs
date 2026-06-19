using NSubstituteTest.Repositories;
using NSubstituteTest.Services;
using System.Linq;

namespace NSubstitute.UnitTest.Services;

public class TaskServiceTest
{
    private readonly ICacheService _cache = Substitute.For<ICacheService>();
    private readonly ITaskRepository _repository = Substitute.For<ITaskRepository>();
    private TaskService CreateSut() => new(_cache, _repository);

    [Fact]
    public async Task GetAllTasksAsync_CacheHit_ReturnsCachedTasks_NoRepositoryCall()
    {
        // Arrange
        var ct = CancellationToken.None;
        var cached = new List<NSubstituteTest.Models.Task>
        {
            new() { Id = 1, ProjectId = 10, ExternalId = "EXT-1", IntegrationId = "INT-1", Name = "Cached Task" }
        };

        _cache
            .GetOrSetAsync(
                "all_tasks",
                Arg.Any<Func<CancellationToken, Task<IEnumerable<NSubstituteTest.Models.Task>>>>(),
                Arg.Is<TimeSpan?>(t => t == TimeSpan.FromMinutes(5)),
                ct)
            .Returns(Task.FromResult<IEnumerable<NSubstituteTest.Models.Task>?>(cached));

        var sut = CreateSut();

        // Act
        var result = await sut.GetAllTasksAsync(ct);

        // Assert
        Assert.Same(cached, result);
        await _repository.DidNotReceiveWithAnyArgs().GetAllTasksAsync(default);
    }

    [Fact]
    public async Task GetAllTasksAsync_CacheMiss_CallsRepositoryAndReturnsTasks()
    {
        // Arrange
        var ct = CancellationToken.None;
        var repoData = new List<NSubstituteTest.Models.Task>
        {
            new() { Id = 2, ProjectId = 11, ExternalId = "EXT-2", IntegrationId = "INT-2", Name = "Repo Task 1" },
            new() { Id = 3, ProjectId = 11, ExternalId = "EXT-3", IntegrationId = "INT-2", Name = "Repo Task 2" }
        };

        _repository.GetAllTasksAsync(ct).Returns(Task.FromResult<IEnumerable<NSubstituteTest.Models.Task>>(repoData));

        _cache
            .GetOrSetAsync(
                "all_tasks",
                Arg.Any<Func<CancellationToken, Task<IEnumerable<NSubstituteTest.Models.Task>>>>(),
                Arg.Is<TimeSpan?>(t => t == TimeSpan.FromMinutes(5)),
                ct)
            .Returns(ci =>
            {
                // Simulate cache miss by invoking the provided factory.
                var factory = ci.ArgAt<Func<CancellationToken, Task<IEnumerable<NSubstituteTest.Models.Task>>>>(1);
                var token = ci.ArgAt<CancellationToken>(3);
                return factory(token);
            });

        var sut = CreateSut();

        // Act
        var result = await sut.GetAllTasksAsync(ct);

        // Assert
        Assert.Equal(repoData.Count, result.Count());
        Assert.True(repoData.SequenceEqual(result));
        await _repository.Received(1).GetAllTasksAsync(ct);
    }

    [Fact]
    public async Task GetAllTasksAsync_CacheReturnsNull_ReturnsEmptyEnumerable()
    {
        // Arrange
        var ct = CancellationToken.None;

        _cache
            .GetOrSetAsync(
                "all_tasks",
                Arg.Any<Func<CancellationToken, Task<IEnumerable<NSubstituteTest.Models.Task>>>>(),
                Arg.Any<TimeSpan?>(),
                ct)
            .Returns(Task.FromResult<IEnumerable<NSubstituteTest.Models.Task>?>(null));

        var sut = CreateSut();

        // Act
        var result = await sut.GetAllTasksAsync(ct);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllTasksAsync_PassesCancellationTokenToCacheService()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var repoData = new List<NSubstituteTest.Models.Task> { new() { Id = 5, ProjectId = 20, ExternalId = "EXT-5", IntegrationId = "INT-5", Name = "X" } };
        _repository.GetAllTasksAsync(ct).Returns(Task.FromResult<IEnumerable<NSubstituteTest.Models.Task>>(repoData));

        _cache
            .GetOrSetAsync(
                Arg.Is("all_tasks"),
                Arg.Any<Func<CancellationToken, Task<IEnumerable<NSubstituteTest.Models.Task>>>>(),
                Arg.Is<TimeSpan?>(t => t == TimeSpan.FromMinutes(5)),
                Arg.Is(ct))
            .Returns(ci =>
            {
                var factory = ci.ArgAt<Func<CancellationToken, Task<IEnumerable<NSubstituteTest.Models.Task>>>>(1);
                return factory(ct);
            });

        var sut = CreateSut();

        // Act
        var result = await sut.GetAllTasksAsync(ct);

        // Assert
        Assert.Single(result);
        await _cache.Received(1).GetOrSetAsync(
            "all_tasks",
            Arg.Any<Func<CancellationToken, Task<IEnumerable<NSubstituteTest.Models.Task>>>>(),
            TimeSpan.FromMinutes(5),
            ct);
    }

    [Fact]
    public async Task GetAllTasksAsync_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var ct = CancellationToken.None;
        var ex = new InvalidOperationException("Repo failure");

        _repository
            .GetAllTasksAsync(ct)
            .Returns<Task<IEnumerable<NSubstituteTest.Models.Task>>>(_ => throw ex);

        _cache
            .GetOrSetAsync(
                "all_tasks",
                Arg.Any<Func<CancellationToken, Task<IEnumerable<NSubstituteTest.Models.Task>>>>(),
                Arg.Any<TimeSpan?>(),
                ct)
            .Returns(ci =>
            {
                var factory = ci.ArgAt<Func<CancellationToken, Task<IEnumerable<NSubstituteTest.Models.Task>>>>(1);
                return factory(ct);
            });

        var sut = CreateSut();

        // Act / Assert
        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAllTasksAsync(ct));
        Assert.Same(ex, thrown);
    }
}
