using Kasi_Server.Common.Consul;
using Kasi_Server.Common.HTTP;
using Kasi_Server.Common.RestEase;
using Kasi_Server.Common.RestEase.Builders;
using Kasi_Server.Common.RestEase.Serializers;
using Kasi_Server.Discovery.Consul;
using Microsoft.Extensions.DependencyInjection;
using RestEase;

namespace Kasi_Server.HTTP.RestEase;

public static class Extensions
{
    private const string SectionName = "restEase";
    private const string RegistryName = "http.restEase";

    public static IKasi_ServerBuilder AddServiceClient<T>(this IKasi_ServerBuilder builder, string serviceName,
        string sectionName = SectionName, string consulSectionName = "consul",
        string httpClientSectionName = "httpClient")
        where T : class
    {
        if (string.IsNullOrWhiteSpace(sectionName))
        {
            sectionName = SectionName;
        }

        var restEaseOptions = builder.GetOptions<RestEaseOptions>(sectionName);
        return builder.AddServiceClient<T>(serviceName, restEaseOptions);
    }

    public static IKasi_ServerBuilder AddServiceClient<T>(this IKasi_ServerBuilder builder, string serviceName,
        Func<IRestEaseOptionsBuilder, IRestEaseOptionsBuilder> buildOptions,
        Func<IConsulOptionsBuilder, IConsulOptionsBuilder> buildConsulOptions,
        HttpClientOptions httpClientOptions)
        where T : class
    {
        var options = buildOptions(new RestEaseOptionsBuilder()).Build();
        return builder.AddServiceClient<T>(serviceName, options);
    }

    public static IKasi_ServerBuilder AddServiceClient<T>(this IKasi_ServerBuilder builder, string serviceName,
        RestEaseOptions options, ConsulOptions consulOptions,
        HttpClientOptions httpClientOptions)
        where T : class
        => builder.AddServiceClient<T>(serviceName, options);

    private static IKasi_ServerBuilder AddServiceClient<T>(this IKasi_ServerBuilder builder, string serviceName,
        RestEaseOptions options)
        where T : class
    {
        if (!builder.TryRegister(RegistryName))
        {
            return builder;
        }

        var clientName = typeof(T).ToString();

        switch (options.LoadBalancer?.ToLowerInvariant())
        {
            case "consul":
                builder.AddConsulHttpClient(clientName, serviceName);
                break;

            default:
                ConfigureDefaultClient(builder.Services, clientName, serviceName, options);
                break;
        }

        ConfigureForwarder<T>(builder.Services, clientName);

        return builder;
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