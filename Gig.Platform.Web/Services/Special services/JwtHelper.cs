using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Gig.Platform.Web.Services.Special_services
{
    public static class JwtHelper
    {
        public static ClaimsPrincipal? ParseToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                return new ClaimsPrincipal(identity);
            }

            return null;
        }
    }
}
