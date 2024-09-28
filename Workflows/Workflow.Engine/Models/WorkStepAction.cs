namespace Workflow.Engine;

public class WorkStepAction
{
    public required string Name { get; set; }
    public required string NextWorkStep { get; set; }
    public required string NextWorkStepType { get; set; }
}