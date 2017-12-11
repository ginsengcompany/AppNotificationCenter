using AppOrdineMediciCaserta.Models;
using AppOrdineMediciCaserta.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppOrdineMediciCaserta.ModelViews
{
    class MainPageModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<DatiEvento> listaEventi = new List<DatiEvento>();

        private DatiEvento dettagli;

        private String visibile = "false";

        /* Setta la lista da visualizare nel Binding*/
        public List<DatiEvento> ListaEventi
        {
            get
            {
                return listaEventi;
            }
            set
            {
                listaEventi = new List<DatiEvento>(value);
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
            ListaDatiEvento List = new ListaDatiEvento();
            List = await connessione.getJsonObject(URL.Eventi);
            foreach (var i in List.data)
            {
                listaEventi.Add(i);
            }
            ListaEventi = listaEventi;
        }
        /*Costruttore del metodo, avvia la connessione*/
        public MainPageModelView()
        {

            leggiDati();
        }

        public void displayButtons(DatiEvento x)
        {
            dettagli = x;
            foreach (var i in listaEventi)
            {
                if (i == x)
                    i.Visible = "true";
                else
                    i.Visible = "false";
            }
            ListaEventi = listaEventi;
        }

        public DatiEvento dettaglio()
        {
            return dettagli;
        }

    }
}
