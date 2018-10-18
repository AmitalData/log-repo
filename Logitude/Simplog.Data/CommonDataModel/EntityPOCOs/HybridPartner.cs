using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class HybridPartner
    {
        [Key]
        public string Id { get; set; }
        //public int Tenant { get; set; }
        public string Name { get; set; }
        public int PartnerTenant { get; set; }
        public string LocalName { get; set; }

        public string LogoId { get; set; }
        [ForeignKey("LogoId")]
        public virtual ImageDetail ImageDetail { get; set; }

        public string SmallLogoId { get; set; }
        [ForeignKey("SmallLogoId")]
        public virtual ImageDetail ImageDetail1 { get; set; }



        public string SearchFields { get; set; }

        public bool IsMislakaActivated { get; set; }
        public bool InActive { get; set; }
        public bool IsExternalPartner { get; set; }
        public bool ReceiveAllStatuses { get; set; }
        public bool AllowSendingDocsToAgent { get; set; }
        //public virtual ICollection<Shipment> Shipments { get; set; }

    }
}
