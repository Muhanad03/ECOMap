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
            if(Settings.IsUserLoggedIn ==true)
            {
                BeforeAddingQuestions.IsVisible = true;
                AfterAddingQuestions.IsVisible = false;
            }
            else
            {
                AfterAddingQuestions.IsVisible = true;
                BeforeAddingQuestions.IsVisible = false;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Settings.IsUserLoggedIn == true)
            {
                BeforeAddingQuestions.IsVisible = true;
                AfterAddingQuestions.IsVisible = false;
            }
            else
            {
                AfterAddingQuestions.IsVisible = true;
                BeforeAddingQuestions.IsVisible = false;
            }
        }
        private async void OnYesClicked(object sender, EventArgs e)
        {
            
            Navigation.PushAsync(new AddTreePage());
        }
        

        private async void OnLoginClicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new SignInPage());
        }
        private async void OnNoClicked(object sender, EventArgs e)
        {
            
            await DisplayAlert("No Tree?", "Please move closer to a tree to continue.", "OK");
           
        }
    }
}
