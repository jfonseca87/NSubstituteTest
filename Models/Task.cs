namespace NSubstituteTest.Models;

public class Task
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ExternalId { get; set; } = default!;
    public string IntegrationId { get; set; } = default!;
    public string? Name { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset DueDate { get; set; }
    public int State { get; set; }
    public DateTimeOffset? SynchronizedDate { get; set; }
    public string? TaskNotes { get; set; }
}
