using LoxSmoke.Cli.Generator;
using LoxSmoke.Cli.Generator.CodeMonkey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CliTests.Generator
{
    public class TextCodeMonkeyTests
    {
        [Theory]
        [InlineData(4, 0, "text", "text")]
        [InlineData(4, 1, "text", "    text")]
        [InlineData(4, 2, "text", "        text")]
        [InlineData(2, 2, "text", "    text")]
        [InlineData(0, 2, "text", "text")]
        public void WriteLine_Tab_Indents(int tabs, int indent, string text, string expectedOutput)
        {
            var monkey = new TextCodeMonkey();
            monkey.TabSpaces = tabs;
            monkey.Indent = indent;
            monkey.WriteLine(text);
            Assert.Equal(indent, monkey.Indent);
            Assert.Equal(tabs, monkey.TabSpaces);
            Assert.Equal(expectedOutput + Environment.NewLine, monkey.Code);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", ",")]
        [InlineData("one,two", "one,two,")]
        public void WriteLines(string lineListString, string expectedListString)
        {
            var lineList = lineListString == null ? null : lineListString.Split(',').ToList();
            var expectedList = string.Join(Environment.NewLine, expectedListString.Split(','));
            var monkey = new TextCodeMonkey();
            monkey.WriteLines(lineList);
            Assert.Equal(expectedList, monkey.Code);
        }

        [Fact]
        public void Clear()
        {
            var monkey = new TextCodeMonkey();
            monkey.TabSpaces = 1;
            monkey.Indent = 2;
            monkey.WriteLine("text");
            monkey.Clear();
            monkey.Indent = 4;
            monkey.WriteLine("other");
            Assert.Equal("    other" + Environment.NewLine, monkey.Code);
        }

        [Fact]
        public void WriteLine()
        {
            var monkey = new TextCodeMonkey();
            monkey.WriteLine();
            Assert.Equal(Environment.NewLine, monkey.Code);
        }

    }
}
