using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace ArduinoComandoVoz
{
    [Table("Usuario")]
    class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int UsuarioId { get; set; }

        [MaxLength(10)]
        public string Login { get; set; }

        [MaxLength(40)]
        public string Senha { get; set; }

        public string Nome { get; set; }
    }
}
