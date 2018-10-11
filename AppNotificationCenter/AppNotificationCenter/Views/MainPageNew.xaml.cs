using AppNotificationCenter.Models;
using AppNotificationCenter.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppNotificationCenter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageNew : ContentPage
    {
        private MainPageModelViewNew z;
        private string token;
        DatiEvento evento = new DatiEvento();
        public MainPageNew()
        {
            InitializeComponent();
            z = new MainPageModelViewNew();
            BindingContext = z;
        }
        private void ListEventi_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            evento = e.Item as DatiEvento;
            z.displayButtons(evento);

        }
    }
}