using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class MoveType
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        public string MoveTypeEnglishName { get; set; }

        public string MoveTypeLocalName { get; set; }

        public string TransportModeId { get; set; }

        public bool AddedManually { get; set; }

        public bool InActive { get; set; }

        public string Code { get; set; }

        public string SearchFields { get; set; }

        //[Include]
        //[Association("MoveTypeTransportMode", "TransportModeId", "Id", IsForeignKey = true)]
        [ForeignKey("TransportModeId")]
        public TransportMode TransportMode { get; set; }        

        //public List<Shipment> Shipments { get; set; }
        //public List<Shipment> MoveTypeShipments { get; set; }
    }
}