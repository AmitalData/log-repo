using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ReportTests.Models
{
    public class ReportFliterItem
    {
        public string FieldName { get; set; }
        public object FieldValue { get; set; }
        public string FieldDataType { get; set; }
        public string Operator { get; set; }
    }
}
