using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

namespace AppNotificationCenter.Database.Models
{
    [Table("login")]
    public class TbLogin
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [NotNull, Column("username")]
        public string username { get; set; }
        [NotNull, Column("password")]
        public string password { get; set; }
        [NotNull,Column("token")]
        public string token { get; set; }
        [NotNull, Column("organizzazione")]
        public string organizzazione { get; set; }
        [Column("splash_logo")]
        public string splash_logo { get; set; }
        [Column("circle_logo")]
        public string circle_logo { get; set; }
        [Column("attivo")]
        public Boolean attivo { get; set; }

        public TbLogin() { }

        public TbLogin(string username, string password, string token, string organizzazione,string circle_logo,string splash_logo, Boolean attivo)
        {
            this.username = username;
            this.splash_logo = splash_logo;
            this.circle_logo = circle_logo;
            this.password = password;
            this.token = token;
            this.organizzazione = organizzazione;
            this.attivo = attivo;

        }
    }
}
