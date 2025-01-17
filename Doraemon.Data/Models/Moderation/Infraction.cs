﻿using System;

namespace Doraemon.Data.Models.Moderation
{
    public class Infraction
    {
        /// <summary>
        /// The moderator that issued the infraction.
        /// </summary>
        public ulong ModeratorId { get; set; }
        /// <summary>
        /// The user that is being issued the infraction.
        /// </summary>
        public ulong SubjectId { get; set; }
        /// <summary>
        /// The ID of the infraction.
        /// </summary>
        public string Id { get; set; } // Define as a string so we can convert a GUID into a string.
        /// <summary>
        /// The reason for the infraction being given.
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// The type of infraction.
        /// </summary>
        public InfractionType Type { get; set; }

        /// <summary>
        /// When the infraction was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// The duration of the infraction.
        /// </summary>
        public TimeSpan? Duration { get; set; }
    }
}
