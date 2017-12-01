using AppOrdineMediciCaserta.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppOrdineMediciCaserta.Database.Data
{
    public static class LoginData
    {
        public static int GetCountUser()
        {
            return Database.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM login");
        }

        public static TbLogin getUser()
        {
            return Database.Connection.ExecuteScalar<TbLogin>("SELECT * FROM login");
        }

        public static void insertUser(TbLogin user)
        {
            Database.Connection.Insert(user);
        }
    }
}
