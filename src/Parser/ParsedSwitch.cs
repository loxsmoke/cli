using LoxSmoke.Cli.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Parser
{
    public class ParsedSwitch : ParsedItem
    {
        public string Name { get; set; }
        public string ShortName { get; set; }

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
                Option = ToString(Name, ShortName),
                HelpText = ""
            };
        }

        public static string ToString(string longName, string shortName) =>
            (string.IsNullOrEmpty(shortName) && longName.Length == 1) ? $"-{longName}" :
            ($"--{longName}" + (string.IsNullOrEmpty(shortName) ? "" : $"|-{shortName}"));

        public override string ToString()
        {
            var option = (string.IsNullOrEmpty(ShortName) && Name.Length == 1) ? $"-{Name}" :
                ($"--{Name}" + (string.IsNullOrEmpty(ShortName) ? "" : $"|-{ShortName}"));
            return IsOptional ? $"[{option}]" : option;
        }
    }
}
