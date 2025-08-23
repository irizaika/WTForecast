namespace WTForecast.Services;

public class LocationService
{
    //public async Task<Location?> GetCurrentLocationAsync()
    //{
    //    try
    //    {
    //        var request = new GeolocationRequest(
    //            GeolocationAccuracy.Medium,
    //            TimeSpan.FromSeconds(10));

    //        return await Geolocation.Default.GetLocationAsync(request);
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

    public async Task<Location?> GetLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null)
                return location;
            // After getting the location:
            //var placemarks = await Geocoding.Default.GetPlacemarksAsync(location);

            //if (placemarks != null)
            //{
            //    var placemark = placemarks.FirstOrDefault();
            //    if (placemark != null)
            //    {
            //        var town = placemark.Locality; // or placemark.SubAdminArea
            //        var country = placemark.CountryName;
            //        // LocationLabel.Text = $"Location: {town}, {country} ({lat}, {lon})";
            //    }
            //    else
            //    {
            //        //  LocationLabel.Text = $"Location: {lat}, {lon}";
            //    }
            //}
            //else
            //{
            //    // LocationLabel.Text = $"Location: {lat}, {lon}";
            //}
        }
        catch (FeatureNotSupportedException)
        {
            //await DisplayAlert("Error", "GPS not supported on this device.", "OK");
        }
        catch (PermissionException)
        {
           // await DisplayAlert("Error", "Location permission denied.", "OK");
        }

        return null;
    }
}
