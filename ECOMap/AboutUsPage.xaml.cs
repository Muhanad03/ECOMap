namespace ECOMap;

public partial class AboutUsPage : ContentPage
{
	public AboutUsPage()
	{
		InitializeComponent();
	}
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Navigation.PopAsync();
    }
}