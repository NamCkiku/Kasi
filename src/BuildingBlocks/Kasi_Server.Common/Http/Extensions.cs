using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using System.ComponentModel;

namespace Kasi_Server.Common.HTTP;

public static class Extensions
{
    private const string SectionName = "httpClient";
    private const string RegistryName = "http.client";

    public static IServiceCollection AddHttpClient(this IServiceCollection services, string clientName = "convey",
        IEnumerable<string> maskedRequestUrlParts = null, string sectionName = SectionName,
        Action<IHttpClientBuilder> httpClientBuilder = null)
    {
        if (string.IsNullOrWhiteSpace(sectionName))
        {
            sectionName = SectionName;
        }

        if (string.IsNullOrWhiteSpace(clientName))
        {
            throw new ArgumentException("HTTP client name cannot be empty.", nameof(clientName));
        }
        var svcProvider = services.BuildServiceProvider();
        var config = svcProvider.GetRequiredService<IConfiguration>();
        var options = config.GetOptions<HttpClientOptions>(sectionName);
        if (maskedRequestUrlParts is not null && options.RequestMasking is not null)
        {
            options.RequestMasking.UrlParts = maskedRequestUrlParts;
        }

        bool registerCorrelationContextFactory;
        bool registerCorrelationIdFactory;
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            registerCorrelationContextFactory = scope.ServiceProvider.GetService<ICorrelationContextFactory>() is null;
            registerCorrelationIdFactory = scope.ServiceProvider.GetService<ICorrelationIdFactory>() is null;
        }

        if (registerCorrelationContextFactory)
        {
            services.AddSingleton<ICorrelationContextFactory, EmptyCorrelationContextFactory>();
        }

        if (registerCorrelationIdFactory)
        {
            services.AddSingleton<ICorrelationIdFactory, EmptyCorrelationIdFactory>();
        }

        services.AddSingleton(options);
        services.AddSingleton<IHttpClientSerializer, SystemTextJsonHttpClientSerializer>();
        var clientBuilder = services.AddHttpClient<IHttpClient, KasiHttpClient>(clientName);
        httpClientBuilder?.Invoke(clientBuilder);

        if (options.RequestMasking?.Enabled == true)
        {
            services.Replace(ServiceDescriptor
                .Singleton<IHttpMessageHandlerBuilderFilter, KasiHttpLoggingFilter>());
        }

        return services;
    }

    [Description("This is a hack related to HttpClient issue: https://github.com/aspnet/AspNetCore/issues/13346")]
    public static void RemoveHttpClient(this IServiceCollection services)
    {
        var registryType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
            .SingleOrDefault(t => t.Name == "HttpClientMappingRegistry");
        var registry = services.SingleOrDefault(s => s.ServiceType == registryType)?.ImplementationInstance;
        var registrations = registry?.GetType().GetProperty("TypedClientRegistrations");
        var clientRegistrations = registrations?.GetValue(registry) as IDictionary<Type, string>;
        clientRegistrations?.Remove(typeof(IHttpClient));
    }
}