using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.IO;

namespace ArduinoComandoVoz
{
    class UsuarioDB
    {
        public static SQLiteConnection conn;
        public static void LoadDatabase()
        {
            //SQLiteConnection.ClearAllPools();

            string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqlitearduino.db");

            conn = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath);
            //conn.DropTable<Usuario>();
            conn.CreateTable<Usuario>();
        }

        internal static void SalvarUsuario(Usuario usuario)
        {
            conn.Insert(usuario);
        }

        internal static void DeletarUsuario(Usuario usuario)
        {
            conn.Delete(usuario);
        }

        internal static List<Usuario> ListaUsuarios()
        {
            List<Usuario> retorno = new List<Usuario>();
            var result = conn.Query<Usuario>("Select UsuarioId, Login, Senha, Nome FROM Usuario");
            foreach (var item in result)
            {
                retorno.Add(item);
            }
            return retorno;
        }

        internal static Usuario GetUsuario(int index)
        {
            List<Usuario> lista = ListaUsuarios();

            if (lista != null)
            {
                return lista.ToArray()[index];
            }
            return null;
        }

        internal static Usuario GetUsuario(string login, string senha)
        {
            string query = "Select UsuarioId, Login, Senha, Nome FROM Usuario WHERE Login like '" +
                            login + "' AND Senha like '" + senha + "'";
            var result = conn.Query<Usuario>(query);
            Usuario usuario = null;
            if (result != null)
            {
                if (result.Count > 0)
                    usuario = result[0];
            }

            return usuario;
        }
    }
}

