﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Microsoft.JSInterop;
using Gig.Platform.Web.Interfaces;
using Gig.Platform.Web.Services.Special_services;

namespace Gig.Platform.Web.Components.Pages
{
    public class SecurePageBase : ComponentBase, IDisposable
    {
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected ILocationService LocationService { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected TranslationService TranslationService { get; set; }

        protected ClaimsPrincipal User { get; private set; }
        private bool _isNavigationHandled;
        private bool _isLocationFetched;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!_isLocationFetched)
                {
                    await FetchLocationAsync();
                }

                if (!_isNavigationHandled)
                {
                    await HandleAuthenticationAsync();
                }
            }
        }

        private async Task FetchLocationAsync()
        {
            try
            {
                var locationJson = await JSRuntime.InvokeAsync<string>("getCurrentLocation");
                var location = System.Text.Json.JsonSerializer.Deserialize<Location>(locationJson);

                if (location != null)
                {
                    LocationService.SetUserLocation(location.Latitude, location.Longitude);
                    _isLocationFetched = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching location: {ex.Message}");
            }
        }

        private async Task HandleAuthenticationAsync()
        {
            _isNavigationHandled = true;

            AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;

            await FetchUserAsync();

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/auth");
            }
        }

        private async Task FetchUserAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            User = authState.User;
            StateHasChanged();
        }

        private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            Task.Run(async () =>
            {
                try
                {
                    User = (await task).User;
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in OnAuthenticationStateChanged: {ex.Message}");
                }
            });
        }

        public void Dispose()
        {
            AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }
    }
}
