using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase;

namespace Kasi_Server.Common.RestEase
{
    public static class Extensions
    {
        public static void RegisterServiceForwarder<T>(this IServiceCollection services, string serviceName)
            where T : class
        {
            var clientName = typeof(T).ToString();
            var svcProvider = services.BuildServiceProvider();
            var config = svcProvider.GetRequiredService<IConfiguration>();
            var restEaseOptions = config.GetOptions<RestEaseOptions>("restEase");
            switch (restEaseOptions.LoadBalancer?.ToLowerInvariant())
            {
                default:
                    ConfigureDefaultClient(services, clientName, serviceName, restEaseOptions);
                    break;
            }

            ConfigureForwarder<T>(services, clientName);
        }
        private static void ConfigureDefaultClient(IServiceCollection services, string clientName,
            string serviceName, RestEaseOptions options)
        {
            services.AddHttpClient(clientName, client =>
            {
                var service = options.Services.SingleOrDefault(s => s.Name.Equals(serviceName,
                    StringComparison.InvariantCultureIgnoreCase));
                if (service == null)
                {
                    throw new RestEaseServiceNotFoundException($"RestEase service: '{serviceName}' was not found.",
                        serviceName);
                }
                if (service.Port>0)
                {
                    client.BaseAddress = new UriBuilder
                    {
                        Scheme = service.Scheme,
                        Host = service.Host,
                        Port = service.Port
                    }.Uri;
                }
                else
                {
                    client.BaseAddress = new UriBuilder
                    {
                        Scheme = service.Scheme,
                        Host = service.Host
                    }.Uri;
                }
            });
        }

        private static void ConfigureForwarder<T>(IServiceCollection services, string clientName) where T : class
        {
            services.AddTransient<T>(c => new RestClient(c.GetService<IHttpClientFactory>().CreateClient(clientName))
            {
                RequestQueryParamSerializer = new QueryParamSerializer()
            }.For<T>());
        }
    }
}