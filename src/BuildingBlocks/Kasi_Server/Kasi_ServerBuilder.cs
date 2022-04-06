using Kasi_Server.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Kasi_Server;

public sealed class Kasi_ServerBuilder : IKasi_ServerBuilder
{
    private readonly ConcurrentDictionary<string, bool> _registry = new();
    private readonly List<Action<IServiceProvider>> _buildActions;
    private readonly IServiceCollection _services;
    IServiceCollection IKasi_ServerBuilder.Services => _services;

    public IConfiguration Configuration { get; }

    private Kasi_ServerBuilder(IServiceCollection services, IConfiguration configuration)
    {
        _buildActions = new List<Action<IServiceProvider>>();
        _services = services;
        _services.AddSingleton<IStartupInitializer>(new StartupInitializer());
        Configuration = configuration;
    }

    public static IKasi_ServerBuilder Create(IServiceCollection services, IConfiguration configuration = null)
        => new Kasi_ServerBuilder(services, configuration);

    public bool TryRegister(string name) => _registry.TryAdd(name, true);

    public void AddBuildAction(Action<IServiceProvider> execute)
        => _buildActions.Add(execute);

    public void AddInitializer(IInitializer initializer)
        => AddBuildAction(sp =>
        {
            var startupInitializer = sp.GetRequiredService<IStartupInitializer>();
            startupInitializer.AddInitializer(initializer);
        });

    public void AddInitializer<TInitializer>() where TInitializer : IInitializer
        => AddBuildAction(sp =>
        {
            var initializer = sp.GetRequiredService<TInitializer>();
            var startupInitializer = sp.GetRequiredService<IStartupInitializer>();
            startupInitializer.AddInitializer(initializer);
        });

    public IServiceProvider Build()
    {
        var serviceProvider = _services.BuildServiceProvider();
        _buildActions.ForEach(a => a(serviceProvider));
        return serviceProvider;
    }
}