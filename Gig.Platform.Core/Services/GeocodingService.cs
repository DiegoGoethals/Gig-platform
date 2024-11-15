using Gig.Platform.Core.Entities;
using Newtonsoft.Json;

namespace Gig.Platform.Core.Services
{
    public class GeocodingService
    {
        private readonly HttpClient _httpClient;
        private const string UserAgent = "GigFinder/1.0";
        private const string NominatimUrl = "https://nominatim.openstreetmap.org";

        public GeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
        }

        public async Task<(double? latitude, double? longitude)> GeocodeAddressAsync(string street, string houseNumber, string postalCode, string city)
        {
            try
            {
                string address = $"{street} {houseNumber}, {postalCode} {city}";
                string apiUrl = $"{NominatimUrl}/search?format=json&q={Uri.EscapeDataString(address)}";

                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic results = JsonConvert.DeserializeObject(json);

                    if (results != null && results.Count > 0)
                    {
                        var resultObject = results[0];
                        if (double.TryParse((string)resultObject["lat"], out double latitude) &&
                            double.TryParse((string)resultObject["lon"], out double longitude))
                        {
                            return (latitude, longitude);
                        }
                    }
                }
                return (null, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error geocoding address: {ex.Message}");
                return (null, null);
            }
        }

        public async Task<GeocodedAddress> ReverseGeocodeAsync(double latitude, double longitude)
        {
            try
            {
                string apiUrl = $"{NominatimUrl}/reverse?lat={latitude}&lon={longitude}&zoom=18&format=jsonv2";

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                    var response = await httpClient.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(json);

                        if (result != null && result.address != null)
                        {
                            var geocodedAddress = new GeocodedAddress
                            {
                                StreetName = result.address.road ?? "",
                                HouseNumber = result.address.house_number ?? "",
                                PostalCode = result.address.postcode ?? "",
                                City = result.address.city ?? result.address.village ?? ""
                            };

                            return geocodedAddress;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reverse geocoding coordinates: {ex.Message}");
                return null;
            }
        }
    }
}
