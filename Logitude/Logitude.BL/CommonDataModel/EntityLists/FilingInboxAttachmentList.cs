using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class FilingInboxAttachmentList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string FileName { get; set; }
        public string DocumentId { get; set; }
        public string FilingInboxId { get; set; }
    }
}
