using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.Steps
{
    public class TextStep : TaskStepBase
    {
        private readonly TaskStep _nextStep;
        private readonly int? _minLength;
        private readonly int? _maxLength;

        public TextStep(string content, TaskStep nextStep, int? minLength = null, int? maxLength = null) : base(content)
        {
            _nextStep = nextStep;
            _minLength = minLength;
            _maxLength = maxLength;
        }

        public Action<string> OnValidResult { get; set; } = delegate { };

        public override TaskStep NextStep =>_nextStep;

        public override async Task<bool> ProccesStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Please Respond Below",
                Description = $"{user.Mention}, {_content}" 
            };

            embedBuilder.AddField("To Stop The Dialogue", "Use the /cancel command");

            if (_minLength.HasValue)
            {
                embedBuilder.AddField("Min Length: ", $"{_minLength.Value} characters");
            }
            if (_maxLength.HasValue)
            {
                embedBuilder.AddField("Max Length: ", $"{_maxLength.Value} characters");
            }

            var interactivity = client.GetInteractivity();

            while (true)
            {
                var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

                OnMessageAdded(embed);

                var messageResult = await interactivity.WaitForMessageAsync(
                    x => x.Channel.Id == channel.Id && x.Author.Id == user.Id).ConfigureAwait(false);

                OnMessageAdded(messageResult.Result);

                if(messageResult.Result.Content.Equals("/cancel", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (_minLength.HasValue)
                {
                    if(messageResult.Result.Content.Length < _minLength.Value)
                    {
                        await TryAgainAsync(channel, $"Your input is {_minLength.Value - messageResult.Result.Content.Length} characters too short").ConfigureAwait(false);
                        continue;
                    }
                }
                if (_maxLength.HasValue)
                {
                    if (messageResult.Result.Content.Length > _maxLength.Value)
                    {
                        await TryAgainAsync(channel, $"Your input is {_maxLength.Value - messageResult.Result.Content.Length} characters too long").ConfigureAwait(false);
                        continue;
                    }
                }

                OnValidResult(messageResult.Result.Content);

                return false;
            }
        }

    }
}
