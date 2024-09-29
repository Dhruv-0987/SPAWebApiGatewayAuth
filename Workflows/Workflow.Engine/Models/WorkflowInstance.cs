namespace Workflow.Engine.Models;

public class WorkflowInstance
{
    public Guid Id { get; set; }
    public string FriendlyId { get; set; }
    public required string Name { get; set; }
    public required WorkflowDefinition WorkflowDefinition { get; set; }
    public string? Action { get; set; }
    public required string CurrentWorkStep { get; set; }
    public required string CurrentWorkStepType { get; set; }
    public string? StepPayload { get; set; }
} 