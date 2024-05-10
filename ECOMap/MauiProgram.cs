using Camera.MAUI;
using CommunityToolkit.Maui;
using ECOMap.API;
using Microsoft.Extensions.Logging;

namespace ECOMap
{
    public static class MauiProgram
    {

       public static ApiService _ApiService { get; set; }

        public static MauiApp CreateMauiApp()
        {
            _ApiService = new ApiService();
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCameraView()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Lato-Bold.ttf", "bold");
                    fonts.AddFont("Lato-Regular.ttf", "regular");

                })
                .UseMauiMaps();


            builder.ConfigureMauiHandlers(handlers =>
            {
#if ANDROID || IOS || MACCATALYST
                handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
#endif

            });


#if DEBUG
            builder.Logging.AddDebug();
#endif


            return builder.Build();
        }
    }
}
