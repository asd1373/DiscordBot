using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.Steps

{
    public abstract class TaskStepBase : TaskStep
    {
        protected readonly string _content;

        public TaskStepBase(string content)
        {
            _content = content;
        }

        public Action<DiscordMessage> OnMessageAdded { get; set; } = delegate { };

        public abstract TaskStep NextStep { get; }
        public Action<DiscordMessage> OnMessageAdd { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public abstract Task<bool> ProccesStep(DiscordClient client, DiscordChannel channel, DiscordUser user);

        public Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            throw new NotImplementedException();
        }

        protected async Task TryAgainAsync(DiscordChannel channel, string problem) 
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Please Try Again",
                Color = DiscordColor.Red,
            };

            embedBuilder.AddField("There is a problem with your previous input", problem);

            var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

            OnMessageAdded(embed);
        }
    }
}
