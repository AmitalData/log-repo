using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityPMs
{
    public class WebhookKeysList
    {
        
       [Key]
        public string Id { get; set; }
        public string AccessKey { get; set; }
        public int Tenant { get; set; }
        public string PartnerName { get; set; }
        public bool InActive { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdatedByUserName { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Description { get; set; }

    }
}
