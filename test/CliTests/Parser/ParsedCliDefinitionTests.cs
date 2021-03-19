using LoxSmoke.Cli.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CliTests.Parser
{
    public class ParsedCliDefinitionTests
    {
        [Theory]
        [InlineData("-x", null)]
        [InlineData("verb", null)]
        [InlineData("verb", "verb")]
        [InlineData("verb", "<param>")]
        public void CheckForLostVerbs(string text1, string text2)
        {
            var stringSegments = new List<string>();
            if (text1 != null) stringSegments.Add(text1);
            if (text2 != null) stringSegments.Add(text2);
            var parsedElements = stringSegments.Select(x => ParsedDefinition.ParseItems(x)).SelectMany(x => x).ToList();

            ParsedDefinition.CheckForLostVerbs(parsedElements, "something");
        }

        [Theory]
        [InlineData("--fail", "verb")]
        public void CheckForLostVerbs_Exception(string text1, string text2)
        {
            var stringSegments = new List<string>();
            if (text1 != null) stringSegments.Add(text1);
            if (text2 != null) stringSegments.Add(text2);
            var parsedElements = stringSegments.Select(x => ParsedDefinition.ParseItems(x)).SelectMany(x => x).ToList();

            Assert.ThrowsAny<Exception>(() => ParsedDefinition.CheckForLostVerbs(parsedElements, "something"));
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        public void CheckForOptionalValues(bool optional1, bool optional2, bool expectException)
        {
            var definitionItems = new List<ParsedItem>()
            {
                new ParsedValueOption()
                {
                    ValueName = "value1",
                    IsOptional = optional1
                },
                new ParsedValueOption()
                {
                    ValueName = "value1",
                    IsOptional = optional2
                },
            };
            if (expectException) Assert.Throws<ParseError>(() => ParsedDefinition.CheckForOptionalValues(definitionItems, ""));
            else ParsedDefinition.CheckForOptionalValues(definitionItems, "");
        }

        [Fact]
        public void GetOptionParameters()
        {
            var definitionItems = new List<ParsedItem>()
            {
                new ParsedSwitch()
                {
                    Name = "switch"
                },
                new ParsedValueOption()
                {
                    ValueName = "value1"
                },
            };
            var list = ParsedDefinition.GetOptionParameters(definitionItems);
            Assert.Single(list);
            var item = list.First() as ParsedScalar;
            Assert.NotNull(item);
            Assert.Equal("switch", item.Name);
            Assert.Equal("value1", item.ValueName);
        }

        [Theory]
        [InlineData("[verb]", "[verb]")]
        [InlineData("verb verb verb", "verb/verb/verb")]
        [InlineData("[-x [<verb>]]", "[-x [<verb>]]")]
        public void SplitToSimpleItems(string text, string expected)
        {
            var item = ParsedDefinition.SplitToSimpleItems(text);
            var result = string.Join("/", item);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("[-x [<verb>]]]")]
        public void SplitToSimpleItems_Exception(string text)
        {
            Assert.ThrowsAny<Exception>(() => ParsedDefinition.SplitToSimpleItems(text));
        }

        [Theory]
        [InlineData("[verb]","[verb]")]
        [InlineData("verb1|verb2|verb3", "verb1/verb2/verb3")]
        [InlineData("<variable>","<variable>")]
        [InlineData("-x","-x")]
        [InlineData("-x <parameter>","-x <parameter>")]
        [InlineData("--switch|-s", "--switch|-s")]
        public void ParseElement(string text, string expected)
        {
            var item = ParsedDefinition.ParseItems(text);
            var result = string.Join("/", item);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("-x|-y|-z")]
        [InlineData("-x| ")]
        [InlineData("--switch|-sss")]
        [InlineData("--")]
        public void ParseElement_Exception(string text)
        {
            Assert.ThrowsAny<Exception>(() => ParsedDefinition.ParseItems(text));
        }

        // Parse


    }
}
