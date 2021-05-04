using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class ListOfCommands: BaseCommandModule
    {

        [Command("Check")]
        [Description("Checking if bot is actually do smth")]
        public async Task WorkCheck(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("I am alive").ConfigureAwait(false);
        }

        [Command("clear")]
        public async Task Clear(CommandContext ctx, int count = 25)
        {
            var messages = await ctx.Channel.GetMessagesAsync(count);
            await ctx.Channel.DeleteMessagesAsync(messages);
        }

        [Command("Join")]
        public async Task Join(CommandContext ctx) 
        {         
               var JoinEmbed = new DiscordEmbedBuilder
               {
                    Title = "Would you like to join?",
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = ctx.Client.CurrentUser.AvatarUrl },
                    Color = DiscordColor.DarkBlue,
               };

                var JoinMessage = await ctx.Channel.SendMessageAsync(JoinEmbed).ConfigureAwait(false);


                var Brain = DiscordEmoji.FromName(ctx.Client, ":brain:");
                var Bust_In_Silhouette = DiscordEmoji.FromName(ctx.Client, ":bust_in_silhouette:");

                await JoinMessage.CreateReactionAsync(Brain).ConfigureAwait(false);
                await JoinMessage.CreateReactionAsync(Bust_In_Silhouette).ConfigureAwait(false);

                var interactivity = ctx.Client.GetInteractivity();

            var ReactionResult = await interactivity.WaitForReactionAsync(
            x => x.Message == JoinMessage &&
            x.User == ctx.User &&
            (x.Emoji == Brain || x.Emoji == Bust_In_Silhouette)).ConfigureAwait(false);

            //var ReactionResult = await interactivity.WaitForReactionAsync(x => x.Emoji == Brain || x.Emoji == Bust_In_Silhouette);

            //var ReactionResult = await interactivity.WaitForReactionAsync(x => x.Emoji == Brain || x.Emoji == Bust_In_Silhouette);


            if (ReactionResult.Result.Emoji == Brain)
            {
                    var TeacherRole = ctx.Guild.GetRole(832489166968193045);
                    await ctx.Member.GrantRoleAsync(TeacherRole).ConfigureAwait(false);
            }
            else if (ReactionResult.Result.Emoji == Bust_In_Silhouette)
            {
               var StudentRole = ctx.Guild.GetRole(832489272950259771);
               await ctx.Member.GrantRoleAsync(StudentRole).ConfigureAwait(false);

               var CourseEmbed = new DiscordEmbedBuilder
               {
                  Title = "What is your course?",
                  Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = ctx.Client.CurrentUser.AvatarUrl },
                  Color = DiscordColor.DarkBlue,
               };

                    var CourseMessage = await ctx.Channel.SendMessageAsync(CourseEmbed).ConfigureAwait(false);

                    var One = DiscordEmoji.FromName(ctx.Client, ":one:");
                    var Two = DiscordEmoji.FromName(ctx.Client, ":two:");
                    var Three = DiscordEmoji.FromName(ctx.Client, ":three:");
                    var Four = DiscordEmoji.FromName(ctx.Client, ":four:");

                    await CourseMessage.CreateReactionAsync(One).ConfigureAwait(false);
                    await CourseMessage.CreateReactionAsync(Two).ConfigureAwait(false);
                    await CourseMessage.CreateReactionAsync(Three).ConfigureAwait(false);
                    await CourseMessage.CreateReactionAsync(Four).ConfigureAwait(false);

                    var Courseinteractivity = ctx.Client.GetInteractivity();

                    var CourseReactionResult = await Courseinteractivity.WaitForReactionAsync(
                   x => x.Message == CourseMessage &&
                   x.User == ctx.User &&
                   (x.Emoji == One || x.Emoji == Two || x.Emoji == Three || x.Emoji == Four)).ConfigureAwait(false);

                    if (CourseReactionResult.Result.Emoji == One)
                    {
                        var AISTbd11 = ctx.Guild.GetRole(833665394380242955);
                        await ctx.Member.GrantRoleAsync(AISTbd11).ConfigureAwait(false);
                    }
                    else if (CourseReactionResult.Result.Emoji == Two)
                    {
                        var AISTbd21 = ctx.Guild.GetRole(833619937243168769);
                        await ctx.Member.GrantRoleAsync(AISTbd21).ConfigureAwait(false);
                    }
                    else if (CourseReactionResult.Result.Emoji == Three)
                    {
                        var AISTbd31 = ctx.Guild.GetRole(833665453942898718);
                        await ctx.Member.GrantRoleAsync(AISTbd31).ConfigureAwait(false);
                    }
                    else if (CourseReactionResult.Result.Emoji == Four)
                    {
                        var AISTbd41 = ctx.Guild.GetRole(833665493311160360);
                        await ctx.Member.GrantRoleAsync(AISTbd41).ConfigureAwait(false);
                    }

                }
                await JoinMessage.DeleteAsync().ConfigureAwait(false);
        }

    }
}
