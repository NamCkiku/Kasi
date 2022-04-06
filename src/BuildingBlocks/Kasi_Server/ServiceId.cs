using System;

namespace Kasi_Server;

internal class ServiceId : IServiceId
{
    public string Id { get; } = $"{Guid.NewGuid():N}";
}