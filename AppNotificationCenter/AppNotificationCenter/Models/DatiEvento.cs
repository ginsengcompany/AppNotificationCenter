using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Services;
using AppNotificationCenter.Views;
using Newtonsoft.Json.Converters;
using Xamarin.Forms;

namespace AppNotificationCenter.Models
{
    public class DatiEvento
    {
        public string _id_utente { get; set; }
        public string _id_evento { get; set; }
        public string organizzazione { get; set; }
        public string url_evento { get; set; }
        public bool confermato { get; set; }
        public bool eliminato { get; set; }
        public string titolo { get; set; }
        public string sottotitolo { get; set; }
        public string data { get; set; }
        public string dat_fine { get; set; }
        public DateTime data_fine { get; set; }
        public DateTime data_ordinamento { get; set; }
        public string luogo { get; set; }
        public string informazioni { get; set; }
        public string relatori { get; set; }
        public string descrizione { get; set; }
        public string immagine { get; set; }
        public string tipo { get; set; }
        public string Visible { get; set; } = "false";
        public string VisibileInfo { get; set; } = "false";
        public string VisibleError { get; set; } = "true";
        public string VisibileInSeguito { get; set; } = "false";
        public string VisibleWeb { get; set; } = "false";
        public ImageSource Immagine { get; set; }
        public string TestoButtonEliminato { get; set; } = "ELIMINA";
   
    }
}
