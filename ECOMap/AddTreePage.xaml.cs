using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;

namespace ECOMap;



public partial class AddTreePage : ContentPage
{
    ObservableCollection<ImageSource> imageSources = new ObservableCollection<ImageSource>();

    public AddTreePage()
    {
        InitializeComponent();
        imagesCollection.ItemsSource = imageSources;
    }

    private async void OnSelectImageButtonClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickMultipleAsync(new PickOptions
        {
            PickerTitle = "Select up to 5 images",
            FileTypes = FilePickerFileType.Images,
        });

        if (result != null)
        {
            foreach (var imageFile in result)
            {
                var stream = await imageFile.OpenReadAsync();
                ImageSource source = ImageSource.FromStream(() => stream);
                if (imageSources.Count < 5) // Limit to 5 images
                {
                    imageSources.Add(source);
                }
            }
        }
    }

    private async void OnTakePhotoButtonClicked(object sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.IsCaptureSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                ImageSource source = ImageSource.FromStream(() => stream);
                if (imageSources.Count < 5) 
                {
                    imageSources.Add(source);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Failed to capture photo", ex.Message, "OK");
        }
    }
}
