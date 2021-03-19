using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.Cli.Generator.Data
{
    public class ClassFile
    {
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string FileName { get; set; }
        public string Code { get; set; }
    }
}
