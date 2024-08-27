using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var identityServer = builder.AddProject<IdentityServerAspNetIdentity>("ids");

var gateway = builder.AddProject<ApiGateway>("api-gateway")
    .WithReference(identityServer);

builder.AddProject<ResultsPatternMinimalApis>("api")
    .WithReference(gateway);

builder.Build().Run();