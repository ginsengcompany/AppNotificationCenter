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

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            ImmagineLogo.WidthRequest =  width;
            ImmagineLogo.HeightRequest = height / 2.152; //given that image is 411 x 191
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {

             if (entryUserName.Text == "" || entryPassword.Text == "") 
            {
                await DisplayAlert("Attenzione", "Completare tutti i campi!", "Ok.");
            }
            else
            {
                busyindicator.IsBusy = true;
                busyindicator.IsVisible = true;
                BtnLogin.IsVisible = false;
                z.organizzazioneScelta();
                try
                {
                    access = await z.login();
                }
                catch (Exception)
                {
                    await DisplayAlert("Attenzione", "Connessione non riuscita", "Riprova!");
                    busyindicator.IsBusy = false;
                    busyindicator.IsVisible = false;
                    BtnLogin.IsVisible = true;
                }
                if (access)
                {
                    App.Current.MainPage = new NavigationPage(new MainPage());
                    busyindicator.IsBusy = false;
                    busyindicator.IsVisible = false;
                    BtnLogin.IsVisible = true;
                }
                else
                {
                    busyindicator.IsBusy = false;
                    busyindicator.IsVisible = false;
                    BtnLogin.IsVisible = true;
                }
            }
        }
    }
}