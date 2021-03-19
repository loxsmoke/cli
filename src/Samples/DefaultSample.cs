using LoxSmoke.Cli.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Samples
{
    public class DefaultSample
    {
        public static CliDefinitionList Create()
        {
            var definition = new CliDefinitionList()
            {
                Description = "Sample command line definition",
                Namespace = "Sample.CommandLine",
                Lines = new List<string>()
                {
                    "using System;"
                },
                Definitions = new List<CliDefinition>()
                {
                    new CliDefinition()
                    {
                        Definition = "[optional_verb] <value> [<optional_value>]",
                        Namespace = "Sample.CommandLine.Verbs",
                        ClassName = "OptionalVerbCommandLine",
                        Items = new List<CliDefinitionItem>()
                        {
                            new CliDefinitionItem()
                            {
                                Verb = "optional_verb",
                                HelpText = "Optional verb."
                            },
                            new CliDefinitionItem()
                            {
                                ValueName = "value",
                                HelpText = "Some value that must be present"
                            },
                            new CliDefinitionItem()
                            {
                                ValueName = "optional_value",
                                HelpText = "Some value that may be omitted. There can be at most one optional value and it must be the last value"
                            }
                        }
                    },
                    new CliDefinition()
                    {
                        Definition = "verb1|verb2 <value> [<optional_value]",
                        Namespace = "Sample.CommandLine.Verbs",
                        ClassName = "Verb1_CommandLine|Verb2_CommandLine",
                        Items = new List<CliDefinitionItem>()
                        {
                            new CliDefinitionItem()
                            {
                                Verb = "verb1",
                                HelpText = "Do some action",
                            },
                            new CliDefinitionItem()
                            {
                                Verb = "verb2",
                                HelpText = "Do some other action",
                            },
                            new CliDefinitionItem()
                            {
                                ValueName = "value",
                                HelpText = "Simple string parameter",
                            },
                            new CliDefinitionItem()
                            {
                                ValueName = "optional_value",
                                HelpText = "Optional string parameter",
                            }
                        }
                    },
                    new CliDefinition()
                    {
                        Definition = "--a_switch|-a --b_option|-b <parameter> [--c_optional_option|-c <parameter>]",
                        ClassName = "CommandLine",
                        Items = new List<CliDefinitionItem>()
                        {
                            new CliDefinitionItem()
                            {
                                Option = "a_switch|a",
                                HelpText = "Command line switch. True if present."
                            },
                            new CliDefinitionItem()
                            {
                                Option = "b_option|b",
                                HelpText = "Mandatory named option",
                                PropertyName = "OptionB"
                            },
                            new CliDefinitionItem()
                            {
                                Option = "c_optional_option|c",
                                HelpText = "Named option that could be missing."
                            }
                        }
                    }
                }
            };
            return definition;
        }
    }
}
