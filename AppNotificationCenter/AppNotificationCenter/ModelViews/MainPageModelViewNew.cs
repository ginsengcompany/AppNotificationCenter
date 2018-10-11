using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Database.Models;
using AppNotificationCenter.Models;
using AppNotificationCenter.Services;
using AppNotificationCenter.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppNotificationCenter.ModelViews
{
    class MainPageModelViewNew : INotifyPropertyChanged
    {
        #region OnPropertychanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertychanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Variabili
        private string cultureName = "it-IT";
        private string dettagli = "";
        private string urlweb = "";
        #region Liste
        ObservableCollection<GroupDatiEvento> groupDatiEvento = new ObservableCollection<GroupDatiEvento>();
        private List<DatiEvento> listaEventi = new List<DatiEvento>();
        private List<DatiEvento> listaNote = new List<DatiEvento>();        
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
            #endregion
            #region Variabili Controllo
        private bool isBusy = false;
        private bool isVoidEvent = false; // se la lista non presenta nessun evento rendi visibile la stringa
        private bool _isRefreshing = false;
        private string nessunEvento = "Nessun evento disponibile \n Scorri in basso per aggiornare";
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
                if (IsBusy == false && ListaEventi.Count == 0 && ListaNote.Count==0)
                    IsVoidEvent = true;
                else
                    IsVoidEvent = false;
            }
        }
        public bool IsVoidEvent
        {
            get
            {
                return isVoidEvent;
            }
            set
            {
                isVoidEvent = (value);
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
        #endregion
        #endregion

        #region Command

        public ICommand visualizzaInfo
        {
            get
            {
                return new Command(() =>
                {
                    App.Current.MainPage.Navigation.PushAsync(new PaginaProfiloUtente());
                });
            }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
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
        #endregion
        #region Funzioni
        /*Effettua la connessione per ricevere i dati dal server*/
        public async void leggiDati()
        {
            IsBusy = true;
            ObservableCollection<GroupDatiEvento> groupList = new ObservableCollection<GroupDatiEvento>();
            REST<Object, DatiEvento> connessione = new REST<Object, DatiEvento>();
            var use = LoginData.getUser();
            Utente user = new Utente();
            user.username = use[0].username;
            user.password = use[0].password;
            user.organizzazione = use[0].organizzazione;
            user.token = use[0].token;
            user.eliminato = "false";
            try
            {
                var list = await connessione.PostJsonList(URL.Eventi, user);
                if (list.Count != 0)
                {
                    foreach (var i in list)
                    {
                        CultureInfo culture = new CultureInfo(cultureName);
                        var formaDateTime = Convert.ToDateTime(i.data, culture);
                        i.data = formaDateTime.ToString().Substring(0, 10);
                        if (i.confermato == true)
                            i.TestoButtonEliminato = "ELIMINA";

                        if (i.tipo == "1")
                            listaEventi.Add(i);
                        else if (i.tipo == "2")
                            listaNote.Add(i);

                    }
                    GroupDatiEvento cGroupListEventi = new GroupDatiEvento(listaEventi);
                    cGroupListEventi.Heading = "Eventi";
                    groupList.Add(cGroupListEventi);
                    GroupDatiEvento cGroupListNote = new GroupDatiEvento(listaNote);
                    cGroupListNote.Heading = "Note";
                    groupList.Add(cGroupListNote);
                    IsBusy = false;
                }
                else
                {
                    DatiEvento x = new DatiEvento();
                    x.titolo = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                    NessunEvento = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                    x.VisibleError = "false";
                    listaEventi.Add(x);
                    listaNote.Add(x);
                    IsBusy = false;
                }
                GroupDatiEvento = groupList;
            }
            catch (Exception a)
            {
                DatiEvento x = new DatiEvento();
                x.titolo = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                x.VisibleError = "false";
                NessunEvento = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                listaEventi.Add(x);
                listaNote.Add(x);
                //ListaEventi = listaEventi;
                GroupDatiEvento = groupList;
                IsBusy = false;
            }
        }
        public void displayButtons(DatiEvento x)
        {
            if (x.VisibleError != "false")
            {
                foreach (var i in listaEventi)
                {
                    if (i == x)
                    {
                        if (i.confermato != true)
                        {
                            if (x.url_evento != null)
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
                //GroupDatiEvento = groupDatiEvento;
            }
            if (x.VisibleError != "false")
            {
                foreach (var i in listaNote)
                {
                    if (i == x)
                    {
                        if (i.confermato != true)
                        {
                            if (x.url_evento != null)
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
        #endregion
        #region costruttore
        public MainPageModelViewNew()
        {
            IsBusy = true;
            leggiDati();

        }
        #endregion
    }
}
