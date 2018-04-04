using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppNotificationCenter.Models;

namespace AppNotificationCenter.Database.Models
{
    public class GroupDatiEvento : List<DatiEvento>
    {
        public string Heading { get; set; }

        public GroupDatiEvento(List<DatiEvento> list)
        {
            this.AddRange(list);

        }
    }
}
