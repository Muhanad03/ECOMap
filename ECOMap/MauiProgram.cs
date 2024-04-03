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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiMaps();

#if DEBUG
    		builder.Logging.AddDebug();
#endif


            return builder.Build();
        }
    }
}
