using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeHelper.Service.Core
{
    public class DiagnosticOut
    {
        public int? Line { get; set; }
        public string? Code { get; set; }
        public string? Source { get; set; }
        public string? Message { get; set; }
    }
}
