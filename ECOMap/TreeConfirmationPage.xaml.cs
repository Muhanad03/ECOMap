using Microsoft.Maui.Controls;
using System;

namespace ECOMap
{
    public partial class TreeConfirmationPage : ContentPage
    {
        public TreeConfirmationPage()
        {
            InitializeComponent();
        }

        private async void OnYesClicked(object sender, EventArgs e)
        {
            
            Navigation.PushAsync(new AddTreePage());
        }

        private async void OnNoClicked(object sender, EventArgs e)
        {
            
            await DisplayAlert("No Tree?", "Please move closer to a tree to continue.", "OK");
           
        }
    }
}
