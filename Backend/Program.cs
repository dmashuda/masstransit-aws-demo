using Amazon.SQS;
using MassTransit;
using Components;
using Components.OrderManagement;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProcessOrderConsumer>();
    x.UsingAmazonSqs((context, cfg) =>
    {
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