using LoxSmoke.Cli.Generator;
using LoxSmoke.Cli.Generator.Data;
using LoxSmoke.Cli.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CliTests.Generator
{
    public class CliClassGeneratorTests
    {
        public CliOptionsClassGenerator Generator = new CliOptionsClassGenerator();

        [Theory]
        [InlineData("Property", "Property")]
        [InlineData("property", "Property")]
        [InlineData("big_property", "BigProperty")]
        [InlineData("big-property", "BigProperty")]
        public void PropName(string name, string expectedResult)
        {
            var result = CliOptionsClassGenerator.PropName(name);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(true, 1, "name", "Prop", false, "help", 
            "[Value(1, MetaName = \"name\", Required = false, HelpText = \"help\")]\npublic string Prop { get; set; }\n")]
        [InlineData(true, 1, "name", "Prop", true, "help", 
            "[Value(1, MetaName = \"name\", Required = false, HelpText = \"help\")]\npublic IEnumerable<string> Prop { get; set; }\n")]
        public void AddValue(bool optional, int index, string name, string propertyName, bool list, string helpText,
            string expectedResult)
        {
            Generator.AddValue(optional, index, name, propertyName, list, helpText);
            Assert.Equal(expectedResult.Replace("\n", Environment.NewLine), Generator.Code);
        }

        [Theory]
        [InlineData(true, "switch", "s", "prop", "help", 
            "[Option('s', \"switch\", Required = false, HelpText = \"help\")]\npublic bool prop { get; set; }\n")]
        public void AddSwitch(bool optional, string longName, string shortName, string propertyName, string helpText,
            string expectedResult)
        {
            Generator.AddSwitch(optional, longName, shortName, propertyName, helpText);
            Assert.Equal(expectedResult.Replace("\n", Environment.NewLine), Generator.Code);
        }

        [Theory]
        [InlineData(true, "scalar", "s", "value", false, "prop", "help!", 
            "[Option('s', \"scalar\", Required = false, HelpText = \"help!\")]\npublic string prop { get; set; }\n")]
        [InlineData(true, "scalar", "s", "value", true, "prop", "help!", 
            "[Option('s', \"scalar\", Required = false, HelpText = \"help!\")]\npublic IEnumerable<string> prop { get; set; }\n")]
        public void AddScalar(bool optional, string longName, string shortName, string valueName, bool list, string propertyName, string helpText,
            string expectedResult)
        {
            Generator.AddScalar(optional, longName, shortName, valueName, propertyName, list, helpText);
            Assert.Equal(expectedResult.Replace("\n", Environment.NewLine), Generator.Code);
        }

        [Theory]
        [InlineData(false, "switch", "s", "help!", "[Option('s', \"switch\", Required = true, HelpText = \"help!\")]")]
        [InlineData(true, "switch", null, "help!", "[Option(\"switch\", Required = false, HelpText = \"help!\")]")]
        public void AddOptionAttribute(bool optional, string longName, string shortName, string helpText, string expectedResult)
        {
            Generator.AddOptionAttribute(optional, longName, shortName, helpText);
            Assert.Equal(expectedResult + Environment.NewLine, Generator.Code);
        }

        [Fact]
        public void GenerateProperties()
        {
            var list = new List<ParsedItem>()
            {
                new ParsedValueOption()
                {
                    ValueName = "value",
                    DefinitionItem = new LoxSmoke.Cli.Model.CliDefinitionItem()
                },
                new ParsedSwitch()
                {
                    Name = "switch1",
                    DefinitionItem = new LoxSmoke.Cli.Model.CliDefinitionItem()
                },
                new ParsedScalar()
                {
                    Name = "switch2",
                    ValueName = "value2",
                    DefinitionItem = new LoxSmoke.Cli.Model.CliDefinitionItem()
                }
            };
            var gc = new CliOptionsClassGenerator();
            gc.GenerateProperties(list);
            var lines = gc.Code.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();

            var expectedLines = new List<string>()
            {
                "[Value(0, MetaName = \"value\", Required = true, HelpText = \"\")]",
                "public string Value { get; set; }",
                "[Option(\"switch1\", Required = true, HelpText = \"\")]",
                "public bool Switch1 { get; set; }",
                "[Option(\"switch2\", Required = true, HelpText = \"\")]",
                "public string Value2 { get; set; }"
            };

            Assert.Equal(expectedLines, lines);
        }

        [Theory]
        [InlineData(false, null, 
            "namespace Namespace", "{",
            "/// <summary>",
            "/// comment",
            "/// </summary>",
            "public class Class",
            "{", "}", "}")]
        [InlineData(false, "verb",
            "namespace Namespace", "{",
            "/// <summary>",
            "/// comment",
            "/// </summary>",
            "[Verb(\"verb\", HelpText = \"\")]",
            "public class Class",
            "{", "}", "}")]
        [InlineData(true, "verb",
            "namespace Namespace", "{",
            "/// <summary>",
            "/// comment",
            "/// </summary>",
            "[Verb(\"verb\", HelpText = \"\")]",
            "public class ClassVerb",
            "{", "}", "}")]
        public void GenerateClass(bool addVerbToClassName, string verb, params string[] expectedLines)
        {
            var gc = new CliOptionsClassGenerator();
            gc.GenerateClass(Settings,
                new ParsedDefinitionList() { Description = "comment" },
                "Namespace", "Class",
                addVerbToClassName,  
                verb == null ? null : new ParsedVerb() { Name = verb, DefinitionItem = new LoxSmoke.Cli.Model.CliDefinitionItem() }, 
                new List<ParsedItem>());
            var lines = gc.Code.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
            Assert.Equal(expectedLines.ToList(), lines);
        }

        public GeneratorSettings Settings = new GeneratorSettings() { TabSpaces = 0 };
    }
}
