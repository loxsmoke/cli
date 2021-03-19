using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Parser
{
    /// <summary>
    /// Definition parsing exception.
    /// </summary>
    public class ParseError : Exception
    {
        /// <summary>
        /// The definition string that was parsed.
        /// </summary>
        public string DefinitionString { get; set; }

        public ParseError(string message) : base(message)
        { }

        public ParseError(string message, string definitionString) : base(message)
        {
            DefinitionString = definitionString;
        }
    }
}
