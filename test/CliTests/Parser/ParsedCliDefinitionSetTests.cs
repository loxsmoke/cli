using LoxSmoke.Cli.ConsoleOutput;
using LoxSmoke.Cli.Model;
using LoxSmoke.Cli.Parser;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CliTests.Parser
{
    public class ParsedCliDefinitionSetTests
    {
        public class TestConsoleWriter : IConsoleWriter
        {
            public List<string> Errors { get; set; } = new List<string>();
            public List<string> Warnings { get; set; } = new List<string>();
            public List<string> Texts { get; set; } = new List<string>();

            public void WriteError(string text) => Errors.Add(text);
            public void WriteLine(string text) => Texts.Add(text);
            public void WriteWarning(string text) => Warnings.Add(text);
        };

        [Fact]
        public void Parse()
        {
            var def = new CliDefinitionList()
            {
                Definitions = new List<CliDefinition>()
                {
                    new CliDefinition()
                    {
                        Definition = "verb -b",
                        Items = new List<CliDefinitionItem>()
                        {
                            new CliDefinitionItem()
                            {
                                 Verb = "verb"
                            },
                            new CliDefinitionItem()
                            {
                                Option = "b"
                            }
                        }
                    }
                }
            };
            var consoleWriter = new TestConsoleWriter();
            var result = ParsedDefinitionList.Parse(def, consoleWriter);
            Assert.NotNull(result);
        }
    }
}
