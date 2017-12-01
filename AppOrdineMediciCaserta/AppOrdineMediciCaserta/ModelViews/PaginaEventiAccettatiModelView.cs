using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppOrdineMediciCaserta.ModelViews
{
    class PaginaEventiAccettatiModelView
    {
        public class ListaEventiAccettati
        {
            public string Titolo { get; set; }
            public DateTime Data { get; set; }
            public string Luogo { get; set; }
            public string Informazioni { get; set; }
        }
    }
}
