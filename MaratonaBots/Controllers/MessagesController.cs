using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace MaratonaBots
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            switch (activity.Type)
            {
                case ActivityTypes.Message:
                    await Conversation.SendAsync(activity, () => new Dialogs.SeverinoDialog());
                    break;
                case ActivityTypes.ConversationUpdate:
                    using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                    {
                        var client = scope.Resolve<IConnectorClient>();
                        if (activity.MembersAdded.Any())
                        {
                            var reply = activity.CreateReply();
                            foreach (var newMember in activity.MembersAdded)
                            {
                                if (newMember.Id != activity.Recipient.Id)
                                {
                                    reply.Text = "Olá! Meu nome é Severino, um bot utilitário para auxiliar no sistema de chamados.";
                                    await client.Conversations.ReplyToActivityAsync(reply);
                                }
                            }
                        }
                    }
                    break;
                default:
                    HandleSystemMessage(activity);
                    break;
            }

            #region CodigoLegado
            //if (activity.Type == ActivityTypes.Message)
            //{
            //    // await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            //    //await Conversation.SendAsync(activity, () => new Dialogs.CotacaoDialog());
            //    //await this.SendConversation(activity);

            //    await Conversation.SendAsync(activity, () => new Dialogs.SeverinoDialog());
            //}
            //else if (activity.Type == ActivityTypes.ConversationUpdate)
            //{
            //    //if (activity.MembersAdded != null && activity.MembersAdded.Any())
            //    //{
            //    //    foreach (var member in activity.MembersAdded)
            //    //    {
            //    //        if (member.Id != activity.Recipient.Id)
            //    //        {
            //    //            await this.SendConversation(activity);
            //    //        }
            //    //    }
            //    //}
            //}
            //else
            //{
            //    HandleSystemMessage(activity);
            //}
            #endregion
            
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task SendConversation(Activity activity)
        {
            await Conversation.SendAsync(activity, () => Chain.From(() => FormDialog.FromForm(() => Formulario.Pedido.BuildForm(), FormOptions.PromptFieldsWithValues)));
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}