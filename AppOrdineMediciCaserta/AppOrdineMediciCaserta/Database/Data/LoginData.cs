using AppOrdineMediciCaserta.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Com.OneSignal;

namespace AppOrdineMediciCaserta.Database.Data
{
    public static class LoginData
    {
        public static Task<int> GetCountUser()
        {
            return Database.Connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM login");
        }

        public static Task<TbLogin> getUser(string token)
        {
            return Database.Connection.Table<TbLogin>().Where(i => i.token == token).FirstOrDefaultAsync();
        }

        public static Task<List<TbLogin>> getUser()
        {
            return Database.Connection.QueryAsync<TbLogin>("SELECT * FROM login");
        }

        public static Task<int> InsertUser(TbLogin user)
        {
           return Database.Connection.InsertAsync(user);
        }
    }
}
