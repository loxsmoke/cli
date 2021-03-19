using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Model
{
    /// <summary>
    /// The "root" object of the command line definition file.
    /// </summary>
    public class CliDefinitionList
    {
        /// <summary>
        /// Description of the definition file. 
        /// This value is written as a summary comment before each command line options class.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Optional default namespace for command line options classes. 
        /// Each command line definition may override this namespace with a different value. 
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// The name of the class with static parsing methods. 
        /// "CliProgram" is used if string is empty or not specified.
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Optional list of lines to add at the beginning of each generated code file. 
        /// Usually some "using" statements or common copyright info.
        /// </summary>
        public List<string> Lines { get; set; }
        /// <summary>
        /// The list of command line definitions. One or more command line options classes 
        /// can be generated from each definition. 
        /// </summary>
        public List<CliDefinition> Definitions { get; set; }
    }
}
