using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml.Serialization;

namespace Logitude.BL.ShipmentsModel.APIDataContract.Messages
{
    public static class ShipmentNumbersXML
    {
        public static Shipments GetShipmentNumbersXMLMessage(GetShipmentNumbers entity, int tenant)
        {
            IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);
            IQueryable<Shipment> shipments = myContext.Shipments.Include("ShipmentMasterData").Where(d => d.Tenant == tenant
            && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(entity.FromDate)
            && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(entity.ToDate));

            var tt = shipments.ToList();
            if (!string.IsNullOrEmpty(entity.Direction))
                shipments = shipments.Where(o => o.DirectionId == entity.Direction);

            var tt1 = shipments.ToList();

            if (!string.IsNullOrEmpty(entity.TransportMode))
                shipments = shipments.Where(o => o.TransportModeId == entity.TransportMode);

            var tt2 = shipments.ToList();

            if (!string.IsNullOrEmpty(entity.ShipmentLevel))
                shipments = shipments.Where(o => o.ShipmentLevelCode == entity.ShipmentLevel);

            var tt3 = shipments.ToList();

            if (!string.IsNullOrEmpty(entity.Carrier))
                shipments = shipments.Where(o => o.ShipmentMasterData.MainCarriageCarrierCard.Code == entity.Carrier);

            if (!string.IsNullOrEmpty(entity.House))
                shipments = shipments.Where(o => o.House == entity.House);

            if (!string.IsNullOrEmpty(entity.Master))
                shipments = shipments.Where(o => o.ShipmentMasterData.Master == entity.Master);

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
                    MasterShipmentNumber = x.ShipmentMasterData != null ? SplitBySlash(x.ShipmentMasterData.MasterShipmentNumber) : " ",
                    CreateDate = x.CreateDateTime
                }).ToList(),
            };
            return response;
        }

        public static void ShipmentDataMappingValidating(GetShipmentNumbers entity, int tenant)
        {
            ValidateDirection(entity.Direction, tenant);
            ValidateTransportMode(entity.TransportMode, tenant);
            ValidateShipmentLevel (entity.ShipmentLevel, tenant);
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
                throw new ApplicationException("Shipment Level with Code " + shipmentLevel + " doesn't exist");
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
                throw new ApplicationException("Transport Mode with Code " + transportMode + " doesn't exist");
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
                throw new ApplicationException("Direction with Code " + direction + " doesn't exist");
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
