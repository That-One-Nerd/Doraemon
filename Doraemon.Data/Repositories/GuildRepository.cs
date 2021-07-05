﻿using Doraemon.Data.Models.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doraemon.Data.Repositories
{
    public class GuildRepository : Repository
    {
        public GuildRepository(DoraemonContext doraemonContext)
            : base(doraemonContext)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Guild"/> with the provided <see cref="GuildCreationData"/>.
        /// </summary>
        /// <param name="data">The data needed to construct a new <see cref="Guild"/> object.</param>
        /// <returns></returns>
        public async Task CreateAsync(GuildCreationData data)
        {
            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            var entity = data.ToEntity();
            await DoraemonContext.Guilds.AddAsync(entity);
            await DoraemonContext.SaveChangesAsync();
        }

        /// <summary>
        /// Fetches a guild.
        /// </summary>
        /// <param name="guildId">The guild's ID to fetch from the database.</param>
        /// <returns></returns>
        public async Task<Guild> FetchGuildAsync(string guildId)
        {
            return await DoraemonContext.Guilds
                .FindAsync(guildId);
        }
        /// <summary>
        /// Fetches all guilds currently present on the whitelist.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{Guild}"/>.</returns>
        public async Task<IEnumerable<Guild>> FetchAllWhitelistedGuildsAsync()
        {
            return await DoraemonContext.Guilds.AsQueryable().ToListAsync();
        }

        /// <summary>
        /// Deletes a <see cref="Guild"/> from the database.
        /// </summary>
        /// <param name="guild">The guild to remove.</param>
        /// <returns></returns>
        public async Task DeleteAsync(Guild guild)
        {
            DoraemonContext.Guilds.Remove(guild);
            await DoraemonContext.SaveChangesAsync();
        }
    }
}