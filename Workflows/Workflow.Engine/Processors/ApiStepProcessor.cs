using System.Text;
using System.Text.Json;
using Workflow.Engine.Interfaces;
using Workflow.Engine.Models;
using Workflow.Engine.Steps;
using Workflows.ClientLib;

namespace Workflow.Engine.Processors;

public class ApiStepProcessor: IStepProcessor
{
    private readonly IWorkflowCosmosDbContext _workflowCosmosDbContext;
    private readonly HttpClient _httpClient;
    
    public ApiStepProcessor(IHttpClientFactory httpClientFactory, IWorkflowCosmosDbContext workflowCosmosDbContext)
    {
        _workflowCosmosDbContext = workflowCosmosDbContext;
        _httpClient = httpClientFactory.CreateClient(WorkflowConstants.WorkflowHttpClient);
    }
    
    public async Task<ProcessedStepResponse> ProcessStepAsync(WorkStep step, WorkflowInstance workflowInstance)
    {
        var apiStep = step as ApiStep;
        var response = new ProcessedStepResponse();
        
        using var request = new HttpRequestMessage(HttpMethod.Post, apiStep!.ApiUrl);
        
        // add the workflow etag and payload to the request
        try
        {
            request.Headers.Add(nameof(workflowInstance.FriendlyId), workflowInstance.FriendlyId);
            request.Content = new StringContent(JsonSerializer.Serialize(workflowInstance.StepPayload), Encoding.UTF8, "application/json");
        
            var httpResponse = await _httpClient.SendAsync(request);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
        
            response.NextActionName = httpResponse.IsSuccessStatusCode ? ActionConstants.Success : ActionConstants.Error;
            response.ResponsePayload = httpResponse.IsSuccessStatusCode ? responseContent : null;
        
            if (apiStep.UpdateWorkFlowFromResponse)
            {
                await HandleApiStepResponse(responseContent, workflowInstance);
            }

            return response;
        }catch(Exception ex)
        {
            response.NextActionName = ActionConstants.Error;
            response.ResponsePayload = ex.Message;
            return response;
        }
    }

    private async Task HandleApiStepResponse(string responseContent, WorkflowInstance workflowInstance)
    {
        var apiContentResponse = JsonSerializer.Deserialize<ApiContentResponse>(responseContent);
        if (apiContentResponse != null)
        {
            workflowInstance.StepPayload = apiContentResponse.Payload;
            var payloadId = await _workflowCosmosDbContext.AddPayloadAsync(apiContentResponse.Payload);
        }
    }
}