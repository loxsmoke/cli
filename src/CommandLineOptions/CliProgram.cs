using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;
using LoxSmoke.Cli.CommandLineOptions;

namespace LoxSmoke.Cli.CommandLineOptions
{
    public class CliProgram
    {
        public static ParserResult<object>
            Parse(string[] args)
        {
            return CommandLine.Parser.Default
                .ParseArguments<SampleOptions,InlineGenerateOptions,GenerateOptions>(args);
        }
        public static void Run(
            string[] args,
            Action<SampleOptions> SampleOptionsHandler,
            Action<InlineGenerateOptions> InlineGenerateOptionsHandler,
            Action<GenerateOptions> GenerateOptionsHandler,
            Action<IEnumerable<Error>> errorHandler = null)
        {
            Parse(args)
                .WithParsed(SampleOptionsHandler)
                .WithParsed(InlineGenerateOptionsHandler)
                .WithParsed(GenerateOptionsHandler)
                .WithNotParsed(errorHandler ?? ((_) => { }));
        }
        public static int RunWithReturnValue(
            string[] args,
            Func<SampleOptions, int> SampleOptionsHandler,
            Func<InlineGenerateOptions, int> InlineGenerateOptionsHandler,
            Func<GenerateOptions, int> GenerateOptionsHandler,
            Func<IEnumerable<Error>, int> errorHandler = null)
        {
            var returnCode = 0;
            Parse(args)
                .WithParsed<SampleOptions>((x) => returnCode = SampleOptionsHandler(x))
                .WithParsed<InlineGenerateOptions>((x) => returnCode = InlineGenerateOptionsHandler(x))
                .WithParsed<GenerateOptions>((x) => returnCode = GenerateOptionsHandler(x))
                .WithNotParsed((x) => { if (errorHandler != null) returnCode = errorHandler(x); });
            return returnCode;
        }
    }
}
