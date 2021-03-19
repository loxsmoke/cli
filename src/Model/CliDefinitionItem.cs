using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace LoxSmoke.Cli.Model
{
    /// <summary>
    /// Detailed information of the command line item such as verb, switch, value or scalar option.
    /// </summary>
    public class CliDefinitionItem
    {
        /// <summary>
        /// Not empty string if this is a verb definition. If verb field is not empty then other fields 
        /// like Option or ValueName are ignored.
        /// Value should match the verb in the command line definition string.
        /// Example: <c>status --switch</c>  the Verb value should be  <c>status</c>
        /// </summary>
        public string Verb { get; set; }
        /// <summary>
        /// Option can be either switch when it has no associated value as in <c>--fire</c>
        /// or scalar as in <c>--fire canon</c>. 
        /// Value in this field should match the name in the command line definition string.
        /// If option has both long and short variants like <c>-c</c> and <c>--cancel</c> then 
        /// both should be present here.
        /// Examples:
        /// Bool property <c>--option|o</c> the Option value should be <c>option|o</c> 
        /// Bool property <c>--option</c> the Option value should be <c>option</c> 
        /// String property <c>--option &lt;value&gt;</c> the Option value should be <c>option</c>
        /// 
        /// The generated command line options class would have bool property for each switch option. 
        /// For scalar option value is either string property or IEnumerable&lt;string&gt; depending 
        /// on the name used in the command line definition. For example <c>-x &lt;prop&gt;</c> definition 
        /// will become a string property and <c>-x &lt;prop...&gt;</c> will be the IEnumerable&lt;string&gt;.
        /// </summary>
        public string Option { get; set; }
        /// <summary>
        /// Not empty string if this is a value name. 
        /// Example: <c>&lt;file_name&gt;</c> the ValueName should be <c>file_name</c>
        /// </summary>
        public string ValueName { get; set; }
        /// <summary>
        /// Optional help text of this command line item.
        /// </summary>
        public string HelpText { get; set; }
        /// <summary>
        /// Optional property name. 
        /// If not specified then the name is generated from verb, option or value name.
        /// </summary>
        public string PropertyName { get; set; }

        #region Type based on fields
        [JsonIgnore]
        public bool IsVerb => !string.IsNullOrEmpty(Verb);
        [JsonIgnore]
        public bool IsSwitchOrScalar => !IsVerb && !string.IsNullOrEmpty(Option);
        [JsonIgnore]
        public bool IsValueOption => !IsVerb && !IsSwitchOrScalar && !string.IsNullOrEmpty(ValueName);
        #endregion

        /// <summary>
        /// Return the short string representation of this object.
        /// This string halps to find the corresponding items in the command line definition string.
        /// </summary>
        /// <returns></returns>
        [JsonIgnore]
        public string NameString
        {
            get
            {
                if (IsVerb) return Verb;
                if (IsSwitchOrScalar) return Option;
                if (IsValueOption) return ValueName;
                return $"Unknown: v={Verb ?? ""} o={Option ?? ""} n={ValueName ?? ""}";
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var typeName = "";
            if (IsVerb) typeName = "Verb: ";
            else if (IsSwitchOrScalar) typeName = "Option: ";
            else if (IsValueOption) typeName = "Value: ";

            return typeName + NameString +
                (string.IsNullOrEmpty(PropertyName) ? "" : $" Property={PropertyName}") +
                (string.IsNullOrEmpty(HelpText) ? "" : $" Help=\"{HelpText}\"");
        }

        /// <summary>
        /// Normalize some types of command line items. 
        /// Trim spaces, remove dashes, etc.
        /// </summary>
        public void Normalize()
        {
            if (IsSwitchOrScalar)
            {
                if (Option.Contains('|'))
                {
                    Option = string.Join('|',
                        Option.Trim().Split('|', StringSplitOptions.RemoveEmptyEntries)
                        .Select(it => it.Trim('-'))
                        .Where(it => it.Length > 0)
                        .OrderByDescending(it => it.Length));
                }
                else
                {
                    Option = Option.Trim().Trim('-');
                }
            }
            else if (IsValueOption)
            {
                ValueName = ValueName.Trim().TrimStart('<').TrimEnd('>').TrimEnd('.');
            }
            else if (IsVerb)
            {
                Verb = Verb.Trim();
            }
        }
    }
}
