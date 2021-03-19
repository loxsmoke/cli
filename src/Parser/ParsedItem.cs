using LoxSmoke.Cli.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Parser
{
    public abstract class ParsedItem
    {
        /// <summary>
        /// Optional item information from definition file. Used by code generator.
        /// </summary>
        public CliDefinitionItem DefinitionItem { get; set; }
        
        /// <summary>
        /// True if this item is optional in the command line.
        /// </summary>
        public bool IsOptional { get; set; }

        /// <summary>
        /// Check if definitionIten describes the same item. Eg. switch with the same name or the same verb.
        /// </summary>
        /// <param name="definitionItem"></param>
        /// <returns>If item is the same definition type and name</returns>
        public abstract bool SameItem(CliDefinitionItem definitionItem);

        /// <summary>
        /// Create definition item from parsed item.
        /// </summary>
        /// <returns></returns>
        public abstract CliDefinitionItem ToDefinitionItem();
    }
}
