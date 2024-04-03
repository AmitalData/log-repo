using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.BL.ShipmentsModel.Tools.ExternalService
{
    public static class ShipmentAdditionalDataService
    {
        public static string SerializeShipmentAdditionalXmlData(ShipmentAdditionalData shipmentAdditionalData)
        {
            try
            {
                if (shipmentAdditionalData == null) return null;
                return LogitudeXmlSerializer.SerializeObjectToXmlElementStringWithoutIndentation<ShipmentAdditionalData>(shipmentAdditionalData);
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Serialize ShipmentAdditionalData Failed");
            }
        }

        public static ShipmentAdditionalData DeserializeShipmentAdditionalXmlData(string shipmentAdditionalDataXml)
        {
            try
            {
                if (String.IsNullOrEmpty(shipmentAdditionalDataXml)) return null;
                shipmentAdditionalDataXml = CorrectRootOfShipmentAdditionalDataXml(shipmentAdditionalDataXml);
                return LogitudeXmlSerializer.DeserializeObject<ShipmentAdditionalData>(shipmentAdditionalDataXml);
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Deserialize ShipmentAdditionalData Failed");
            }
        }

        private static string CorrectRootOfShipmentAdditionalDataXml(string shipmentAdditionalDataXml)
        {
            if (!shipmentAdditionalDataXml.StartsWith("<PLForwarding>")) return shipmentAdditionalDataXml;

            return "<ShipmentAdditionalData>" + shipmentAdditionalDataXml + "</ShipmentAdditionalData>";
        }
    }
}


[XmlRoot(ElementName = "", Namespace = "")]
public class ShipmentAdditionalData
{
    [XmlElement(ElementName = "PLForwarding", Namespace = "")]
    public bool PLForwarding { get; set; }
    [XmlElement(ElementName = "ShipmentOrderNumber", Namespace = "")]
    public string ShipmentOrderNumber { get; set; }
}