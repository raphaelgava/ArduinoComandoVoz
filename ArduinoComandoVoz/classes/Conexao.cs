using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace ArduinoComandoVoz
{
    [Table("Conexao")]
    class Conexao
    {
        [PrimaryKey, AutoIncrement]
        public int key { get; set; }

        [MaxLength(15)]
        public string ip { get; set; }
        
        public int porta { get; set; }
        
    }
}
