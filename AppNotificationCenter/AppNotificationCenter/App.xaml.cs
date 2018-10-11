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

        private async void check()
        {
            if (checkUser())
            {
                var utente = LoginData.getUser();
                Utente user = new Utente();
                user.username = utente[0].username;
                user.password = utente[0].password;
                user.token = utente[0].token;
                user.organizzazione = utente[0].organizzazione;
                user.eliminato = "false";
                REST<Utente, Final> rest = new REST<Utente, Final>();
                var response = await rest.PostJson(URL.Login, user);
                if (response.status)
                {
                    if (response.final[0].attivo == false)
                    {

                        await App.Current.MainPage.DisplayAlert("Login", "Utenza Scaduta", "OK");
                        LoginData.dropUser(new TbLogin(user.username, user.password, user.token, user.organizzazione));
                        UtenzaData.DropUser(new TbUtente(response.final[0]));
                        MainPage = new NavigationPage(new Login());
                    }
                    else
                    {
                        //await App.Current.MainPage.DisplayAlert("Login", "Login Effettuata con successo", "OK");
                        response.final[0].organizzazione = user.organizzazione;
                        LoginData.updateUser(new TbLogin(user.username, user.password, user.token, user.organizzazione));
                        UtenzaData.UpdateUser(new TbUtente(response.final[0]));
                        MainPage = new NavigationPage(new MainPage());
                    }

                }
                else
                {

                    LoginData.dropUser(new TbLogin(user.username, user.password, user.token, user.organizzazione));
                    UtenzaData.DropUser(new TbUtente(response.final[0]));
                    MainPage = new NavigationPage(new Login());
                }
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
