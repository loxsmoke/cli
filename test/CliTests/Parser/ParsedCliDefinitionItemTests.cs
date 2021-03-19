using LoxSmoke.Cli.Model;
using LoxSmoke.Cli.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CliTests.Parser
{
    public class ParsedCliDefinitionItemTests
    {
        [Theory]
        [InlineData("-x", true, false, false, false)]
        [InlineData("-x <value>", false, true, false, false)]
        [InlineData("verb", false, false, true, false)]
        [InlineData("<value>", false, false, false, true)]
        public void IsSwitchScalarVerbValue(string text, bool isSwitch, bool isScalar, bool isVerb, bool isValue)
        {
            var result = ParsedDefinition.ParseItems(text).First();
            Assert.Equal(isSwitch, result is ParsedSwitch);
            Assert.Equal(isScalar, result is ParsedScalar);
            Assert.Equal(isVerb, result is ParsedVerb);
            Assert.Equal(isValue, result is ParsedValueOption);
        }

        [Theory]
        [InlineData(true, "-x", null, "x", null)]
        [InlineData(true, "verb", "verb", null, null)]
        [InlineData(true, "<value>", null, null, "value")]
        [InlineData(false, "-x", null, "y", null)]
        [InlineData(false, "verb", "zerb", null, null)]
        [InlineData(false, "<value>", null, null, "zalue")]
        [InlineData(false, "-x", "verb", null, null)]
        [InlineData(false, "verb", null, "m", null)]
        [InlineData(false, "<value>", null, "m", null)]
        public void IsSame(bool same, string itemText, string verb, string option, string valueName)
        {
            var definitionItem = new CliDefinitionItem()
            {
                Verb = verb,
                ValueName = valueName,
                Option = option
            };
            var result = ParsedDefinition.ParseItems(itemText).First();
            Assert.Equal(same, result.SameItem(definitionItem));
        }
    }
}
