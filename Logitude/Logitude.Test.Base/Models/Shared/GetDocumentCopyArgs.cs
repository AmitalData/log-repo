using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Test.Base.Models.Shared
{
    public class GetDocumentCopyArgs
    {
        public string DocumentTypeId { get; set; }
        public string EntityId { get; set; }
        public string EntityObjectTableId { get; set; }
        public string DocumentOutId { get; set; }
        public int Tenant { get; set; }
        public string DocumentTypeCopyId { get; set; }
        public string UserId { get; set; }
    }
}
