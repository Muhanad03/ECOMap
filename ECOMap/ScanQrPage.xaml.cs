using Camera.MAUI;
using System.Diagnostics;

namespace ECOMap;

public partial class ScanQrPage : ContentPage
{
    public ScanQrPage()
    {
        InitializeComponent();
       
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Initialize and start the camera asynchronously
   
    }

    protected override async void OnDisappearing()
    {
    

        base.OnDisappearing();
    }

    private async Task InitializeCameraAsync()
    {
        if (Camera.Cameras.Count > 0)
        {
            Camera.Camera = Camera.Cameras.First();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                 Camera.StopCameraAsync();
                 Camera.StartCameraAsync();
            });
           
        }
    }

    private void Camera_BarCodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            TitleLabel.Text = args.Result[0].Text;
        });
    }

    private void ScanBtn_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Clicked");
    }

    private void Camera_CamerasLoaded(object sender, EventArgs e)
    {
        if (Camera.Cameras.Count > 0)
        {
            Camera.Camera = Camera.Cameras.First();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Camera.StopCameraAsync();
                Camera.StartCameraAsync();
            });

        }
    }
}
