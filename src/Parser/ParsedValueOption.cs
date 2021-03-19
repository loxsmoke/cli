using LoxSmoke.Cli.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Parser
{
    public class ParsedValueOption : ParsedItem
    {
        public string ValueName { get; set; }
        public bool IsValueList { get; set; }

        public override bool SameItem(CliDefinitionItem definitionItem)
        {
            return definitionItem.IsValueOption && ValueName == definitionItem.ValueName;
        }

        public override CliDefinitionItem ToDefinitionItem()
        {
            return new CliDefinitionItem()
            {
                ValueName = $"<{ValueName}>",
                HelpText = ""
            };      
        }

        public override string ToString()
        {
            return IsOptional ? $"[<{ValueName}>]" : $"<{ValueName}>";
        }
    }
}
