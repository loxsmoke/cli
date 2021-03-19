# CLI data model reference

Created by 
[mddox](https://github.com/loxsmoke/mddox) on 3/13/2021

# All types

|   |   |   |
|---|---|---|
| [CliDefinitionList Class](#clidefinitionlist-class) | [CliDefinition Class](#clidefinition-class) | [CliDefinitionItem Class](#clidefinitionitem-class) |
# CliDefinitionList Class

Namespace: LoxSmoke.Cli.Model

The "root" object of the command line definition file.

## Properties

| Name | Type | Summary |
|---|---|---|
| **Description** | string | Description of the definition file. <br>This value is written as a summary comment before each command line options class. |
| **Namespace** | string | Optional default namespace for command line options classes. <br>Each command line definition may override this namespace with a different value. |
| **ClassName** | string | The name of the class with static parsing methods. <br>"CliProgram" is used if string is empty or not specified. |
| **Lines** | List\<string\> | Optional list of lines to add at the beginning of each generated code file. <br>Usually some "using" statements or common copyright info. |
| **Definitions** | List\<[CliDefinition](#clidefinition-class)\> | The list of command line definitions. One or more command line options classes <br>can be generated from each definition. |
# CliDefinition Class

Namespace: LoxSmoke.Cli.Model

The item of the Definitions list. Describes one command line options class. 
If definition contains more than one verb then
code generator creates separate classes for each verb.

## Properties

| Name | Type | Summary |
|---|---|---|
| **Definition** | string | Command line definition string. String contains the list of <br>command line items separated by spaces. <br>Each item in this string may have additional data like help text and property name<br>in the items list.<br>Examples:<br>Verb and switch `"verb --switch"`<br>Verb and value `"verb <value>"`<br>Switch and scalar option `"--switch --option <value>"` |
| **Namespace** | string | Namespace for the generated command line class. |
| **ClassName** | string | The generated command line options class name. <br>If more than one verb is present in definition then verb is appended to the <br>name of the class. |
| **Items** | List\<[CliDefinitionItem](#clidefinitionitem-class)\> | Details of the command line items from the definition string. <br>Each verb, switch and value can be described here. <br>If list is empty then generator creates property names <br>based on definition string without any help text. |
# CliDefinitionItem Class

Namespace: LoxSmoke.Cli.Model

Detailed information of the command line item such as verb, switch, value or scalar option.

## Properties

| Name | Type | Summary |
|---|---|---|
| **Verb** | string | Not empty string if this is a verb definition. If verb field is not empty then other fields <br>like Option or ValueName are ignored.<br>Value should match the verb in the command line definition string.<br>Example: `status --switch`  the Verb value should be  `status` |
| **Option** | string | Option can be either switch when it has no associated value as in `--fire`<br>or scalar as in `--fire canon`. <br>Value in this field should match the name in the command line definition string.<br>If option has both long and short variants like `-c` and `--cancel` then <br>both should be present here.<br>Examples:<br>Bool property `--option\|o` the Option value should be `option\|o` <br>Bool property `--option` the Option value should be `option` <br>String property `--option <value>` the Option value should be `option`<br><br>The generated command line options class would have bool property for each switch option. <br>For scalar option value is either string property or IEnumerable<string> depending <br>on the name used in the command line definition. For example `-x <prop>` definition <br>will become a string property and `-x <prop...>` will be the IEnumerable<string>. |
| **ValueName** | string | Not empty string if this is a value name. <br>Example: `<file_name>` the ValueName should be `file_name` |
| **HelpText** | string | Optional help text of this command line item. |
| **PropertyName** | string | Optional property name. <br>If not specified then the name is generated from verb, option or value name. |
