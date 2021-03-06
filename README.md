[![NuGet version](https://badge.fury.io/nu/LoxSmoke.cli.svg)](https://badge.fury.io/nu/LoxSmoke.mddox) [![NuGet](https://img.shields.io/nuget/dt/LoxSmoke.cli.svg)](https://www.nuget.org/packages/LoxSmoke.cli) 

# cli

CLI is the .net tool that generates command line parsing classes used by 
[CommandLine](https://github.com/commandlineparser/commandline) [Nuget](https://www.nuget.org/packages/CommandLineParser/) package.
To quickly add command line parsing to the project do the following:

1. [Install](#intallation) **cli** tool.
2. Add [Commandine nuget](https://www.nuget.org/packages/CommandLineParser/) package to the project.
3. Use **cli** to generate command line parsing class (or classes).
4. And generated class files to the project.
5. Enjoy.

The following command generates two files with classes to parse the command line with one mandatory file parameter, switch and the optional value.
```bash
cli inline "<file> --switch|s [--option|-o <option_value>]"
```

The first of the two files is the command line options class file named **CommandLineOptions.cs**. This is the class that [CommandLine parser](https://github.com/commandlineparser/commandline) creates after parsing the command line. The code of the class is here:
```cs
using CommandLine;
using System;
using System.Collections.Generic;

namespace CommandLineOptions
{
    public class CommandLineOptions
    {
        [Value(0, MetaName = "file", Required = true, HelpText = "")]
        public string File { get; set; }
        [Option('s', "switch", Required = false, HelpText = "")]
        public bool Switch { get; set; }
        [Option('o', "option", Required = false, HelpText = "")]
        public string OptionValue { get; set; }
    }
}
```

The second file **CliProgram.cs** contains the **CliProgram** helper class with static **Parse(string[] args)** method and two parse and run 
methods **Run** and **RunWithReturnValue**. If program does not return the error code then the following few lines in your main() method would do the trick:
```cs
public static void Main(string[] args)
{
    CliProgram.Run(args, 
        (options) => // Parsed command line CommandLineOptions object
        { 
           /* ... your code here. ... */ 
        });
}

```

This is just a simple example with one command line options class. More complex command line parsing with multiple options classes could be generated by using specification file. For more details see below.

## Tool installation

```bash
dotnet tool install -g loxsmoke.cli
```

## Tool Uninstallation

```bash
dotnet tool uninstall -g loxsmoke.cli
```

## Usage

### Sample

Generate sample specification file that can be used as a template. Specification file is JSON file that this tool uses to 
generate command line options parsing classes. See [here](#Command-line-specification-file-format) for specification file format.

```
cli sample [-n|--name <name>] [-f|--force] [<output_file_name>]
```

Examples:
```bash
cli sample
cli sample --name cli
cli sample --name default sample.json
```

Parameters:

**\<output_file_name\>** Optional output file name.

Short format | Long format | Comment
|---|---|---|
| -**n** \<name\> | --**name** \<name\> | The name of the sample. Currently two samples are available: **default** and **cli**. **Default** sample contains all available options. **Cli** sample is the command line definition file of this utility. |
| -**f** | --**force** | Overwrite output file if one already exists. |

### Generate

Generate command line options classes from specification file. 

```
cli generate <input_file> [<output_path>] [-f|--force] [-v|--verbose]
```
Examples:
```bash
cli generate Specification.json
cli generate Specification.json myapp/generatedCode -f
```
Parameters:

**\<input_file\>** Command line specification file.
**\<output_path\>** The optional path for generated class files. 

Short format | Long format | Comment
|---|---|---|
| -**f** | --**force** | Overwrite output files if they already exist. |   
| -**v** | --**verbose** | Show more details when generating code. |

### Inline

Generate command line options classes without using specification file. Suitable for simple utilities that do not use multiple verbs. This command also creates specification file that can be edited later.

```
cli inline <definition> [<output_path>] [-n|--namespace <namespace>] [-c|--class <class_name>] [-f|--force] [-v|--verbose]
```
Examples:
```bash
cli inline "[--switch] <value>"
cli inline "[--switch] <value>" --namespace MyNamespace --class CommandLineOptions
```
Parameters:

**\<definition\>** Command line definition. See [here](#command-line-definition) for details.
**\<output_path\>** The optional path for generated class files. 

Short format | Long format | Comment
|---|---|---|
| -**n** \<namespace\> | --**namespace** \<namespace\> | The namespace of the generated command line options class. |
| -**c** \<class_name\> | --**class** \<class_name\> | The name of the command line options class. |
| -**f** | --**force** | Overwrite output files if they already exist. |   
| -**v** | --**verbose** | Show more details when generating code. |

## Command line specification file format
Command line specification file is JSON file. It can be created manually but it is easier to use the sample definition file generated by the **CLI** tool using **sample** option. 
This is the basic structure of the specification file:

```json
{
  "description": "Sample command line definition",
  "namespace": "Sample.CommandLine",
  "lines": [ 
    "using System;"
  ],
  "definitions": [
    {
      "definition": "[optional_verb] <value> [<optional_value>]",
      "namespace": "Sample.CommandLine.Verbs",
      "className": "OptionalVerbCommandLine",
      "items": [
        {
          "verb": "optional_verb",
          "helpText": "Optional verb."
        },
        {
          "valueName": "value",
          "helpText": "Some value that must be present"
        },
        {
          "valueName": "optional_value",
          "helpText": "Some value that may be omitted. There can be at most one optional value and it must be the last value"
        }
      ]
    }
  ]
}
```

Detailed documentation of objects and fields of the specification file is available [here](clidefinition.md).

### Command line definition string

Command line definition is the **definitions[].definition** field in the specification JSON file or a parameter of **inline** command.

Command line definition string is the list of items. There are four types of items:
1. Verb. This is an optional element that must be the first element in the command line. It is an alphanumeric identifier. For example **status** as in `git status` command is the verb.
2. Switch. Another optional element that starts with **--** for long name switch or **-** for short name. For example **--activate** or **-a**. 
3. Scalar option is a switch followed by the value. For example **--name \<example\>** or **-n \<name\>**. Three trailing dots "..." after the value name indicate that multiple values are allowed. For example **--name \<example...\>**
4. Value option is a string that is enclosed in angle brackets as **\<value_option\>**. Only the last value option in the command line can be optional.

The table below shows uses of each item in the command line definition string.

Element | Example
|---|---|
| Single verb | **status** |
| Multiple alternative verbs | **status\|view\|delete** |
| Long name switch | **--activate** |
| Short name switch | **-a** |
| Long and short name switch | **--activate\|a** |
| Long named scalar option | **--name \<name\>** |
| Multiple long named scalar options | **--name \<name...\>** |
| Short named scalar option | **-n \<name\>** |
| Long and short named scalar option | **--name\|n \<name\>** |
| Value option | **\<value\>** | 
| Optional value | **[\<value\>]** |
 

## Generated code

Cli tool generates one parsing helper class (default name **CliProgram**) and one or more command line options classes. 
The parsing helper class has the following methods:

**public static ParserResult\<object\> Parse(string[] args)**

This method parses command line and returns ParserResult which is either an error or one of generated command line options classes.


**public static void Run(string[] args, Action\<GeneratedOptionsClass1\> handler1, Action\<GeneratedOptionsClass2\> handler2, ... Action\<IEnumerable\<Error\>\> errorHandler = null)**

This method parses command line and executed the handler for the corresponding command line option class. Cli generates separate class for each verb that is used. If there are no verbs in command line definitions then only one class is generated and only one handler is needed.


**public static int RunWithReturnValue(string[] args, Func\<GeneratedOptionsClass1, int\> handler1, Func\<GeneratedOptionsClass2, int\> handler2, ..., Func\<IEnumerable\<Error\>, int\> errorHandler = null)**

This method works the same as method above. It requires Func handlers that return int value. This value can be later returned from the program main() function. It could be useful for command line utilities used in batch files that check error codes.


