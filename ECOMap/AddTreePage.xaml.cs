using ECOMap.API;
using ECOMap.config;
using ECOMap.Services;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;


namespace ECOMap;

public partial class AddTreePage : ContentPage
{
    ObservableCollection<ImageSource> imageSources = new ObservableCollection<ImageSource>();

    double Longitude, Latitude;
    public AddTreePage()
    {
        InitializeComponent();
        InitializeLocation();
        Height_Picker.SelectedIndex = 1;
        Circumference_Picker.SelectedIndex = 0;

        imagesCollection.ItemsSource = imageSources;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();


    }
    private async Task InitializeLocation()
    {

        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                var locationRequest = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                Location? location = await Geolocation.GetLocationAsync(locationRequest);
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(5) // Increased timeout
                    });
                }
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(0.5)
                    });
                }

                if (location != null)
                {
                    // Update your model or UI here with the location data
                    Longitude = location.Longitude;
                    Latitude = location.Latitude;
                    Debug.WriteLine($"Long:{Longitude}" + Longitude);
                }
            }
            else
            {
                await DisplayAlert("Location Permission", "Permission to access location was denied.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Location Error", "Unable to get location: " + ex.Message, "OK");
        }
    }

    private List<string> base64Images = new List<string>();
    private async Task<string> ConvertStreamToBase64(Stream stream)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            await stream.CopyToAsync(ms);
            byte[] imageBytes = ms.ToArray();
            return Convert.ToBase64String(imageBytes);
        }
    }
    private async void OnSelectImageButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var results = await FilePicker.PickMultipleAsync(new PickOptions
            {
                PickerTitle = "Select up to 5 images",
                FileTypes = FilePickerFileType.Images,
            });

            if (results != null)
            {
                foreach (var imageFile in results)
                {

                    using (var stream = await imageFile.OpenReadAsync())
                    {
                        var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);

                        memoryStream.Position = 0;
                        var imageSource = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));
                        imageSources.Add(imageSource);

                        memoryStream.Position = 0;
                        var base64Image = await ConvertStreamToBase64(memoryStream);
                        base64Images.Add(base64Image);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to pick images: " + ex.Message, "OK");
        }
    }

    private async void OnTakePhotoButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                using (var stream = await photo.OpenReadAsync())
                {
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    memoryStream.Position = 0;
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));
                    imageSources.Add(imageSource);

                    memoryStream.Position = 0;
                    var base64Image = await ConvertStreamToBase64(memoryStream);
                    base64Images.Add(base64Image);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to capture photo: " + ex.Message, "OK");
        }
    }


    private async void Confirm_Clicked(object sender, EventArgs e)
    {
        if (ValidateInputs())
        {
            var treeData = new treeData
            {
                // Assuming additional properties are properly bound or set
                tree_type = TreeType_Entry.Text,
                longitude = Longitude,
                latitude = Latitude,
                height = double.Parse(Height_Entry.Text),
                circumference = double.Parse(Circumference_Entry.Text),
                plant_Age = Age_Picker.SelectedItem.ToString(),
                comment = Comment_Entry.Text,
                addedByUser_ID = Settings.CurrentUser.id 
            };

            string response = await new ApiService().AddTreeDataAsync(treeData);

            try
            {
                var responseObject = JObject.Parse(response);

                if (responseObject["status"] != null && (int)responseObject["status"] == 201)
                {
                    var userDataObject = responseObject["Tree_ID"];
                    if (userDataObject != null)
                    {
                        var treeId = int.Parse(userDataObject.ToString());
                        await UploadImages(treeId); // Call method to upload images
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Failed to add tree data.", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unexpected error: " + ex.Message, "OK");
                return;
            }

            await DisplayAlert("Success", "Data Submitted Successfully", "OK");
        }
        else
        {
            await DisplayAlert("Validation Error", "Please check your inputs and try again.", "OK");
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Navigation.PopAsync();
    }
    private async Task UploadImages(int treeId)
    {
        foreach (var imageSource in base64Images)
        {
            var image = imageSource;
            var imageData = new imageData
            {
                tree_id = treeId,
                base64 = image,
                user_id = 23
            };

            string imageResponse = await new ApiService().PostImage(imageData);
            var page = (MainPage)App.Current.MainPage;
            page.UpdatePins();
            Navigation.PopAsync();
        }
    }


    private bool ValidateInputs()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(TreeType_Entry.Text))
        {
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(Height_Entry.Text) || Height_Picker.SelectedItem == null)
        {
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(Circumference_Entry.Text) || Circumference_Picker.SelectedItem == null)
        {
            isValid = false;
        }

        if (Age_Picker.SelectedItem == null)
        {
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(Comment_Entry.Text))
        {
            isValid = false;
        }

        return isValid;
    }
}
