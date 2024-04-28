using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Maps;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ECOMap
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadPolygonFromFile();


            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(51.74171, -2.21926),Distance.FromMiles(10)));
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (accessType == NetworkAccess.Internet)
                {
                    SetMap();
                }
            }catch (Exception ex) 
            {
                Debug.WriteLine("Network error: "+ex.Message);
            }
            
      
        }


        private void LoadPolygonFromFile()
        {
            try
            {
                // Read the contents of the text file
                string filePath = "Resources/Charlton Kings Area.txt";
                string fileContents = File.ReadAllText(filePath);

                // Parse the polygon boundary coordinates
                List<Tuple<double, double>> polygonCoordinates = ParsePolygonCoordinates(fileContents);

                // Create a polygon using the parsed coordinates
                AddPolygonToMap(polygonCoordinates);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Error loading polygon: {ex.Message}");
            }
        }


        private List<Tuple<double, double>> ParsePolygonCoordinates(string fileContents)
        {
            List<Tuple<double, double>> polygonCoordinates = new List<Tuple<double, double>>();

            // Remove unnecessary text and split the coordinates
            string[] parts = fileContents.Replace("POLYGON ((", "").Replace("))", "").Split(',');

            foreach (string part in parts)
            {
                // Split each coordinate pair and parse latitude and longitude
                string[] coordinates = part.Trim().Split(' ');
                double latitude = double.Parse(coordinates[0]);
                double longitude = double.Parse(coordinates[1]);

                polygonCoordinates.Add(new Tuple<double, double>(latitude, longitude));
            }

            return polygonCoordinates;
        }

        private void AddPolygonToMap(List<Tuple<double, double>> polygonCoordinates)
        {
            // Create a polygon using the parsed coordinates
            Polygon polygon = new Polygon
            {
                StrokeWidth = 2,
                StrokeColor = Color.Red,
                FillColor = Color.FromRgba(255, 0, 0, 128)
            };

            // Add the coordinates to the polygon
            foreach (var coordinate in polygonCoordinates)
            {
                polygon.Geopath.Add(new Position(coordinate.Item1, coordinate.Item2));
            }

            // Add the polygon to the map
            map.Polygons.Add(polygon);
        }

        private async void UpdatePins()
        {

            var treeDataList = await MauiProgram._ApiService.GetTreeDataAsync();

            foreach (var tree in treeDataList)
            {
                Pin pin = new Pin
                {
                    Label = tree.id.ToString(),
                    Address = tree.planter_Name,
                    Type = PinType.SavedPin,
                    Location = new Location(tree.longitude, tree.latitude),
                };
                map.Pins.Add(pin);
            }

        }
        private async Task<bool> CheckPins()
        {
            try
            {
                var treeDataList = await MauiProgram._ApiService.GetTreeDataAsync();

                var tempList = treeDataList.Select(tree => new Pin
                {
                    Label = tree.id.ToString(),
                    Address = tree.planter_Name,
                    Type = PinType.SavedPin,
                    Location = new Location(tree.longitude, tree.latitude),
                }).ToList();

                var currentPinsList = map.Pins.ToList();

                if (tempList.Count != currentPinsList.Count)
                    return false;

                foreach (var pin in currentPinsList)
                {
                    if (!tempList.Any(tempPin =>
                        tempPin.Location.Latitude == pin.Location.Latitude &&
                        tempPin.Location.Longitude == pin.Location.Longitude))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error checking pins: {e.Message}");
                return false;
            }
        }

        private async void SetMap()
        {
            PermissionStatus result = await CheckAndRequestLocationPermission();

            if (result == PermissionStatus.Granted)
            {
                var locationRequest = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(2));
                Location? location = await Geolocation.GetLocationAsync(locationRequest);

                var treeDataList = await MauiProgram._ApiService.GetTreeDataAsync();

                foreach (var treeData in treeDataList)
                {
                    Pin pin = new Pin
                    {
                        Label = treeData.id.ToString(),
                        Address = "",
                        Type = PinType.SavedPin,
                        Location = new Location(treeData.longitude, treeData.latitude),
                    };

                    pin.MarkerClicked += Pin_InfoWindowClicked;
                    map.Pins.Add(pin);
                }

                if (location != null)
                {
                    map.IsVisible = true;
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMiles(0.1)));
                }
                else
                {
                    await DisplayAlert("Can't detect your current location", "Try restarting the app", "OK");
                }
            }
        }

        private async void Pin_InfoWindowClicked(object sender, EventArgs e)
        {
            Pin pin = (Pin)sender;
            // Display a small note with a button to show more
            string note = $"Tree ID: {pin.Label}\n {pin.Location.ToString()}";
            bool result = await DisplayAlert("Tree Information", note, "Yes", "No");

            if (result)
            {
                // Handle the button click to show more details
                // You can implement your logic here to navigate to another page or display more information
            }
        }



        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var check = await CheckPins();

            if (check == false)
            {
                UpdatePins();
            }
           




        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }
       







        async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
            {
                map.IsEnabled = true;


                return status;
            }

            else if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
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