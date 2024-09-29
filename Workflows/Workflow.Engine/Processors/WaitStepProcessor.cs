using Workflow.Engine.Interfaces;
using Workflow.Engine.Models;
using Workflow.Engine.Steps;

namespace Workflow.Engine.Processors;

public class WaitStepProcessor: IStepProcessor
{
    public async Task<ProcessedStepResponse> ProcessStepAsync(WorkStep step, WorkflowInstance workflowInstance)
    {
        var response = new ProcessedStepResponse();

        if (string.IsNullOrEmpty(workflowInstance.Action))
        {
            response.NextActionName = ActionConstants.WaitingForAction;
            response.Suspend = true;
        }
        else
        {
            response.NextActionName = workflowInstance.Action;
            response.Suspend = false;
        }

        workflowInstance.Action = null;
        return response;
    }
}