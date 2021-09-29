using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DocumentTests.Models
{
    public class DocumentTypeCopyPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DocumentTypeId { get; set; }
        public int IndexOrder { get; set; }
        public bool IsSelectedByDefault { get; set; }
        public bool InActive { get; set; }
        public bool HasDocumentOutCopy { get; set; }
        public bool IsOriginal { get; set; }
    }
}
