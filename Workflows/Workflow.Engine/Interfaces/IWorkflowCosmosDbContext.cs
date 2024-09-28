using FluentResults;
using Workflow.Engine.Models;

namespace Workflow.Engine.Interfaces;

public interface IWorkflowCosmosDbContext
{
    Task<Result<string>> CreateWorkflowInstanceAsync(WorkflowDefinition workflowDefinition);
    Task<Result<string>> GetWorkflowInstanceAsync(Guid id);
    Task<Result<string>> UpdateWorkflowInstanceAsync(WorkflowInstance workflowInstance);
    Task<Result<Guid>> AddPayloadAsync(WorkStepPayload workStepPayload);
}