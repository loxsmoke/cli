using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using LoxSmoke.Cli.Parser;
using LoxSmoke.Cli.Generator;
using LoxSmoke.Cli.Model;
using LoxSmoke.Cli.CommandLineOptions;
using LoxSmoke.Cli.ConsoleOutput;
using LoxSmoke.Cli.Generator.Data;

namespace LoxSmoke.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var console = new ConsoleWriter();
            CliProgram.Run(args,
                (option) => Sample(option, console),
                (option) => InlineGenerate(option, console),
                (option) => Generate(option, console));
        }

        public static void Generate(GenerateOptions options, IConsoleWriter console)
        {
            if (string.IsNullOrEmpty(options.File))
            {
                options.File = "CommandLine.json";
                if (options.Verbose) console.WriteLine($"Definition file name not specified. Using '{options.File}'");
            }
            if (!File.Exists(options.File))
            {
                console.WriteError($"ERROR: Command line definition file {options.File} not found.");
                return;
            }
            
            CliDefinitionList cliDefinition;
            try
            {
                cliDefinition = JsonConvert.DeserializeObject<CliDefinitionList>(File.ReadAllText(options.File));
            }
            catch (Exception exc)
            {
                console.WriteError($"ERROR: Could not load the definition file {options.File}");
                console.WriteError(exc.Message);
                return;
            }
            Generate(cliDefinition, options.OutputPath, options.OverwriteFiles, options.Verbose, console);
        }

        public static void InlineGenerate(InlineGenerateOptions options, IConsoleWriter console)
        {
            var cliDefinition = new CliDefinitionList()
            {
                Lines = new List<string>()
                {
                    "using CommandLine;",
                    "using System;",
                    "using System.Collections.Generic;"
                },
                Definitions = new List<CliDefinition>()
                {
                    new CliDefinition()
                    {
                        Namespace = options.Namespace ?? "CommandLineOptions",
                        ClassName = options.Class ?? "CommandLineOptions",
                        Definition = options.Definition
                    }
                }
            };

            try
            {
                var definition = cliDefinition.Definitions.First();
                var parsedDefinition = ParsedDefinition.Parse(definition);
                definition.Items = parsedDefinition.ParsedItems.Select(it => it.ToDefinitionItem()).ToList();
            }
            catch { }
            WriteFile(cliDefinition, "inlineCli.json");
            Generate(cliDefinition, options.OutputPath, options.OverwriteFiles, options.Verbose, console);
        }

        public static void Sample(SampleOptions option, IConsoleWriter console)
        {
            var definition = Samples.DefaultSample.Create();
            var fileName = "SampleCommandLine.json";
            if (!string.IsNullOrEmpty(option.SampleName))
            {
                switch (option.SampleName.ToLowerInvariant())
                {
                    case "cli":
                        definition = Samples.CliSample.Create();
                        fileName = "CliCommandLine.json";
                        break;
                    case "default":
                        break;
                    default:
                        console.WriteWarning($"Unknown sample name {option.SampleName}. Generating default sample.");
                        break;
                }
            }

            if (string.IsNullOrEmpty(option.FileName))
            {
                option.FileName = fileName;
            }
            if (File.Exists(option.FileName) && !option.OverwriteSampleFile)
            {
                console.WriteError($"File \'{option.FileName}\'already exists.");
                return;
            }

            WriteFile(definition, option.FileName);
        }

        public static void Generate(
            CliDefinitionList cliDefinition,
            string outputPath,
            bool overwriteFiles, 
            bool verbose, 
            IConsoleWriter console)
        {
            ParsedDefinitionList parsedDefinitionSet;
            try
            {
                parsedDefinitionSet = ParsedDefinitionList.Parse(cliDefinition, console);
            }
            catch (Exception exc)
            {
                console.WriteError($"ERROR: Could not parse the definition.");
                console.WriteError(exc.Message);
                return;
            }

            try
            {
                var generator = new CliGenerator();
                generator.Generate(parsedDefinitionSet, 
                    new GeneratorSettings() 
                    { 
                        OverwriteFiles = overwriteFiles, 
                        Verbose = verbose,
                        OutputPath = outputPath
                    }, 
                    console);
            }
            catch (Exception exc)
            {
                console.WriteError($"ERROR: Could not generate command line classes.");
                console.WriteError(exc.Message);
                return;
            }
        }

        static void WriteFile(CliDefinitionList cliDefinitionSet, string fileName)
        {
            var json = JsonConvert.SerializeObject(cliDefinitionSet, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            File.WriteAllText(fileName, json);
        }
    }
}
