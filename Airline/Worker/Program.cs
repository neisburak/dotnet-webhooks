using Microsoft.EntityFrameworkCore;
using Worker.App;
using Worker.Client;
using Worker.Data;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AirlineDbContext>(o => o.UseSqlServer(context.Configuration.GetConnectionString("SqlServer")));
        services.AddScoped<IWebhookClient, WebhookHttpClient>();
        services.AddHttpClient();
        services.AddScoped<IAppHost, AppHost>();
    }).Build();

using var scope = host.Services.CreateScope();
scope.ServiceProvider.GetService<IAppHost>()?.Run();