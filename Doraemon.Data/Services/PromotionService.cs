﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Doraemon.Common;
using System.Threading.Tasks;
using Discord.WebSocket;
using Doraemon.Data.Models.Promotion;
using Microsoft.EntityFrameworkCore;
using Doraemon.Common.Utilities;
using Discord;
using Doraemon.Common.Extensions;
using Doraemon.Data.Models.Core;

namespace Doraemon.Data.Services
{
    public class PromotionService
    {
        public DoraemonContext _doraemonContext;
        public AuthorizationService _authorizationService;
        public DiscordSocketClient _client;
        public const string DefaultApprovalMessage = "I approve of this campaign.";
        public const string DefaultOpposalMessage = "I do not approve of this campaign.";
        public static DoraemonConfiguration DoraemonConfig { get; private set; } = new();
        public PromotionService(AuthorizationService authorizationService, DoraemonContext doraemonContext, DiscordSocketClient client)
        {
            _authorizationService = authorizationService;
            _doraemonContext = doraemonContext;
            _client = client;
        }
        public async Task NominateUserAsync(ulong userId, ulong initiatorId, string comment, ulong guildId, ulong channelId)
        {
            await _authorizationService.RequireClaims(initiatorId, ClaimMapType.PromotionStart);
            var promo = await _doraemonContext
                .Set<Campaign>()
                .Where(x => x.UserId == userId)
                .AnyAsync();
            if (promo)
            {
                throw new ArgumentException("There is already an ongoing campaign for this user.");
            }
            var ID = await DatabaseUtilities.ProduceIdAsync();
            _doraemonContext.Campaigns.Add(new Campaign { Id = ID, ReasonForCampaign = comment, UserId = userId, InitiatorId = initiatorId });
            await _doraemonContext.SaveChangesAsync();
            var embed = new EmbedBuilder()
                .WithTitle("Campaign Started")
                .WithDescription($"A campaign was started for <@{userId}>, with reason: `{comment}`\nPlease save this ID, it will be needed for anything involving this campaign: `{ID}`")
                .WithColor(Color.DarkPurple)
                .Build();
            var guild = _client.GetGuild(guildId);
            var channel = guild.GetTextChannel(channelId);
            await channel.SendMessageAsync(embed: embed);
        }
        public async Task AddNoteToCampaignAsync(ulong authorId, string campaignId, string note)
        {
            await _authorizationService.RequireClaims(authorId, ClaimMapType.PromotionComment);
            var promo = await _doraemonContext
                .Set<Campaign>()
                .AsQueryable()
                .Where(x => x.Id == campaignId)
                .SingleOrDefaultAsync();
            if (promo is null)
            {
                throw new ArgumentException("The campaign ID provided is not valid.");
            }
            var currentPromoNotes = await _doraemonContext
                .Set<CampaignComment>()
                .AsQueryable()
                .Where(x => x.Content == note)
                .AnyAsync();
            if (currentPromoNotes)
            {
                throw new ArgumentException("There is already an existing comment on the campaign provided that matches the Content provided.");
            }
            _doraemonContext.CampaignComments.Add(new CampaignComment { Id = await DatabaseUtilities.ProduceIdAsync(), Content = note, AuthorId = authorId, CampaignId = campaignId });
            await _doraemonContext.SaveChangesAsync();
        }
        public async Task ApproveCampaignAsync(ulong authorId, string campaignId)
        {
            await _authorizationService.RequireClaims(authorId, ClaimMapType.PromotionComment);
            var promo = await _doraemonContext
                .Set<Campaign>()
                .AsQueryable()
                .Where(x => x.Id == campaignId)
                .AnyAsync();
            var alreadyVoted = await _doraemonContext
                .Set<CampaignComment>()
                .AsQueryable()
                .Where(x => x.CampaignId == campaignId)
                .Where(x => x.AuthorId == authorId)
                .Where(x => x.Content == DefaultApprovalMessage||x.Content == DefaultOpposalMessage)
                .AnyAsync();
            if (!promo)
            {
                throw new ArgumentException("The campaign ID provided is not valid.");
            }
            if (alreadyVoted)
            {
                throw new ArgumentException("You have already voted for the current campaign, so you cannot vote again.");
            }
            _doraemonContext.CampaignComments.Add(new CampaignComment { AuthorId = authorId, Content = DefaultApprovalMessage, Id = await DatabaseUtilities.ProduceIdAsync(), CampaignId = campaignId });
            await _doraemonContext.SaveChangesAsync();
        }
        public async Task OpposeCampaignAsync(ulong authorId, string campaignId)
        {
            await _authorizationService.RequireClaims(authorId, ClaimMapType.PromotionComment);
            var promo = await _doraemonContext
                .Set<Campaign>()
                .AsQueryable()
                .Where(x => x.Id == campaignId)
                .AnyAsync();
            if (!promo)
            {
                throw new ArgumentException("The campaign ID provided is not valid.");
            }
            var alreadyVoted = await _doraemonContext
                .Set<CampaignComment>()
                .AsQueryable()
                .Where(x => x.CampaignId == campaignId)
                .Where(x => x.AuthorId == authorId)
                .AnyAsync();
            if (alreadyVoted)
            {
                throw new ArgumentException("You have already voted for the current campaign, so you cannot vote again.");
            }
            _doraemonContext.CampaignComments.Add(new CampaignComment { AuthorId = authorId, Content = DefaultOpposalMessage, Id = await DatabaseUtilities.ProduceIdAsync(), CampaignId = campaignId });
            await _doraemonContext.SaveChangesAsync();
        }
        public async Task RejectCampaignAsync(string campaignId, ulong managerId, ulong guildId)
        {
            await _authorizationService.RequireClaims(managerId, ClaimMapType.PromotionManage);
            var promo = await _doraemonContext
                .Set<Campaign>()
                .AsQueryable()
                .Where(x => x.Id == campaignId)
                .SingleOrDefaultAsync();
            var promoComments = await _doraemonContext
                .Set<CampaignComment>()
                .AsQueryable()
                .Where(x => x.CampaignId == campaignId)
                .ToListAsync();
            if (promo is null)
            {
                throw new ArgumentException("The campaign ID provided is not valid.");
            }
            _doraemonContext.Campaigns.Remove(promo);
            await _doraemonContext.SaveChangesAsync();
            foreach (var comment in promoComments)
            {
                _doraemonContext.CampaignComments.Remove(comment);
                await _doraemonContext.SaveChangesAsync();
            }
        }
        public async Task AcceptCampaignAsync(string campaignId, ulong managerId, ulong guildId)
        {
            await _authorizationService.RequireClaims(managerId, ClaimMapType.PromotionManage);
            var guild = _client.GetGuild(guildId);
            var role = guild.GetRole(DoraemonConfig.PromotionRoleId);
            var promo = await _doraemonContext
                .Set<Campaign>()
                .AsQueryable()
                .Where(x => x.Id == campaignId)
                .SingleOrDefaultAsync();
            var promoComments = await _doraemonContext
                .Set<CampaignComment>()
                .AsQueryable()
                .Where(x => x.CampaignId == campaignId)
                .ToListAsync();
            var user = guild.GetUser(promo.UserId);
            var n = user.Username + "#" + user.Discriminator;
            await user.AddRoleAsync(role);
            if (promo is null)
            {
                throw new ArgumentException("The campaign ID provided is not valid.");
            }
            _doraemonContext.Campaigns.Remove(promo);
            await _doraemonContext.SaveChangesAsync();
            foreach (var comment in promoComments)
            {
                _doraemonContext.CampaignComments.Remove(comment);
                await _doraemonContext.SaveChangesAsync();
            }
            var promotionLog = guild.GetTextChannel(DoraemonConfig.LogConfiguration.PromotionLogChannelId);
            var promoLogEmbed = new EmbedBuilder()
                .WithAuthor(n, user.GetDefiniteAvatarUrl())
                .WithTitle("The campaign is over!")
                .WithDescription($"Staff accepted the campaign, and **{user.GetFullUsername()}** was promoted to <@&{DoraemonConfig.PromotionRoleId}>!🎉")
                .WithFooter("Congrats on the promotion!")
                .Build();
            await promotionLog.SendMessageAsync(embed: promoLogEmbed);
        }
    }
}
