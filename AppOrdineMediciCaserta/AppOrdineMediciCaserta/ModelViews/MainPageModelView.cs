using AppOrdineMediciCaserta.Models;
using AppOrdineMediciCaserta.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppOrdineMediciCaserta.Database.Data;
using AppOrdineMediciCaserta.Database.Models;
using Xamarin.Forms;
using Com.OneSignal;

namespace AppOrdineMediciCaserta.ModelViews
{
    class MainPageModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<DatiEvento> listaEventi = new List<DatiEvento>();

        private DatiEvento dettagli;
        private Medico user = new Medico();
        private string token;
        ImageSource immagine;
        private bool isBusy = false;


        private String visibile = "false";

        private bool _isRefreshing = false;

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = (value);
                OnPropertychanged();
            }
        }

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

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertychanged(nameof(IsRefreshing));
            }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command( () =>
                {
                    IsRefreshing = true;

                    ListaEventi.Clear();

                    leggiDati();

                    IsRefreshing = false;
                });
            }
        }

        private void OnPropertychanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /*Effettua la connessione per ricevere i dati dal server*/
        public async void leggiDati()
        {
            REST<Object, DatiEvento> connessione = new REST<Object, DatiEvento>();
            List<DatiEvento> List = new List<DatiEvento>();
            var medico = LoginData.getUser();
            user.matricola = medico[0].matricola;
            user.token = medico[0].token;
            try
            {
                List = await connessione.PostJsonList(URL.Eventi, user);
                if (List.Count != 0)
                {
                    foreach (var i in List)
                    {
                        string img = "";
                        if (i.immagine.Contains("jpeg;"))
                        {
                            img = i.immagine.Substring(23);
                        }
                        else
                        {
                            img = i.immagine.Substring(22);
                        }
                        immagine = Xamarin.Forms.ImageSource.FromStream(
                            () => new MemoryStream(Convert.FromBase64String(img)));
                        i.Immagine = immagine;
                        if (i.confermato == true)
                            i.TestoButtonEliminato = "ELIMINA";

                        listaEventi.Add(i);
                    }
                    ListaEventi = listaEventi;
                    IsBusy = false;
                }
                else
                {
                    DatiEvento x = new DatiEvento();
                    x.titolo = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                    x.VisibleError = "false";
                    listaEventi.Add(x);
                }
                ListaEventi = listaEventi;
                IsBusy = false;

            }
            catch (Exception a)
            {
                DatiEvento x = new DatiEvento();
                x.titolo = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                x.VisibleError = "false";
                listaEventi.Add(x);
                ListaEventi = listaEventi;
                IsBusy = false;
            }

        }
        /*Costruttore del metodo, avvia la connessione*/
        public MainPageModelView(string token)
        {
            this.token = token;
            IsBusy = true;
            leggiDati();
            
        }

        public void displayButtons(DatiEvento x)
        {
            dettagli = x;
            if (dettagli.VisibleError != "false")
            {
                foreach (var i in listaEventi)
                {
                    if (i == x)
                    {
                        if (i.confermato != true)
                            i.Visible = "true";

                        i.VisibileInfo = "true";

                    }
                    else
                    {
                        i.VisibileInfo = "false";
                        i.Visible = "false";
                    }
                        
                }
                ListaEventi = listaEventi;
            }
            
        }

        public async Task<bool> ConfermaButton(DatiEvento x)
        {
            REST<Object, bool> connessione = new REST<Object, bool>();
            x.confermato = true;
            x.eliminato = false;
            x.immagine = null;
            x.Immagine = null;
            bool esito = await connessione.PostJson(URL.ConfermaElimina, x);
            return esito;
        }

        public async Task<bool> EliminaButton(DatiEvento x)
        {
            REST<Object, bool> connessione = new REST<Object, bool>();
            x.confermato = false;
            x.eliminato = true;
            x.immagine = null;
            x.Immagine = null;
            bool esito = await connessione.PostJson(URL.ConfermaElimina, x);
            return esito;
        }

        public DatiEvento dettaglio()
        {
            return dettagli;
        }
      

    }
}
