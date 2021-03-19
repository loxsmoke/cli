using LoxSmoke.Cli.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Samples
{
    public class CliSample
    {
        public static CliDefinitionList Create()
        {
            var definition = new CliDefinitionList()
            {
                Namespace = "LoxSmoke.Cli.CommandLineOptions",
                Description = "Command line options classes.",
                Lines = new List<string>()
                {
                    "using CommandLine;",
                    "using System;",
                    "using System.Collections.Generic;",
                    "using System.Text;"
                },
                Definitions = new List<CliDefinition>()
                {
                    new CliDefinition()
                    {
                        ClassName = "SampleOptions",
                        Definition = "sample [<file_name>] [--force|-f] [--name|-n <sample_name>]",
                        Items = new List<CliDefinitionItem>()
                        {
                            new CliDefinitionItem()
                            {
                                Verb = "sample",
                                HelpText = "Generate the sample command line definition file."
                            },
                            new CliDefinitionItem()
                            {
                                ValueName = "file_name",
                                HelpText = "Optional command line definition sample file name."
                            },
                            new CliDefinitionItem()
                            {
                                Option = "force|f",
                                HelpText = "Overwrite the sample definition file if file with the same name already exists",
                                PropertyName = "OverwriteSampleFile"
                            },
                            new CliDefinitionItem()
                            {
                                Option = "name|n",
                                HelpText = "The name of the sample. Available names: default, cli"
                            }
                        }
                    },
                    new CliDefinition()
                    {
                        ClassName = "InlineGenerateOptions",
                        Definition = "inline <definition> [<output>] [--namespace|-n <namespace>] [--class|-c <class>] [--force|-f] [--verbose|-v]",
                        Items = new List<CliDefinitionItem>()
                        {
                            new CliDefinitionItem()
                            {
                                Verb = "inline",
                                HelpText = "Generate command line options class without using definition file."
                            },
                            new CliDefinitionItem()
                            {
                                ValueName = "definition",
                                HelpText = "Command line definition string."
                            },
                            new CliDefinitionItem()
                            {
                                ValueName = "output",
                                PropertyName = "OutputPath",
                                HelpText = "Output path for generated file."
                            },
                            new CliDefinitionItem()
                            {
                                Option = "namespace|n",
                                HelpText = "The namespace of the command line options class."
                            },
                            new CliDefinitionItem()
                            {
                                Option = "class|c",
                                HelpText = "The name of the command line options class."
                            },
                            new CliDefinitionItem()
                            {
                                Option = "force|f",
                                HelpText = "Overwrite already existing class files.",
                                PropertyName = "OverwriteFiles"
                            },
                            new CliDefinitionItem()
                            {
                                Option = "verbose|v",
                                HelpText = "Show detailed info when generating code."
                            }
                        }
                    },
                    new CliDefinition()
                    {
                        ClassName = "GenerateOptions",
                        Definition = "generate <file> [<output>] [--force|-f] [--verbose|-v]",
                        Items = new List<CliDefinitionItem>()
                        {
                            new CliDefinitionItem()
                            {
                                Verb = "generate",
                                HelpText = "Generate command line options classes from definition file"
                            },
                            new CliDefinitionItem()
                            {
                                ValueName = "file",
                                HelpText = "The input file containing command line definition."
                            },
                            new CliDefinitionItem()
                            {
                                ValueName = "output",
                                PropertyName = "OutputPath",
                                HelpText = "Output path for generated file(s)."
                            },
                            new CliDefinitionItem()
                            {
                                Option = "force|f",
                                HelpText = "Overwrite already existing class files.",
                                PropertyName = "OverwriteFiles"
                            },
                            new CliDefinitionItem()
                            {
                                Option = "verbose|v",
                                HelpText = "Show detailed info when generating code."
                            }
                        }
                    }
                }
            };
            return definition;
        }
    }
}
