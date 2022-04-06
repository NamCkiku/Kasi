using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;
using System.Reflection;

namespace Kasi_Server.Logging
{
    public static class SeqExtensions
    {
        public static IHostBuilder UseLogging(this IHostBuilder hostBuilder, string applicationName = null)
        {
            return hostBuilder.UseSerilog(((context, configuration) =>
            {
                var appOptions = context.Configuration.GetOptions<AppOptions>("app");
                var seqOptions = context.Configuration.GetOptions<SeqOptions>("seq");
                var elasticSearchOptions = context.Configuration.GetOptions<ElasticSearchOptions>("elasticsearch");
                var rabbitMQSinksOptions = context.Configuration.GetOptions<RabbitMQSinksOptions>("rabbitmqsinksoptions");
                var serilogOptions = context.Configuration.GetOptions<SerilogOptions>("serilog");
                if (!Enum.TryParse<LogEventLevel>(serilogOptions.Level, true, out var level))
                {
                    level = LogEventLevel.Warning;
                }

                applicationName = string.IsNullOrWhiteSpace(applicationName) ? appOptions.Name : applicationName;
                configuration.Enrich.FromLogContext()
                    .MinimumLevel.Is(level)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
                Configure(configuration, level, seqOptions, serilogOptions, elasticSearchOptions, rabbitMQSinksOptions);
            }));
        }

        private static void Configure(LoggerConfiguration loggerConfiguration, LogEventLevel level, SeqOptions seqOptions, SerilogOptions serilogOptions, ElasticSearchOptions elasticSearchOptions, RabbitMQSinksOptions rabbitMQSinksOptions)
        {
            if (seqOptions.Enabled)
            {
                loggerConfiguration.WriteTo.Seq(seqOptions.Url, apiKey: seqOptions.ApiKey);
            }

            if (serilogOptions.ConsoleEnabled)
            {
                loggerConfiguration.WriteTo.Console();
            }

            // Enable writeing log events to one or more text files.
            if (serilogOptions.FileEnabled)
            {
                loggerConfiguration.WriteTo.File(@"logs\log_.txt", rollingInterval: RollingInterval.Hour, retainedFileCountLimit: null,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
            }

            if (elasticSearchOptions.Enabled)
            {
                loggerConfiguration.Enrich.WithProperty("ApplicationName", elasticSearchOptions.ApplicationName);
                loggerConfiguration.WriteTo.Elasticsearch(ConfigureElasticSink(elasticSearchOptions));
            }
            if (rabbitMQSinksOptions.Enabled)
            {
                loggerConfiguration.Enrich.WithProperty("ApplicationName", rabbitMQSinksOptions.ApplicationName);
                loggerConfiguration.WriteTo.RabbitMQ(ConfigureRabbitMQSink(rabbitMQSinksOptions), new RabbitMQSinkConfiguration() { TextFormatter = new ElasticsearchJsonFormatterRendered() });
            }
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(ElasticSearchOptions elasticSearchOptions)
        {
            return new ElasticsearchSinkOptions(new Uri(elasticSearchOptions.ElasticsearchUrl))
            {
                ModifyConnectionSettings = connectionConfig =>
                {
                    return connectionConfig.BasicAuthentication(elasticSearchOptions.ElasticsearchUser, elasticSearchOptions.ElasticsearchPassword);
                },
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                RegisterTemplateFailure = RegisterTemplateRecovery.IndexAnyway,
                TemplateName = $"serilog-{elasticSearchOptions.ApplicationName.ToLower()}",
                OverwriteTemplate = true,
                CustomFormatter = new ElasticsearchJsonFormatter(),
                NumberOfShards = 1,
                MinimumLogEventLevel = LogEventLevel.Information,
                EmitEventFailure = EmitEventFailureHandling.RaiseCallback,
                FailureCallback = logEvent =>
                {
                    Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
                    Log.Error($"Unable to submit event to Elasticsearch. {logEvent.MessageTemplate}");
                },
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{elasticSearchOptions.ApplicationName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }

        private static RabbitMQClientConfiguration ConfigureRabbitMQSink(RabbitMQSinksOptions rabbitMQSinksOptions)
        {
            var config = new RabbitMQClientConfiguration()
            {
                Username = rabbitMQSinksOptions.Username,
                Password = rabbitMQSinksOptions.Password,
                Exchange = rabbitMQSinksOptions.Exchange,
                ExchangeType = ExchangeType.Topic,
                DeliveryMode = Serilog.Sinks.RabbitMQ.RabbitMQDeliveryMode.Durable,
                Port = rabbitMQSinksOptions.Port,
                RouteKey = rabbitMQSinksOptions.RouteKey,
            };
            config.Hostnames.Add(rabbitMQSinksOptions.Hostname);
            return config;
        }

        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);

            return model;
        }
    }
}