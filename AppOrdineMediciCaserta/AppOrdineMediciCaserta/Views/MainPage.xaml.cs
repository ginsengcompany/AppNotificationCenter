using AppOrdineMediciCaserta.ModelViews;
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
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageModelView();
        }

        private void listEventi_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}
