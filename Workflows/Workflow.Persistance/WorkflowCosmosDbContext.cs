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
    
    public async Task<Result<string>> CreateWorkflowInstanceAsync(WorkflowInstance workflowInstance)
    {
        var response = await WorkflowInstance.UpsertItemAsync(workflowInstance, new PartitionKey(workflowInstance.Name), new ItemRequestOptions()
        {
            EnableContentResponseOnWrite = false
        });
        
        return Result.Ok(response.ETag);
    }

    public async Task<Result<WorkflowInstance>> GetWorkflowInstanceAsync(string id, string workflowName)
    {
        var response = await WorkflowInstance.ReadItemAsync<WorkflowInstance>(id, new PartitionKey(workflowName));
        
        var workflowInstance = response.Resource;
        return Result.Ok(workflowInstance);
    }

    public async Task<Result<string>> UpdateWorkflowInstanceAsync(WorkflowInstance workflowInstance)
    {
        var response = await WorkflowInstance.UpsertItemAsync(workflowInstance, new PartitionKey(workflowInstance.Name), new ItemRequestOptions
        {
            EnableContentResponseOnWrite = false
        });
        
        return Result.Ok(response.ETag);
    }
    
    public async Task<Result<string>> UpdateWorkflowInstanceAsync(WorkflowInstance workflowInstance, string etag)
    {
        var response = await WorkflowInstance.UpsertItemAsync(workflowInstance, new PartitionKey(workflowInstance.Name), new ItemRequestOptions
        {
            EnableContentResponseOnWrite = false,
            IfMatchEtag = etag
        });
        
        return Result.Ok(response.ETag);
    }

    public async Task<Result<string>> AddPayloadAsync(string? payload)
    {
        var payloadId = Guid.NewGuid().ToString();
        var workStepPayload = new WorkStepPayload
        {
            WorkStepPayloadId = payloadId,
            Payload = payload,
            CreatedAt = DateTime.Now
        };
        
        var response = await WorkStepPayload.UpsertItemAsync(workStepPayload, new PartitionKey(workStepPayload.WorkStepPayloadId), new ItemRequestOptions
        {
            EnableContentResponseOnWrite = false
        });
        
        return Result.Ok(payloadId);
    }

    public async Task<Result<string>> GetPayloadAsync(string payloadId)
    {
        var response = await WorkStepPayload.ReadItemAsync<WorkStepPayload>(payloadId, new PartitionKey(payloadId), new ItemRequestOptions
        {
            EnableContentResponseOnWrite = false
        });
        
        return Result.Ok(response.ETag);
    }
}