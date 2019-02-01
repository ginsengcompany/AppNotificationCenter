using AppNotificationCenter.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Com.OneSignal;

namespace AppNotificationCenter.Database.Data
{
    public static class LoginData
    {
        public static int updateUser(TbLogin user)
        {
            return Database.Connection.Update(user);
        }
        public static int dropUser(TbLogin user)
        {
            return Database.Connection.Delete(user);
        }
        public static int GetCountUser()
        {
            return Database.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM login");
        }

        public static TbLogin getUser(string token)
        {
            return Database.Connection.Table<TbLogin>().Where(i => i.token == token).FirstOrDefault();
        }

        public static List<TbLogin> getUser()
        {
            return Database.Connection.Query<TbLogin>("SELECT * FROM login");
        }

        public static List<TbLogin> getUserAttivo()
        {
            return Database.Connection.Query<TbLogin>("SELECT * FROM login WHERE attivo=?",true);
        }

        public static int InsertUser(TbLogin user)
        {
           return Database.Connection.Insert(user);
        }
    }
}
