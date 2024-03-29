using Android.App;
using Android.Runtime;

namespace ECOMap
{
    [Application]
    [MetaData("com.google.android.geo.API_KEY",
            Value = "AIzaSyB1RJMmRK6AXl9lcK7tcKz69i3FNifsGFc")]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
