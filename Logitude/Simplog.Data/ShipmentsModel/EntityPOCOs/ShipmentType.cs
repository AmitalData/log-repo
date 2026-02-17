using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentType
    {
        [Key]
        public string Id { get; set; }
        
        public string Name { get; set; }
        public string TransportModeId { get; set; }
        public string SearchFields { get; set; }

        ////[Include]
        ////[Association("ShipmentShipmentType", "Id", "ShipmentTypeId")]
        //public virtual List<Shipment> Shipments { get; set; }

        [ForeignKey("TransportModeId")]
        public virtual TransportMode TransportMode { get; set; }

        //public virtual List<Quote> Quotes { get; set; }


    }
}