using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Generator.CodeMonkey
{
    public class TextCodeMonkey
    {
        protected StringBuilder text = new StringBuilder();
        public string Code => text.ToString();

        int tabSpaces = 4;
        public int TabSpaces 
        {
            get => tabSpaces;
            set 
            {
                tabSpaces = Math.Max(value, 0);
                Indent = indent;
            }
        }

        protected string indentText = "";
        int indent = 0;
        public int Indent
        {
            get => indent;
            set
            {
                indent = Math.Max(value, 0);
                indentText = new string(' ', indent * tabSpaces);
            }
        }

        public void Clear()
        {
            Indent = 0;
            indentText = "";
            text.Clear();
        }

        public void WriteLines(List<string> lines)
        {
            if (lines == null || lines.Count == 0) return;
            foreach (var line in lines) text.AppendLine(indentText + line);
        }

        public void WriteLine(string line)
        {
            text.Append(indentText);
            text.AppendLine(line);
        }

        public void WriteLine()
        {
            text.AppendLine();
        }
    }
}
