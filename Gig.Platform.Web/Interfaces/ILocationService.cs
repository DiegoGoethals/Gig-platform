namespace Gig.Platform.Web.Interfaces
{
    public interface ILocationService
    {
        void SetUserLocation(double latitude, double longitude);
        (double? Latitude, double? Longitude) GetUserLocation();
        int GetDistance(double latitude, double longitude);
    }
}
