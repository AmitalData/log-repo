using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.Helpers.BatchPrint
{
    public class BatchPrintManagerArgs
    {
        public string DocumentId { get; set; }
        public string TemplateId { get; set; }
        public string CopyId { get; set; }
        public string ObjectTableId { get; set; }
        public int Tenant { get; set; }
        public List<PrintEntityKeys> EntityIds { get; set; }
        public string ChildObjectTableId { get; set; }
        public string Email { get; set; }
    }
    public class PrintEntityKeys
    {
        public string EntityId { get; set; }
        public string ChildEntityId { get; set; }

    }
}
