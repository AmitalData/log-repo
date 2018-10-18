using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ComputingPartnerTranslationList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string ComputingPartnerId { get; set; }
        public string OurCode { get; set; }
        public string PartnerCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserName { get; set; }
        public string CreatedByUserName { get; set; }
        public string ObjectTableName { get; set; }
        public string ComputingPartnerName { get; set; }
        public string SearchFields { get; set; }

    }
}
