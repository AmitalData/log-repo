using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    
    
   public class AgentSharedManifestList
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
        public string TransportModeId { get; set; }
        public double? GrossWeight { get; set; }
        public double? ChargeableWeight { get; set; }
        public double? TEU { get; set; }
        public int? PackagesQuantity { get; set; }
        public string TransportModeName { get; set; }
        public string DirectionName { get; set; }
        public string ShipmentLevelName { get; set; }
        public string AgentName { get; set; }
        public string AgentId { get; set; }
        public string DirectionId { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string StatusCode { get; set; }
        public string Routing { get; set; }
        public string StatusName { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string ShipmentTypeId { get; set; }









    }
}
