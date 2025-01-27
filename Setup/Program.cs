// See https://aka.ms/new-console-template for more information

using Amazon.SQS;
using MassTransit;
using Components;
using Components.OrderManagement;
using Setup;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProcessOrderConsumer>();
    x.AddConsumer<ProcessOrderConsumerDup>();
    x.UsingAmazonSqs((context, cfg) =>
    {
        cfg.DeployTopologyOnly = true;
        cfg.Host("us-east-1", h =>
        {
            h.Config(new AmazonSQSConfig
            {
                ServiceURL = builder.Configuration["AWS_ENDPOINT_URL"],
            });
            h.AccessKey(builder.Configuration["AWS_ACCESS_KEY"]);
            h.SecretKey(builder.Configuration["AWS_SECRET_KEY"]);
        });
        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();