namespace Workflow.Engine.Models;

public class WorkStepPayload
{
    public Guid WorkStepPayloadId { get; set; }
    public string? Payload { get; set; }
    public DateTime CreatedAt { get; set; }
}