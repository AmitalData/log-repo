using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{
    public class DocumentOutCopyArgs
    {
        public string DocumentTypeCopyId { get;  set; }
        public string DocumentTypeId { get;  set; }
        public string EntityId { get;  set; }
        public string ObjectTableId { get;  set; }
        public int Tenant { get; set; }
        public string DocumentTemplateId { get; set; }
        public string ChildEntityId { get; set; }
    }
}
