using Workflow.Engine.Models;

namespace Workflow.Engine.Interfaces;

public interface IStepProcessor
{
    Task<ProcessedStepResponse> ProcessStepAsync(WorkStep step, WorkflowInstance workflowInstance);
}