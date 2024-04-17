using ECOMap.config;

namespace ECOMap
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();
           
        }
        public void EnableUserLoggedInTabs()
        {
            //LoginTab.IsVisible = false;
        }
        public void DisableUserLoggedInTabs()
        {

        }
    }
}
