using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.CommandLineOptions
{
    [Verb("generate", HelpText="Generate command line options classes from definition file")]
    public class GenerateOptions
    {
        [Value(0, MetaName = "file", Required = true, HelpText = "The input file containing command line definition.")]
        public string File { get; set; }
        [Value(1, MetaName = "output", HelpText = "Output path for generated file(s).")]
        public string OutputPath { get; set; }
        [Option('f', "force", HelpText = "Overwrite already existing class files.")]
        public bool OverwriteFiles { get; set; }
        [Option('v', "verbose", HelpText = "Show detailed info when generating code.")]
        public bool Verbose { get; set; }
    }
}
