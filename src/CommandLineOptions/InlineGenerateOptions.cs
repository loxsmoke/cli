using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.CommandLineOptions
{
    [Verb("inline", HelpText="Generate command line options class")]
    public class InlineGenerateOptions
    {
        [Value(0, MetaName = "definition", Required = true, HelpText = "Command line definition string.")]
        public string Definition { get; set; }
        [Value(1, MetaName = "output", HelpText = "Output path for generated file.")]
        public string OutputPath { get; set; }
        [Option('n', "namespace", HelpText = "The namespace of the command line options class.")]
        public string Namespace { get; set; }
        [Option('c', "class", HelpText = "The name of the command line options class.")]
        public string Class { get; set; }
        [Option('f', "force", HelpText = "Overwrite already existing class files.")]
        public bool OverwriteFiles { get; set; }
        [Option('v', "verbose", HelpText = "Show detailed info when generating code.")]
        public bool Verbose { get; set; }
    }
}
