using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var gateway = builder.AddProject<ApiGateway>("api-gateway");

builder.AddProject<ResultsPatternMinimalApis>("api")
    .WithReference(gateway);

builder.Build().Run();