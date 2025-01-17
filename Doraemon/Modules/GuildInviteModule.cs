﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doraemon.Data;
using Doraemon.Services.Core;
using Doraemon.Data.Models;
using Doraemon.Data.Models.Core;
using Doraemon.Common.Extensions;
using Discord.Commands;
using Discord;
using Doraemon.Common.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Doraemon.Services.Events.MessageReceivedHandlers;

namespace Doraemon.Modules
{
    [Name("Guilds")]
    [Summary("Adds commands for blacklisting and whitelisting guilds.")]
    [Group("guild")]
    [Alias("guilds")]
    public class GuildInviteModule : ModuleBase
    {
        public IServiceScopeFactory _serviceScopeFactory;
        public AutoModeration _autoModeration;
        public GuildManagementService _guildService;
        public GuildInviteModule
        (
            GuildManagementService guildService,
             AutoModeration autoModeration
        )
        {
            _autoModeration = autoModeration;
            _guildService = guildService;
        }
        [Command("whitelist")]
        [RequireGuildOwner]
        [Summary("Adds a guild to the list of guilds that will not be filtered by Auto Moderation system.")]
        public async Task WhitelistGuildAsync(
            [Summary("The ID of the guild to whitelist.")]
                string guildId,
            [Summary("The name of the guild")]
                [Remainder] string guildName)
        {
            await _guildService.AddWhitelistedGuildAsync(guildId, guildName, Context.User.Id);
            await Context.AddConfirmationAsync();
        }
        [Command("blacklist")]
        [RequireGuildOwner]
        [Summary("Blacklists a guild, causing all invites to be moderated.")]
        public async Task BlacklistGuildAsync(
            [Summary("The ID of the guild to be removed from the whitelist.")]
                string guildId)
        {
            await _guildService.BlacklistGuildAsync(guildId, Context.User.Id);
        }
        [Command]
        [Priority(10)]
        [Alias("list")]
        [Summary("Lists all whitelisted guilds.")]
        public async Task ListWhitelistedGuildsAsync()
        {
            var builder = new StringBuilder();
            foreach (var guild in await _guildService.FetchAllWhitelistedGuildsAsync())
            {
                builder.AppendLine($"**Guild Name: {guild.Name}**");
                builder.AppendLine($"**Guild ID:** `{guild.Id}`");
                builder.AppendLine();
            }
            var embed = new EmbedBuilder()
                .WithTitle("Whitelisted Guilds")
                .WithDescription(builder.ToString())
                .WithFooter($"Use \"!help guilds\" to view available commands relating to guilds!")
                .Build();
            await ReplyAsync(embed: embed);
        }
    }
}
