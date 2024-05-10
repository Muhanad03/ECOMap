using CommunityToolkit.Mvvm.ComponentModel;
using ECOMap.config;
using Microsoft.Maui.Controls;

namespace ECOMap
{
    public partial class MoreTab : ContentPage
    {
        public MoreTab()
        {
            InitializeComponent();

            if(Settings.IsUserLoggedIn == true)
            {
                LoginBtn.Text = "Log out";
            }


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Settings.IsUserLoggedIn == true)
            {
                LoginBtn.Text = "Log out";
                Requestsbtn.IsVisible = true;
                Profilebtn.IsVisible = true;
            }
            else
            {
                LoginBtn.Text = "Login";
                Profilebtn.IsVisible= false;
                Requestsbtn.IsVisible = false;
            }
        }
        private async void Requests_Clicked(object sender, EventArgs e)
        {

        }
        private async void Login_Clicked(object sender, EventArgs e)
        {
            if(LoginBtn.Text == "Login")
            {
               await Navigation.PushAsync(new SignInPage());
            }
            else
            {
                var result = await DisplayAlert("Logging out", "Are you sure you want to log out", "yes", "no");

                if(result == true)
                {
                    Settings.CurrentUser = null;
                    Settings.IsUserLoggedIn = false;
                    LoginBtn.Text = "Login";
                    Requestsbtn.IsVisible = false;
                    Profilebtn.IsVisible = false;
                }
            }
           
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new SettingsPage());
        }
        
        private async void ScanQR_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScanQrPage());
        }
        private async void Profile_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProfilePage());
        }
    
        private async void TreeGuide_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new TreeGuidePage());
        }

        private async void Help_Clicked(object sender, EventArgs e)
        {
     
        }

        private async void AboutUs_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutUsPage());
        }
    }

}

