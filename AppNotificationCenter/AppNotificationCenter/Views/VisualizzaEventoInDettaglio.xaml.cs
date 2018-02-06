using AppNotificationCenter.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppNotificationCenter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisualizzaEventoInDettaglio : ContentPage
    {
         DatiEvento evento;

        public VisualizzaEventoInDettaglio(DatiEvento x)
        {
            InitializeComponent();
            evento = x;
            BindingContext = new ModelViews.VisualizzaEventiInDettaglioModelView(evento);

        }

       
    }
}