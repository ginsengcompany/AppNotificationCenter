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

        public static List<TbLogin> getUser()
        {
            return Database.Connection.Query<TbLogin>("SELECT * FROM Utente");
        }

        public static int InsertUser(TbUtente user)
        {
            return Database.Connection.Insert(user);
        }
    }
}
