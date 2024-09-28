namespace Workflow.Engine;

public abstract class WorkflowDefinition
{
    public required string Name { get; set; }
    public required Workflow Workflow { get; set; }
}