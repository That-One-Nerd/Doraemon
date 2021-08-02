﻿//using System.Threading.Tasks;
//using Discord;
//using Discord.Commands;
//using Doraemon.Common.Extensions;
//using Doraemon.Services.Events;
//
//namespace Doraemon.Modules
//{
//    [Name("Snipe")]
//    [Summary("Provides utilities for sniping deleted messages.")]
//    public class SnipeModule : ModuleBase<SocketCommandContext>
//    {
//        [Command("snipe")]
//        [Summary("Snipes a deleted message.")]
//        public async Task SnipeDeletedMessageAsync(
//            [Summary("The channel to snipe, defaults to the current channel.")]
//                ITextChannel channel = null)
//        {
//            if (channel == null)
//            {
//                var deletedMessage = GuildEvents.DeletedMessages
//                    .Find(x => x.ChannelId == Context.Channel.Id);
//
//                if (deletedMessage == null)
//                {
//                    await ReplyAsync("Nothing has been deleted yet!");
//                }
//                else
//                {
//                    var user = Context.Guild.GetUser(deletedMessage.UserId);
//
//                    var embed = new EmbedBuilder()
//                        .WithAuthor(user.GetFullUsername(), user.GetDefiniteAvatarUrl())
//                        .WithDescription(deletedMessage.Content)
//                        .WithTimestamp(deletedMessage.Time)
//                        .WithColor(new Color(235, 0, 0))
//                        .Build();
//
//                    await ReplyAsync(embed: embed);
//                }
//
//                return;
//            }
//
//            //calling other channel's message
//            var deletedMessage1 = GuildEvents.DeletedMessages
//                .Find(x => x.ChannelId == channel.Id);
//
//            if (deletedMessage1 == null)
//            {
//                await ReplyAsync("Nothing has been deleted yet!");
//            }
//            else
//            {
//                var user = Context.Guild.GetUser(deletedMessage1.UserId);
//
//                var embed = new EmbedBuilder()
//                    .WithAuthor(user.GetFullUsername(), user.GetDefiniteAvatarUrl())
//                    .WithDescription(deletedMessage1.Content)
//                    .WithTimestamp(deletedMessage1.Time)
//                    .WithColor(new Color(235, 0, 0))
//                    .Build();
//
//                await ReplyAsync(embed: embed);
//            }
//        }
//    }
//}