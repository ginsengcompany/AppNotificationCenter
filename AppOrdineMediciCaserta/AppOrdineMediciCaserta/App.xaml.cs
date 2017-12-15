using AppOrdineMediciCaserta.Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppOrdineMediciCaserta.Views;
using Xamarin.Forms;
using Com.OneSignal;
using System.Diagnostics;

namespace AppOrdineMediciCaserta
{
    public partial class App : Application
    {

        private const string APP_ID_ONE_SIGNAL = "b560b667-aa97-4980-a740-c8fc7925e208";

        public App()
        {
            InitializeComponent();
            Database.Database.Initialize();
            check();
        }

        private void check()
        {
            var user = checkUser();
            if (user)
                MainPage = new NavigationPage(new MainPage());
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
