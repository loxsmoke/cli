using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.ConsoleOutput
{
    public interface IConsoleWriter
    {
        /// <summary>
        /// Write regular text line without any extra coloring.
        /// </summary>
        /// <param name="text"></param>
        void WriteLine(string text);
        /// <summary>
        /// Write warning line using slightly scary color.
        /// </summary>
        /// <param name="text"></param>
        void WriteWarning(string text);
        /// <summary>
        /// Write error line using very scary color.
        /// </summary>
        /// <param name="text"></param>
        void WriteError(string text);
    }
}
