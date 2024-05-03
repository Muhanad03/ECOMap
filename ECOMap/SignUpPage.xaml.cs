using ECOMap.API;
using ECOMap.config;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace ECOMap;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
	}
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        //Navigation.PopAsync();
        Navigation.RemovePage(this);

    }
    private bool IsValidEmail(string email)
    {
        string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        return Regex.IsMatch(email, emailPattern);
    }
    private async void registerBtn_Clicked(object sender, EventArgs e)
    {
        var Email = Email_Entry.Text;
        var First_Name = FirstName_Entry.Text;
        var Last_Name = LastName_Entry.Text;
        var Password1 = Password_Entry.Text;
        var Password2 = Password_Entry2.Text;


        if (string.IsNullOrWhiteSpace(Email)&& string.IsNullOrWhiteSpace(First_Name)&& string.IsNullOrWhiteSpace(Last_Name)
           && string.IsNullOrWhiteSpace(Password1)&& string.IsNullOrWhiteSpace(Password2))
        {
            DisplayAlert("Error", "Please fill your information.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            DisplayAlert("Error", "Please enter your email address.", "OK");
            return;
        }
        if (!IsValidEmail(Email))
        {
            DisplayAlert("Error", "Please enter a valid email address.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(First_Name))
        {
             DisplayAlert("Error", "Please enter your first name.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Last_Name))
        {
             DisplayAlert("Error", "Please enter your last name.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Password1))
        {
             DisplayAlert("Error", "Please enter a password.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(Password2))
        {
            DisplayAlert("Error", "Please enter a password.", "OK");
            return;
        }


         Email = Email_Entry.Text.Trim();
         First_Name = FirstName_Entry.Text.Trim();
         Last_Name = LastName_Entry.Text.Trim();
         Password1 = Password_Entry.Text.Trim();
         Password2 = Password_Entry2.Text.Trim();

        if (Password1 != Password2)
        {
             DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        if (Password1.Length < 8)
        {
             DisplayAlert("Error", "Password must be at least 8 characters long.", "OK");
            return;
        }

        var HashedPassword = Settings.hashPassword(Password1);
        var newUser = new userData()
        {
            email = Email,
            password = HashedPassword,
            first_Name = First_Name,
            last_Name = Last_Name,


        };

        var t = await MauiProgram._ApiService.RegisterNewUser(newUser);

        try
        {
            var responseObject = JObject.Parse(t);

            if (responseObject["status"] != null && (int)responseObject["status"] == 201)
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
                    await DisplayAlert("Loggedin", "yes", "ok");
                }
            }
        }catch (Exception ex)
        {
            await DisplayAlert("Error", "User already registered", "OK");
            Console.WriteLine($"Error: {ex.Message}");
        }
       


    }


}

