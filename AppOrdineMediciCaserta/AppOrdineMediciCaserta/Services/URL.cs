using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppOrdineMediciCaserta.Services
{
    public static class URL
    {
        public static string Eventi = "http://192.168.125.56:3000/getEventiById";

        public static string Login = "http://192.168.125.56:3000/cercaMatricola";

        public static string ConfermaElimina = "http://192.168.125.56:3000/setEliminatoConfermato";
    }
}
