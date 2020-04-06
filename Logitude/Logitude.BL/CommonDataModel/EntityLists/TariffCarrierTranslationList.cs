using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class TariffCarrierTranslationList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PartnerCode { get; set; }
        public string PortId { get; set; }
        public string CarrierId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string SearchFields { get; set; }
    }
}
