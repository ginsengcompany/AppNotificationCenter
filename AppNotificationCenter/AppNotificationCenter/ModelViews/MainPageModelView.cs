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
using AppNotificationCenter.Views;
using Xamarin.Forms;
using System.Linq;
using Plugin.Connectivity;

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
                    IsBusy = true;
                   // GroupDatiEvento.Clear();
                    leggiDati();
                    IsBusy = false;
                    IsRefreshing = false;
                });
            }
        }

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

            if (CrossConnectivity.Current.IsConnected)
            {
                List = await connessione.PostJsonList(URL.Eventi, user);
                if (List == null)
                    {
                        NessunEvento = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                        IsVoidEvent = true;
                    }
                else
                    {
                        if (List.Count != 0)
                        {
                            foreach (var i in List)
                            {
                                    CultureInfo culture = new CultureInfo(cultureName);
                                    formaDateTime = Convert.ToDateTime(i.data, culture);
                                    i.data = formaDateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    i.data_ordinamento = formaDateTime;
                                    string img = "";
                                try
                                {
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
                                }
                                catch (Exception e)
                                {
                                    i.Immagine = "";
                                }

                                if (i.confermato == true)
                                        i.TestoButtonEliminato = "ELIMINA";

                                if (i.tipo=="1")
                                    listaEventi.Add(i);

                                else if(i.tipo=="2")
                                    listaNote.Add(i);
                            }

                            ListaEventi = ListaEventi.OrderByDescending(o => o.data_ordinamento).ToList();
                            GroupDatiEvento cGroupListEventi = new GroupDatiEvento(listaEventi);
                            cGroupListEventi.Heading = "Eventi";
                            groupList.Add(cGroupListEventi);
                            

                            listaNote = listaNote.OrderByDescending(o => o.data_ordinamento).ToList();
                            GroupDatiEvento cGroupListNote = new GroupDatiEvento(listaNote);
                            cGroupListNote.Heading = "Note";
                            groupList.Add(cGroupListNote);
                            //ListaEventi = listaEventi;
                            GroupDatiEvento = groupList;

                            IsBusy = false;
                        }
                        else
                        {
                            DatiEvento evento = new DatiEvento();
                            evento.titolo = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                            IsVoidEvent = true;
                            NessunEvento = "Nessun evento disponibile \n Scorri in basso per aggiornare";
                            evento.VisibleError = "false";
                            listaEventi.Add(evento);
                            listaNote.Add(evento);
                        }

                        //ListaEventi = listaEventi;
                        GroupDatiEvento = groupList;
                        IsBusy = false;
                    }
                }
                else
                {
                    DatiEvento evento = new DatiEvento();
                    evento.titolo = "Nessun evento \n Scorri in basso per aggiornare";
                    evento.VisibleError = "false";
                    IsVoidEvent = true;
                    NessunEvento = "Nessun evento \n Scorri in basso per aggiornare";
                    listaEventi.Add(evento);
                    listaNote.Add(evento);
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

        public async void displayButtons(DatiEvento eventoSelezionato)
        {
            dettagli = eventoSelezionato;
            urlScelto = eventoSelezionato.url_evento;
            if (dettagli.VisibleError != "false")
            {
                foreach (var i in listaEventi)
                {
                    if (i == eventoSelezionato)
                    {
                        if (i.confermato != true)
                        {
                            if (urlScelto != null)
                            {
                                var actionSheet = await App.Current.MainPage.DisplayActionSheet("Attenzione", "Cancel", null, "Conferma", "Declina", "Dettagli", "Sito web");
                                switch (actionSheet)
                                {
                                    case "Conferma":
                                        await ConfermaButton(eventoSelezionato);
                                        break;
                                    case "Declina":
                                        await EliminaButton(eventoSelezionato);
                                        break;
                                    case "Dettagli":
                                        await App.Current.MainPage.Navigation.PushAsync(new VisualizzaEventoInDettaglio(dettaglio()));
                                        break;
                                    case "Sito web":
                                        VaiPaginaWeb();
                                        break;
                                    case "Cancel":
                                        break;
                                }
                                return;

                            }
                            else
                            {
                                var actionSheet = await App.Current.MainPage.DisplayActionSheet("Attenzione", "Cancel", null, "Conferma", "Declina", "Dettagli");
                                switch (actionSheet)
                                {
                                    case "Conferma":
                                        await ConfermaButton(eventoSelezionato);
                                        break;
                                    case "Declina":
                                        await EliminaButton(eventoSelezionato);
                                        break;
                                    case "Dettagli":
                                        await App.Current.MainPage.Navigation.PushAsync(new VisualizzaEventoInDettaglio(dettaglio()));
                                        break;
                                    case "Cancel":
                                        break;
                                }
                                return;
                            }
                          

                        }
                        else
                        {
                            var actionSheet = await App.Current.MainPage.DisplayActionSheet("Attenzione", "Cancel", null,  "Declina", "Dettagli", "Sito web");
                            switch (actionSheet)
                            {
                               
                                case "Declina":
                                    await EliminaButton(eventoSelezionato);
                                    break;
                                case "Dettagli":
                                    await App.Current.MainPage.Navigation.PushAsync(new VisualizzaEventoInDettaglio(dettaglio()));
                                    break;
                                case "Sito web":
                                    VaiPaginaWeb();
                                    break;
                                case "Cancel":
                                    break;
                            }
                            return;

                        }
                    }
                }
                //ListaEventi = listaEventi;
                //GroupDatiEvento = groupDatiEvento;
            }
            if (dettagli.VisibleError != "false")
            {
                foreach (var i in listaNote)
                {
                    if (i == eventoSelezionato)
                    {
                        if (i.confermato != true)
                        {
                            if (urlScelto != null)
                            {
                               
                                var actionSheet = await App.Current.MainPage.DisplayActionSheet("Attenzione", "Cancel", null,  "Dettagli", "Sito web");
                                switch (actionSheet)
                                {
                                    
                                    case "Dettagli":
                                        await App.Current.MainPage.Navigation.PushAsync(new VisualizzaEventoInDettaglio(dettaglio()));
                                        break;
                                    case "Sito web":
                                        VaiPaginaWeb();
                                        break;
                                    case "Cancel":
                                        break;
                                }
                                return;
                            }
                            else
                            {

                                var actionSheet = await App.Current.MainPage.DisplayActionSheet("Attenzione", "Cancel", null,  "Dettagli");
                                switch (actionSheet)
                                {
                                    case "Dettagli":
                                        await App.Current.MainPage.Navigation.PushAsync(new VisualizzaEventoInDettaglio(dettaglio()));
                                        break;
                                    case "Cancel":
                                        break;
                                } return;
                            }
                           
                        }
                        else
                        {
                            var actionSheet = await App.Current.MainPage.DisplayActionSheet("Attenzione", "Cancel", null, "Dettagli");
                            switch (actionSheet)
                            {
                              
                                case "Dettagli":
                                    await App.Current.MainPage.Navigation.PushAsync(new VisualizzaEventoInDettaglio(dettaglio()));
                                    break;
                                case "Cancel":
                                    break;
                            }
                            return;
                        }
                    }
                
                }
                //ListaEventi = listaEventi;
                //GroupDatiEvento = groupDatiEvento;
            }
        }

        public async void VaiPaginaWeb()
        {
            if (urlScelto.Contains("http"))
                Device.OpenUri(new Uri(urlScelto));
            else
               await App.Current.MainPage.DisplayAlert("Attenzione", "url non valido", "ok");
        }

        public async Task ConfermaButton(DatiEvento eventoConfermato)
        {
            REST<Object, bool> connessione = new REST<Object, bool>();
            eventoConfermato.confermato = true;
            eventoConfermato.eliminato = false;
            eventoConfermato.organizzazione = user.organizzazione;
            eventoConfermato.immagine = null;
            eventoConfermato.Immagine = null;
            var medico = LoginData.getUser();
            IsBusy = true;
            bool esito = await connessione.PostJson(URL.ConfermaElimina, eventoConfermato);
            if (esito == true)
            {
                await App.Current.MainPage.DisplayAlert("CONFERMA", "L'evento è stato confermato", "Ok");
            }
            else
                await App.Current.MainPage.DisplayAlert("Attenzione", "Connessione non riuscita riprovare", "Ok");
            ListaEventi.Clear();
            ListaNote.Clear();
            GroupDatiEvento.Clear();
            leggiDati();
            IsBusy = false;
        }

        public async Task EliminaButton(DatiEvento eventoDeclinato)
        {
            REST<Object, bool> connessione = new REST<Object, bool>();
            eventoDeclinato.confermato = false;
            eventoDeclinato.eliminato = true;
            eventoDeclinato.organizzazione = user.organizzazione;
            eventoDeclinato.immagine = null;
            eventoDeclinato.Immagine = null;
            IsBusy = true;
            bool esito = await connessione.PostJson(URL.ConfermaElimina, eventoDeclinato);
            if (esito)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione", "L'evento è stato declinato", "Ok");
            }
            else
                await App.Current.MainPage.DisplayAlert("Attenzione", "Connessione non riuscita, riprovare", "Ok");
            ListaEventi.Clear();
            ListaNote.Clear();
            GroupDatiEvento.Clear();
            leggiDati();
            IsBusy = false;
        }

        public DatiEvento dettaglio()
        {
            return dettagli;
        }
    }
}
