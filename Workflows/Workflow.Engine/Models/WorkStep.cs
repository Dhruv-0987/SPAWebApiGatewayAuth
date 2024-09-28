namespace Workflow.Engine;

public abstract class WorkStep
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public virtual List<WorkStepAction>? Actions { get; set; }
}