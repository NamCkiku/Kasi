namespace Kasi_Server.Common.RestEase;

public interface IRestEaseServiceBuilder
{
    IRestEaseServiceBuilder WithName(string name);

    IRestEaseServiceBuilder WithScheme(string scheme);

    IRestEaseServiceBuilder WithHost(string host);

    IRestEaseServiceBuilder WithPort(int port);

    RestEaseOptions.Service Build();
}