using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Maps;
namespace ECOMap
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(51.74171, -2.21926),Distance.FromMiles(10)));

        }
        

        private async void SetMap()
        {
            PermissionStatus result = await CheckAndRequestLocationPermission();

            if (result == PermissionStatus.Granted)
            {
                var locationRequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                Location location = await Geolocation.GetLocationAsync(locationRequest);

                if (location != null)
                {
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMiles(0.5)));

                }

            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            SetMap();


        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }






        async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            return status;
        }

    }
}