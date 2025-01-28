using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var localStackUri = new Uri("http://localhost:4566");


// use localstack so you don't need to set up an aws account
var localStack = builder.AddContainer("localstack", "localstack/localstack")
    .WithEndpoint(port: localStackUri.Port, targetPort: 4566, scheme: "http")
    .WithEnvironment("DEBUG", "1")
    .WithHttpHealthCheck("/_localstack/health") 
    .WithLifetime(ContainerLifetime.Persistent); // Use a persistent lifetime so the container does not restart 


// Setup job will create the sqs/sns resources then exit
var setup = builder.AddProject<Setup>("Setup")
    .WaitFor(localStack)
    .WithEnvironment($"AWS_ENDPOINT_URL", localStackUri.ToString())
    .WithEnvironment($"AWS_SECRET_KEY", "default")
    .WithEnvironment($"AWS_ACCESS_KEY", "default");


builder.AddProject<Backend>("Backend")
    .WaitFor(localStack)
    .WaitForCompletion(setup) // wait for setup job to complete before starting 
    .WithEnvironment($"AWS_ENDPOINT_URL", localStackUri.ToString())
    .WithEnvironment($"AWS_SECRET_KEY", "default")
    .WithEnvironment($"AWS_ACCESS_KEY", "default");

builder.AddProject<Api>("Api")
    .WaitFor(localStack)
    .WaitForCompletion(setup) // wait for setup job to complete before starting 
    .WithEnvironment($"AWS_ENDPOINT_URL", localStackUri.ToString())
    .WithEnvironment($"AWS_SECRET_KEY", "default")
    .WithEnvironment($"AWS_ACCESS_KEY", "default");


builder.Build().Run();