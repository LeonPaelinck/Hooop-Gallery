using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project3H04.Client.Shared {
    public class ArrayClaimsPrincipalFactory<TAccount> : AccountClaimsPrincipalFactory<TAccount> where TAccount : RemoteUserAccount {
        public ArrayClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor) : base(accessor) { }

        // when a user belongs to multiple roles, IS4 returns a single claim with a serialised array of values
        // this class improves the original factory by deserializing the claims in the correct way
        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(TAccount account, RemoteAuthenticationUserOptions options) {
            var user = await base.CreateUserAsync(account, options);
            var claimsIdentity = (ClaimsIdentity)user.Identity;

            if (account == null)
                return user;

            foreach (var kvp in account.AdditionalProperties) {
                var name = kvp.Key;
                var value = kvp.Value;

                if (value == null || (value is not JsonElement element || element.ValueKind != JsonValueKind.Array))
                    continue;

                claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(kvp.Key));

                var claims = element.EnumerateArray()
                    .Select(x => new Claim(kvp.Key, x.ToString()));

                claimsIdentity.AddClaims(claims);
            }

            return user;
        }
    }
}