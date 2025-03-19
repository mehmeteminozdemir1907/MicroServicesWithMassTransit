using MassTransit;
using Microsoft.EntityFrameworkCore;
using StockService.Consumer;
using StockService.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MainDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        // b.microservice.order.created.event kuyruðuna mesaj geldiðinde OrderCreatedEventConsumer'ýna gönderilir
        cfg.ReceiveEndpoint("b.microservice.order.created.event", endpoint =>
        {
            endpoint.ConfigureConsumer<OrderCreatedEventConsumer>(context);
           // endpoint.Consumer<OrderCreatedEventConsumer>(context);
        });
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();