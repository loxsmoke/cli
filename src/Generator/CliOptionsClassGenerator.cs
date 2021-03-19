using LoxSmoke.Cli.Parser;
using Humanizer;
using System;
using System.Collections.Generic;
using LoxSmoke.Cli.Generator.Interfaces;
using LoxSmoke.Cli.Generator.CodeMonkey;
using LoxSmoke.Cli.Generator.Data;

namespace LoxSmoke.Cli.Generator
{
    public class CliOptionsClassGenerator : CSharpCodeMonkey, IOptionsClassGenerator
    {
        public ClassFile GenerateClass(
            GeneratorSettings settings,
            ParsedDefinitionList definitionList,
            string nameSpace,
            string className,
            bool addVerbToClassName,
            ParsedVerb verb, 
            List<ParsedItem> items)
        {
            Clear();
            TabSpaces = settings.TabSpaces;

            className = className + (addVerbToClassName ? verb.Name.Titleize() : "");

            WriteLines(definitionList.Lines);
            WriteLine();
            WriteNamespace(nameSpace);
            WriteSummaryComment(definitionList.Description);
            if (verb != null)
            {
                WriteAttribute("Verb", (null, verb.Name), ("HelpText", verb.DefinitionItem.HelpText ?? ""));
            }
            WriteClass(className);
            GenerateProperties(items);
            ClosingBracket();
            ClosingBracket();

            return new ClassFile()
            {
                Namespace = nameSpace,
                ClassName = className,
                FileName = className + ".cs",
                Code = Code
            };
        }

        public void GenerateProperties(List<ParsedItem> items)
        {
            var valueIndex = 0;
            foreach (var item in items)
            {
                if (item is ParsedValueOption valueOption)
                {
                    AddValue(valueOption.IsOptional, valueIndex++, 
                        valueOption.ValueName, 
                        valueOption.DefinitionItem?.PropertyName ?? PropName(valueOption.ValueName), valueOption.IsValueList,
                        valueOption.DefinitionItem?.HelpText ??  "");
                    continue;
                }
                if (item is ParsedSwitch aSwitch)
                {
                    AddSwitch(aSwitch.IsOptional, aSwitch.Name, aSwitch.ShortName,
                        aSwitch.DefinitionItem?.PropertyName ?? PropName(aSwitch.Name),
                        aSwitch.DefinitionItem?.HelpText ??  "");
                    continue;
                }
                if (item is ParsedScalar scalar)
                {
                    AddScalar(scalar.IsOptional, scalar.Name, scalar.ShortName, scalar.ValueName, 
                        scalar.DefinitionItem?.PropertyName ?? PropName(scalar.ValueName), scalar.IsValueList,
                        scalar.DefinitionItem?.HelpText ??  "");
                    continue;
                }
            }
        }

        public void AddValue(bool optional, int index, string name, string propertyName, bool valueList, string helpText)
        {
            WriteAttribute("Value", (null, index), ("MetaName", name), ("Required", !optional), ("HelpText", helpText));
            WriteProperty(valueList ? "IEnumerable<string>" : "string", propertyName);
        }

        public void AddSwitch(bool optional, string longName, string shortName, string propertyName,  string helpText)
        {
            AddOptionAttribute(optional, longName, shortName, helpText);
            WriteProperty("bool", propertyName);
        }

        public void AddScalar(bool optional, string longName, string shortName, string valueName, string propertyName, bool valueList, string helpText)
        {
            AddOptionAttribute(optional, longName, shortName, helpText);
            WriteProperty(valueList ? "IEnumerable<string>" : "string", propertyName);
        }

        public void AddOptionAttribute(bool optional, string longName, string shortName, string helpText)
        {
            if (string.IsNullOrEmpty(shortName))
                WriteAttribute("Option", (null, longName), ("Required", !optional), ("HelpText", helpText));
            else WriteAttribute("Option", (null, shortName[0]), (null, longName), ("Required", !optional), ("HelpText", helpText));
        }

        public static string PropName(string text)
        {
            return text.Replace("-", " ").Replace("_", " ").Titleize().Dehumanize();
        }
    }
}
