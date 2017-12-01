using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace AppOrdineMediciCaserta.Database.Models
{
    [Table("login")]
    public class TbLogin
    {
        [PrimaryKey, NotNull, Column("user")]
        public string user { get; set; }
        [NotNull,Column("token")]
        public string token { get; set; }
    }
}
