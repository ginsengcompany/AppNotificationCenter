using AppNotificationCenter.Models;
using AppNotificationCenter.ModelViews;
using AppNotificationCenter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.OneSignal;
using Xamarin.Forms;

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

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VisualizzaEventoInDettaglio(z.dettaglio()));
        }

        private void ListEventi_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            evento = e.Item as DatiEvento;
            z.displayButtons(evento);

        }

        private async void ButtonConferma_OnClicked(object sender, EventArgs e)
        {
            
           bool esito = await z.ConfermaButton(evento);
            if(esito==true)
            {
              await  DisplayAlert("CONFERMA", "L'evento è stato confermato", "Ok");
            }
            else
                await DisplayAlert("Attenzione", "L'evento è stato declinato", "Ok");
            z.ListaEventi.Clear();
            z.ListaNote.Clear();
            z.GroupDatiEvento.Clear();
            z.leggiDati();
        }

        private async void BtnInSeguito_OnClicked(object sender, EventArgs e)
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
        }
    }
}
