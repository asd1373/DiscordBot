using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.Steps
{
    public interface TaskStep
    {
        Action<DiscordMessage> OnMessageAdd { get; set; }
        TaskStep NextStep { get; }
        Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);
    }
}
