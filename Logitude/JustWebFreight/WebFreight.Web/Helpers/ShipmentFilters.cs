using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ShipmentFilters
    {
        public string PartnerId { get; set; }
        public string PartnerType { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public bool? IsOperationalClosed { get; set; }
        public string SearchField { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public bool IsShipmentTracking { get; set; }
        public string QueryType { get; set; }
        public string ContactId { get; set; }

    }
}