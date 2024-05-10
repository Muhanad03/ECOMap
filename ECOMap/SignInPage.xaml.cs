using ECOMap.API;
using ECOMap.config;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;

namespace ECOMap
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if(Settings.IsUserLoggedIn == true)
            {
                Navigation.PopAsync();
            }

        }

        private void ShowPassword(object? sender, CheckedChangedEventArgs e)
        {
            Password_Entry.IsPassword = !Password_Entry.IsPassword;
        }

        private void CreateAccount_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());

        }

        private async void loginBtn_Clicked(object sender, EventArgs e)
        {
            var email = Email_Entry.Text;
            var password = Password_Entry.Text;
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
            {
                email = email.Trim();
                password = password.Trim();
                password = Settings.hashPassword(password);
                var user = new API.userLoginDetails()
                {
                    email = email,
                    password = password,
                };

                var t = await MauiProgram._ApiService.checkLogin(user);

                try
                {
                    var responseObject = JObject.Parse(t);

                    if (responseObject["status"] != null && (int)responseObject["status"] == 200)
                    {
                        var userDataObject = responseObject["data"];

                        if (userDataObject != null)
                        {
                            userData user1 = new userData
                            {
                                id = int.Parse(userDataObject["User_ID"].ToString()),
                                first_Name = userDataObject["First_Name"].ToString(),
                                last_Name = userDataObject["Last_Name"].ToString(),
                                email = userDataObject["Email"].ToString(),
                                user_Type = userDataObject["User_Type"].ToString(),
                            };

                            Settings.CurrentUser = user1;
                            Settings.IsUserLoggedIn = true;
                            Navigation.PopAsync();
                            await DisplayAlert("Login successful", "Login", "ok");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "User not found", "OK");
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

    }
}
