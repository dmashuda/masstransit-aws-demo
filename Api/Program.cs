using Amazon.SQS;
using Components.OrderManagement;
using Contracts;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.MapPost("/order", async (OrderModel order, IPublishEndpoint publishEndpoint) =>
    {
        await publishEndpoint.Publish(new ProcessOrder
        {
            OrderId = order.OrderId,
            CustomerNumber = order.CustomerNumber
        });

        return Results.Ok(new OrderInfoModel(order.OrderId, DateTime.UtcNow));
    })
    .WithName("ProcessOrder")
    .WithOpenApi();


app.Run();