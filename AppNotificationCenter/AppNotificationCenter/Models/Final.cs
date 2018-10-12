using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotificationCenter.Models
{
    public class Utenza
    {
        public string _id { get; set; }
        public string nome { get; set; }
        public string cognome { get; set; }
        public string token { get; set; }
        public string specializzazione { get; set; }
        public string provincia { get; set; }
        public string mail { get; set; }
        public string username { get; set; }
        public string numero_telefono { get; set; }
        public string pec { get; set; }
        public string password { get; set; }
        public string organizzazione { get; set; }
        public List<Interessi> interessi { get; set; }
        public bool attivo { get; set; }
    }
    public class Interessi
    {
        public string id { get; set; }
        public string interesse { get; set; }
        public string descrizione { get; set; }
    }
    public class Final
    {
        public bool status { get; set; }
        public List<Utenza> final { get; set; }
    }
}
