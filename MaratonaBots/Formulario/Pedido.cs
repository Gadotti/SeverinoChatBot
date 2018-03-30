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
    public class Pedido
    {
        public Salgadinhos Salgadinhos { get; set; }
        public TipoEntrega TipoEntrega { get; set; }
        public CPFNaNota CPFNaNota { get; set; }
        public Bebidas Bebidas { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }

        public static IForm<Pedido> BuildForm()
        {
            var form = new FormBuilder<Pedido>();
            form.Configuration.DefaultPrompt.ChoiceStyle = ChoiceStyleOptions.Buttons;
            form.Configuration.Yes = new string[] { "sim", "yes", "s", "y", "yep" };
            form.Configuration.No = new string[] { "não", "não", "ñ", "no", "n" };
            form.Message("Olá seja bem vindo. Será um prazer atender você.");
            form.OnCompletion(async (context, pedido) =>
            {
                //Salvar na base de dados
                //Gerar pedido
                //Integrar com serviço xpto                
                await context.PostAsync("Seu pedido número 123456 foi gerado e instantes será entregue");
            });
            return form.Build();
        }
    }

    [Describe("Tipo de Entrega")]
    public enum TipoEntrega
    {
        [Describe("Retirar no local")]
        [Terms("Retirar no local", "retirar", "local")]
        RetirarLocal = 1,
        [Terms("Motoboy", "motoca", "moto")]
        Motoboy
    }

    [Describe("Saldagados")]
    public enum Salgadinhos
    {
        Esfirra = 1,
        [Terms("Quibe", "kibe", "k", "q")]
        Quibe,
        Coxinha
    }

    public enum Bebidas
    {
        [Terms("Água", "agua", "h2o", "a")]
        [Describe("Água")]
        Agua = 1,
        [Terms("Refrigerante", "refri", "r")]
        Refrigerante,
        [Terms("Suco", "s")]
        Suco
    }

    [Describe("CPF na nota")]
    public enum CPFNaNota
    {
        Sim =1,
        [Describe("Não")]
        Nao
    }
}