using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using UXDivers.Artina.Grial;
using HockeyApp.iOS;

namespace airmily.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Appearance.Configure();

            var manager = BITHockeyManager.SharedHockeyManager;             //Should work
            manager.Configure("1578f0ce30e440b98f4478e239d38dd6");          //Works
            manager.StartManager();
            manager.Authenticator.AuthenticateInstallation(); //Obsolete for crash only builds (assuming we're using it for more) 

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            SQLitePCL.CurrentPlatform.Init();

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App(new iOSInitializer()));



            return base.FinishedLaunching(app, options);
        }
    }
}
