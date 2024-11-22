using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace Gig.Platform.Web.Components.Pages
{
    public class SecurePageBase : ComponentBase, IDisposable
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

                AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;

                await FetchUserAsync();

                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    NavigationManager.NavigateTo("/auth");
                }
            }
        }

        private async Task FetchUserAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            User = authState.User;
            StateHasChanged();
        }

        private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            User = (await task).User;
            StateHasChanged();
        }

        public void Dispose()
        {
            AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }
    }
}
