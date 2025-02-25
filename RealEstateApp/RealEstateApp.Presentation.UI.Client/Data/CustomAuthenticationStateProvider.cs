using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RealEstateApp.Presentation.UI.Client.Data
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            Console.WriteLine($"Token from LocalStorage: {token}");

            if (string.IsNullOrWhiteSpace(token))
            {
                return CreateAnonymousState();
            }

            JwtSecurityToken tokenContent;
            try
            {
                // Check if token is well-formed before attempting to parse
                if (!_tokenHandler.CanReadToken(token))
                {
                    Console.WriteLine("Invalid JWT format.");
                    await _localStorage.RemoveItemAsync("authToken");
                    return CreateAnonymousState();
                }

                tokenContent = _tokenHandler.ReadJwtToken(token);
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine($"Token parsing error: {ex.Message}");
                await _localStorage.RemoveItemAsync("authToken");
                return CreateAnonymousState();
            }

            var expiry = tokenContent.ValidTo;
            if (expiry < DateTime.UtcNow)
            {
                Console.WriteLine($"Token expired at: {expiry}");
                await _localStorage.RemoveItemAsync("authToken");
                return CreateAnonymousState();
            }

            var claims = ParseClaims(tokenContent);
            var identity = new ClaimsIdentity(claims, "jwtAuthType");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public void MarkUserAsAuthenticated(string token)
        {
            if (!_tokenHandler.CanReadToken(token))
            {
                Console.WriteLine("Invalid JWT format.");
                return;
            }

            var tokenContent = _tokenHandler.ReadJwtToken(token);
            var claims = ParseClaims(tokenContent);
            var identity = new ClaimsIdentity(claims, "jwtAuthType");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
        }

        private IEnumerable<Claim> ParseClaims(JwtSecurityToken token)
        {
            var claims = token.Claims.ToList();

            // Example: Add a custom claim if necessary
            if (!claims.Any(c => c.Type == "CustomClaim"))
            {
                claims.Add(new Claim("CustomClaim", "CustomValue"));
            }

            // Example: Ensure Name claim exists
            if (!claims.Any(c => c.Type == ClaimTypes.Name))
            {
                claims.Add(new Claim(ClaimTypes.Name, token.Subject ?? "Unknown"));
            }

            return claims;
        }

        private AuthenticationState CreateAnonymousState()
        {
            // Optional: Add custom claims for anonymous users
            var anonymousIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim("IsAnonymous", "true")
            });

            return new AuthenticationState(new ClaimsPrincipal(anonymousIdentity));
        }
    }
}
