using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TariffCarrierTranslation
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

        public virtual Port Port { get; set; }
        public virtual Card Carrier { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User UpdatedByUser { get; set; }
    }
}
