using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MaratonaBots.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            await context.PostAsync("**Olá, tudo bem?**");

            var message = activity.CreateReply();

            if (activity.Text.Equals("herocard", StringComparison.InvariantCultureIgnoreCase))
            {
                var heroCard = new HeroCard();
                heroCard.Title = "Planeta";
                heroCard.Subtitle = "Universo";
                heroCard.Images = new List<CardImage>
                {
                    new CardImage("https://upload.wikimedia.org/wikipedia/commons/2/2b/Jupiter_and_its_shrunken_Great_Red_Spot.jpg", "Planeta Jubter")
                };

                message.Attachments.Add(heroCard.ToAttachment());
            }else if (activity.Text.Equals("videocard", StringComparison.InvariantCultureIgnoreCase))
            {
                var videoCard = new VideoCard();
                videoCard.Title = "Vídeo qualquer";
                videoCard.Subtitle = "Subtitulo";
                videoCard.Autostart = true;
                videoCard.Autoloop = false;
                videoCard.Media = new List<MediaUrl>
                {
                    new MediaUrl("https://youtu.be/Fy6tIMRyuhg?list=RDFy6tIMRyuhg")
                };
                message.Attachments.Add(videoCard.ToAttachment());
            }else if(activity.Text.Equals("audiocard", StringComparison.InvariantCultureIgnoreCase))
            {
                var audioCard = new AudioCard();
                audioCard.Title = "Vídeo qualquer";
                audioCard.Subtitle = "Subtitulo";
                audioCard.Image = new ThumbnailUrl("http://1.bp.blogspot.com/-4ggVT-LZLPo/VLsXjNqN5-I/AAAAAAAAO3o/HLo59vwwA7g/s1600/Filarm%C3%B4nica%2Bde%2BPas%C3%A1rgada.jpg");
                audioCard.Autostart = true;
                audioCard.Autoloop = false;
                audioCard.Media = new List<MediaUrl>
                {
                    new MediaUrl("https://youtu.be/Fy6tIMRyuhg?list=RDFy6tIMRyuhg")
                };
                message.Attachments.Add(audioCard.ToAttachment());
            } else if(activity.Text.Equals("carousel", StringComparison.InvariantCultureIgnoreCase))
            {
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                var video = CreateVideoCard();
                var animation = CreateAnimationCard();

                message.Attachments.Add(video);
                message.Attachments.Add(animation);
            }


            await context.PostAsync(message);

            //// calculate something for us to return
            //int length = (activity.Text ?? string.Empty).Length;

            //// return our reply to the user
            //await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }

        private Attachment CreateAnimationCard()
        {
            var animationCard = new AnimationCard();
            animationCard.Title = "Título qualquer";
            animationCard.Subtitle = "Subtitulo";
            animationCard.Autostart = true;
            animationCard.Autoloop = false;
            animationCard.Media = new List<MediaUrl>
            {
                new MediaUrl("https://media.giphy.com/media/Wnl8UpbANoaUU/giphy.gif")
            };
            return animationCard.ToAttachment();
        }

        private Attachment CreateVideoCard()
        {
            var videoCard = new VideoCard();
            videoCard.Title = "Vídeo qualquer";
            videoCard.Subtitle = "Subtitulo";
            videoCard.Autostart = true;
            videoCard.Autoloop = false;
            videoCard.Media = new List<MediaUrl>
                {
                    new MediaUrl("https://youtu.be/Fy6tIMRyuhg?list=RDFy6tIMRyuhg")
                };
            return videoCard.ToAttachment();
        }
    }
}