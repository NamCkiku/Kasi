using System.Threading.Tasks;

namespace Kasi_Server.Auth;

public interface IAccessTokenService
{
    Task<bool> IsCurrentActiveToken();
    Task DeactivateCurrentAsync();
    Task<bool> IsActiveAsync(string token);
    Task DeactivateAsync(string token);
}