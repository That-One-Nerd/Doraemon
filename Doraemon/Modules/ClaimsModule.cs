﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot;
using Disqord.Rest;
using Doraemon.Common.Extensions;
using Doraemon.Data.Models.Core;
using Doraemon.Services.Core;
using Humanizer;
using Qmmands;

namespace Doraemon.Modules
{
    [Name("Claims")]
    [Description("Provides helpful configuration for managing claims to users and roles.")]
    public class ClaimsModule : DiscordGuildModuleBase
    {
        private readonly ClaimService _claimService;

        public ClaimsModule(ClaimService claimService)
        {
            _claimService = claimService;
        }

        [Command("auth claims add", "auth claims grant", "auth claims allow")]
        [Priority(10)]
        [Description("Adds a claim to the given role.")]
        public async Task AddRoleClaimAsync(
            [Description("The role for the claim to be granted.")]
                IRole role,
            [Description("The claim to be added.")] 
                params ClaimMapType[] claimType)
        {
            foreach (var claim in claimType) await _claimService.AddRoleClaimAsync(role.Id, claim);
            await Context.AddConfirmationAsync();
        }

        [Command("auth claims remove", "auth claims revoke", "auth claims disallow")]
        [Priority(10)]
        [Description("Removes a claim from the role provided.")]
        public async Task RemoveRoleClaimAsync(
            [Description("The role for the claim to be removed from.")]
                IRole role,
            [Description("The claim ato be removed from the role.")]
                ClaimMapType claimType)
        {
            await _claimService.RemoveRoleClaimAsync(role.Id, claimType);
            await Context.AddConfirmationAsync();
        }

        [Command("auth claims add", "auth claims grant", "auth claims allow")]
        [Description("Add claims to the given user.")]
        public async Task AddUserClaimAsync(
            [Description("The user to add the claim to.")]
                IMember user,
            [Description("The claims to add to the user.")]
                params ClaimMapType[] claims)
        {
            foreach (var claim in claims)
            {
                await _claimService.AddUserClaimAsync(user.Id, claim);
            }

            await Context.AddConfirmationAsync();
        }

        [Command("auth claims remove", "auth claims revoke", "auth claims disallow")]
        [Description("Removes the given claim from the user provided.")]
        public async Task RemoveUserClaimAsync(
            [Description("The user to have the claim removed from.")]
                IMember user,
            [Description("The claim to remove")] 
                ClaimMapType claim)
        {
            await _claimService.RemoveUserClaimAsync(user.Id, claim);
            await Context.AddConfirmationAsync();
        }

        [Command("auth claims")]
        [Description("Fetches all the claims that the user posesses.")]

        public async Task<DiscordCommandResult> FetchAuthClaimsAsync(
            [Description("The user to fetch claims for.")]
                IMember user)
        {
            var totalClaims = await _claimService.FetchAllClaimsForUserAsync(user.Id);
            if (!totalClaims.Any())
            {
                return Response("No claims assigned.");
            }

            
            var humanizedClaims = string.Join("\n", totalClaims);
            return Response($"```\n{humanizedClaims}\n```");

        }

        [Command("auth claims", "auth claims list")]
        [Description("Fetches all claims for the given role.")]

        public async Task<DiscordCommandResult> FetchAuthClaimsAsync(
            [Description("The role to fetch claims for.")]
                IRole role)
        {
            var totalClaims = await _claimService.FetchAllClaimsForRoleAsync(role.Id);
            if (!totalClaims.Any())
            {
                return Response($"No claims assigned.");
            }
            var humanizedClaims = string.Join("\n", totalClaims);
            return Response($"```\n{humanizedClaims}\n```");
        }

        [Command("auth claims")]
        [Description("Shows all valid Authorization Claims.")]
        public DiscordCommandResult DisplayAuthClaimsAsync()
        {
            var claims = Enum.GetValues(typeof(ClaimMapType)).Cast<ClaimMapType>();
            var splitClaims = string.Join("\n", claims);
            return Response($"```\n{splitClaims}\n```");
        }
    }
}