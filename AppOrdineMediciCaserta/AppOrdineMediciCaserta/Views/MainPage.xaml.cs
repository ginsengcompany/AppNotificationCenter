using AppOrdineMediciCaserta.Models;
using AppOrdineMediciCaserta.ModelViews;
using AppOrdineMediciCaserta.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppOrdineMediciCaserta
{
    public partial class MainPage : ContentPage
    {

        private MainPageModelView z;
        public MainPage()
        {
            InitializeComponent();
            z = new MainPageModelView();
            BindingContext = z;
        }

        private void listEventi_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var x = e.SelectedItem as DatiEvento;
            z.displayButtons(x);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VisualizzaEventoInDettaglio(z.dettaglio()));
        }
    }
}
