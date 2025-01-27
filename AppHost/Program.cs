using Amazon.CloudFormation;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);


var localStackUri = new Uri("http://localhost:4566");

// var cfConfig = new AmazonCloudFormationConfig
// {
//     ServiceURL = localStackUri.ToString()
// };
// var cfClient = new AmazonCloudFormationClient(cfConfig);

var localStack = builder.AddContainer("localstack", "localstack/localstack")
    .WithEndpoint(port: localStackUri.Port, targetPort: 4566, scheme: "http")
    .WithEnvironment("DEBUG", "1");

// var awsResources = builder.AddAWSCloudFormationTemplate("LocalStackExample-Stack", "aws-resources.template");
// awsResources.Resource.CloudFormationClient = cfClient;

var setup = builder.AddProject<Setup>("Setup")
    .WaitFor(localStack)
    .WithEnvironment($"AWS_ENDPOINT_URL", localStackUri.ToString())
    .WithEnvironment($"AWS_SECRET_KEY", "default")
    .WithEnvironment($"AWS_ACCESS_KEY", "default");


builder.AddProject<Backend>("Backend")
    .WaitFor(localStack)
    .WaitForCompletion(setup)
    .WithEnvironment($"AWS_ENDPOINT_URL", localStackUri.ToString())
    .WithEnvironment($"AWS_SECRET_KEY", "default")
    .WithEnvironment($"AWS_ACCESS_KEY", "default");

builder.AddProject<Api>("Api")
    .WaitFor(localStack)
    .WaitForCompletion(setup)
    .WithEnvironment($"AWS_ENDPOINT_URL", localStackUri.ToString())
    .WithEnvironment($"AWS_SECRET_KEY", "default")
    .WithEnvironment($"AWS_ACCESS_KEY", "default");



;

builder.Build().Run();