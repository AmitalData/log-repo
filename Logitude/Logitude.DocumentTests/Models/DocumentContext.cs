using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DocumentTests.Models
{
    public class DocumentContext
    {
        public List<DocumentTypeList> DocumentTypes { get; internal set; }
        public object Document { get; internal set; }
    }
}
