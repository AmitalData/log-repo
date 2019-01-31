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
   public class AgentSharedManifest
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string AgentReference { get; set; }
        public string Master { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string ManifestXML { get; set; }
        public string SearchFields { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string ShipmentTypeId { get; set; }
        public string TransportModeId { get; set; }
        public double? GrossWeight { get; set; }
        public double? ChargeableWeight { get; set; }
        public double? TEU { get; set; }
        public int? PackagesQuantity { get; set; }

        public string AgentId { get; set; }
        public string DirectionId { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string StatusCode { get; set; }

        [ForeignKey("StatusCode")]
        public virtual SharedManifestsStatus SharedManifestsStatus { get; set; }

        [ForeignKey("FromPortId")]
        public virtual Port FromPort { get; set; }

        [ForeignKey("ToPortId")]
        public virtual Port ToPort { get; set; }


        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("ShipmentLevelCode")]
        public virtual ShipmentLevel ShipmentLevel { get; set; }

        [ForeignKey("AgentId")]
        public virtual Agent Agent { get; set; }


        [ForeignKey("ShipmentTypeId")]
        public virtual ShipmentType ShipmentType { get; set; }

    }
}
