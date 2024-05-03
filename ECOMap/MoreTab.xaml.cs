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

       
        protected override void OnAppearing()
        {
            base.OnAppearing();

           


        }

        private void PageNameLabel_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignInPage()); 
        }
    }
}

