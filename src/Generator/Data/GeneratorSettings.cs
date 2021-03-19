using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Generator.Data
{
    public class GeneratorSettings
    {
        /// <summary>
        /// True if tool can overwrite existing files.
        /// </summary>
        public bool OverwriteFiles { get; set; }
        /// <summary>
        /// Display more messages than necessary.
        /// </summary>
        public bool Verbose { get; set; }
        /// <summary>
        /// The number of spaces in the tab.
        /// </summary>
        public int TabSpaces { get; set; } = 4;
        /// <summary>
        /// Output path for files. Could be null.
        /// </summary>
        public string OutputPath { get; set; }
    }
}
