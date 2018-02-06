using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppNotificationCenter.Models;
using AppNotificationCenter.ModelViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppNotificationCenter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaEventiInSeguito : ContentPage
    {
        private PagineEventiInSeguitoModelView z;
        private string token;
        DatiEvento evento = new DatiEvento();

        public PaginaEventiInSeguito()
        {
            InitializeComponent();
           
            z = new PagineEventiInSeguitoModelView(token);
            BindingContext = z;
        }

        private void ListEventi_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            evento = e.Item as DatiEvento;
            z.displayButtons(evento);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VisualizzaEventoInDettaglio(z.dettaglio()));
        }

        private async void ButtonConferma_OnClicked(object sender, EventArgs e)
        {

            bool esito = await z.ConfermaButton(evento);
            if (esito == true)
            {
                await DisplayAlert("CONFERMA", "L' evento è stato confermato", "Ok");
            }
            else
                await DisplayAlert("ERRORE", "Errore", "Ok");
            z.ListaEventiSeguito.Clear();
            z.leggiDati();
        }

        private async void ButtonElimina_OnClicked(object sender, EventArgs e)
        {
            bool esito = await z.EliminaButton(evento);
            if (esito == true)
            {
                await DisplayAlert("Evento", "L' evento è stato eliminato", "Ok");
            }
            else
                await DisplayAlert("ERRORE", "Errore", "Ok");
            z.ListaEventiSeguito.Clear();
            z.leggiDati();
        }
    }
}
