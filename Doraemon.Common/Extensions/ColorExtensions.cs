﻿using System;
using System.Diagnostics;
using Disqord;
using StandardColor = System.Drawing.Color;
namespace Doraemon.Common.Extensions
{
    public struct DColor
    {
                /// <summary> Gets the default user color value. </summary>
        public static readonly Color Default = new Color(0);
        /// <summary> Gets the teal color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/1ABC9C">1ABC9C</see>.</returns>
        public static readonly Color Teal = new Color(0x1ABC9C);
        /// <summary> Gets the dark teal color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/11806A">11806A</see>.</returns>
        public static readonly Color DarkTeal = new Color(0x11806A);
        /// <summary> Gets the green color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/2ECC71">2ECC71</see>.</returns>
        public static readonly Color Green = new Color(0x2ECC71);
        /// <summary> Gets the dark green color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/1F8B4C">1F8B4C</see>.</returns>
        public static readonly Color DarkGreen = new Color(0x1F8B4C);
        /// <summary> Gets the blue color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/3498DB">3498DB</see>.</returns>
        public static readonly Color Blue = new Color(0x3498DB);
        /// <summary> Gets the dark blue color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/206694">206694</see>.</returns>
        public static readonly Color DarkBlue = new Color(0x206694);
        /// <summary> Gets the purple color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/9B59B6">9B59B6</see>.</returns>
        public static readonly Color Purple = new Color(0x9B59B6);
        /// <summary> Gets the dark purple color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/71368A">71368A</see>.</returns>
        public static readonly Color DarkPurple = new Color(0x71368A);
        /// <summary> Gets the magenta color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/E91E63">E91E63</see>.</returns>
        public static readonly Color Magenta = new Color(0xE91E63);
        /// <summary> Gets the dark magenta color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/AD1457">AD1457</see>.</returns>
        public static readonly Color DarkMagenta = new Color(0xAD1457);
        /// <summary> Gets the gold color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/F1C40F">F1C40F</see>.</returns>
        public static readonly Color Gold = new Color(0xF1C40F);
        /// <summary> Gets the light orange color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/C27C0E">C27C0E</see>.</returns>
        public static readonly Color LightOrange = new Color(0xC27C0E);
        /// <summary> Gets the orange color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/E67E22">E67E22</see>.</returns>
        public static readonly Color Orange = new Color(0xE67E22);
        /// <summary> Gets the dark orange color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/A84300">A84300</see>.</returns>
        public static readonly Color DarkOrange = new Color(0xA84300);
        /// <summary> Gets the red color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/E74C3C">E74C3C</see>.</returns>
        public static readonly Color Red = new Color(0xE74C3C);
        /// <summary> Gets the dark red color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/992D22">992D22</see>.</returns>
        public static readonly Color DarkRed = new Color(0x992D22);
        /// <summary> Gets the light grey color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/979C9F">979C9F</see>.</returns>
        public static readonly Color LightGrey = new Color(0x979C9F);
        /// <summary> Gets the lighter grey color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/95A5A6">95A5A6</see>.</returns>
        public static readonly Color LighterGrey = new Color(0x95A5A6);
        /// <summary> Gets the dark grey color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/607D8B">607D8B</see>.</returns>
        public static readonly Color DarkGrey = new Color(0x607D8B);
        /// <summary> Gets the darker grey color value. </summary>
        /// <returns> A color struct with the hex value of <see href="http://www.color-hex.com/color/546E7A">546E7A</see>.</returns>
        public static readonly Color DarkerGrey = new Color(0x546E7A);
    }
}