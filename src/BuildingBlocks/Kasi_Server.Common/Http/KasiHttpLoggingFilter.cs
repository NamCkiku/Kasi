using System;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Kasi_Server.Common.HTTP;

// Credits goes to https://www.stevejgordon.co.uk/httpclientfactory-asp-net-core-logging
internal sealed class KasiHttpLoggingFilter : IHttpMessageHandlerBuilderFilter
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly HttpClientOptions _options;

    public KasiHttpLoggingFilter(ILoggerFactory loggerFactory, HttpClientOptions options)
    {
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _options = options;
    }

    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        if (next is null)
        {
            throw new ArgumentNullException(nameof(next));
        }

        return builder =>
        {
            next(builder);
            var logger = _loggerFactory.CreateLogger($"System.Net.Http.HttpClient.{builder.Name}.LogicalHandler");
            builder.AdditionalHandlers.Insert(0, new KasiLoggingScopeHttpMessageHandler(logger, _options));
        };
    }
}