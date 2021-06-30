﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doraemon.Services.Core
{
    public class GuildUser
    {
        public ulong Id { get; set; }

        public string Username { get; set; }

        public string Discriminator { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsModmailBlocked { get; set; }

    }
}
