using LoxSmoke.Cli.Generator.Data;
using LoxSmoke.Cli.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Generator.Interfaces
{
    public interface IProgramClassGenerator
    {
        ClassFile GenerateClass(
            GeneratorSettings settings,
            ParsedDefinitionList definitionList,
            List<ClassFile> cliClasses);
    }
}
