using AppOrdineMediciCaserta.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.OneSignal;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppOrdineMediciCaserta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private LoginModelView z;
        private bool access;

        public Login()
        {
            InitializeComponent();
            z = new LoginModelView();
            BindingContext = z;

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Load.IsVisible = true;
            Load.IsEnabled = true;
            await Load.ProgressTo(1, 1500, Easing.SinIn);
            try
            {
                 access = await z.login();
            }
            catch (Exception)
            {
               await DisplayAlert("Attenzione", "connessione non riuscita", "riprova");
            }
            if (access == true)
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        App.Current.MainPage = new NavigationPage(new ListaEventiIoS());
                        break;
                    default:
                        App.Current.MainPage = new NavigationPage(new MainPage());
                        break;
                }

            }
            else
            {
                await DisplayAlert("Attenzione", "connessione non riuscita", "riprova");
            }

        }

    }
}