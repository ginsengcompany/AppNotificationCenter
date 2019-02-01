using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppNotificationCenter.Models
{
    public class Utente
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string circle_logo { get; set; }
        public string splash_logo { get; set; }
        public string organizzazione { get; set; }
        public ImageSource img { get; set; }
        public string token { get; set; }
        public Boolean attivo { get; set; }
        public string eliminato { get; set; }
    }
}
