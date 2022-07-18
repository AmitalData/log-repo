using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.BL.ShipmentsModel.APIDataContract
{
    [XmlRoot("Shipments")]
    public class Shipments
    {
        [XmlElement("Shipment")]
        public List<ShipmentByReferences> ShipmentList { get; set; }

        public Shipments()
        {
            this.ShipmentList = new List<ShipmentByReferences>();
        }
    }

    public class ShipmentByReferences
    {
        public string ShipmentNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Routing { get; set; }

        public CardByReferences Customer { get; set; }

        public CardByReferences Agent { get; set; }
        public string AgentReference1 { get; set; }
        public string AgentReference2 { get; set; }
        public CardByReferences Shipper { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }
        public CardByReferences Consignee { get; set; }
        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }
        public CardByReferences ShipperNotExporter { get; set; }
        public string ShipperNotExporterReference1 { get; set; }
        public string ShipperNotExporterReference2 { get; set; }
        public CardByReferences ConsigneeNotImporter { get; set; }
        public string ConsigneeNotImporterReference { get; set; }
        public CardByReferences Forwarder { get; set; }
        public string ForwarderReference { get; set; }        

        public ShipmentByReferences()
        {
            this.Customer = new CardByReferences();
            this.Agent = new CardByReferences();
            this.Shipper = new CardByReferences();
            this.Consignee = new CardByReferences();
            this.ShipperNotExporter = new CardByReferences();
            this.ConsigneeNotImporter = new CardByReferences();
            this.Forwarder = new CardByReferences();
        }
    }

    public class CardByReferences
    {
        public string Code { get; set; }

        public string EnglishName { get; set; }
    }

    [XmlRoot("Query")]
    public class Query
    {
        public string ComputingPartnerCode { get; set; }
        public string Master { get; set; }
        public string House { get; set; }
        public QueryCard Agent { get; set; }
        public QueryCard Shipper { get; set; }
        public QueryCard Consignee { get; set; }
        public QueryCard ShipperNotExporter { get; set; }
        public QueryCard ConsigneeNotImporter { get; set; }
        public QueryCard Forwarder { get; set; }
    }

    public class QueryCard
    {
        [XmlAttribute]
        public string Code { get; set; }

        [XmlAttribute]
        public string PartnerCode { get; set; }

        [XmlAttribute]
        public string Reference1 { get; set; }

        [XmlAttribute]
        public string Reference2 { get; set; }
    }
}
