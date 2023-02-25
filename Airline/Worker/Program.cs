using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Worker.App;
using Worker.Client;
using Worker.Data;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddLogging();
        services.AddDbContext<AirlineDbContext>(o => o.UseSqlServer(context.Configuration.GetConnectionString("SqlServer")));
        services.AddScoped<IWebhookClient, WebhookHttpClient>();
        services.AddHttpClient();
        services.AddScoped<IAppHost, AppHost>();
    }).Build();

using var scope = host.Services.CreateScope();
scope.ServiceProvider.GetService<IAppHost>()?.Run();