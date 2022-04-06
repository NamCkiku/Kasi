using Kasi_Server.Common.Consul.Builders;
using Kasi_Server.Common.Consul.Http;
using Kasi_Server.Common.Consul.MessageHandlers;
using Kasi_Server.Common.Consul.Models;
using Kasi_Server.Common.Consul.Services;
using Kasi_Server.Common.HTTP;
using Kasi_Server.Common.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kasi_Server.Common.Consul;

public static class Extensions
{
    private const string DefaultInterval = "5s";
    private const string SectionName = "consul";

    public static IServiceCollection AddConsul(this IServiceCollection services, string sectionName = SectionName,
        string httpClientSectionName = "httpClient")
    {
        if (string.IsNullOrWhiteSpace(sectionName))
        {
            sectionName = SectionName;
        }
        var svcProvider = services.BuildServiceProvider();
        var config = svcProvider.GetRequiredService<IConfiguration>();
        var consulOptions = config.GetOptions<ConsulOptions>(sectionName);
        var httpClientOptions = config.GetOptions<HttpClientOptions>(httpClientSectionName);
        return services.AddConsul(consulOptions, httpClientOptions);
    }

    public static IServiceCollection AddConsul(this IServiceCollection services,
        Func<IConsulOptionsBuilder, IConsulOptionsBuilder> buildOptions, HttpClientOptions httpClientOptions)
    {
        var options = buildOptions(new ConsulOptionsBuilder()).Build();
        return services.AddConsul(options, httpClientOptions);
    }

    public static IServiceCollection AddConsul(this IServiceCollection services, ConsulOptions options,
        HttpClientOptions httpClientOptions)
    {
        services.AddSingleton(options);
        if (!options.Enabled)
        {
            return services;
        }

        if (httpClientOptions.Type?.ToLowerInvariant() == "consul")
        {
            services.AddTransient<ConsulServiceDiscoveryMessageHandler>();
            services.AddHttpClient<IConsulHttpClient, ConsulHttpClient>("consul-http")
                .AddHttpMessageHandler<ConsulServiceDiscoveryMessageHandler>();
            services.RemoveHttpClient();
            services.AddHttpClient<IHttpClient, ConsulHttpClient>("consul")
                .AddHttpMessageHandler<ConsulServiceDiscoveryMessageHandler>();
        }

        services.AddTransient<IConsulServicesRegistry, ConsulServicesRegistry>();
        var registration = services.CreateConsulAgentRegistration(options);
        if (registration is null)
        {
            return services;
        }

        services.AddSingleton(registration);

        return services;
    }

    public static void AddConsulHttpClient(this IServiceCollection services, string clientName, string serviceName)
        => services.AddHttpClient<IHttpClient, ConsulHttpClient>(clientName)
            .AddHttpMessageHandler(c => new ConsulServiceDiscoveryMessageHandler(
                c.GetRequiredService<IConsulServicesRegistry>(),
                c.GetRequiredService<ConsulOptions>(), serviceName, true));

    private static ServiceRegistration CreateConsulAgentRegistration(this IServiceCollection services,
        ConsulOptions options)
    {
        var enabled = options.Enabled;
        var consulEnabled = Environment.GetEnvironmentVariable("CONSUL_ENABLED")?.ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(consulEnabled))
        {
            enabled = consulEnabled is "true" or "1";
        }

        if (!enabled)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(options.Address))
        {
            throw new ArgumentException("Consul address can not be empty.",
                nameof(options.PingEndpoint));
        }

        services.AddHttpClient<IConsulService, ConsulService>(c => c.BaseAddress = new Uri(options.Url));

        if (services.All(x => x.ServiceType != typeof(ConsulHostedService)))
        {
            services.AddHostedService<ConsulHostedService>();
        }

        string serviceId;
        using (var serviceProvider = services.BuildServiceProvider())
        {
            serviceId = serviceProvider.GetRequiredService<IServiceId>().Id;
        }

        var registration = new ServiceRegistration
        {
            Name = options.Service,
            Id = $"{options.Service}:{serviceId}",
            Address = options.Address,
            Port = options.Port,
            Tags = options.Tags,
            Meta = options.Meta,
            EnableTagOverride = options.EnableTagOverride,
            Connect = options.Connect?.Enabled == true ? new Connect() : null
        };

        if (!options.PingEnabled)
        {
            return registration;
        }

        var pingEndpoint = string.IsNullOrWhiteSpace(options.PingEndpoint) ? string.Empty :
            options.PingEndpoint.StartsWith("/") ? options.PingEndpoint : $"/{options.PingEndpoint}";
        if (pingEndpoint.EndsWith("/"))
        {
            pingEndpoint = pingEndpoint.Substring(0, pingEndpoint.Length - 1);
        }

        var scheme = options.Address.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)
            ? string.Empty
            : "http://";
        var check = new ServiceCheck
        {
            Interval = ParseTime(options.PingInterval),
            DeregisterCriticalServiceAfter = ParseTime(options.RemoveAfterInterval),
            Http = $"{scheme}{options.Address}{(options.Port > 0 ? $":{options.Port}" : string.Empty)}" +
                   $"{pingEndpoint}"
        };
        registration.Checks = new[] { check };

        return registration;
    }

    private static string ParseTime(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return DefaultInterval;
        }

        return int.TryParse(value, out var number) ? $"{number}s" : value;
    }
}