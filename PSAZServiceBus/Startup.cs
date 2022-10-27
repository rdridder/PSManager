using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PSClient;
using PSClient.Options;
using PSInterfaces;

[assembly: FunctionsStartup(typeof(PSAZServiceBus.Startup))]

namespace PSAZServiceBus
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<IPSAPIClient, PSAPIClient>();
            builder.Services.AddOptions<PSAPIClientOptions>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("PSAPIClientOptions").Bind(settings);
            });



            //builder.Services.AddSingleton<IMyService>((s) =>
            //{
            //    return new MyService();
            //});

            //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        }
    }
}
