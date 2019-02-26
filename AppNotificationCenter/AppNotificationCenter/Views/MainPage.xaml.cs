using AppNotificationCenter.Models;
using AppNotificationCenter.ModelViews;
using AppNotificationCenter.Views;
using System.Threading.Tasks;
using Xamarin.Forms;
using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Services;
using AppNotificationCenter.Database.Models;
using Plugin.Connectivity;

namespace AppNotificationCenter
{
    public partial class MainPage : ContentPage
    {

        private MainPageModelView z;
        private string token;
        DatiEvento evento = new DatiEvento();
        public MainPage()
        {
            InitializeComponent();
            z = new MainPageModelView(token);
            BindingContext = z;
        }
        private async Task check()
        {
            var utente = LoginData.getUser();
            Utente user = new Utente();
            foreach(var i in utente)
            {
                if (i.attivo)
                {
                    user.id = i.id;
                    user.attivo = i.attivo;
                    user.username = i.username;
                    user.password = i.password;
                    user.token = i.token;
                    user.organizzazione = i.organizzazione;
                    user.eliminato = "false";
                    user.splash_logo = i.splash_logo;
                    user.circle_logo = i.circle_logo;
                }
            }
            if (string.IsNullOrEmpty(user.username))
            {
                user.id = utente[0].id;
                user.attivo = true;
                user.username = utente[0].username;
                user.password = utente[0].password;
                user.token = utente[0].token;
                user.organizzazione = utente[0].organizzazione;
                user.splash_logo = utente[0].splash_logo;
                user.circle_logo = utente[0].circle_logo;
                user.eliminato = "false";
            }
            REST<Utente, Final> rest = new REST<Utente, Final>();
            if (CrossConnectivity.Current.IsConnected)
            {
                var response = await rest.PostJson(URL.Login, user);
                if (response != null)
                {
                    if (response.status)
                    {
                        if (response.final[0].attivo == false)
                        {

                            await App.Current.MainPage.DisplayAlert("Login", "Utenza non attiva", "OK");
                            LoginData.dropUser(new TbLogin(user.username, user.password, user.token, user.organizzazione,user.circle_logo,user.splash_logo,true));
                            UtenzaData.DropUser(new TbUtente(response.final[0]));
                            App.Current.MainPage = new Login();
                        }
                        else
                        {
                            //await App.Current.MainPage.DisplayAlert("Login", "Login Effettuata con successo", "OK");
                            response.final[0].organizzazione = user.organizzazione;
                            foreach (var i in utente)
                            {
                                i.attivo = false;
                                LoginData.updateUser(i);
                            }
                            TbLogin us = new TbLogin(user.username, user.password, user.token, user.organizzazione, user.circle_logo, user.splash_logo,  user.attivo);
                            us.id = user.id;
                            us.attivo = true;
                            LoginData.updateUser(us);
                            UtenzaData.UpdateUser(new TbUtente(response.final[0]));
                        }

                    }
                    else
                    {
                        LoginData.dropUser(new TbLogin(user.username, user.password, user.token, user.organizzazione, user.circle_logo, user.splash_logo, true));
                        UtenzaData.DropUser(new TbUtente(response.final[0]));
                        App.Current.MainPage = new NavigationPage(new Login());
                    }
                }
                else
                {
                    await DisplayAlert("Attenzione", "Il servizio è momentaneamente non disponibile, riprova più tardi",
                        "OK");
                   await check();
                }
            }

        }

   

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await check();
        }

      /*  private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VisualizzaEventoInDettaglio(z.dettaglio()));
        }*/

        private void ListEventi_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            evento = e.Item as DatiEvento;
            z.displayButtons(evento);
        }

     /*   private async void ButtonConferma_OnClicked(object sender, EventArgs e)
        {
            
           bool esito = await z.ConfermaButton(evento);
            if(esito==true)
            {
              await  DisplayAlert("CONFERMA", "L'evento è stato confermato", "Ok");
            }
            else
                await DisplayAlert("Attenzione", "Connessione non riuscita riprovare", "Ok");
            z.ListaEventi.Clear();
            z.ListaNote.Clear();
            z.GroupDatiEvento.Clear();
            z.leggiDati();
        }*/

      /*  private async void BtnInSeguito_OnClicked(object sender, EventArgs e)
        {
            bool esito = await z.EliminaButton(evento);
            if (esito == true)
            {
                await DisplayAlert("Attenzione", "L'evento è stato declinato", "Ok");
            }
            else
                await DisplayAlert("Attenzione", "Connessione non riuscita, riprovare", "Ok");
            z.ListaEventi.Clear();
            z.ListaNote.Clear();
            z.GroupDatiEvento.Clear();
            z.leggiDati();
        }
        
        private void VaiSitoWeb(object sender, EventArgs e)
        {
            z.VaiPaginaWeb();
        }*/
    }
}
