using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaratonaBots.Models
{
    public class Arquivo
    {
        public int Id { get; set; }
        public byte[] Documento { get; set; }
        public string Nome { get; set; }
        public int TamanhoBytes { get; set; }
    }
}
