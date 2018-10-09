using AppNotificationCenter.Models;
using AppNotificationCenter.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Database.Models;
using Xamarin.Forms;

namespace AppNotificationCenter.ModelViews
{
    class MainPageModelView : INotifyPropertyChanged
    {
        ObservableCollection<GroupDatiEvento> groupDatiEvento = new ObservableCollection<GroupDatiEvento>();
        public event PropertyChangedEventHandler PropertyChanged;
        private List<DatiEvento> listaEventi = new List<DatiEvento>();
        private List<DatiEvento> listaNote = new List<DatiEvento>();
        private List<Utenza>DatiPersonaliUtente = new List<Utenza>();
        private Utente user = new Utente();
        private string urlScelto;
        private bool isVoidEvent = false;
        private DatiEvento dettagli;
        private string nessunEvento;
        public ImageSource immagine;
        private bool visibileInSeguito=true;
        private String visibile = "false";
        private string token;
        private bool isBusy = false;
        private bool _isRefreshing = false;
        private DateTime formaDateTime;
        private string cultureName = "it-IT";


        public bool IsVoidEvent
        {
            get
            {
                return isVoidEvent ;
            }
            set
            {
                isVoidEvent = (value);
                OnPropertychanged();
     
            }
        }

        public bool VisibileInSeguito
        {
            get
            {
                return visibileInSeguito;
            }
            set
            {
                visibileInSeguito = (value);
                OnPropertychanged();

            }
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

        public List<DatiEvento> ListaNote
        {
            get
            {
                return listaNote;
            }
            set
            {
                listaNote = new List<DatiEvento>(value);
                OnPropertychanged();
            }
        }


        public ObservableCollection<GroupDatiEvento> GroupDatiEvento
        {
            get { return groupDatiEvento; }
            set
            {
                groupDatiEvento = new ObservableCollection<GroupDatiEvento>(value);
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

        public string NessunEvento
        {
            get
            {
                return nessunEvento;
            }
            set
            {
                nessunEvento = value;
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
                    ListaNote.Clear();
                    GroupDatiEvento.Clear();
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
            IsVoidEvent = false;
            ObservableCollection<GroupDatiEvento> groupList = new ObservableCollection<GroupDatiEvento>();
            REST<Object, DatiEvento> connessione = new REST<Object, DatiEvento>();
            List<DatiEvento> List = new List<DatiEvento>();
            var medico = LoginData.getUser();
            user.username = medico[0].username;
            user.token = medico[0].token;
            user.organizzazione = medico[0].organizzazione;
            user.eliminato = "false";
            try
            {
                List = await connessione.PostJsonList(URL.Eventi, user);
                if (List.Count != 0)
                {
                    foreach (var i in List)
                    {
                        
                        if (i.tipo == "1")
                        {
                            CultureInfo culture = new CultureInfo(cultureName);
                            //i.data = i.data.Substring(0, 10);
                            formaDateTime = Convert.ToDateTime(i.data,culture);
                            i.data = formaDateTime.ToString().Substring(0, 10);
                            string img = "";
                            if (!String.IsNullOrEmpty(i.immagine))
                            {
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
                            }

                            if (i.confermato == true)
                                i.TestoButtonEliminato = "ELIMINA";
                            listaEventi.Add(i);
                        }
                    }
                    GroupDatiEvento cGroupListEventi = new GroupDatiEvento(listaEventi);
                    cGroupListEventi.Heading = "Eventi";
                    groupList.Add(cGroupListEventi);
                    foreach (var i in List)
                    {
                        if (i.tipo == "2")
                        {
                            string img = "";
                            if (!String.IsNullOrEmpty(img))
                            {
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
                            }

                            if (i.confermato == true)
                                i.TestoButtonEliminato = "ELIMINA";
                            listaNote.Add(i);
                        }
                    }
                    GroupDatiEvento cGroupListNote = new GroupDatiEvento(listaNote);
                    cGroupListNote.Heading = "Note";
                    groupList.Add(cGroupListNote);
                    //ListaEventi = listaEventi;
                    GroupDatiEvento = groupList;
                    IsBusy = false;
                }
                else
                {
                    DatiEvento x = new DatiEvento();
                    x.titolo = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                    IsVoidEvent = true;
                    NessunEvento= "Nessun evento disponibile \n Scorri in basso per aggiornare";
                    x.VisibleError = "false";
                    listaEventi.Add(x);
                    listaNote.Add(x);
                }
                //ListaEventi = listaEventi;
                GroupDatiEvento = groupList;
                IsBusy = false;

            }
            catch (Exception a)
            {
                DatiEvento x = new DatiEvento();
                x.titolo = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                x.VisibleError = "false";
                IsVoidEvent = true;
                NessunEvento = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                listaEventi.Add(x);
                listaNote.Add(x);
                //ListaEventi = listaEventi;
                GroupDatiEvento = groupList;
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
            urlScelto = x.url_evento;
            if (dettagli.VisibleError != "false")
            {
                foreach (var i in listaEventi)
                {
                    if (i == x)
                    {
                        if (i.confermato != true)
                        {
                            if (urlScelto != null)
                            {
                                i.Visible = "true";
                                i.VisibileInfo = "true";
                                i.VisibileInSeguito = "true";
                                i.VisibleWeb = "true";
                            }
                            else
                            {
                                i.Visible = "true";
                                i.VisibileInfo = "true";
                                i.VisibileInSeguito = "true";
                                i.VisibleWeb = "false";
                            }
                          

                        }
                        else
                        {
                            i.VisibileInfo = "true";
                            i.VisibileInSeguito = "true";

                        }
                    }
                    else
                    {
                        i.VisibileInfo = "false";
                        i.Visible = "false";
                        i.VisibileInSeguito = "false";
                        i.VisibleWeb = "false";
                    }
                }
                //ListaEventi = listaEventi;
                GroupDatiEvento = groupDatiEvento;
            }
            if (dettagli.VisibleError != "false")
            {
                foreach (var i in listaNote)
                {
                    if (i == x)
                    {
                        if (i.confermato != true)
                        {
                            if (urlScelto != null)
                            {
                                i.Visible = "false";
                                i.VisibileInfo = "true";
                                i.VisibileInSeguito = "false";
                                i.VisibleWeb = "true";
                            }
                            else
                            {
                                i.Visible = "false";
                                i.VisibileInfo = "true";
                                i.VisibileInSeguito = "false";
                                i.VisibleWeb = "false";
                            }
                        }
                        else
                        {
                            i.VisibileInfo = "true";
                            i.VisibileInSeguito = "false";
                        }
                    }
                    else
                    {
                        i.VisibileInfo = "false";
                        i.Visible = "false";
                        i.VisibileInSeguito = "false";
                        i.VisibleWeb = "false";
                    }
                }
                //ListaEventi = listaEventi;
                GroupDatiEvento = groupDatiEvento;
            }
        }

        public async void VaiPaginaWeb()
        {
            Device.OpenUri(new Uri(urlScelto));
        }

        public async Task<bool> ConfermaButton(DatiEvento x)
        {
            REST<Object, bool> connessione = new REST<Object, bool>();
            x.confermato = true;
            x.eliminato = false;
            x.organizzazione = user.organizzazione;
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
            x.organizzazione = user.organizzazione;
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
