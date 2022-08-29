using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace PocElasticSearch.API.Infrastructure.Logging
{
    public class SerilogExtensions
    {
        public static void ConfigureLogging(IConfiguration configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(CreateElasticsearchSinkOptions(configuration, environment))
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static ElasticsearchSinkOptions CreateElasticsearchSinkOptions(IConfiguration configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{configuration["ElasticConfiguration:IndexPattern"]}-{environment}-{DateTime.UtcNow:yyyy-MM}",
            };
        }
    }
}
