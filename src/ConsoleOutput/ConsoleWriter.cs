using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.ConsoleOutput
{
    public class ConsoleWriter : IConsoleWriter
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void WriteError(string text)
        {
            WriteLine(ConsoleColor.Red, text);
        }

        public void WriteWarning(string text)
        {
            WriteLine(ConsoleColor.Yellow, text);
        }

        public void WriteLine(ConsoleColor color, string text)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }
    }
}
