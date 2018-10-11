using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Views;
using Xamarin.Forms;
using Com.OneSignal;
using AppNotificationCenter.Models;
using AppNotificationCenter.Services;
using AppNotificationCenter.Database.Models;

namespace AppNotificationCenter
{
    public partial class App : Application
    {

        private const string APP_ID_ONE_SIGNAL = "b560b667-aa97-4980-a740-c8fc7925e208";

        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjk3NzdAMzEzNjJlMzMyZTMwQXRHYmpuSDdrK1U5bkhzN0E3UFpBaXc1d0JJUTR0SWRYOWdDZzF1OWMrUT0=");
            Database.Database.Initialize();
            check();
        }

        private void check()
        {
            if (checkUser())
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
                MainPage = new Login();
            OneSignal.Current.StartInit(APP_ID_ONE_SIGNAL)
                  .EndInit();
        }

        private bool checkUser()
        {
            int count = LoginData.GetCountUser();
            if (count > 0)
                return true;
            else
                return false;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            OneSignal.Current.IdsAvailable(((string userID, string pushToken) =>
            {
                App.Current.Properties["token"] = userID;
                
            }));
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
