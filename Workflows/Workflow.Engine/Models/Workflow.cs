namespace Workflow.Engine;

public abstract class Workflow
{
    public virtual required List<WorkStep> Steps { get; set; }
}