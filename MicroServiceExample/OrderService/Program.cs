using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Consumer;
using OrderService.Context;
using OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MainDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PaymentCompletedEventConsumer>();
    x.AddConsumer<StockNotAvailableEventConsumer>();
    x.AddConsumer<PaymentFailedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        // a.microservice.stock.reserved.event kuyruðuna mesaj geldiðinde PaymentCompletedEventConsumer'ýna gönderilir
        cfg.ReceiveEndpoint("a.microservice.payment.completed.event", endpoint =>
        {
            endpoint.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("a.microservice.stock.not.available.event", endpoint =>
        {
            endpoint.ConfigureConsumer<StockNotAvailableEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("a.microservice.payment.failed.event", endpoint =>
        {
            endpoint.ConfigureConsumer<PaymentFailedEventConsumer>(context);
        });
    });
});

// Add services to the container.
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<BusService>();

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