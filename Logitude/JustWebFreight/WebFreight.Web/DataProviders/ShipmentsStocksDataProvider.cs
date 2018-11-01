using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class ShipmentsStocksDataProvider
    {
        public decimal? TotlaShipment { get; set; }
      
        public List<ShipmentsStocksResult> ResultList { get; set; }
      
    }

    public class ShipmentsStocksResult
    {
        [Key]
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public decimal? TotalShipments { get; set; }
        public string ParentId { get; set; }
        public DateTime? LastShipmentDate { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }
        
    }


}