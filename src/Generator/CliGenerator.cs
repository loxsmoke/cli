using LoxSmoke.Cli.ConsoleOutput;
using LoxSmoke.Cli.Generator.Data;
using LoxSmoke.Cli.Generator.Interfaces;
using LoxSmoke.Cli.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LoxSmoke.Cli.Generator
{
    public class CliGenerator
    {
        public IOptionsClassGenerator OptionsClassGenerator { get; set; }
        public IProgramClassGenerator ProgramClassGenerator { get; set; }

        public CliGenerator(
            IOptionsClassGenerator optionsClassGenerator = null, 
            IProgramClassGenerator programClassGenerator = null)
        {
            OptionsClassGenerator = optionsClassGenerator ?? new CliOptionsClassGenerator();
            ProgramClassGenerator = programClassGenerator ?? new CliProgramClassGenerator();
        }

        public void Generate(
            ParsedDefinitionList definitionList, 
            GeneratorSettings settings,
            IConsoleWriter consoleWriter)
        {
            var cliClasses = new List<ClassFile>();
            foreach (var definition in definitionList.Definitions)
            {
                var verbs = definition.ParsedItems.OfType<ParsedVerb>().ToList();
                var notVerbs = definition.ParsedItems.Where(e => !(e is ParsedVerb)).ToList();
                var nameSpace = definition.Definition.Namespace ?? definitionList.Namespace;
                var className = definition.Definition.ClassName ?? "CliOptions";
                if (verbs.Count == 0) verbs.Add(null);

                // Multiple classes. One class for each verb
                foreach (var verb in verbs)
                {
                    cliClasses.Add(OptionsClassGenerator.GenerateClass(settings, definitionList, nameSpace, className, verbs.Count > 1, verb, notVerbs));
                }
            }
            cliClasses.Add(ProgramClassGenerator.GenerateClass(settings, definitionList, cliClasses));

            WriteFiles(cliClasses, settings, consoleWriter);
        }

        public void WriteFiles(
            List<ClassFile> cliClasses,
            GeneratorSettings settings,
            IConsoleWriter consoleWriter)
        {
            foreach (var item in cliClasses)
            {
                if (item == null)
                {
                    if (settings.Verbose) consoleWriter.WriteWarning($"Null generated class.");
                    continue;
                }

                var outputFileName = item.FileName;
                if (!string.IsNullOrEmpty(settings.OutputPath))
                {
                    outputFileName = Path.Combine(settings.OutputPath, outputFileName);
                }
                if (!settings.OverwriteFiles && File.Exists(outputFileName))
                {
                    consoleWriter.WriteError($"ERROR: Cannot overwrite existing file \"{outputFileName}\"");
                    continue;
                }
                if (settings.Verbose) consoleWriter.WriteLine($"Writing class {item.ClassName} file \"{outputFileName}\"");
                File.WriteAllText(outputFileName, item.Code);
            }
        }
    }
}
