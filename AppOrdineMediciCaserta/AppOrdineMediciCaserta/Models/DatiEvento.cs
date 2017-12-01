using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppOrdineMediciCaserta.Models
{
    public class ListaDatiEvento
    {
        public List<DatiEvento> rows { get; set; }
    }
    public class DatiEvento
    {
        public string titolo { get; set; }
        public string sottotitolo { get; set; }
        public string data { get; set; }
        public string luogo { get; set; }
        public string informazioni { get; set; }
        public string relatori { get; set; }
        public string descrizione { get; set; }
        public ImageSource immagine { get; set; }
        public string Visible { get; set; } = "false";
    }
}

