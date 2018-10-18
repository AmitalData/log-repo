using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class Direction
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

       // public List<Shipment> Shipments { get; set; }       

        ////[Include]
        ////[Association("DirectionQuote", "Id", "DirectionId")]
      //  public List<Quote> Quotes { get; set; }
    }
}
