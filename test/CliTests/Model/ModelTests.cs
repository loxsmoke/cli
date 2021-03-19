using LoxSmoke.Cli.Model;
using System;
using Xunit;

namespace CliTests.Model
{
    public class ModelTests
    {
        [Theory]
        [InlineData("verb", "", null, null, null, "verb")]
        [InlineData(null, "option", null, null, null, "option")]
        [InlineData(null, "option", "value", null, null, "option")]
        [InlineData(null, null, "value", null, null, "value")]
        public void CliDefinitionItem_ToNameString(
            string verb, string option, string valueName, string propName, string helpText,
            string expectedText)
        {
            var obj = new CliDefinitionItem()
            {
                Verb = verb,
                Option = option,
                ValueName = valueName,
                HelpText = helpText,
                PropertyName = propName
            };
            var text = obj.NameString;
            Assert.Equal(expectedText, text);
        }

        [Theory]
        [InlineData("verb", "", null, null, null, "Verb: verb")]
        [InlineData(null, "option", null, null, null, "Option: option")]
        [InlineData(null, "option", "value", null, null, "Option: option")]
        [InlineData(null, null, "value", null, null, "Value: value")]
        [InlineData(null, null, "value", "PropName", null, "Value: value Property=PropName")]
        [InlineData(null, null, "value", "PropName", "hints!", "Value: value Property=PropName Help=\"hints!\"")]
        public void CliDefinitionItem_ToString(
            string verb, string option, string valueName, string propName, string helpText,
            string expectedText)
        {
            var obj = new CliDefinitionItem()
            {
                Verb = verb,
                Option = option,
                ValueName = valueName,
                HelpText = helpText,
                PropertyName = propName
            };
            var text = obj.ToString();
            Assert.Equal(expectedText, text);
        }
    }
}
