using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    [DataContract]
    public class ShipmentApiFilters
    {
        [DataMember]
        public int Skip { get; set; }
        [DataMember]
        public int Take { get; set; }
        [DataMember]
        public bool MyShipments { get; set; }
        [DataMember]
        public bool OperationallyOpen { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string SearchFields { get; set; }

        [DataMember]
        public string TransPortMod { get; set; }

        [DataMember]
        public string Direction { get; set; }

        [DataMember]
        public string ShipmentLevel { get; set; }
    }
}