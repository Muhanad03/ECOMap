using Microsoft.Maui.Controls;

namespace ECOMap
{
    public partial class MoreTab : ContentPage
    {
        public MoreTab()
        {
            InitializeComponent();
            PopulateListView();
          
        }

        private void PopulateListView()
        {
            var pageNames = new List<PageName>
            {
               new PageName(){Title = "QR Scanner"},
               new PageName(){Title = "Settings"},
               new PageName(){Title = "Info"},
               new PageName(){Title = "Login"},

            };

            PagesListView.ItemsSource = pageNames;
        }

        private async void PagesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var pageName = (PageName)e.SelectedItem;
            var temp = pageName.Title;
            // Navigate to the selected content page
            switch (temp)
            {
                case "QR Scanner":
                    await Navigation.PushAsync(new ScanQrPage());
                    break;
                case "Settings":
                    await Navigation.PushAsync(new SettingsPage());
                    break;

                case "Login":
                    await Navigation.PushAsync(new SignInPage());
                    break;
            }

            // Deselect the item
            PagesListView.SelectedItem = null;
        }
    }
}

public partial class PageName
{
    public string Title { get; set; }   
}
