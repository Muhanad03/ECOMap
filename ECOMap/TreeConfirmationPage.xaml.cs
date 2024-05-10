using ECOMap.config;
using Microsoft.Maui.Controls;
using System;

namespace ECOMap
{
    public partial class TreeConfirmationPage : ContentPage
    {
        public TreeConfirmationPage()
        {
            InitializeComponent();
            if(Settings.CurrentUser != null)
            {
                BeforeAddingQuestions.IsVisible = true;
            }
            else
            {
                BeforeAddingQuestions.IsVisible = false;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Settings.CurrentUser != null)
            {
                BeforeAddingQuestions.IsVisible = true;
            }
            else
            {
                BeforeAddingQuestions.IsVisible = false;
            }
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
