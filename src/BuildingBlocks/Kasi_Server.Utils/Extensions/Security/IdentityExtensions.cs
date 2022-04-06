using Kasi_Server.Utils.Helpers;
using System.Security.Claims;
using System.Security.Principal;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class IdentityExtensions
    {
        public static string GetValue(this IIdentity identity, string type)
        {
            Check.NotNull(identity, nameof(identity));
            if (!(identity is ClaimsIdentity claimsIdentity))
            {
                return null;
            }

            return claimsIdentity.FindFirst(type)?.Value ?? string.Empty;
        }

        public static T GetValue<T>(this IIdentity identity, string type)
        {
            var result = identity.GetValue(type);
            return result.IsEmpty() ? default(T) : Conv.To<T>(result);
        }

        public static string[] GetValues(this IIdentity identity, string type)
        {
            Check.NotNull(identity, nameof(identity));
            if (!(identity is ClaimsIdentity claimsIdentity))
            {
                return null;
            }

            return claimsIdentity.Claims.Where(x => x.Type == type).Select(x => x.Value).ToArray();
        }

        public static void RemoveClaim(this IIdentity identity, string claimType)
        {
            Check.NotNull(identity, nameof(identity));
            if (!(identity is ClaimsIdentity claimsIdentity))
            {
                return;
            }

            var claim = claimsIdentity.FindFirst(claimType);
            if (claim == null)
            {
                return;
            }

            claimsIdentity.RemoveClaim(claim);
        }
    }
}