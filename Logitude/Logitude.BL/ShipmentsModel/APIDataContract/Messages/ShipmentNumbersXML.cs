using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml.Serialization;

namespace Logitude.BL.ShipmentsModel.APIDataContract.Messages
{
    public static class ShipmentNumbersXML
    {
        private static string errorMsg = "";
        public static Shipments GetShipmentNumbersXMLMessage(GetShipmentNumbers entity, int tenant)
        {
            IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);
            IQueryable<Shipment> shipments = myContext.Shipments.Include("ShipmentMasterData").Include("ShipmentMasterData.MainCarriageCarrierCard").Where(d => d.Tenant == tenant
            && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(entity.FromDate)
            && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(entity.ToDate));

            if (!string.IsNullOrEmpty(entity.Direction))
                shipments = shipments.Where(o => o.DirectionId == entity.Direction);

            if (!string.IsNullOrEmpty(entity.TransportMode))
                shipments = shipments.Where(o => o.TransportModeId == entity.TransportMode);

            if (!string.IsNullOrEmpty(entity.ShipmentLevel))
                shipments = shipments.Where(o => o.ShipmentLevelCode == entity.ShipmentLevel);

            if (!string.IsNullOrEmpty(entity.Carrier))
            {
                shipments = shipments.Where(o => o.ShipmentMasterData.MainCarriageCarrierCard.Code == entity.Carrier);
            }
                
            if (!string.IsNullOrEmpty(entity.House))
                shipments = shipments.Where(o => o.House == entity.House);

            if (!string.IsNullOrEmpty(entity.Master))
            {
                if (entity.TransportMode == "A")
                {
                    shipments = shipments.Where(o => !string.IsNullOrEmpty(o.ShipmentMasterData.AirlinePrefix) && !string.IsNullOrEmpty(o.ShipmentMasterData.Master) &&
                    o.ShipmentMasterData.AirlinePrefix + "-" + o.ShipmentMasterData.Master == entity.Master);
                }
                else
                {
                    shipments = shipments.Where(o => o.ShipmentMasterData.Master == entity.Master);
                }
            }

            if (!string.IsNullOrEmpty(entity.ContainerNumber))
            {
                shipments = (from shipment in shipments
                             join shipmentPackage in myContext.ShipmentPackages
                             on shipment.Id equals shipmentPackage.ShipmentId
                             where shipmentPackage.ContainerNumber == entity.ContainerNumber
                             select shipment).GroupBy(s => s.Id).Select(grp => grp.FirstOrDefault());
            }


            var response = new Shipments()
            {
                ShipmentList = shipments.ToList().Select(x => new ShipmentResponseItem()
                {
                    ShipmentNumber = SplitBySlash(x.ShipmentNumber),
                    MasterShipmentNumber = x.ShipmentLevelCode == "H" ? GetHouseData(x.MasterShipmentDataId, myContext) : " ",
                    CreateDate = x.CreateDateTime
                }).ToList(),
            };
            return response;
        }

        private static string GetHouseData(string masterShipmentDataId, IShipmentsContext myContext)
        {
            var masterShipmentNumber = " ";
            ShipmentMasterData masterData = (from a in myContext.ShipmentMasterDatas
                                             where a.Id == masterShipmentDataId
                                             select a).FirstOrDefault();

            if (masterData != null)
            {
                masterShipmentNumber = SplitBySlash(masterData.MasterShipmentNumber);
            }
            return masterShipmentNumber;
        }

        public static void ShipmentDataMappingValidating(GetShipmentNumbers entity, int tenant)
        {
            errorMsg = "";
            ValidateDirection(entity.Direction, tenant);
            ValidateTransportMode(entity.TransportMode, tenant);
            ValidateShipmentLevel (entity.ShipmentLevel, tenant);
            ValidateCarrier(entity.Carrier, tenant);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                throw new ApplicationException(errorMsg);
            }
        }

        private static void ValidateCarrier(string carrier, int tenant)
        {
            var temp = new CardPM();
            CardQuery query = new CardQuery(tenant);
            if (!string.IsNullOrEmpty(carrier))
            {
                temp = query.GetSinglePMByCode(carrier, tenant);
            }

            if (temp == null)
            {
                errorMsg = errorMsg + "Carrier with Code " + carrier + " doesn't exist. ";
            }
        }

        private static void ValidateShipmentLevel(string shipmentLevel, int tenant)
        {
            var temp = new ShipmentLevelPM();
            ShipmentLevelQuery query = new ShipmentLevelQuery(tenant);
            if (!string.IsNullOrEmpty(shipmentLevel))
            {
                temp = query.GetSinglePM(shipmentLevel);
            }

            if (temp == null)
            {
                errorMsg = errorMsg + "Shipment Level with Code " + shipmentLevel + " doesn't exist. ";
            }
        }

        private static void ValidateTransportMode(string transportMode, int tenant)
        {
            var temp = new TransportModePM();
            TransportModeQuery query = new TransportModeQuery(tenant);
            if (!string.IsNullOrEmpty(transportMode))
            {
                temp = query.GetSinglePM(transportMode);
            }

            if (temp == null)
            {
                errorMsg = errorMsg + "Transport Mode with Code " + transportMode + " doesn't exist. ";
            }
        }

        private static void ValidateDirection(string direction, int tenant)
        {
            var temp = new DirectionPM();
            DirectionQuery query = new DirectionQuery(tenant);
            if (!string.IsNullOrEmpty(direction))
            {
                temp = query.GetSinglePM(direction);
            }

            if (temp == null)
            {
                errorMsg = errorMsg + "Direction with Code " + direction + " doesn't exist. ";
            }
        }

        private static string SplitBySlash(string text)
        {
            string splittext = text;
            if (!string.IsNullOrEmpty(text) && text.Contains('/'))
            {
                splittext = text.Split('/')[1];
            }
            return splittext;
        }
    }

    [XmlRoot("Shipments")]
    public class Shipments
    {
        [XmlElement("Shipment")]
        public List<ShipmentResponseItem> ShipmentList { get; set; }

        public Shipments()
        {
            this.ShipmentList = new List<ShipmentResponseItem>();
        }
    }
    public class ShipmentResponseItem
    {
        public string ShipmentNumber { get; set; }
        public string MasterShipmentNumber { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
