using LoxSmoke.Cli.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Parser
{
    public class ParsedScalar : ParsedItem
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string ValueName { get; set; }
        public bool IsValueList { get; set; }

        public override bool SameItem(CliDefinitionItem definitionItem)
        {
            return definitionItem.IsSwitchOrScalar &&
                (!string.IsNullOrEmpty(Name) && Name == definitionItem.Option ||
                !string.IsNullOrEmpty(ShortName) && ShortName == definitionItem.Option ||
                !string.IsNullOrEmpty(ShortName) && !string.IsNullOrEmpty(Name) &&
                Name + "|" + ShortName == definitionItem.Option);
        }

        public override CliDefinitionItem ToDefinitionItem()
        {
            return new CliDefinitionItem()
            {
                Option = ParsedSwitch.ToString(Name, ShortName),
                HelpText = ""
            };
        }

        public override string ToString()
        {
            var text = (string.IsNullOrEmpty(ShortName) && Name.Length == 1) ? $"-{Name}" :
                ($"--{Name}" + (string.IsNullOrEmpty(ShortName) ? "" : $"|-{ShortName}"));
            text += $" <{ValueName}>";
            return IsOptional ? $"[{text}]" : text;
        }
    }
}
