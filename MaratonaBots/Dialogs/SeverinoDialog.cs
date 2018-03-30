using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace MaratonaBots.Dialogs
{
    [Serializable]
    [LuisModel("", "")]
    public class SeverinoDialog : LuisDialog<object>
    {
        private string EntidadeChamado;
        private string EntidadeUsuario;
        private string EntidadeCliente;
        public const int UsuarioLogado = 1;

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            
            var videoCard = new VideoCard();
            videoCard.Title = "Desculpe, não entendi.";
            videoCard.Subtitle = "";
            videoCard.Autostart = true;
            videoCard.Autoloop = false;
            videoCard.Media = new List<MediaUrl>
                {
                    new MediaUrl("https://www.youtube.com/watch?v=gBGOldt3zqM")
                };
            message.Attachments.Add(videoCard.ToAttachment());


            await context.PostAsync(message);
        }

        [LuisIntent("SobreBot")]
        public async Task SobreBot(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            var heroCard = new HeroCard();
            heroCard.Title = "Olá, meu nome é Severino";
            heroCard.Subtitle = "A resposta você já tem, qual é a sua pergunta?";
            heroCard.Images = new List<CardImage>
            {
                new CardImage("http://www.simionovich.com/wp-content/uploads/2016/12/42cluster.jpg", "42")
            };
            message.Attachments.Add(heroCard.ToAttachment());
            await context.PostAsync(message);
        }

        [LuisIntent("Saudacao")]
        public async Task Saudacao(IDialogContext context, LuisResult result)
        {
            int totalChamados = Repository.PopulaObjetos.Instancia.ListaChamados.Where(x => x.UsuarioResponsavel.Id == 1).Count();

            await context.PostAsync($"Olá! Como vai? Há '{totalChamados}' chamado sob sua resposabilidade. \n\r O que posso fazer por você hoje?");
        }

        [LuisIntent("AvisosDiario")]
        public async Task AvisosDiario(IDialogContext context, LuisResult result)
        {
            var listaAvisos = Repository.PopulaObjetos.Instancia.ListaAvisos;

            if (listaAvisos.Count > 0)
            {
                await context.PostAsync("Aqui está os avisos de hoje:");

                var message = context.MakeMessage();

                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                foreach (var aviso in listaAvisos)
                {
                    var heroCard = new HeroCard();
                    heroCard.Title = aviso.Titulo;
                    heroCard.Subtitle = aviso.Texto;                    

                    message.Attachments.Add(heroCard.ToAttachment());
                }

                await context.PostAsync(message);
            }
            else
            {
                await context.PostAsync("Não há avisos para hoje.");
            }
        }

        #region Rotinas de Transferecia
        [LuisIntent("TransferirChamado")]
        public async Task TransferirChamado(IDialogContext context, LuisResult result)
        {
            EntidadeChamado = result.Entities?.Where(t => t.Type == "chamado").Select(e => e.Entity).FirstOrDefault();
            EntidadeUsuario  = result.Entities?.Where(t => t.Type == "usuario").Select(e => e.Entity).FirstOrDefault();

            if (string.IsNullOrEmpty(EntidadeChamado))
            {
                PromptDialog.Text(context, TransferirChamado_ObterChamado, "Vou transferir o chamado para você. Qual é o número?");
                return;
            }

            if (string.IsNullOrEmpty(EntidadeUsuario))
            {
                PromptDialog.Text(context, TransferirChamado_ObterUsuario, $"Beleza! Vou transferir o chamado '{EntidadeChamado}'. Para qual usuário devo mandar?");
                return;
            }

            await TransferirChamado_Final(context);
        }

        public async Task TransferirChamado_ObterChamado(IDialogContext context, IAwaitable<string> result)
        {
            EntidadeChamado = await result;
            
            if (string.IsNullOrEmpty(EntidadeUsuario))
            {
                PromptDialog.Text(context, TransferirChamado_ObterUsuario, $"E para qual usuário devo enviar o chamado '{EntidadeChamado}'?");
            }
        }

        public async Task TransferirChamado_ObterUsuario(IDialogContext context, IAwaitable<string> result)
        {
            EntidadeUsuario = await result;

            await TransferirChamado_Final(context);
        }

        public async Task TransferirChamado_Final(IDialogContext context)
        {
            //Tratamentos básicos antes de utilizar o nome do usuário
            EntidadeUsuario = EntidadeUsuario.Replace("o ", "").Replace("a ", "");
            EntidadeUsuario = EntidadeUsuario.Replace("usuario ", "");

            //Tratamento número do chamado
            int numeroChamado;
            if (int.TryParse(EntidadeChamado, out numeroChamado) == false)
            {
                await context.PostAsync($"O número do chamado informado '{EntidadeChamado}' deve conter apenas números.");
                return;
            }

            //Procura objeto de usuário pelo nome informado
            var usuario = Repository.PopulaObjetos.Instancia.ListaUsuarios.Where(x => x.Nome.ToLower().Contains(EntidadeUsuario.ToLower())).FirstOrDefault();

            if (usuario != null)
            {
                //Pesquisa chamado informado
                var chamado = Repository.PopulaObjetos.Instancia.ListaChamados.Where(x => x.Id == numeroChamado).FirstOrDefault();

                if (chamado != null)
                {
                    //Efetua a transferencia
                    chamado.UsuarioResponsavel = usuario;
                    await context.PostAsync($"Transferencia realizada com sucesso. Chamado '{EntidadeChamado}' para Usuário '{EntidadeUsuario}'.");
                }
                else
                {
                    await context.PostAsync($"Desculpe não foi possível efetuar a transferência, pois não encontrei nenhum chamado com o número '{numeroChamado}'");
                }
            }
            else
            {
                await context.PostAsync($"Desculpe não foi possível efetuar a transferência, pois não encontrei nenhum usuário '{EntidadeUsuario}'");
            }
        }
        #endregion

        #region Rotinas Pesquisa de Chamado
        [LuisIntent("PesquisarChamado")]
        public async Task PesquisarChamado(IDialogContext context, LuisResult result)
        {
            PromptDialog.Text(context, PesquisarChamado_Final, "Vou pesquisar um chamado para você. Informe um termo para eu pesquisar.");
            return;
        }

        public async Task PesquisarChamado_Final(IDialogContext context, IAwaitable<string> result)
        {
            var termo = await result;
            if (string.IsNullOrEmpty(termo))
            {
                await context.PostAsync("Você informou nada para eu pesquisar =(");
                return;
            }

            var resultados = Repository.PopulaObjetos.Instancia.ListaChamados.Where(x => 
                x.Titulo.ToLower().Contains(termo.ToString().ToLower()) ||
                x.Descricao.ToLower().Contains(termo.ToString().ToLower()) ||
                x.Cliente.Nome.ToLower().Contains(termo.ToString().ToLower()) ||
                x.UsuarioResponsavel.Nome.ToLower().Contains(termo.ToString().ToLower())
                )
            .Distinct()
            .ToList();

            if (resultados != null && resultados.Count > 0)
            {
                var msgInicial = $"Aqui está a lista de chamado que encontrei utilizando o termo '{termo}'";
                var informacoes = new StringBuilder();
                informacoes.AppendLine(msgInicial);
                foreach (var chamado in resultados)
                {
                    informacoes.AppendLine($"**Chamado**: {chamado.Id}. **Título**: {chamado.Titulo} \r\n");
                }
                informacoes.AppendLine($"**Total**: " + resultados.Count());

                await context.PostAsync(informacoes.ToString());
            }
            else
            {
                await context.PostAsync($"Não encontrei nenhum chamado com o termo '{termo}'");
            }

        }
        #endregion

        [LuisIntent("ObterAnexos")]
        public async Task ObterAnexos(IDialogContext context, LuisResult result)
        {
            string chamado = result.Entities?.Where(t => t.Type == "chamado").Select(e => e.Entity).FirstOrDefault();

            await context.PostAsync("Obter Anexos..");
        }

        [LuisIntent("ListarChamado")]
        public async Task ListarChamado(IDialogContext context, LuisResult result)
        {
            EntidadeUsuario = result.Entities?.Where(t => t.Type == "usuario").Select(e => e.Entity).FirstOrDefault();
            List<Models.Chamado> listaFiltrada;
            var msgInicial = string.Empty;

            if (!string.IsNullOrEmpty(EntidadeUsuario))
            {
                //Procura objeto de usuário pelo nome informado
                var usuario = Repository.PopulaObjetos.Instancia.ListaUsuarios.Where(x => x.Nome.ToLower().Contains(EntidadeUsuario.ToLower())).FirstOrDefault();

                if (usuario != null)
                {
                    //Busca chamados do usuário informado na entidade
                    listaFiltrada = Repository.PopulaObjetos.Instancia.ListaChamados.Where(x => x.UsuarioResponsavel.Id == usuario.Id && !x.Concluido).ToList();

                    msgInicial = $"Segue os chamados ativos que encontrei para o usuário '{EntidadeUsuario}' \r\n";
                }
                else
                {
                    await context.PostAsync($"Não encontrei nenhum usuário encontrado com o nome '{EntidadeUsuario}'");
                    return;
                }
            }
            else
            {
                //Lista todos os chamados
                listaFiltrada = Repository.PopulaObjetos.Instancia.ListaChamados.Where(x => !x.Concluido).ToList();
                msgInicial = "Segue todos os chamados ativos que encontrei cadastrados \r\n";
            }

            var informacoes = new StringBuilder();
            informacoes.AppendLine(msgInicial);            
            foreach (var chamado in listaFiltrada)
            {
                informacoes.AppendLine($"**Chamado**: {chamado.Id}. **Título**: {chamado.Titulo} \r\n");
            }
            informacoes.AppendLine($"**Total**: " + listaFiltrada.Count());

            await context.PostAsync(informacoes.ToString());
        }

        [LuisIntent("ListarUsuarios")]
        public async Task ListarUsuarios(IDialogContext context, LuisResult result)
        {
            //Monta listagem de usuários
            var listaUsuarios = new StringBuilder();
            foreach (var usuario in Repository.PopulaObjetos.Instancia.ListaUsuarios)
            {
                listaUsuarios.AppendLine("** " + usuario.Nome + "\r\n");
            }

            await context.PostAsync("Aqui está a lista completa dos usuários que encontrei: \r\n " + Environment.NewLine + listaUsuarios.ToString());
        }

        [LuisIntent("ListarClientes")]
        public async Task ListarClientes(IDialogContext context, LuisResult result)
        {
            //Monta listagem de clientes
            var listaClientes = new StringBuilder();
            foreach (var cliente in Repository.PopulaObjetos.Instancia.ListaClientes)
            {
                listaClientes.AppendLine("* " + cliente.Nome + "\r\n");
            }

            await context.PostAsync("Aqui está a lista completa dos clientes que encontrei: \r\n " + Environment.NewLine + listaClientes.ToString());
        }

        [LuisIntent("ListarComandos")]
        public async Task ListarComandos(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            List<CardAction> cardButtons = new List<CardAction>();

            #region Monta lista de comandos
            cardButtons.Add(new CardAction()
            {
                Value = "Listar os chamados",
                Type = "postBack",
                Title = "Listar os chamados"
            });
                        
            cardButtons.Add(new CardAction()
            {
                Value = "Transferir chamado",
                Type = "postBack",
                Title = "Transferir chamados"
            });

            cardButtons.Add(new CardAction()
            {
                Value = "Pesquisar chamado",
                Type = "postBack",
                Title = "Pesquisar chamado"
            });

            cardButtons.Add(new CardAction()
            {
                Value = "detalhes do chamado",
                Type = "postBack",
                Title = "Consultar detalhes do chamado"
            });

            cardButtons.Add(new CardAction()
            {
                Value = "Concluir chamado",
                Type = "postBack",
                Title = "Concluir chamado"
            });

            cardButtons.Add(new CardAction()
            {
                Value = "Acompanhar chamado",
                Type = "postBack",
                Title = "Acompanhar chamado"
            });

            cardButtons.Add(new CardAction()
            {
                Value = "Abrir chamado",
                Type = "postBack",
                Title = "Abrir um novo chamado"
            });

            cardButtons.Add(new CardAction()
            {
                Value = "Listar usuarios",
                Type = "postBack",
                Title = "Listar usuários"
            });

            cardButtons.Add(new CardAction()
            {
                Value = "Consultar cliente",
                Type = "postBack",
                Title = "Consultar informações do cliente"
            });

            cardButtons.Add(new CardAction()
            {
                Value = "Listar clientes",
                Type = "postBack",
                Title = "Listar clientes"
            });

            cardButtons.Add(new CardAction()
            {
                Value = "avisos",
                Type = "postBack",
                Title = "Verificar os avisos do dia"
            });
            #endregion


            HeroCard plCard = new HeroCard()
            {
                Title = "Olha tudo que eu posso fazer:",
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();

            message.Attachments.Add(plAttachment);
            await context.PostAsync(message);
        }
                
        [LuisIntent("ComandosNaoImplementados")]
        public async Task ComandosNaoImplementados(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Me desculpe, ainda não aprendi esta funcionalidade.");
            await ListarComandos(context, result);
        }
                
        [LuisIntent("Ofender")]
        public async Task Ofender(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Que boca suja!");
        }

        [LuisIntent("Agradecimento")]
        public async Task Agradecimento(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("De nada! Sempre que precisar, estarei aqui =)");
        }

        #region Rotinas de Informações do cliente       
        [LuisIntent("InformacaoCliente")]
        public async Task InformacaoCliente(IDialogContext context, LuisResult result)
        {
            EntidadeCliente = result.Entities?.Where(t => t.Type == "cliente").Select(e => e.Entity).FirstOrDefault();

            if (string.IsNullOrEmpty(EntidadeCliente))
            {
                PromptDialog.Text(context, InformacaoCliente_Final, "Certo, qual é o nome do cliente?");
                return;
            }

            await InformacaoCliente_Final(context, null);
        }

        public async Task InformacaoCliente_Final(IDialogContext context, IAwaitable<string> result)
        {
            if (string.IsNullOrEmpty(EntidadeCliente))
            {
                EntidadeCliente = await result;
            }

            //Tratamentos básicos para o nome do cliente
            EntidadeCliente = EntidadeCliente.Replace("o ", "").Replace("a ", "").Replace("cliente ", "").Replace("empresa ", "");

            //Procura objeto de usuário pelo nome informado
            var cliente = Repository.PopulaObjetos.Instancia.ListaClientes.Where(x => x.Nome.ToLower().Contains(EntidadeCliente.ToLower())).FirstOrDefault();

            if (cliente != null)
            {
                var informacoes = new StringBuilder();
                informacoes.AppendLine("Segue as informações completas que encontrei desse cliente \r\n \r\n");
                informacoes.AppendLine($"**Código**: {cliente.Id} \r\n");
                informacoes.AppendLine($"**Nome**: {cliente.Nome} \r\n");
                informacoes.AppendLine($"**Sistema em produção**: {cliente.EnderecoSistemaProducao} \r\n");
                informacoes.AppendLine($"**Sistema em homologação**: {cliente.EnderecoSistemaHomologacao} \r\n");

                await context.PostAsync(informacoes.ToString());
            }
            else
            {
                await context.PostAsync($"Não encontrei nenhum cliente com o nome '{EntidadeCliente}'");
            }
        }
        #endregion

        [LuisIntent("Documentacao")]
        public async Task Documentacao(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Olá! Meu objetivo é apresentar documentações de algum módulo do sistema que você possa ter dúvidas, no entanto ainda não tenho nenhum documento registrado. \r\n Desculpe.");
        }

        #region Rotinas Detalhes do chamado
        [LuisIntent("DetalhesChamado")]
        public async Task DetalhesChamado(IDialogContext context, LuisResult result)
        {
            EntidadeChamado = result.Entities?.Where(t => t.Type == "chamado").Select(e => e.Entity).FirstOrDefault();
            
            if (string.IsNullOrEmpty(EntidadeChamado))
            {
                PromptDialog.Text(context, DetalhesChamado_Final, "Sem problemas! Qual é o número do chamado?");
                return;
            }
            
            await DetalhesChamado_Final(context, null);            
        }

        public async Task DetalhesChamado_Final(IDialogContext context, IAwaitable<string> result)
        {
            if (string.IsNullOrEmpty(EntidadeChamado))
            {
                EntidadeChamado = await result;
            }
            
            //Tratamento número do chamado
            int numeroChamado;
            if (int.TryParse(EntidadeChamado, out numeroChamado) == false)
            {
                await context.PostAsync($"O número do chamado informado '{EntidadeChamado}' deve conter apenas números.");
                return;
            }            

            //Pesquisa chamado informado
            var chamado = Repository.PopulaObjetos.Instancia.ListaChamados.Where(x => x.Id == numeroChamado).FirstOrDefault();

            if (chamado != null)
            {
                var informacoes = new StringBuilder();
                informacoes.AppendLine($"**Código**: {chamado.Id} \r\n");
                informacoes.AppendLine($"**Cliente**: {chamado.Cliente.Nome} \r\n");
                informacoes.AppendLine($"**Título**: {chamado.Titulo} \r\n");
                informacoes.AppendLine($"**Descrição**: {chamado.Descricao}\r\n");
                informacoes.AppendLine($"**Qtde de anexos**: {(chamado.Anexos != null ? chamado.Anexos.Count : 0)}\r\n");
                informacoes.AppendLine($"**Usuário responsavel**: {chamado.UsuarioResponsavel.Nome}\r\n");
                informacoes.AppendLine($"**Concluido**: {(chamado.Concluido ? "Sim" : "Não")} \r\n");

                await context.PostAsync($"**Detalhes do chamado:** \r\n" + Environment.NewLine + informacoes.ToString());
            }
            else
            {
                await context.PostAsync($"Desculpe não encontrei nenhum chamado com o número '{numeroChamado}'");
            }
        }
        #endregion

        [LuisIntent("ConcluirChamado")]
        public async Task ConcluirChamado(IDialogContext context, LuisResult result)
        {
            EntidadeChamado = result.Entities?.Where(t => t.Type == "chamado").Select(e => e.Entity).FirstOrDefault();

            if (string.IsNullOrEmpty(EntidadeChamado))
            {
                PromptDialog.Text(context, ConcluirChamado_Final, "Sem problemas! Qual é o número do chamado?");
                return;
            }

            await ConcluirChamado_Final(context, null);
        }

        public async Task ConcluirChamado_Final(IDialogContext context, IAwaitable<string> result)
        {
            if (string.IsNullOrEmpty(EntidadeChamado))
            {
                EntidadeChamado = await result;
            }

            //Tratamento número do chamado
            int numeroChamado;
            if (int.TryParse(EntidadeChamado, out numeroChamado) == false)
            {
                await context.PostAsync($"O número do chamado informado '{EntidadeChamado}' deve conter apenas números.");
                return;
            }

            //Pesquisa chamado informado
            var chamado = Repository.PopulaObjetos.Instancia.ListaChamados.Where(x => x.Id == numeroChamado).FirstOrDefault();

            if (chamado != null)
            {
                chamado.Concluido = true;
                await context.PostAsync($"Feito! Chamado '{EntidadeChamado}' concluído com sucesso.");
            }
            else
            {
                await context.PostAsync($"Desculpe não encontrei nenhum chamado com o número '{numeroChamado}'");
            }
        }
        
        [LuisIntent("AnexarArquivos")]
        public async Task AnexarArquivos(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Anexar Arquivos..");
        }

        #region Rotinas acompanhar chamado
        [LuisIntent("AcompanharChamado")]
        public async Task AcompanharChamado(IDialogContext context, LuisResult result)
        {
            EntidadeChamado = result.Entities?.Where(t => t.Type == "chamado").Select(e => e.Entity).FirstOrDefault();

            if (string.IsNullOrEmpty(EntidadeChamado))
            {
                PromptDialog.Text(context, AcompanharChamado_Final, "Sem problemas! Qual é o número do chamado?");
                return;
            }

            await AcompanharChamado_Final(context, null);
        }

        public async Task AcompanharChamado_Final(IDialogContext context, IAwaitable<string> result)
        {
            if (string.IsNullOrEmpty(EntidadeChamado))
            {
                EntidadeChamado = await result;
            }

            //Tratamento número do chamado
            int numeroChamado;
            if (int.TryParse(EntidadeChamado, out numeroChamado) == false)
            {
                await context.PostAsync($"O número do chamado informado '{EntidadeChamado}' deve conter apenas números.");
                return;
            }

            //Pesquisa chamado informado
            var chamado = Repository.PopulaObjetos.Instancia.ListaChamados.Where(x => x.Id == numeroChamado).FirstOrDefault();

            if (chamado != null)
            {
                //Obtém o usuário e instancia uma nova lista se necessário
                var usuarioLogado = Repository.PopulaObjetos.Instancia.ListaUsuarios.Where(x => x.Id == UsuarioLogado).FirstOrDefault();
                if (usuarioLogado.ListaChamadosAcompanha == null)
                    usuarioLogado.ListaChamadosAcompanha = new List<Models.Chamado>();

                //Adiciona o chamado na lista de chamados que o usuário acompanha
                if (!usuarioLogado.ListaChamadosAcompanha.Contains(chamado))
                {
                    usuarioLogado.ListaChamadosAcompanha.Add(chamado);
                    await context.PostAsync($"A partir de agora você também está acompanhando o chamado '{EntidadeChamado}'.");
                }
                else
                {
                    await context.PostAsync($"Você já está acompanhando o chamado '{EntidadeChamado}'.");
                }
            }
            else
            {
                await context.PostAsync($"Desculpe não encontrei nenhum chamado com o número '{numeroChamado}'");
            }
        }
        #endregion

        [LuisIntent("AbrirChamado")]
        public async Task AbrirChamado(IDialogContext context, LuisResult result)
        {
            context.Call(Chain.From(() => FormDialog.FromForm(() => Formulario.ChamadoForm.BuildForm(), FormOptions.PromptInStart)), AbrirChamado_Fim);            
        }
        private async Task AbrirChamado_Fim(IDialogContext context, IAwaitable<Formulario.ChamadoForm> result)
        {            
            //Verificar qual tipo de ação, se for 'quit' por exemplo, apresentar a mensagem de abertura finalizada
            // Ao abortar não apresenta nenhuma mensagem
            await context.PostAsync("Abertura encerrada");
        }        
    }
}