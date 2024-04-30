using Microsoft.Maui.Controls;

namespace ECOMap
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();

        }

        private void ShowPassword(object? sender, CheckedChangedEventArgs e)
        {

        }

        private void CreateAccount_Clicked(object sender, EventArgs e)
        {
             Navigation.PushAsync(new SignUpPage());

        }

        private async void loginBtn_Clicked(object sender, EventArgs e)
        {
            var email = Email_Entry.Text.Trim();
            var password = Password_Entry.Text.Trim();
            if (email.Length>0 && password.Length > 0)
            {
                var user = new API.userLoginDetails()
                {
                    email = email,
                    password = password,
                };

                var t = await MauiProgram._ApiService.checkLogin(user);
           

            }
        }
    }
}
