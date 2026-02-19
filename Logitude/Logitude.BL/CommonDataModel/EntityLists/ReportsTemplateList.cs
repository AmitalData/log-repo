using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
   public class ReportsTemplateList
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ReportId { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string CreatedByUserId { get; set; }
        public int CurrentVersion { get; set; }
        public bool IsSystem { get; set; }
        public bool InActive { get; set; }
        public string UpdateByUserName { get; set; }
        public bool IsDefault { get; set; }
        public string TemplateType { get; set; }

        public string From { get; set; }
        public string ReplyTo { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string ObjectTableId { get; set; }
        public string EntityId { get; set; }
    }
}
