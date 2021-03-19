using LoxSmoke.Cli.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Parser
{
    public class ParsedVerb : ParsedItem
    {
        public string Name { get; set; }

        public override bool SameItem(CliDefinitionItem definitionItem)
        {
            return definitionItem.IsVerb && Name == definitionItem.Verb;
        }
        public override CliDefinitionItem ToDefinitionItem()
        {
            return new CliDefinitionItem() { Verb = Name, HelpText = "" };
        }

        public override string ToString()
        {
            return IsOptional ? $"[{Name}]" : Name;
        }
    }
}
