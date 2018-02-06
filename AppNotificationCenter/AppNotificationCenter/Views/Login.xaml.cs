using AppNotificationCenter.ModelViews;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppNotificationCenter.Views
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
            if ((z._OrganizzazioneSelezionata.cod_org == "" && entryMatricola.Text == "") || (z._OrganizzazioneSelezionata.cod_org != "" && entryMatricola.Text == "") || (z._OrganizzazioneSelezionata.cod_org == "" && entryMatricola.Text != "")) 
            {
                await DisplayAlert("Attenzione", "Inserire tutti i campi!", "Ok");
            }
            else
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
                    await DisplayAlert("Attenzione", "Connessione non riuscita", "Riprova!");
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
                    await DisplayAlert("Attenzione", "Connessione non riuscita", "Riprova!");
                    await Load.ProgressTo(0, 2, Easing.Linear);
                }
            }
        }
    }
}