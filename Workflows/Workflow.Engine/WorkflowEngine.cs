using Workflow.Engine.Interfaces;
using Workflow.Engine.Models;

namespace Workflow.Engine;

public class WorkflowEngine: IWorkflowEngine
{
    public Task<string> StartWorkflowAsync(WorkflowInstance workflowInstance)
    {
        throw new NotImplementedException();
    }
}