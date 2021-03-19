using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Model
{
    /// <summary>
    /// The item of the Definitions list. Describes one command line options class. 
    /// If definition contains more than one verb then
    /// code generator creates separate classes for each verb.
    /// </summary>
    public class CliDefinition
    {
        /// <summary>
        /// Command line definition string. String contains the list of 
        /// command line items separated by spaces. 
        /// Each item in this string may have additional data like help text and property name
        /// in the items list.
        /// Examples:
        /// Verb and switch <c>"verb --switch"</c>
        /// Verb and value <c>"verb &lt;value&gt;"</c>
        /// Switch and scalar option <c>"--switch --option &lt;value&gt;"</c>
        /// </summary>
        public string Definition { get; set; }
        /// <summary>
        /// Namespace for the generated command line class.
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// The generated command line options class name. 
        /// If more than one verb is present in definition then verb is appended to the 
        /// name of the class.
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Details of the command line items from the definition string. 
        /// Each verb, switch and value can be described here. 
        /// If list is empty then generator creates property names 
        /// based on definition string without any help text.
        /// </summary>
        public List<CliDefinitionItem> Items { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Namespace ?? ""}.{ClassName ??""} \"{Definition ?? ""}\"";
        }
    }
}
