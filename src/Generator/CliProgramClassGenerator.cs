using LoxSmoke.Cli.Generator.CodeMonkey;
using LoxSmoke.Cli.Generator.Data;
using LoxSmoke.Cli.Generator.Interfaces;
using LoxSmoke.Cli.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoxSmoke.Cli.Generator
{
    public class CliProgramClassGenerator : CSharpCodeMonkey, IProgramClassGenerator
    {
        public ClassFile GenerateClass(
            GeneratorSettings settings,
            ParsedDefinitionList definitionList,
            List<ClassFile> cliClasses)
        {
            Clear();
            TabSpaces = settings.TabSpaces;

            WriteLines(definitionList.Lines);
            foreach (var ns in cliClasses.Select(x => x.Namespace).Distinct()) WriteLine($"using {ns};");
            WriteLine();
            WriteNamespace(definitionList.Namespace);
            WriteSummaryComment(definitionList.Description);
            WriteClass(definitionList.ClassName);
            var resultType = cliClasses.Count == 1 ? cliClasses.First().ClassName : "object";
            WriteLine($"public static ParserResult<{resultType}>");
            WriteLine("    Parse(string[] args)");
            OpeningBracket();
            WriteLine("return CommandLine.Parser.Default");
            Indent++;
            WriteLine($".ParseArguments<{string.Join(',', cliClasses.Select(cl => cl.ClassName))}>(args);");
            Indent--;
            ClosingBracket();

            WriteLine("public static void Run(");
            Indent++;
            WriteLine("string[] args,");
            foreach (var item in cliClasses)
            {
                WriteLine($"Action<{item.ClassName}> {item.ClassName}Handler,");
            }
            WriteLine("Action<IEnumerable<Error>> errorHandler = null)");
            Indent--;
            OpeningBracket();
            WriteLine("Parse(args)");
            Indent++;
            foreach (var item in cliClasses)
            {
                WriteLine($".WithParsed({item.ClassName}Handler)");
            }
            WriteLine(".WithNotParsed(errorHandler ?? ((_) => { }));");
            Indent--;
            ClosingBracket();

            WriteLine("public static int RunWithReturnValue(");
            Indent++;
            WriteLine("string[] args,");
            foreach (var item in cliClasses)
            {
                WriteLine($"Func<{item.ClassName}, int> {item.ClassName}Handler,");
            }
            WriteLine("Func<IEnumerable<Error>, int> errorHandler = null)");
            Indent--;
            OpeningBracket();
            WriteLine("var returnCode = 0;");
            WriteLine("Parse(args)");
            Indent++;
            foreach (var item in cliClasses)
            {
                WriteLine($".WithParsed<{item.ClassName}>((x) => returnCode = {item.ClassName}Handler(x))");
            }
            WriteLine(".WithNotParsed((x) => { if (errorHandler != null) returnCode = errorHandler(x); });");
            Indent--;
            WriteLine("return returnCode;");
            ClosingBracket();
            ClosingBracket();
            ClosingBracket();

            return new ClassFile()
            {
                Namespace = definitionList.Namespace, 
                ClassName = definitionList.ClassName, 
                FileName = definitionList.ClassName + ".cs", 
                Code = Code
            };
        }
    }
}
