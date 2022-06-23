using CodeHelper.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeHelper.Service.Modules.Default.Search
{
    public class Query
    {
        public Query()
        {
            Diagnostic = new List<DiagnosticOut>();
        }

        public string ID { get; set; }
        public string Descriprion { get; set; }
        public string Link { get; set; }
        public List<DiagnosticOut> Diagnostic { get; set; }
        public bool IsInitialized => Diagnostic.Count > 0;
    }
}
