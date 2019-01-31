using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppNotificationCenter.Database.Models;

namespace AppNotificationCenter.Database.Data
{
    public static class UtenzaData
    {
        public static int GetCountUser()
        {
            return Database.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Utente");
        }

        public static TbUtente getUser(string organizzazione)
        {
            var temp = Database.Connection.Query<TbUtente>("SELECT * FROM Utente WHERE organizzazione=?",organizzazione);
            return temp[0];
        }

        public static int InsertUser(TbUtente user)
        {
            return Database.Connection.Insert(user);
        }
        public static int UpdateUser(TbUtente user)
        {
            return Database.Connection.Update(user);
        }
        public static int DropUser(TbUtente user)
        {
            return Database.Connection.Delete(user);
        }
    }
}
