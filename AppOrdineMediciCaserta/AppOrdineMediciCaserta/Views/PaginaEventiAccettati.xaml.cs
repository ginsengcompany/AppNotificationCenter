using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppOrdineMediciCaserta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaEventiAccettati : ContentPage
    {
        public PaginaEventiAccettati()
        {
            InitializeComponent();
        }

        private void ListEventiNonAccetati_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}