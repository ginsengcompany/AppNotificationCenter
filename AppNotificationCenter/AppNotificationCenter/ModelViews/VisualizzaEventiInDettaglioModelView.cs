using AppNotificationCenter.Models;
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
        ImageSource immagine;
        public DatiEvento dettagliEvento;

        public VisualizzaEventiInDettaglioModelView(DatiEvento evento)
        {
            this.dettagliEvento = evento;
            inserimentoDati();
        }
        
        private void OnPropertychanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

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
                    Reindirizzamento();
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
        public void inserimentoDati()
        {
            Titolo = dettagliEvento.titolo;
            Sottotitolo = dettagliEvento.sottotitolo;
            Relatori = dettagliEvento.relatori;


            Data = "Data evento: " + dettagliEvento.data.Day.ToString() + "-" + dettagliEvento.data.Month + "-" +
                   dettagliEvento.data.Year;
            Luogo = dettagliEvento.luogo;
            Informazioni = dettagliEvento.informazioni;
            Descrizione = dettagliEvento.descrizione;
            string img = "";
            if (dettagliEvento.immagine.Contains("jpeg;"))
            {
                img = dettagliEvento.immagine.Substring(23);
            }
            else if (dettagliEvento.immagine.Contains("png;") || dettagliEvento.immagine.Contains("jpg;"))
            {
                img = dettagliEvento.immagine.Substring(22);
            }
            Immagine = Xamarin.Forms.ImageSource.FromStream(
           () => new MemoryStream(Convert.FromBase64String(img)));
       
        }
    }
}

