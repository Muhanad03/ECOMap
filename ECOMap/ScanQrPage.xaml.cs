using Camera.MAUI;

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
       
        CameraRestart();
    }
    protected override async void OnDisappearing()
    {
       
    }
    
    //private void CameraOnLoad(object sender, EventArgs e)
    //{
    //    if(Camera.Cameras.Count > 0)
    //    {
    //        Camera.Camera = Camera.Cameras.First();
    //        MainThread.BeginInvokeOnMainThread(async () =>
    //        {
    //            await Camera.StopCameraAsync();
    //            await Camera.StartCameraAsync();
    //        });
    //    }

    //}
    private void CameraRestart()
    {
        if (Camera.Cameras.Count > 0)
        {
            Camera.Camera = Camera.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Camera.StopCameraAsync();
                await Camera.StartCameraAsync();
            });
        }

    }
    private void Camera_BarCodeDetected(object sender,Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            TitleLabel.Text = args.Result[0].Text;
        });
    }
}