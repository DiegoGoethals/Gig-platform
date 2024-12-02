using Gig.Platform.Web.Interfaces;

namespace Gig.Platform.Web.Services
{
    public class LocationService : ILocationService
    {
        public double? Latitude { get; private set; }
        public double? Longitude { get; private set; }
        public bool HasLocation => Latitude.HasValue && Longitude.HasValue;

        public void SetUserLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public (double? Latitude, double? Longitude) GetUserLocation()
        {
            return (Latitude, Longitude);
        }

        public int GetDistance(double latitude, double longitude)
        {
            if (!HasLocation)
            {
                return -1;
            }

            var R = 6371; // Radius of the Earth in km
            var phi1 = latitude * System.Math.PI / 180;
            var phi2 = Latitude.Value * System.Math.PI / 180;
            var deltaPhi = (Latitude.Value - latitude) * System.Math.PI / 180;
            var deltaLambda = (Longitude.Value - longitude) * System.Math.PI / 180;

            // Haversine formula
            var a = System.Math.Sin(deltaPhi / 2) * System.Math.Sin(deltaPhi / 2) +
                    System.Math.Cos(phi1) * System.Math.Cos(phi2) *
                    System.Math.Sin(deltaLambda / 2) * System.Math.Sin(deltaLambda / 2);

            var c = 2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a));

            // Distance in kilometers
            var distance = R * c;

            return (int)distance;
        }
    }
}
