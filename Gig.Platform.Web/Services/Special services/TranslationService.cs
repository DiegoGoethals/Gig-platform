namespace Gig.Platform.Web.Services.Special_services
{
    public class TranslationService
    {
        private readonly HttpClient _httpClient;

        public TranslationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> TranslateAsync(string text, string targetLanguage)
        {
            var response = await _httpClient.PostAsJsonAsync("translate", new
            {
                q = text,
                source = "auto",
                target = targetLanguage,
                format = "html"
            });

            var result = await response.Content.ReadFromJsonAsync<LibreTranslateResponse>();
            return result?.TranslatedText ?? text;
        }

        public class LibreTranslateResponse
        {
            public string TranslatedText { get; set; }
        }
    }
}
