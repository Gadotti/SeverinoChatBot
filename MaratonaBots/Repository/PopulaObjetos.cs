using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaratonaBots.Models;

namespace MaratonaBots.Repository
{
    public class PopulaObjetos
    {
        private static PopulaObjetos instancia;

        public List<Usuario> ListaUsuarios { get; set; }
        public List<Cliente> ListaClientes { get; set; }
        public List<Aviso> ListaAvisos { get; set; }
        public List<Chamado> ListaChamados { get; set; }        

        public PopulaObjetos()
        {
            //ta dando erro em algum lugar dessa rotina, tem q testar

            #region Cria usuários
            ListaUsuarios = new List<Usuario>();
            ListaUsuarios.Add(new Usuario()
            {
                Id = 1,
                Nome = "Eduardo Gadotti"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 2,
                Nome = "Taruana Gomes"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 3,
                Nome = "Nicole Correia Sousa"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 4,
                Nome = "Leonardo Silva Rodrigues"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 5,
                Nome = "Gabrielly Barros Melo"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 6,
                Nome = "Letícia Souza Ferreira"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 7,
                Nome = "Emily Pereira Costa"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 8,
                Nome = "Rebeca Melo Martins"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 9,
                Nome = "Murilo Fernandes Martins"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 10,
                Nome = "Leonardo Fernandes Pinto"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 11,
                Nome = "Felipe Costa Oliveira"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 12,
                Nome = "Kauan Pereira Martins"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 13,
                Nome = "Rafaela Ribeiro Dias"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 14,
                Nome = "Julieta Rocha Cardoso"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 15,
                Nome = "Kai Gomes Dias"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 16,
                Nome = "Mariana Castro Cavalcanti"
            });
            ListaUsuarios.Add(new Usuario()
            {
                Id = 17,
                Nome = "Matheus Pinto Correia"
            });
            #endregion

            #region Cria clientes
            ListaClientes = new List<Cliente>();
            ListaClientes.Add(new Cliente()
            {
                Id = 1,
                Nome = "Ire",
                EnderecoSistemaProducao = "http://sistema.Ire.com.br",
                EnderecoSistemaHomologacao = "http://homologacao.Ire.com.br"
            });
            ListaClientes.Add(new Cliente()
            {
                Id = 2,
                Nome = "Meeba",
                EnderecoSistemaProducao = "http://sistema.Meeba.com.br",
                EnderecoSistemaHomologacao = "http://homologacao.Meeba.com.br"
            });
            ListaClientes.Add(new Cliente()
            {
                Id = 3,
                Nome = "Tetrante",
                EnderecoSistemaProducao = "http://sistema.Tetrante.com.br",
                EnderecoSistemaHomologacao = "http://homologacao.Tetrante.com.br"
            });
            ListaClientes.Add(new Cliente()
            {
                Id = 4,
                Nome = "Wordtype",
                EnderecoSistemaProducao = "http://sistema.Wordtype.com.br",
                EnderecoSistemaHomologacao = "http://homologacao.Wordtype.com.br"
            });
            ListaClientes.Add(new Cliente()
            {
                Id = 5,
                Nome = "Blapulse",
                EnderecoSistemaProducao = "http://sistema.Blapulse.com.br",
                EnderecoSistemaHomologacao = "http://homologacao.Blapulse.com.br"
            });
            ListaClientes.Add(new Cliente()
            {
                Id = 6,
                Nome = "Plambo",
                EnderecoSistemaProducao = "http://sistema.Plambo.com.br",
                EnderecoSistemaHomologacao = "http://homologacao.Plambo.com.br"
            });
            ListaClientes.Add(new Cliente()
            {
                Id = 7,
                Nome = "Clinicspot",
                EnderecoSistemaProducao = "http://sistema.Clinicspot.com.br",
                EnderecoSistemaHomologacao = "http://homologacao.Clinicspot.com.br"
            });
            ListaClientes.Add(new Cliente()
            {
                Id = 8,
                Nome = "Novajam Centro",
                EnderecoSistemaProducao = "http://sistema.Novajam.com.br",
                EnderecoSistemaHomologacao = "http://homologacao.Novajam.com.br"
            });
            ListaClientes.Add(new Cliente()
            {
                Id = 9,
                Nome = "Novajam Norte",
                EnderecoSistemaProducao = "http://sistema.Novajam.com.br",
                EnderecoSistemaHomologacao = "http://homologacao.Novajam.com.br"
            });
            ListaClientes.Add(new Cliente()
            {
                Id = 10,
                Nome = "Novajam Sul",
                EnderecoSistemaProducao = "http://sistema.Novajam.com.br",
                EnderecoSistemaHomologacao = "http://homologacao.Novajam.com.br"
            });
            #endregion

            #region Cria avisos
            ListaAvisos = new List<Aviso>();
            ListaAvisos.Add(new Aviso()
            {
                Id = 1,
                Titulo = "Teste primiero aviso",
                Texto = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                DataPublicacao = DateTime.Now
            });
            ListaAvisos.Add(new Aviso()
            {
                Id = 2,
                Titulo = "Aviso de fim da maratona",
                Texto = "A Maratona Bots está em reta final! Para concluir o treinamento e receber seu comprovante de participação, você deverá desenvolver, individualmente, um Bot usando o conteúdo aprendido durante os módulos. Para acessar todos os requisitos da entrega do seu bot, acesse aka.ms/maratonabots. Boa notícia: a comunidade MVP preparou uma série de transmissões live com o intuito de tirar possíveis dúvidas dos participantes.",
                DataPublicacao = DateTime.Now
            });
            ListaAvisos.Add(new Aviso()
            {
                Id = 3,
                Titulo = "Último módulo disponível",
                Texto = " Módulo 5: Deploy and Chat \n As primeiras lições do último módulo da Maratona Bots estão no ar!Ao concluir este módulo, você aprenderá sobre o processo de publicação do seu bot no Skype, Microsoft Teams, Telegram, Web e Slack. \n Confira o conteúdo disponibilizado! \n Você está pronto(a)?",
                DataPublicacao = DateTime.Now
            });
            #endregion

            #region Cria chamados
            ListaChamados = new List<Chamado>();
            ListaChamados.Add(new Chamado()
            {
                Id = 101010,
                Cliente = ListaClientes[0],
                UsuarioResponsavel = ListaUsuarios[0],
                Titulo = "Erro ao acessar severino",
                Descricao = "Ao abrir o sistema hoje e chamar o severino, deu o erro 'object reference not set an instance of an object'. \n Acesso realizado em ambiente de produção \n No aguardo de uma solução urgente",
                Concluido = false,
                TipoChamado = TipoChamado.Erro
                
            });
            ListaChamados.Add(new Chamado()
            {
                Id = 202020,
                Cliente = ListaClientes[0],
                UsuarioResponsavel = ListaUsuarios[0],
                Titulo = "Exceção ao chamar o severino no Skype",
                Descricao = "Hoje pela manhã quando chamei o severino pelo Skype ele retornou o erro: 'aabbcc erro codigo 133123. null'",
                Concluido = false,
                TipoChamado = TipoChamado.Erro
            });
            ListaChamados.Add(new Chamado()
            {
                Id = 303030,
                Cliente = ListaClientes[1],
                UsuarioResponsavel = ListaUsuarios[2],
                Titulo = "Dúvida sobre o severino web",
                Descricao = "Estou tentando tranferir um chamado para a usuário Nicole, mas o severino não entende para qual usuário ele deve transferir na primeira tentativa. \n Mensagem enviada: Olá, gostaria de transferir o chamado 123123 para a Nicole. \n Em seguida o severino me pergunta novamente para qual usuário você deseja enviar o chamado.",
                Concluido = false,
                TipoChamado = TipoChamado.Duvida
            });
            ListaChamados.Add(new Chamado()
            {
                Id = 404040,
                Cliente = ListaClientes[2],
                UsuarioResponsavel = ListaUsuarios[3],
                Titulo = "Sugestão de melhoria para o Severino",
                Descricao = "Olá! \n Utilizo muito o Slack no meu dia-a-dia. É possível uma integração do meu sistema com esta plataforma?.",
                Concluido = false,
                TipoChamado = TipoChamado.Melhoria
            });
            ListaChamados.Add(new Chamado()
            {
                Id = 505050,
                Cliente = ListaClientes[7],
                UsuarioResponsavel = ListaUsuarios[5],
                Titulo = "Estou precisando da documentação do módulo de requisição",
                Descricao = "Estou tentando acessar a documentação do módulo de requisições mas o sistema informa que ainda não existe.",
                Concluido = false,
                TipoChamado = TipoChamado.Duvida
            });
            #endregion
        }
        
        public static PopulaObjetos Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new PopulaObjetos();
                }
                return instancia;
            }
        }
    }
}
