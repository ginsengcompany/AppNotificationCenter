using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using PCLStorage;
using System.IO;
using AppOrdineMediciCaserta.Database.Models;

namespace AppOrdineMediciCaserta.Database
{
    public static class Database
    {
        private static string dbname = "dbutente.db";
        private static string dbpath;
        private static ExistenceCheckResult exist;
        private static SQLiteConnection connection { get; set; }
        public static SQLiteConnection Connection
        {
            get
            {
                return connection;
            }
        }

        public static void Initialize()
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            dbpath = Path.Combine(folder.Path, dbname);
            connection = new SQLiteConnection(dbpath);
            connection.CreateTable<TbLogin>();
        }
    }
}
