using Kasi_Server.Common.Consul;
using Kasi_Server.Common.HTTP;
using Kasi_Server.Common.RestEase.Builders;
using Kasi_Server.Common.RestEase.Serializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase;

namespace Kasi_Server.Common.RestEase;

public static class Extensions
{
    private const string SectionName = "restEase";

    public static IServiceCollection AddServiceClient<T>(this IServiceCollection services, string serviceName,
        string sectionName = SectionName, string consulSectionName = "consul", string fabioSectionName = "fabio",
        string httpClientSectionName = "httpClient")
        where T : class
    {
        if (string.IsNullOrWhiteSpace(sectionName))
        {
            sectionName = SectionName;
        }
        var svcProvider = services.BuildServiceProvider();
        var config = svcProvider.GetRequiredService<IConfiguration>();
        var restEaseOptions = config.GetOptions<RestEaseOptions>(sectionName);
        return services.AddServiceClient<T>(serviceName, restEaseOptions);
    }

    public static IServiceCollection AddServiceClient<T>(this IServiceCollection services, string serviceName,
        Func<IRestEaseOptionsBuilder, IRestEaseOptionsBuilder> buildOptions,
        Func<IConsulOptionsBuilder, IConsulOptionsBuilder> buildConsulOptions,
        HttpClientOptions httpClientOptions)
        where T : class
    {
        var options = buildOptions(new RestEaseOptionsBuilder()).Build();
        return services.AddServiceClient<T>(serviceName, options);
    }

    public static IServiceCollection AddServiceClient<T>(this IServiceCollection services, string serviceName,
        RestEaseOptions options, ConsulOptions consulOptions,
        HttpClientOptions httpClientOptions)
        where T : class
        => services.AddServiceClient<T>(serviceName, options);

    private static IServiceCollection AddServiceClient<T>(this IServiceCollection services, string serviceName,
        RestEaseOptions options)
        where T : class
    {
        var clientName = typeof(T).ToString();

        switch (options.LoadBalancer?.ToLowerInvariant())
        {
            case "consul":
                services.AddConsulHttpClient(clientName, serviceName);
                break;

            default:
                ConfigureDefaultClient(services, clientName, serviceName, options);
                break;
        }

        ConfigureForwarder<T>(services, clientName);

        return services;
    }

    private static void ConfigureDefaultClient(IServiceCollection services, string clientName,
        string serviceName, RestEaseOptions options)
    {
        services.AddHttpClient(clientName, client =>
        {
            var service = options.Services.SingleOrDefault(s => s.Name.Equals(serviceName,
                StringComparison.InvariantCultureIgnoreCase));
            if (service is null)
            {
                throw new RestEaseServiceNotFoundException($"RestEase service: '{serviceName}' was not found.",
                    serviceName);
            }

            client.BaseAddress = new UriBuilder
            {
                Scheme = service.Scheme,
                Host = service.Host,
                Port = service.Port
            }.Uri;
        });
    }

    private static void ConfigureForwarder<T>(IServiceCollection services, string clientName) where T : class
    {
        services.AddTransient<T>(c => new RestClient(c.GetRequiredService<IHttpClientFactory>().CreateClient(clientName))
        {
            RequestQueryParamSerializer = new QueryParamSerializer()
        }.For<T>());
    }
}