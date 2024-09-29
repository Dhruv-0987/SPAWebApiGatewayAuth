namespace Workflow.Engine.Steps;

public class ApiStep: WorkStep
{
    public required string NextStepIfSuccess { get; set; }
    public required string NextStepIfFailure { get; set; }
    public required string ApiUrl { get; set; }
    public bool UpdateWorkFlowFromResponse { get; set; }
    
    public override List<WorkStepAction>? GetActions()
    {
        return
        [
            new WorkStepAction
            {
                Name = "Success",
                NextWorkStep = NextStepIfSuccess,
                NextWorkStepType = "SuccessStep"
            },

            new WorkStepAction()
            {
                Name = "Failure",
                NextWorkStep = NextStepIfFailure,
                NextWorkStepType = "FailedStep"
            }
        ];
    }

    public override bool IsValidStep()
    {
        return Uri.TryCreate(ApiUrl, UriKind.Absolute, out _);
    }
}