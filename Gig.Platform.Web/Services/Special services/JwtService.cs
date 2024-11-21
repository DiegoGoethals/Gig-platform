using Microsoft.JSInterop;

namespace Gig.Platform.Web.Services.Special_services
{
    public class JwtService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string TokenKey = "authToken";

        public JwtService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SaveTokenAsync(string token)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", TokenKey, token);
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", TokenKey);
        }

        public async Task RemoveTokenAsync()
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", TokenKey);
        }
    }
}
