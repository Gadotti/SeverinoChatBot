using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaratonaBots.Formulario
{
    [Serializable]
    [Template(TemplateUsage.NotUnderstood, "Desculpe não entendi \"{0}\".")]
    public class ChamadoForm
    {
        [Prompt("Selecione o {&}. {||}")]
        public TipoChamado TipoChamado { get; set; }

        [Prompt("Informe um título.")]
        public string Titulo { get; set; }

        [Prompt("Descreva sua solicição.")]
        public string Descricao { get; set; }

        [Prompt("Informe o nome do cliente.")]
        public string Cliente { get; set; }

        public static IForm<ChamadoForm> BuildForm()
        {
            var form = new FormBuilder<ChamadoForm>();
            form.Configuration.DefaultPrompt.ChoiceStyle = ChoiceStyleOptions.Buttons;
            form.Configuration.Yes = new string[] { "sim", "yes", "s", "y", "yep", "ok", "k" };
            form.Configuration.No = new string[] { "nao", "não", "ñ", "no", "n", "cancelar", "abortar" };
            form.Message("Vou lhe fazer algumas perguntas e então vou abrir automaticamente o chamado para você.");
            form.Field(nameof(TipoChamado));
            form.Field(nameof(Cliente),
                    validate: async (state, response) =>
                    {
                        var result = new ValidateResult { IsValid = true, Value = response };
                        var cliente = Repository.PopulaObjetos.Instancia.ListaClientes.Where(x => x.Nome.ToLower().Contains(response.ToString().ToLower())).ToList();

                        if (cliente == null || cliente.Count == 0)
                        {
                            result.Feedback = $"Não encontrei nenhum cliente com o nome '{response}'. Informe um cliente válido.";
                            result.IsValid = false;
                        }
                        else if (cliente.Count > 1)
                        {
                            result.Feedback = $"Encontrei alguns cliente com nomes parecidos: {string.Join(", ", cliente.Select(x => x.Nome).ToList())}. Qual deles é o certo?";
                            result.IsValid = false;
                        }
                        return result;
                    });
            form.Field(nameof(Titulo));
            form.Field(nameof(Descricao));            
            form.AddRemainingFields();
            form.Confirm("Verifique as informações. Está correto? {*}");
            form.OnCompletion(async (context, chamado) =>
            {
                //Carregar cliente
                var cliente = Repository.PopulaObjetos.Instancia.ListaClientes.Where(x => x.Nome.ToLower().Contains(chamado.Cliente.ToLower())).FirstOrDefault();
                
                //Definir usuário
                var usuario = Repository.PopulaObjetos.Instancia.ListaUsuarios.Where(x => x.Id == Dialogs.SeverinoDialog.UsuarioLogado).FirstOrDefault();

                //Cria item de chamado
                var novoChamado = new Models.Chamado();
                novoChamado.Cliente = cliente;
                novoChamado.UsuarioResponsavel = usuario;
                novoChamado.Id = Repository.PopulaObjetos.Instancia.ListaChamados.OrderByDescending(x => x.Id).First().Id + 1;
                novoChamado.TipoChamado = (Models.TipoChamado)chamado.TipoChamado;
                novoChamado.Titulo = chamado.Titulo;
                novoChamado.Descricao = chamado.Descricao;
                novoChamado.Concluido = false;

                //Adicionar chamado na lista
                Repository.PopulaObjetos.Instancia.ListaChamados.Add(novoChamado);

                await context.PostAsync($"O chamado '{novoChamado.Id}' foi criado com sucesso.");
            });

            #region  Muda a msg do que o usuário gostaria de mudar
            var templateAttribute = form.Configuration.Template(TemplateUsage.Navigation);
            var patterns = templateAttribute.Patterns;
            patterns[0] = "O que você gostaria de mudar? {||}";
            templateAttribute.Patterns = patterns;
            #endregion

            #region  Muda a opção "No preference"
            var noPreferenceStrings = new string[] { "Nada" };
            // Set the new "no Preference" value
            form.Configuration.Templates.Single(t => t.Usage == TemplateUsage.NoPreference).Patterns = noPreferenceStrings;
            // Change this one to help detection of what you typed/selected
            form.Configuration.NoPreference = noPreferenceStrings;
            #endregion

            #region Muda os comandos de navegação do formulário
            //Adicionar opções para "Backup"
            var backup = form.Configuration.Commands[FormCommand.Backup];
            var backupTerms = new List<string>();
            backupTerms.AddRange(backup.Terms);
            backupTerms.Add("voltar");
            backupTerms.Add("volta");
            backupTerms.Add("retornar");
            backup.Terms = backupTerms.ToArray();

            //Adicionar opções para "Help"
            var help = form.Configuration.Commands[FormCommand.Help];
            var helpTerms = new List<string>();
            helpTerms.AddRange(help.Terms);
            helpTerms.Add("ajuda");
            helpTerms.Add("opcoes");
            helpTerms.Add("opções");
            help.Terms = helpTerms.ToArray();

            //Adicionar opções para "Quit"
            var quit = form.Configuration.Commands[FormCommand.Quit];
            var quitTerms = new List<string>();
            quitTerms.AddRange(quit.Terms);
            quitTerms.Add("sair");
            quitTerms.Add("abortar");
            quitTerms.Add("cancelar");
            quit.Terms = quitTerms.ToArray();

            //Adicionar opções para "Reset"
            var reset = form.Configuration.Commands[FormCommand.Reset];
            var resetTerms = new List<string>();
            resetTerms.AddRange(reset.Terms);
            resetTerms.Add("reiniciar");
            resetTerms.Add("resetar");
            resetTerms.Add("começar de novo");
            reset.Terms = resetTerms.ToArray();

            //Adicionar opções para "Status"
            var status = form.Configuration.Commands[FormCommand.Status];
            var statusTerms = new List<string>();
            statusTerms.AddRange(status.Terms);
            statusTerms.Add("progresso");
            statusTerms.Add("parcial");
            status.Terms = statusTerms.ToArray();
            #endregion

            return form.Build();
        }
    }

    [Describe("Qual é o tipo do chamado?")]
    public enum TipoChamado
    {
        [Describe("Dúvida")]
        [Terms("d", "duv", "dúvida", "duvida")]
        Duvida = 1,
        [Describe("Erro/Bug")]
        [Terms("e", "b", "erro", "bug", "correção", "correcao")]
        Erro,
        [Describe("Sugestão de melhoria")]
        [Terms("m", "s", "melhoria", "sugestão", "sugestao")]
        Melhoria
    }
}