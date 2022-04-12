using Microsoft.AspNetCore.Authorization;

namespace Kasi_Server.Auth;

public class AuthAttribute : AuthorizeAttribute
{
    public AuthAttribute(string scheme, string policy = "") : base(policy)
    {
        AuthenticationSchemes = scheme;
    }
}