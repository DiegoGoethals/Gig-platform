using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Gig.Platform.Web.Services.Special_services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly JwtService _jwtService;

        public CustomAuthenticationStateProvider(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _jwtService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var principal = JwtHelper.ParseToken(token);
            return new AuthenticationState(principal ?? new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task NotifyUserAuthentication(string token)
        {
            await _jwtService.SaveTokenAsync(token);
            var principal = JwtHelper.ParseToken(token);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal ?? new ClaimsPrincipal(new ClaimsIdentity()))));
        }

        public async Task LogoutAsync()
        {
            await _jwtService.RemoveTokenAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }
    }
}