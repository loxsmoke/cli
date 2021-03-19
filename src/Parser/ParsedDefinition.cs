using LoxSmoke.Cli.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoxSmoke.Cli.Parser
{
    public class ParsedDefinition
    {
        public CliDefinition Definition;
        public List<ParsedItem> ParsedItems;

        public static void CheckForLostVerbs(List<ParsedItem> definitionItems, string cmdLineDefinition)
        {
            // Check that verbs if exist are only in the beginning 
            if (!definitionItems.Any(x => x is ParsedVerb) || 
                !definitionItems.SkipWhile(x => x is ParsedVerb).Any(x => x is ParsedVerb)) return;
            throw new ParseError(
                "Verbs are allowed only at the start of the command line definition. The following verbs are invalid: " +
                $"{string.Join(',', definitionItems.SkipWhile(x => x is ParsedVerb).Where(x => x is ParsedVerb))}",
                cmdLineDefinition);
        }

        public static void CheckForOptionalValues(List<ParsedItem> definitionItems, string cmdLineDefinition)
        {
            // One or zero values - no problem
            if (definitionItems.Count(it => it is ParsedValueOption) <= 1) return;
            // No optional values - no problem
            if (definitionItems.Count(it => it is ParsedValueOption && it.IsOptional) == 0) return;

            // More than one optional value - problem
            if (definitionItems.Count(it => it is ParsedValueOption && it.IsOptional) > 1)
            {
                throw new ParseError("More than one optional value is not allowed. Parser would not know how to parse it.",
                    cmdLineDefinition);
            }

            // Optional valua cannot be followed by non-optional one
            var wasOptional = false;
            foreach (var item in definitionItems.OfType<ParsedValueOption>())
            {
                if (!item.IsOptional && wasOptional)
                {
                    throw new ParseError(
                        $"Value {item.ValueName} should be marked as optional or there should be no optional value before it.",
                        cmdLineDefinition);
                }
                wasOptional = item.IsOptional;
            }
        }

        public static List<ParsedItem> GetOptionParameters(List<ParsedItem> definitionItems)
        {
            if (definitionItems.Count <= 1) return definitionItems;
            // Concat Parameter following flag
            var prev = definitionItems.First();
            var newList = new List<ParsedItem>() { prev };
            foreach (var current in definitionItems.Skip(1))
            {
                if (prev is ParsedSwitch aSwitch && 
                    current is ParsedValueOption valueOption)
                {
                    newList[newList.Count - 1] = new ParsedScalar()
                    { 
                        IsOptional = aSwitch.IsOptional,
                        Name = aSwitch.Name,
                        ShortName = aSwitch.ShortName,
                        DefinitionItem = aSwitch.DefinitionItem,
                        ValueName = valueOption.ValueName,
                        IsValueList = valueOption.IsValueList
                    };
                }
                else newList.Add(current);
                prev = current;
            }
            return newList;
        }

        public static List<string> SplitToSimpleItems(string cmdLineDefinition)
        {
            var subStrings = cmdLineDefinition.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            var elements = new List<string>();
            var stillOpenOptionalBrackets = 0;
            foreach (var str in subStrings)
            {
                var openCount = str.Count(c => c == '[');
                var closeCount = str.Count(c => c == ']');

                if (stillOpenOptionalBrackets > 0) elements[elements.Count - 1] = elements.Last() + " " + str;
                else elements.Add(str);

                if (openCount == closeCount) continue;
                if (openCount < closeCount)
                {
                    stillOpenOptionalBrackets -= (closeCount - openCount);
                    if (stillOpenOptionalBrackets < 0)
                    {
                        throw new ParseError("Too many ']' closing brackets", cmdLineDefinition);
                    }
                }
                else
                {
                    stillOpenOptionalBrackets += openCount - closeCount;
                }
            }
            return elements;
        }

        public static List<ParsedItem> ParseItems(string elementText)
        {
            if (elementText.StartsWith("["))
            {
                var data = ParseItems(elementText.TrimStart('[').TrimEnd(']'));
                foreach (var x in data) x.IsOptional = true;
                return data;
            }

            if (elementText.StartsWith("<"))
            {
                var defItem = new ParsedValueOption() { ValueName = elementText.TrimStart('<').TrimEnd('>') };
                if (defItem.ValueName.EndsWith("..."))
                {
                    defItem.ValueName = defItem.ValueName.Trim('.');
                    defItem.IsValueList = true;
                }
                return new List<ParsedItem>() { defItem };
            }

            if (elementText.StartsWith("-"))
            {
                elementText = elementText.Trim(' ');
                // Possible parameter detection
                ParsedItem detectedParameter = null;
                if (elementText.Contains(' '))
                {
                    var itemList = elementText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (itemList.Length > 1) detectedParameter = ParseItems(itemList[1]).FirstOrDefault();
                    elementText = elementText.Split(' ')[0];
                }

                var data = new List<ParsedItem>();
                string optionName = "", shortOptionName = null;
                if (elementText.Contains('|')) // Alternative names for options
                {
                    var altNames = elementText.Split('|', StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => t.TrimStart('-'))
                        .OrderByDescending(t => t.Length)
                        .ToList();
                    if (altNames.Count != 2) throw new Exception("Possible invalid '|' position. Exactly two not empty named options are expected");
                    if (altNames.Any(it => it.Length < 1)) throw new Exception("Option name cannot be empty");
                    if (altNames[1].Length != 1) throw new Exception("Short name must be exactly one character long");
                    optionName = altNames[0];
                    shortOptionName = altNames[1];
                }
                else
                {
                    optionName = elementText.Trim('-');
                    if (optionName.Length < 1) throw new Exception("Option name cannot be empty");
                }
                if (detectedParameter is ParsedValueOption valueOption)
                {
                    data.Add(new ParsedScalar() 
                    {
                        Name = optionName, 
                        ShortName = shortOptionName, 
                        ValueName = valueOption.ValueName,
                        IsValueList = valueOption.IsValueList
                    });
                }
                else data.Add(new ParsedSwitch() { Name = optionName, ShortName = shortOptionName });
                return data;
            }

            if (elementText.Contains('|')) // Alternative verbs
            {
                return elementText.Split('|', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => new ParsedVerb() { Name = t }).ToList<ParsedItem>();
            }
            return new List<ParsedItem>() { new ParsedVerb() { Name = elementText } };
        }


        public static ParsedDefinition Parse(CliDefinition definition)
        {
            var stringSegments = SplitToSimpleItems(definition.Definition);
            var parsedElements = stringSegments.Select(x => ParseItems(x)).SelectMany(x => x).ToList();
            parsedElements = GetOptionParameters(parsedElements);
            parsedElements.ForEach(it => { if (it is ParsedSwitch item) item.IsOptional = true; });
            CheckForLostVerbs(parsedElements, definition.Definition);
            CheckForOptionalValues(parsedElements, definition.Definition);
            return new ParsedDefinition() { ParsedItems = parsedElements, Definition = definition };
        }

        public override string ToString()
        {
            return Definition?.ToString() ?? "";
        }
    }
}
