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
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignInPage());
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
          
        }
    }

}

