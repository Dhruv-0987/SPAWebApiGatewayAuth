namespace Workflow.Engine.Steps;

public class WaitStep: WorkStep
{
    public List<WorkStepAction> Actions { get; set; } = [];
    
    public override List<WorkStepAction> GetActions()
    {
        return Actions;
    }
}