using LoxSmoke.Cli.Generator.Data;
using LoxSmoke.Cli.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Generator.Interfaces
{
    public interface IOptionsClassGenerator
    {
        ClassFile GenerateClass(
            GeneratorSettings settings,
            ParsedDefinitionList definitionList,
            string nameSpace,
            string className,
            bool addVerbToClassName,
            ParsedVerb verb,
            List<ParsedItem> items);
    }
}
