using Api.Data;
using Api.MessageBus;
using Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers(options =>
    {
        options.SuppressAsyncSuffixInActionNames = false;
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<AirlineDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    });

    builder.Services.AddSingleton<IMessageBus, RabbitMQMessageBus>();

    builder.Services.AddScoped<IFlightService, FlightManager>();
    builder.Services.AddScoped<IWebhookSubscriptionService, WebhookSubscriptionManager>();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
