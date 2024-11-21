using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace Gig.Platform.Web.Components.Pages
{
    public class SecurePageBase : ComponentBase
    {
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        protected ClaimsPrincipal User { get; private set; }
        private bool _isNavigationHandled;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !_isNavigationHandled)
            {
                _isNavigationHandled = true;

                var authState = await AuthStateProvider.GetAuthenticationStateAsync();
                User = authState.User;

                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    NavigationManager.NavigateTo("/auth");
                }
            }
        }
    }
}
