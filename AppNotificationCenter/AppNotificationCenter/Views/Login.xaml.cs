using AppNotificationCenter.Models;
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
        private Organizzazione org = new Organizzazione();


        public Login()
        {
            InitializeComponent();
            z = new LoginModelView();
            BindingContext = z;
        }

        private void organizzazioneSelected(object sender, EventArgs args)
        {
            var a = sender as Picker;
            if (a.SelectedIndex != -1)
            {
                org = a.SelectedItem as Organizzazione;
                z.organizzazioneScelta(org);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

             if (entryUserName.Text == "" || entryPassword.Text == "") 
            {
                await DisplayAlert("Attenzione", "Completare tutti i campi!", "Ok.");
            }
            else
            {
                Load.IsVisible = true;
                Load.IsEnabled = true;
                await z.organizzazioneScelta();
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
                    await Load.ProgressTo(0, 2, Easing.Linear);
                }
            }
        }
    }
}