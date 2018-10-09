using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppNotificationCenter.Models;

namespace AppNotificationCenter.Database.Models
{
    [Table("Utente")]
    public class TbUtente
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [NotNull, Column("cognome")]
        public string cognome { get; set; }
        [NotNull, Column("nome")]
        public string nome { get; set; }
        [NotNull,Column("specializzazione")]
        public string specializzazione { get; set; }
        [NotNull,Column("provincia")]
        public string provincia { get; set; }
        [NotNull,Column("mail")]
        public string mail { get; set; }
        [NotNull,Column("numero_telefono")]
        public string numero_telefono { get; set; }
        [Column("pec")]
        public string pec { get; set; }
        [NotNull,Column("password")]
        public string password { get; set; }


        public TbUtente() { }

        public TbUtente(Utenza utenteLoggato)
        {
            this.cognome = utenteLoggato.cognome;
            this.nome = utenteLoggato.nome;
            this.mail = utenteLoggato.mail;
            this.numero_telefono = utenteLoggato.numero_telefono;
            this.specializzazione = utenteLoggato.specializzazione;
            this.password = utenteLoggato.password;
            this.pec = utenteLoggato.pec;
            this.provincia = utenteLoggato.provincia;
        }
    }

}