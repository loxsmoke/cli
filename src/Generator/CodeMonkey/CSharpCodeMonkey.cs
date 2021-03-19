using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoxSmoke.Cli.Generator.CodeMonkey
{
    public class CSharpCodeMonkey : TextCodeMonkey
    {
        public void WriteSummaryComment(string comment)
        {
            WriteLines(GenerateSummaryCommentLines(comment));
        }

        public void WriteNamespace(string nameSpace)
        {
            WriteLine($"namespace {nameSpace}");
            WriteLine("{");
            Indent++;
        }

        public void WriteClass(string className)
        {
            WriteLine($"public class {className}");
            WriteLine($"{{");
            Indent++;
        }

        public void OpeningBracket()
        {
            WriteLine("{");
            Indent++;
        }
        public void ClosingBracket()
        {
            Indent--;
            WriteLine("}");
        }

        public void WriteAttribute(string attributeName, params (string attrParamName, object attrValue)[] values)
        {
            if (values.Length == 0) WriteLine($"[{attributeName}]");
            else WriteLine($"[{attributeName}({string.Join(", ", values.Select(v => ToString(v.attrParamName, v.attrValue)))})]");
        }

        public void WriteProperty(string type, string name)
        {
            WriteLine($"public {type} {name} {{ get; set; }}");
        }

        public static string ToString(string attrParamName, object obj)
        {
            var text = string.IsNullOrEmpty(attrParamName) ? "" : (attrParamName + " = ");
            if (obj is string s) text += $"\"{s.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"")}\"";
            else if (obj is char c) text += $"\'{c}\'";
            else if (obj is bool b) text += b ? "true" : "false";
            else text += obj.ToString();
            return text;
        }

        public static List<string> GenerateSummaryCommentLines(string classComment)
        {
            if (string.IsNullOrEmpty(classComment)) return null;
            var list = new List<string>() { "/// <summary>" };
            if (classComment.Contains(Environment.NewLine) ||
                classComment.Contains('\n'))
            {
                list.AddRange(classComment.Replace(Environment.NewLine, "\n").Split("\n").Select(txt => "/// " + txt));
            }
            else list.Add("/// " + classComment);
            list.Add("/// </summary>");
            return list;
        }
    }
}
