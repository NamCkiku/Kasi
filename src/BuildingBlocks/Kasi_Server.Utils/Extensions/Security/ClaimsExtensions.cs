using System.Security.Claims;

namespace Kasi_Server.Utils.Extensions
{
    public static class ClaimsExtensions
    {
        public static void TryAddClaim(this List<Claim> claims, string type, string value,
            string valueType = ClaimValueTypes.String)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            if (claims.Exists(x => x.Type.Equals(type, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            claims.Add(new Claim(type, value, valueType));
        }
    }
}