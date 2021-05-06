using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Otchetnost;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class ListOfCommands : BaseCommandModule
    {

        public static int day_last = 0;
        public static int id_last = 0;
        public static List<List<Schedule>> tasks = new List<List<Schedule>>();


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

                await JoinMessage.DeleteAsync().ConfigureAwait(false);


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


        [Command("Sched")]

        public async Task StartSched(CommandContext ctx)
        {
            while (true)
            {
                List<List<Schedule>> sched = new List<List<Schedule>>();
                var day_now = new DataAccess().Select<int, dynamic>(SqlSchedule.WEEKDAY, new { }, DataAccess.CONNECTION_STRING_IATU)[0];

                if (day_now > day_last)
                {

                    var buf_day = day_now;

                    day_last = day_now;
                    if (day_now == 6) day_last = day_now = 0;


                    for (int i = 0; i < 6; i++)
                    {
                        var day_sched = new DataAccess().Select<Schedule, dynamic>(new SqlSchedule().sql_SelectScheduleDay, new { sql_group = "АИСТбд-21", sql_date = i }, DataAccess.CONNECTION_STRING_IATU);
                        sched.Add(day_sched);

                    }


                    for (int i = 0; i < sched.Count; i++) //int i = 0; i < sched.Count; i++
                    {
                        var eeembend = new DiscordEmbedBuilder() 
                        {
                            Title = sched[i][0].dayName,
                            Color = DiscordColor.NotQuiteBlack,
                            //if (i == sched.Count - 1) eeembend.Color = DiscordColor.Gold;
                            //else eeembend.Color = DiscordColor.Azure;
                        };

                        int countLesson = 1;

                        for (int j = 0; j < sched[i].Count; j++)
                        {
                            eeembend.AddField("______", "" + countLesson, false);

                            if (j < sched[i].Count - 1)
                            {
                                if(sched[i].Count == 0)
                                {
                                    continue;
                                }
                                //if(j == sched[i].Count - 1)
                                //{
                                //    eeembend.AddField(sched[i][j].discipline, sched[i][j].timeStart + " - " + sched[i][j].timeStop + " |  " + sched[i][j].subgroup + " \n " + sched[i][j].type + " - " + sched[i][j].teacher + " - " + sched[i][j].cabinet, true);
                                //    eeembend.AddField(sched[i][j + 1].discipline, sched[i][j].timeStart + " - " + sched[i][j].timeStop + " |  " + sched[i][j + 1].subgroup + " \n " + sched[i][j + 1].type + " - " + sched[i][j + 1].teacher + " - " + sched[i][j + 1].cabinet, true);
                                //    ++j;
                                //    ++countLesson;
                                //}
                                if (sched[i][j].timeStart == sched[i][j + 1].timeStart)
                                {
                                    eeembend.AddField(sched[i][j].discipline, sched[i][j].timeStart + " - " + sched[i][j].timeStop + " |  " + sched[i][j].subgroup + " \n " + sched[i][j].type + " - " + sched[i][j].teacher + " - " + sched[i][j].cabinet, true);
                                    eeembend.AddField(sched[i][j + 1].discipline, sched[i][j].timeStart + " - " + sched[i][j].timeStop + " |  " + sched[i][j + 1].subgroup + " \n " + sched[i][j + 1].type + " - " + sched[i][j + 1].teacher + " - " + sched[i][j + 1].cabinet, true);
                                    ++j;
                                    ++countLesson;
                                }
                                else if (sched[i][j].timeStart != sched[i][j + 1].timeStart && sched[i][j].subgroup != "0")
                                {
                                    eeembend.AddField(sched[i][j].discipline, sched[i][j].timeStart + " - " + sched[i][j].timeStop + " | " + sched[i][j].subgroup + " \n " + sched[i][j].type + " - " + sched[i][j].teacher + " - " + sched[i][j].cabinet, true);
                                    ++j;
                                    ++countLesson;
                                }
                                else
                                {
                                    eeembend.AddField(sched[i][j].discipline, sched[i][j].timeStart + " - " + sched[i][j].timeStop + " | " + sched[i][j].subgroup + " - " + sched[i][j].type + " - " + sched[i][j].teacher + " - " + sched[i][j].cabinet, false);
                                    ++countLesson;
                                }
                            }
                            else if (sched[i][j].subgroup != "0")
                            {
                                eeembend.AddField(sched[i][j].discipline, sched[i][j].timeStart + " - " + sched[i][j].timeStop + " | " + sched[i][j].subgroup + " \n " + sched[i][j].type + " - " + sched[i][j].teacher + " - " + sched[i][j].cabinet, true);
                                
                                ++j;
                                ++countLesson;
                            }
                            else
                            {
                                eeembend.AddField(sched[i][j].discipline, sched[i][j].timeStart + " - " + sched[i][j].timeStop + " | " + sched[i][j].subgroup + " - " + sched[i][j].type + " - " + sched[i][j].teacher + " - " + sched[i][j].cabinet, false);
                                ++countLesson;
                            }
                            //await ctx.Channel.SendMessageAsync(embed: eeembend).ConfigureAwait(false);
                        }
                        await ctx.Channel.SendMessageAsync(embed: eeembend).ConfigureAwait(false);

                    }
                }
                await Task.Delay(60000);
            }
        }
    }
}   
