using Elastic.Apm.SerilogEnricher;
using Serilog;
using Serilog.Configuration;
using Serilog.Enrichers.AspNetCore;
using Serilog.Sinks.Elasticsearch;

namespace PocElasticSearch.API.Infrastructure.Logging
{
    public static class SerilogExtensions
    {
        public static void UseMySerilog(this IHostBuilder host, IConfiguration configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithElasticApmCorrelationInfo()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(CreateElasticsearchSinkOptions(configuration, environment))
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            host.UseSerilog(Log.Logger);
        }

        private static ElasticsearchSinkOptions CreateElasticsearchSinkOptions(IConfiguration configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{configuration["ElasticConfiguration:IndexPattern"]}_{environment}_{DateTime.UtcNow:yyyy-MM}",
            };
        }
    }
}
