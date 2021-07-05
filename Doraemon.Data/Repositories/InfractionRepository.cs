﻿using Doraemon.Data.Models.Moderation;
using Doraemon.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doraemon.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Doraemon.Data.Repositories
{
    public class InfractionRepository : Repository
    {
        public InfractionRepository(DoraemonContext doraemonContext)
            : base(doraemonContext)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Infraction"/> with the given <see cref="InfractionCreationData"/>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task CreateAsync(InfractionCreationData data)
        {
            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            var infractionEntity = data.ToEntity();
            await DoraemonContext.Infractions.AddAsync(infractionEntity);
            await DoraemonContext.SaveChangesAsync();
        }

        /// <summary>
        /// Returns a <see cref="List{Infraction}"/> that aren't notes or selfmutes.
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Infraction>> FetchNormalizedInfractionsAsync(ulong subjectId)
        {
            return await DoraemonContext.Infractions
                .Where(x => x.SubjectId == subjectId)
                .Where(x => x.Type != InfractionType.Note)
                .Where(x => x.ModeratorId != subjectId)
                .ToListAsync();
        }

        /// <summary>
        /// Fetches an infraction by the type and user ID provided.
        /// </summary>
        /// <param name="subjectId">The <see cref="Infraction.SubjectId"/> to query for.</param>
        /// <param name="type">The <see cref="InfractionType"/> to check against.</param>
        /// <returns>A <see cref="Infraction"/>.</returns>
        public async Task<Infraction> FetchInfractionForUserByTypeAsync(ulong subjectId, InfractionType type)
        {
            if (type == InfractionType.Warn || type == InfractionType.Note)
                throw new InvalidOperationException($"Due to the possibility of returning multiple items, you are barred from using this method for querying for notes or warns.");
            return await DoraemonContext.Infractions
                .Where(x => x.SubjectId == subjectId)
                .Where(x => x.Type == type)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Returns an infraction by the specified ID.
        /// </summary>
        /// <param name="caseId">The <see cref="Infraction.Id"/> to search for.</param>
        /// <returns></returns> 
        public async Task<Infraction> FetchInfractionByIDAsync(string caseId)
        {
            var infractionToRetrieve = await DoraemonContext.Infractions
                .FindAsync(caseId);
            if(infractionToRetrieve is null)
            {
                throw new ArgumentNullException(nameof(infractionToRetrieve));
            }
            return infractionToRetrieve;
        }

        /// <summary>
        /// Fetches all infractions for the given user.
        /// </summary>
        /// <param name="subjectId">The userID to query for.</param>
        /// <returns>A <see cref="List{Infraction}"/></returns>
        public async Task<IEnumerable<Infraction>> FetchAllUserInfractionsAsync(ulong subjectId)
        {
            return await DoraemonContext.Infractions
                .Where(x => x.SubjectId == subjectId)
                .ToListAsync();
        }

        /// <summary>
        /// Updates an infraction's reason.
        /// </summary>
        /// <param name="caseId">The <see cref="Infraction.Id"/> to query for.</param>
        /// <param name="newReason">The new reason to apply to the <see cref="Infraction"/>.</param>
        /// <returns></returns>
        public async Task UpdateAsync(string caseId, string newReason)
        {
            var infractionToUpdate = await DoraemonContext.Infractions
                .FindAsync(caseId);
            if(infractionToUpdate is null)
            {
                throw new ArgumentNullException(nameof(caseId));
            }
            infractionToUpdate.Reason = newReason;
            await DoraemonContext.SaveChangesAsync();
        }

        /// <summary>
        /// Fetches a <see cref="IEnumerable{Infraction}"/> of infractions that have a duration.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Infraction>> FetchTimedInfractionsAsync()
        {
            return await DoraemonContext.Infractions
                .Where(x => x.Duration != null)
                .ToListAsync();
        }
        /// <summary>
        /// Deletes the given infraction.
        /// </summary>
        /// <param name="infractionId">The <see cref="Infraction.Id"/> to delete.</param>
        /// <returns></returns>
        public async Task DeleteAsync(Infraction infraction)
        {
            DoraemonContext.Infractions.Remove(infraction);
            await DoraemonContext.SaveChangesAsync();
        }
    }
}