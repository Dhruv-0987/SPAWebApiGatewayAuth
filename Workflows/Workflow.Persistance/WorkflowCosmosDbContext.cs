using FluentResults;
using Microsoft.Azure.Cosmos;
using Workflow.Engine;
using Workflow.Engine.Interfaces;
using Workflow.Engine.Models;

namespace Workflow.Persistance;

public class WorkflowCosmosDbContext: IWorkflowCosmosDbContext
{
    public Container WorkflowInstance { get; private set; }
    public Container WorkStepPayload { get; private set; }
    
    public WorkflowCosmosDbContext(CosmosClient cosmosClient, string databaseName)
    {
        var database = cosmosClient.GetDatabase(databaseName);
        
        WorkflowInstance = database.CreateContainerIfNotExistsAsync(new ContainerProperties()
        {
            Id = nameof(WorkflowInstance),
            PartitionKeyPath = "/Name"
        }).Result.Container;
        
        WorkStepPayload = database.CreateContainerIfNotExistsAsync(new ContainerProperties()
        {
            Id = nameof(WorkStepPayload),
            PartitionKeyPath = "/WorkStepPayloadId"
        }).Result.Container;
    }
    
    public Task<Result<string>> CreateWorkflowInstanceAsync(WorkflowDefinition workflowDefinition)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> GetWorkflowInstanceAsync(Guid id)
    {
        throw new NotImplementedException(); 
    }

    public Task<Result<string>> UpdateWorkflowInstanceAsync(WorkflowInstance workflowInstance)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Guid>> AddPayloadAsync(WorkStepPayload workStepPayload)
    {
        throw new NotImplementedException();
    }
}