using Kasi_Server.Common.HTTP;

namespace Kasi_Server.Common.Consul.Http;

internal sealed class ConsulHttpClient : KasiHttpClient, IConsulHttpClient
{
    public ConsulHttpClient(HttpClient client, HttpClientOptions options, IHttpClientSerializer serializer,
        ICorrelationContextFactory correlationContextFactory, ICorrelationIdFactory correlationIdFactory)
        : base(client, options, serializer, correlationContextFactory, correlationIdFactory)
    {
    }
}