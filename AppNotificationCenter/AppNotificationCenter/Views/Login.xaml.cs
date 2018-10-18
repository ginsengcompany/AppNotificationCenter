using AppNotificationCenter.Models;
using AppNotificationCenter.ModelViews;
using Com.OneSignal;
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
                if (access)
                {

                    App.Current.MainPage = new NavigationPage(new MainPage());
                    
                }
                else
                {
                    await Load.ProgressTo(0, 2, Easing.Linear);
                }
            }
        }
    }
}