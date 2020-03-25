using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITest1
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .InstalledApp("com.companyname.lostandfound")
                    .PreferIdeSettings()
                    .EnableLocalScreenshots()
                    .StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}