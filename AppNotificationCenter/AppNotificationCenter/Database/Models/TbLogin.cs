using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace AppNotificationCenter.Database.Models
{
    [Table("login")]
    public class TbLogin
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [NotNull, Column("nomeUtente")]
        public string nomeUtente { get; set; }
        [NotNull, Column("password")]
        public string password { get; set; }
        [NotNull,Column("token")]
        public string token { get; set; }
        [NotNull, Column("organizzazione")]
        public string organizzazione { get; set; }

        public TbLogin() { }

        public TbLogin(string nomeUtente, string password, string token, string organizzazione)
        {
            this.nomeUtente = nomeUtente;
            this.password = password;
            this.token = token;
            this.organizzazione = organizzazione;
        }
    }
}
