using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaratonaBots.Models
{
    public enum TipoChamado
    {
        [Description("Dúvida")]
        Duvida = 1,
        [Description("Erro/Bug")]
        Erro,
        [Description("Sugestão de melhoria")]
        Melhoria
    }

    public class Chamado
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public List<Arquivo> Anexos { get; set; }
        public Usuario UsuarioResponsavel { get; set; }
        public Cliente Cliente { get; set; }
        public bool Concluido { get; set; }
        public TipoChamado TipoChamado { get; set; }
    }
}
