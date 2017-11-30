using AppOrdineMediciCaserta.Models;
using AppOrdineMediciCaserta.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppOrdineMediciCaserta.ModelViews
{
    class MainPageModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<ListaDatiEvento> listaEventi = new List<ListaDatiEvento>();

        private String visibile = "false";

        /* Setta la lista da visualizare nel Binding*/
        public List<ListaDatiEvento> ListaEventi
        {
            get
            {
                return listaEventi;
            }
            set
            {
                listaEventi = new List<ListaDatiEvento>(value);
                OnPropertychanged();
            }
        }

        public string Visibile
        {
            get
            {
                return visibile;
            }
            set
            {
                visibile = "false";
                OnPropertychanged();
            }
        }
        private void OnPropertychanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /*Effettua la connessione per ricevere i dati dal server*/
        public async void leggiDati()
        {
            REST<Object, ListaDatiEvento> connessione = new REST<Object, ListaDatiEvento>();
            ListaEventi = await connessione.GetJson(URL.Eventi);
        }
        /*Costruttore del metodo, avvia la connessione*/
        public MainPageModelView()
        {
           leggiDati();
           
        }

    }
}
