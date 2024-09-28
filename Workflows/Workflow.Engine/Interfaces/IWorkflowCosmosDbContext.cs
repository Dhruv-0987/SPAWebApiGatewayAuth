using FluentResults;
using Workflow.Engine.Models;

namespace Workflow.Engine.Interfaces;

public interface IWorkflowCosmosDbContext
{
    Task<Result<string>> CreateWorkflowInstanceAsync(WorkflowInstance workflowDefinition);
    Task<Result<WorkflowInstance>> GetWorkflowInstanceAsync(string id, string workflowName);
    Task<Result<string>> UpdateWorkflowInstanceAsync(WorkflowInstance workflowInstance);
    Task<Result<string>> AddPayloadAsync(string payload);
    Task<Result<string>> GetPayloadAsync(string payloadId);
}