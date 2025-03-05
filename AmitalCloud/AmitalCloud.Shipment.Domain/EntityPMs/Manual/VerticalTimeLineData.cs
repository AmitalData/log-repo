using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AmitalCloud.Shipment.Domain.EntityPMs
{

    [JsonObject(IsReference = false)]
    public class VerticalTimeLineData
    {
        [DataMember]
        public VerticalTimeLineStop Pickup { get; set; }
        public VerticalTimeLineStop MainCarriageFrom { get; set; }
        [DataMember]
        public VerticalTimeLineStop MainCarriageTo { get; set; }
        [DataMember]
        public VerticalTimeLineStop Delivery { get; set; }
        [DataMember]
        public VerticalTimeLineStop Transshipment1 { get; set; }
        [DataMember]
        public VerticalTimeLineStop Transshipment2 { get; set; }
        [DataMember]
        public VerticalTimeLineStop Transshipment3 { get; set; }
        [DataMember]
        public VerticalTimeLineStop PreCarriage { get; set; }
        [DataMember]
        public VerticalTimeLineStop OnCarriage { get; set; }
        [DataMember]
        public VerticalTimeLineStop Warehouse1 { get; set; }
        [DataMember]
        public VerticalTimeLineStop Warehouse2 { get; set; }

    }

    [JsonObject(IsReference = false, ItemIsReference = false)]
    public class VerticalTimeLineStop
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public string CountryCode { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public DateTime? ATDDate { get; set; }
        [DataMember]
        public string ATDDateType { get; set; }
        [DataMember]
        public DateTime? Date { get; set; }
        [DataMember]
        public string DateType { get; set; }
        [DataMember]
        public DateTime? ATADate { get; set; }
        [DataMember]
        public string ATADateType { get; set; }
        [DataMember]
        public bool IsViaPortsDatesFilled { get; set; }
        [DataMember]
        public string TransportModeId { get; set; }
        public string LegTransportModeId { get; set; }
        [XmlIgnore]
        public Dictionary<string, string> LegDetails { get; set; }
    }
}
