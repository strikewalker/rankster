using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Rankster.Data;
using Rankster.Data.External;
using Rankster.Data.External.Strike;
using System.Net.Http.Headers;

namespace Rankster.App;
public static class DependencyInjection
{
    public static void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR();
        services.AddAzureClients(builder =>
        {
            builder.AddBlobServiceClient(configuration.GetConnectionString("AzureStorage"));
        });
        services.Configure<RanksterSettings>(configuration.GetSection("RanksterSettings"));
        services.AddDbContext<AppDbContext>(opts =>
        {
            var connString = configuration.GetConnectionString("AppDb");
            opts.UseSqlServer(connString, options =>
            {
                options.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName.Split(',')[0]);
            });
        });
        services.AddHttpClient<IStrikeClient, StrikeClient>((x, client) =>
        {
            var settings = x.GetService<IOptions<RanksterSettings>>().Value;
            client.BaseAddress = new Uri(settings.StrikeApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.StrikeApiKey);
        });
        services.AddSingleton<IStrikeFacade, StrikeFacade>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IRanksterService, RanksterService>();

        services.AddControllersWithViews().AddNewtonsoftJson();
    }
}
