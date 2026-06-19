using Microsoft.AspNetCore.Mvc;
using NSubstituteTest.Data;
using NSubstituteTest.Repositories;
using NSubstituteTest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlServerConnectionFactory(connectionString!);
});

builder.Services.AddMemoryCache();

// Repositories
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Services
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/api/tasks", async ([FromServices] ITaskService taskService, CancellationToken ct) =>
{
    var result = await taskService.GetAllTasksAsync(ct);
    return Results.Ok(result);
})
.WithName("GetAllTasks");

app.Run();