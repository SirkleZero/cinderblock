﻿using System.Diagnostics;

namespace System
{
    /// <summary>
    /// An extension method class that extends the <see cref="Console"/> type.
    /// </summary>
    public static class ConsoleExtensions
    {
        /// <summary>
        /// Writes green text to the console.
        /// </summary>
        /// <param name="text">The text.</param>
        [DebuggerStepThrough]
        public static void ConsoleGreen(this string text)
        {
            text.WriteLineWithColor(ConsoleColor.Green);
        }

        /// <summary>
        /// Writes red text to the console.
        /// </summary>
        /// <param name="text">The text.</param>
        [DebuggerStepThrough]
        public static void ConsoleRed(this string text)
        {
            text.WriteLineWithColor(ConsoleColor.Red);
        }

        /// <summary>
        /// Writes yellow text to the console.
        /// </summary>
        /// <param name="text">The text.</param>
        [DebuggerStepThrough]
        public static void ConsoleYellow(this string text)
        {
            text.WriteLineWithColor(ConsoleColor.Yellow);
        }

        /// <summary>
        /// Writes out text with the specified ConsoleColor.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        [DebuggerStepThrough]
        public static void WriteLineWithColor(this string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
