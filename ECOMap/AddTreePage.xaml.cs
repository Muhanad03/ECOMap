using ECOMap.API;
using ECOMap.Services;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace ECOMap;

public partial class AddTreePage : ContentPage
{
    ObservableCollection<ImageSource> imageSources = new ObservableCollection<ImageSource>();

    double Longitude, Latitude;
    public AddTreePage()
    {
        InitializeComponent();
        imagesCollection.ItemsSource = imageSources;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        InitializeLocation();

    }
    private async void InitializeLocation()
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
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                if (location != null)
                {
                    // Update your model or UI here with the location data
                    Longitude = location.Longitude;
                    Latitude = location.Latitude;
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
                    if (imageSources.Count < 5) // Limit to 5 images
                    {
                        var stream = await imageFile.OpenReadAsync();

                        // Ensure the stream is reset to the beginning
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            memoryStream.Position = 0;

                            // Display image in the preview
                            imageSources.Add(ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray())));

                            // Reset position for base64 conversion
                            memoryStream.Position = 0;
                            var base64Image = await ConvertStreamToBase64(memoryStream);
                            base64Images.Add(base64Image);
                        }
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
                tree_type = TreeType_Entry.Text,
                longitude = Longitude,
                latitude = Latitude,
                height = double.Parse(Height_Entry.Text),
                circumference = double.Parse(Circumference_Entry.Text),
                plant_Age = Age_Picker.SelectedItem.ToString(),
                comment = Comment_Entry.Text,
                addedByUser_ID = 23 // Assuming a fixed user ID for demonstration
            };

            string response = await new ApiService().AddTreeDataAsync(treeData);

            try
            {
                var responseObject = JObject.Parse(response);

                if (responseObject["status"] != null && (int)responseObject["status"] == 201)
                {
                    var userDataObject = responseObject["data"];
                    if (userDataObject != null && userDataObject["Tree_ID"] != null)
                    {
                        var treeId = int.Parse(userDataObject["Tree_ID"].ToString());
                        await UploadImages(treeId); // Call method to upload images
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Failed to add tree data. Server response: " + response, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unexpected error: " + ex.Message + "\nServer response: " + response, "OK");
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
        foreach (var base64Image in base64Images)
        {
            var imageData = new imageData
            {
                tree_id = treeId,
                base64 = base64Image,
                user_id = 23
            };

            string imageResponse = await new ApiService().PostImage(imageData);
           
        }

        Navigation.PopAsync();
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
