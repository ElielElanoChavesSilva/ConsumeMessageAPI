using ConsumerAPI;
using ConsumerAPI.Conection;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var hostName = configuration["RabbitMQ:HostName"];
    var userName = configuration["RabbitMQ:UserName"];
    var password = configuration["RabbitMQ:Password"];

    if (string.IsNullOrEmpty(hostName) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        throw new InvalidOperationException("RabbitMQ configuration is missing.");

    return new RabbitMqConnection(hostName, userName, password);
});
builder.Services.AddHostedService<OrderCreatedConsumer>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();

