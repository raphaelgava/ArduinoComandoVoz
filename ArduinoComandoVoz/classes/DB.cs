using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.IO;


namespace ArduinoComandoVoz.classes
{
    class DB
    {
        public static SQLiteConnection conn;
        public static SQLiteConnection LoadDatabase()
        {
            //SQLiteConnection.ClearAllPools();

            string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqlitearduino.db");
            if (conn == null)
            {
                conn = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath);
            }
            return conn;
        }
    }
}
