using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaratonaBots.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Chamado> ListaChamadosAcompanha { get; set; }
    }
}
