﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Humanizer.Localisation;
using System.Text.RegularExpressions;
using Discord;

namespace Doraemon.Common.Utilities
{
    public static class FormatUtilities
    {
        private static readonly Regex buildContent = new Regex(@"```([^\s]+|)");
        public static string SanitizeAllMentions(string text)
        {
            var everyoneSanitized = SanitizeEveryone(text);
            var userSanitized = SanitizeUserMentions(everyoneSanitized);
            var roleSanitized = SanitizeRoleMentions(userSanitized);

            return roleSanitized;
        }
        public static string SanitizeEveryone(string text)
    => text.Replace("@everyone", "@\x200beveryone")
           .Replace("@here", "@\x200bhere");

        public static string SanitizeUserMentions(string text)
            => _userMentionRegex.Replace(text, "<@\x200b${Id}>");

        public static string SanitizeRoleMentions(string text)
            => _roleMentionRegex.Replace(text, "<@&\x200b${Id}>");

        private static readonly Regex _userMentionRegex = new Regex("<@!?(?<Id>[0-9]+)>", RegexOptions.Compiled);

        private static readonly Regex _roleMentionRegex = new Regex("<@&(?<Id>[0-9]+)>", RegexOptions.Compiled);
        public static string FixIndentation(string code)
        {
            var lines = code.Split('\n');
            var indentLine = lines.SkipWhile(d => d.FirstOrDefault() != ' ').FirstOrDefault();

            if (indentLine != null)
            {
                var indent = indentLine.LastIndexOf(' ') + 1;

                var pattern = $@"^[^\S\n]{{{indent}}}";

                return Regex.Replace(code, pattern, "", RegexOptions.Multiline);
            }

            return code;
        }
        public static string StripFormatting(string code)
        {
            var cleanCode = buildContent.Replace(code.Trim(), string.Empty); //strip out the ` characters and code block markers
            cleanCode = cleanCode.Replace("\t", "    "); //spaces > tabs
            cleanCode = FixIndentation(cleanCode);
            return cleanCode;
        }
        public static string GetCodeLanguage(string message)
        {
            var match = buildContent.Match(message);
            if (match.Success)
            {
                var codeLanguage = match.Groups[1].Value;
                return string.IsNullOrEmpty(codeLanguage) ? null : codeLanguage;
            }
            else
            {
                return null;
            }
        }

    }
}
