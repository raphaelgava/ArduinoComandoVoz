using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.IO;
using ArduinoComandoVoz.classes;

namespace ArduinoComandoVoz
{
    class ConexaoDB
    {
        public static SQLiteConnection conn;
        public static void LoadDatabase()
        {
            //SQLiteConnection.ClearAllPools();
            /*
            string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqlitearduino.db");

            conn = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath);
            */
            conn = DB.LoadDatabase();
            conn.CreateTable<Conexao>();
        }

        internal static void SalvarConexao(Conexao conexao)
        {
            Conexao con = GetConexao();
            if (con != null)
            {                
                if (conexao != null)
                {
                    con.ip = conexao.ip;
                    con.porta = conexao.porta;
                    conn.Update(con);
                }
            }
        }

        internal static void DeletarConexao(Conexao conexao)
        {
            conn.Delete(conexao);
        }
        
        internal static Conexao GetConexao()
        {
            string query = "Select * FROM Conexao WHERE key = 1";

            var result = conn.Query<Conexao>(query);
            Conexao conexao = null;
            if (result != null)
            {
                if (result.Count > 0)
                    conexao = result[0];                
            }

            if (conexao == null)
            {
                //Salva valor padrão
                conexao = new Conexao();
                conexao.key = 1;
                conexao.ip = "192.168.1.177";
                conexao.porta = 23;
                conn.Insert(conexao);
            }

            return conexao;
        }
    }
}

