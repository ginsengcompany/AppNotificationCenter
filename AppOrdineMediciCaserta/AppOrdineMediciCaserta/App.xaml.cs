using AppOrdineMediciCaserta.Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppOrdineMediciCaserta.Views;
using Xamarin.Forms;

namespace AppOrdineMediciCaserta
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
            Database.Database.Initialize();
            check();
        }

        private void check()
        {
            bool user = checkUser();
            if (user)
                MainPage = new NavigationPage(new MainPage());
            else
                MainPage = new Login();
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
