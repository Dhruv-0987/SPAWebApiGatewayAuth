namespace Workflow.Engine.Models;

public class WorkStepPayload
{
    public string WorkStepPayloadId { get; set; }
    public string? Payload { get; set; }
    public DateTime CreatedAt { get; set; }
}