using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.Helpers.DataProviderHelpers
{
    public class ShipmentsEventsListDataProvider: BaseDataProvider
    {
       public List<ShipmentEventsList> ShipmentEventsLists { get; set; }
    }

    public class ShipmentEventsList
    {
        public string ShipmentNumber { get; set; }
        public string EventCode { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime LogDate { get; set; }
        public string UserName { get; set; }
        public string Notes { get; set; }
    }
}