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
    public class AgentSharedDocument
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string AgentReference { get; set; }
        public string AgentSharedManifestRef { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string DocumentXML { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string AgentId { get; set; }
        public string StatusCode { get; set; }

        [ForeignKey("StatusCode")]
        public virtual SharedManifestsStatus SharedManifestsStatus { get; set; }

        [ForeignKey("AgentId")]
        public virtual Agent Agent { get; set; }

        [ForeignKey("ShipmentLevelCode")]
        public virtual ShipmentLevel ShipmentLevel { get; set; }

    }
}
