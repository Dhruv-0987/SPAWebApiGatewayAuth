using Workflow.Engine.Models;

namespace Workflow.Engine.Interfaces;

public interface IWorkflowEngine
{
    Task<string> StartWorkflowAsync(WorkflowInstance workflowInstance);
}