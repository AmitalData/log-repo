using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ContainerTruckingDataProvider : BaseDataProvider
    {
        public string CustomerFilter { get; set; }
        public string DirectionFilter { get; set; }
        public string AgentFilter { get; set; }

        public List<ContainerTruckingRecord> RecordsList { get; set; }
    }

    public class ContainerTruckingRecord
    {
        public string AgentName { get; set; }
        public string FBLNumber { get; set; }
        public string FileNumber { get; set; }
        public string ContainerNumber { get; set; }
        public string ShippingLine { get; set; }
        public DateTime? DeliveryATD { get; set; }
        public DateTime? DeliveryATA { get; set; }
        public string DischargePort { get; set; }
        public string TEU { get; set; }
    }
}