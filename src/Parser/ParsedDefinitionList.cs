using LoxSmoke.Cli.ConsoleOutput;
using LoxSmoke.Cli.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LoxSmoke.Cli.Parser
{
    public class ParsedDefinitionList
    {
        public string Description { get; set; }
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public List<string> Lines { get; set; }
        public List<ParsedDefinition> Definitions { get; set; } = new List<ParsedDefinition>();

        public static void Warning(IConsoleWriter console, 
            CliDefinition definition, int definitionIndex,
            string item, string message)
        {
            console.WriteWarning($"Warning: {message} Definition #{definitionIndex} Item: {item}" + Environment.NewLine +
                $"  Definition string: \"{definition.Definition}\"");
        }

        public static ParsedDefinitionList Parse(CliDefinitionList definitionSet, IConsoleWriter console)
        {
            var parsedDefinitionSet = new ParsedDefinitionList()
            {
                Description = definitionSet.Description,
                Namespace = definitionSet.Namespace ?? "CommandLine.Options",
                ClassName = definitionSet.ClassName ?? "CliProgram",
                Lines = definitionSet.Lines
            };
            var index = 0;
            foreach (var definition in definitionSet.Definitions)
            {
                if (definition.Items == null) definition.Items = new List<CliDefinitionItem>();
                index++;
                definition.Items.ForEach(item => item.Normalize());
                var parsedDefinition = ParsedDefinition.Parse(definition);
                parsedDefinitionSet.Definitions.Add(parsedDefinition);
                foreach (var item in definition.Items)
                {
                    var parsedItem = parsedDefinition.ParsedItems.FirstOrDefault(i => i.SameItem(item));
                    if (parsedItem == null)
                    {
                        Warning(console, definition, index, item.NameString, "Unused item description.");
                        continue;
                    }
                    parsedItem.DefinitionItem = item;
                }
                foreach (var parsedItem in parsedDefinition.ParsedItems.Where(ci => ci.DefinitionItem == null))
                {
                    Warning(console, definition, index, parsedItem.ToString(), "Missing item description");
                }
                foreach (var parsedItemGroup in parsedDefinition.ParsedItems.GroupBy(it => it.ToString()).Where(g => g.Count() > 1))
                {
                    Warning(console, definition, index, parsedItemGroup.First().ToString(), "Item in command line is defined more than once");
                }
                foreach (var itemGroup in definition.Items.GroupBy(it => it.NameString).Where(g => g.Count() > 1))
                {
                    Warning(console, definition, index, itemGroup.First().NameString, "Item description defined more than once");
                }
            }
            if (definitionSet.Definitions.Count > 1)
            {
                // Check if more than one command line with no verbs exists.
                if (definitionSet.Definitions.Any(it => !it.Items.Any(x => !string.IsNullOrEmpty(x.Verb))))
                {
                    throw new ParseError("There should be either only one definition or all definitions should contain verbs.");
                }
                // Check if different definitions use unique verbs
                var verbs = new HashSet<string>();
                foreach (var definition in definitionSet.Definitions)
                {
                    foreach (var def in definition.Items.Where(it => !string.IsNullOrEmpty(it.Verb)))
                    {
                        if (verbs.Contains(def.Verb))
                        {
                            throw new ParseError($"Verb \'{def.Verb}\' exists in more than one definition. Same verbs cannot be used for different definitions.");
                        }
                    }
                }
            }
            return parsedDefinitionSet;
        }

        public override string ToString()
        {
            return $"{Namespace ?? ""} Definitions={Definitions?.Count ?? 0} Description=\"{Description ?? ""}\"";
        }
    }
}
