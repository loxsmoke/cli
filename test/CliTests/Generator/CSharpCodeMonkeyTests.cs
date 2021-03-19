using LoxSmoke.Cli.Generator;
using LoxSmoke.Cli.Generator.CodeMonkey;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CliTests.Generator
{
    public class CSharpCodeMonkeyTests
    {
        public CSharpCodeMonkey Coder = new CSharpCodeMonkey();

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("one line comment", "/// <summary>\n/// one line comment\n/// </summary>\n")]
        [InlineData("line 1\nline 2", "/// <summary>\n/// line 1\n/// line 2\n/// </summary>\n")]
        public void WriteSummaryComment(string text, string expected)
        {
            Coder.WriteSummaryComment(text);
            var result = Coder.Code.Replace(Environment.NewLine, "\n");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WriteNamespace()
        {
            Coder.WriteNamespace("TheSpace");
            Assert.Equal("namespace TheSpace" + Environment.NewLine + "{" + Environment.NewLine, 
                Coder.Code);
        }

        [Fact]
        public void WriteClass()
        {
            Coder.WriteClass("TheClass");
            Assert.Equal("public class TheClass" + Environment.NewLine + "{" + Environment.NewLine, 
                Coder.Code);
            Assert.Equal(1, Coder.Indent);
        }

        [Fact]
        public void OpeningBracket()
        {
            Assert.Equal(0, Coder.Indent);
            Coder.OpeningBracket();
            Assert.Equal("{" + Environment.NewLine, Coder.Code);
            Assert.Equal(1, Coder.Indent);
        }

        [Fact]
        public void ClosingBracket()
        {
            Coder.Indent++;
            Assert.Equal(1, Coder.Indent);
            Coder.ClosingBracket();
            Assert.Equal("}" + Environment.NewLine, Coder.Code);
            Assert.Equal(0, Coder.Indent);
        }

        [Fact]
        public void WriteAttribute()
        {
            Coder.WriteAttribute("Attribute", (null, 'x'), ("Param1", 1), ("Param2", "2"));
            Assert.Equal("[Attribute('x', Param1 = 1, Param2 = \"2\")]" + Environment.NewLine,
                Coder.Code);
        }

        [Fact]
        public void WriteProperty()
        {
            Coder.WriteProperty("bool", "Prop");
            Assert.Equal("public bool Prop { get; set; }" + Environment.NewLine, Coder.Code);
        }

        [Theory]
        [InlineData(null, "aa", "\"aa\"")]
        [InlineData(null, 'a', "\'a\'")]
        [InlineData(null, 1, "1")]
        [InlineData(null, 2L, "2")]
        [InlineData("Attr", "aa", "Attr = \"aa\"")]
        [InlineData("Attr", 'a', "Attr = \'a\'")]
        [InlineData("Attr", 1, "Attr = 1")]
        [InlineData("Attr", 2L, "Attr = 2")]
        public void ToString_Static(string attrParamName, object obj, string expectedResult)
        {
            var result = CSharpCodeMonkey.ToString(attrParamName, obj);
            Assert.Equal(expectedResult, result);
        }
    }
}
