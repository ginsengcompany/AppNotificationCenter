using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Models;
using AppNotificationCenter.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppNotificationCenter.ModelViews
{
    class VisualizzaEventiInDettaglioModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string titolo,sottotitolo, data, luogo, informazioni, relatori, descrizione;
        private bool enablePartecipo = true;
        private bool visiblePartecipo = false;
        ImageSource immagine;
        public DatiEvento dettagliEvento;

        public VisualizzaEventiInDettaglioModelView(DatiEvento evento)
        {
            this.dettagliEvento = evento;
            if (dettagliEvento.tipo == "1")
                VisiblePartecipo = true;
            else
                VisiblePartecipo = false;
            inserimentoDati();
            
        }
        
        private void OnPropertychanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }
        public bool EnablePartecipo
        {
            get { return enablePartecipo; }
            set
            {
                OnPropertychanged();
                enablePartecipo = value;
            }
        }
        public bool VisiblePartecipo
        {
            get { return visiblePartecipo; }
            set
            {
                OnPropertychanged();
                visiblePartecipo = value;
            }
        }
        public string Titolo
        {
            set
            {
                titolo = value;
                OnPropertychanged();
            }
            get
            {
                return titolo;
            }

        }
        public ICommand LinkSito
        {
            get
            {
                return new Command(() =>
                {
                    VaiPaginaWeb();
                });
            }
        }
        public ICommand Partecipo
        {
            get
            {
                return new Command(async() =>
                {
                   await ConfermaButton();
                });
            }
        }
        public ICommand Declino
        {
            get
            {
                return new Command(() =>
                {
                    EliminaButton();
                });
            }
        }

        public void Reindirizzamento()
        {
            Device.OpenUri(new Uri(Informazioni));
        }
        public string Sottotitolo
        {
            set
            {
                sottotitolo = value;
                OnPropertychanged();
            }
            get
            {
                return sottotitolo;
            }
        }
        public string Data
        {
            set
            {
                data = value;
                OnPropertychanged();
            }
            get
            {
                return data;
            }
        }
        public string Luogo
        {
            set
            {
                luogo = value;
                OnPropertychanged();
            }
            get
            {
                return luogo;
            }
        }
        public string Informazioni
        {
            set
            {
                informazioni = value;
                OnPropertychanged();
            }
            get
            {
                return informazioni;
            }
        }
        public string Relatori
        {
            set
            {
                relatori = value;
                OnPropertychanged();
            }
            get
            {
                return relatori;
            }
        }
        public string Descrizione
        {
            set
            {
                descrizione = value;
                OnPropertychanged();
            }
            get
            {
                return descrizione;
            }
        }
        public ImageSource Immagine
        {
            set
            {
                immagine = value;
                OnPropertychanged();
            }
            get
            {
                return immagine;
            }
        }


        public async void VaiPaginaWeb()
        {
            if (dettagliEvento.url_evento.Contains("http"))
                Device.OpenUri(new Uri(dettagliEvento.url_evento));
            else
                await App.Current.MainPage.DisplayAlert("Attenzione", "url non valido", "ok");
        }

        public async Task ConfermaButton()
        {
            var medico = LoginData.getUser();
            REST<Object, bool> connessione = new REST<Object, bool>();
            dettagliEvento.confermato = true;
            dettagliEvento.eliminato = false;
            dettagliEvento.organizzazione = medico[0].organizzazione;
            dettagliEvento.immagine = null;
            bool esito = await connessione.PostJson(URL.ConfermaElimina, dettagliEvento);
            if (esito == true)
            {
                await App.Current.MainPage.DisplayAlert("CONFERMA", "Complimenti la partecipazione è andata a buon fine", "Ok");
                App.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
                await App.Current.MainPage.DisplayAlert("Attenzione", "Connessione non riuscita riprovare", "Ok");

        }

        public async Task EliminaButton()
        {
            var medico = LoginData.getUser();
            REST<Object, bool> connessione = new REST<Object, bool>();
            dettagliEvento.confermato = false;
            dettagliEvento.eliminato = true;
            dettagliEvento.organizzazione = medico[0].organizzazione;
            dettagliEvento.immagine = null;
            bool esito = await connessione.PostJson(URL.ConfermaElimina, dettagliEvento);
            if (esito)
            {
                if(dettagliEvento.tipo=="1")
                    await App.Current.MainPage.DisplayAlert("Attenzione", "L'evento è stato declinato", "Ok");
                else
                    await App.Current.MainPage.DisplayAlert("Attenzione", "La nota è stata eliminata", "Ok");

                App.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
                await App.Current.MainPage.DisplayAlert("Attenzione", "Connessione non riuscita, riprovare", "Ok");
      
        }


        public void inserimentoDati()
        {
            if(dettagliEvento.confermato)
            {
                EnablePartecipo = false;
            }

                Titolo = dettagliEvento.titolo;
                Sottotitolo = dettagliEvento.sottotitolo;
            if (string.IsNullOrEmpty(dettagliEvento.relatori))
                Relatori = "Non disponibile";
            else
                Relatori = dettagliEvento.relatori;

            if (string.IsNullOrEmpty(dettagliEvento.luogo))
                Luogo = "Non disponibile";
            else
                Luogo = dettagliEvento.luogo;

            if (string.IsNullOrEmpty(dettagliEvento.informazioni))
                Informazioni = "Non disponibile";
            else
                Informazioni = dettagliEvento.informazioni;

            Descrizione = dettagliEvento.descrizione;
            string img = "";
            if(dettagliEvento.immagine!=null)
            {
                if (dettagliEvento.immagine.Contains("jpeg;"))
                {
                    img = dettagliEvento.immagine.Substring(23);
                }
                else if (dettagliEvento.immagine.Contains("png;") || dettagliEvento.immagine.Contains("jpg;"))
                {
                    img = dettagliEvento.immagine.Substring(22);
                }
            }
                Immagine = Xamarin.Forms.ImageSource.FromStream(
                    () => new MemoryStream(Convert.FromBase64String(img)));
            
  
        }
    }
}

