﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doraemon.Data.Models.Core
{
    public class GuildUserCreationData
    {
        /// <summary>
        /// See <see cref="GuildUser.Id"/>.
        /// </summary>
        public ulong Id { get; set; }

        /// <summary>
        /// See <see cref="GuildUser.Username"/>.
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// See <see cref="GuildUser.Discriminator"/>.
        /// </summary>
        public string Discriminator { get; set; }

        /// <summary>
        /// See <see cref="GuildUser.IsModmailBlocked"/>.
        /// </summary>
        public bool IsModmailBlocked { get; set; }

        internal GuildUser ToEntity()
            => new GuildUser()
            {
                Id = Id,
                Username = Username,
                Discriminator = Discriminator,
                IsModmailBlocked = IsModmailBlocked
            };
    }
}