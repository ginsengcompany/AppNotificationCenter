using AppOrdineMediciCaserta.Models;
using AppOrdineMediciCaserta.ModelViews;
using AppOrdineMediciCaserta.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.OneSignal;
using Xamarin.Forms;

namespace AppOrdineMediciCaserta
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

        private void ButtonConferma_OnClicked(object sender, EventArgs e)
        {
            z.ConfermaButton(evento);
        }

        private void ButtonElimina_OnClicked(object sender, EventArgs e)
        {
            z.EliminaButton(evento);
        }
    }
}
