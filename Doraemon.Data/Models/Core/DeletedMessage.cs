﻿using System;

namespace Doraemon.Data.Models.Core
{
    public class DeletedMessage
    {
        /// <summary>
        /// The Content of the deleted
        /// </summary>
        public string Content;
        /// <summary>
        /// The user who sent the message originally.
        /// </summary>
        public ulong UserId;
        /// <summary>
        /// The channel the message was sent in.
        /// </summary>
        public ulong ChannelId;
        /// <summary>
        /// The Time that the message was sent.
        /// </summary>
        public DateTime Time;
        /// <summary>
        /// The Time that the message was deleted.
        /// </summary>
        public DateTime DeleteTime;
    }
}
