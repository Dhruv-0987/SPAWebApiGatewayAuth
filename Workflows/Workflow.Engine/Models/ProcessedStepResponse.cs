namespace Workflow.Engine.Models;

public class ProcessedStepResponse
{
    public string? NextActionName { get; set; }
    public object? ResponsePayload { get; set; }
    public bool Suspend { get; set; }
}