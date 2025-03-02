using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace VentilatorAlignerAC
{
    public static class MauiProgram 
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>();
        

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
