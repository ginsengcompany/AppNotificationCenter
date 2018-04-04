using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppNotificationCenter.Annotations;
using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Models;
using AppNotificationCenter.Services;
using Xamarin.Forms;

namespace AppNotificationCenter.ModelViews
{
   public  class PagineEventiInSeguitoModelView: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private List<DatiEvento> listaEventiSeguito = new List<DatiEvento>();
        private String visibile = "false";
        private DatiEvento dettagli;
        private Utente user = new Utente();
        private string token;
        ImageSource immagine;
        private bool isBusy = false;
        private bool _isRefreshing = false;

        public PagineEventiInSeguitoModelView(string token)
        {
            this.token = token;
            IsBusy = true;
            leggiDati();
        }
        
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = (value);
                OnPropertyChanged();
            }
        }
        public List<DatiEvento> ListaEventiSeguito
        {
            get
            {
                return listaEventiSeguito;
            }
            set
            {
                listaEventiSeguito = new List<DatiEvento>(value);
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsRefreshing = true;

                    listaEventiSeguito.Clear();

                    leggiDati();

                    IsRefreshing = false;
                });
            }
        }

        public async void leggiDati()
        {
            REST<Object, DatiEvento> connessione = new REST<Object, DatiEvento>();
            List<DatiEvento> List = new List<DatiEvento>();
            var medico = LoginData.getUser();
            user.username = medico[0].username;
            user.token = medico[0].token;
            user.eliminato = "true";
            
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

                        listaEventiSeguito.Add(i);
                    }
                    ListaEventiSeguito = listaEventiSeguito;
                    IsBusy = false;
                }
                else
                {
                    DatiEvento x = new DatiEvento();
                    x.titolo = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                    x.VisibleError = "false";
                    listaEventiSeguito.Add(x);
                }
                ListaEventiSeguito = listaEventiSeguito;
                IsBusy = false;

            }
            catch (Exception a)
            {
                DatiEvento x = new DatiEvento();
                x.titolo = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                x.VisibleError = "false";
                listaEventiSeguito.Add(x);
                ListaEventiSeguito = listaEventiSeguito;
                IsBusy = false;
            }
        }


        public void displayButtons(DatiEvento x)
        {
            dettagli = x;
            if (dettagli.VisibleError != "false")
            {
                foreach (var i in listaEventiSeguito)
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
                ListaEventiSeguito = listaEventiSeguito;
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




        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

     
    }
}

