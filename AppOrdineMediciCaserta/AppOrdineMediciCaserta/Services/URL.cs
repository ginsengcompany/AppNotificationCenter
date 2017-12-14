using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppOrdineMediciCaserta.Services
{
    public static class URL
    {
        public static string Eventi = "http://192.168.125.3:3001/getEventiById";

        public static string Login = "http://192.168.125.3:3001/cercaMatricola";

        public static string ConfermaElimina = "http://192.168.125.3:3001/setEliminatoConfermato";
    }
}
