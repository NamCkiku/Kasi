using Kasi_Server.SignalR.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Kasi_Server.SignalR
{
    public static class Extensions
    {
        private const string SectionName = "signalr";

        public static IKasi_ServerBuilder AddSignalRService(this IKasi_ServerBuilder builder)
        {
            var restEaseOptions = builder.GetOptions<SignalrOptions>(SectionName);
            if (restEaseOptions != null && restEaseOptions.Enable)
            {
                builder.Services.AddSingleton(typeof(IConnectionMapping<string>), typeof(ConnectionMapping<string>));
                builder.Services.AddSignalR();
            }
            return builder;
        }
    }
}