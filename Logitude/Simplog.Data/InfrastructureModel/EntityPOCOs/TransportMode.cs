using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class TransportMode
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }
        public string LocalName { get; set; }

        //public List<Shipment> Shipments { get; set; }

        //public List<ShipmentType> ShipmentTypes { get; set; }


        //public List<Shipment> PreCarriageShipments { get; set; }

        //public List<Shipment> OnCarriageShipments { get; set; }





        //public List<MoveType> MoveTypes { get; set; }


        ////[Include]
        ////[Association("TransportModeQuote", "Id", "TransportModeId")]
        //public List<Quote> Quotes { get; set; }


    }
}
