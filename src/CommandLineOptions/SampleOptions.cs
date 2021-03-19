using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.CommandLineOptions
{
    [Verb("sample", HelpText="Generate the sample command line definition file.")]
    public class SampleOptions
    {
        [Value(0, MetaName = "file_name", HelpText = "Optional command line definition sample file name.")]
        public string FileName { get; set; }
        [Option('f', "force", HelpText = "Overwrite the sample definition file if file with the same name already exists")]
        public bool OverwriteSampleFile { get; set; }
        [Option('n', "name", HelpText = "The name of the sample. Available names: default, cli")]
        public string SampleName { get; set; }
    }
}
