using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaratonaBots.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string EnderecoSistemaProducao { get; set; }
        public string EnderecoSistemaHomologacao { get; set; }
    }
}
