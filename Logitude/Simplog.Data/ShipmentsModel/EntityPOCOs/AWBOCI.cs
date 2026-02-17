using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class AWBOCI
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string CountryId { get; set; }
        public string AWBCustomsInformationCode { get; set; }
        public string AWBInformationCode { get; set; }
        public string SupplementaryCustomsInfo  { get; set; }

        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        [ForeignKey("AWBCustomsInformationCode")]
        public virtual AWBCustomsInformation AWBCustomsInformation { get; set; }

        [ForeignKey("AWBInformationCode")]
        public virtual AWBInformation AWBInformation { get; set; }
        
    }
}
