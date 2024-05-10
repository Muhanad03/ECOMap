using ECOMap.config;

namespace ECOMap;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		BindingContext = Settings.CurrentUser;
		InitializeComponent();
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
		Navigation.PopAsync();
    }
}