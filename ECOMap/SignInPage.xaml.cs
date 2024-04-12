using Microsoft.Maui.Controls;

namespace ECOMap
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();

            isPasswordVisible.CheckedChanged += ShowPassword;
        }

        private void ShowPassword(object? sender, CheckedChangedEventArgs e)
        {
            PasswordEntry.IsPassword = isPasswordVisible.IsChecked ? false : true;
        }
    }
}
