using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.BL.ShipmentsModel.APIDataContract.Messages
{
    public static class ShipmentNumbersXML
    {
        public static Shipments GetShipmentNumbersXMLMessage(ShipmentNumbers entity, int tenant)
        {
            IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);
            List<Shipment> shipments = myContext.Shipments.Include("ShipmentMasterData").Where(d => d.Tenant == tenant
            && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(entity.FromDate)
            && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(entity.ToDate))
                .ToList();

            if (entity.Direction != null && entity.Direction.Code != null)
                shipments = shipments.Where(o => o.DirectionId == entity.Direction.Code).ToList();

            if (entity.TransportMode != null && entity.TransportMode.Code != null)
                shipments = shipments.Where(o => o.TransportModeId == entity.TransportMode.Code).ToList();

            if (entity.ShipmentType != null && entity.ShipmentType.Code != null)
                shipments = shipments.Where(o => o.ShipmentTypeId == entity.ShipmentType.Code).ToList();

            if (!string.IsNullOrEmpty(entity.House))
                shipments = shipments.Where(o => o.House == entity.House).ToList();

            if (!string.IsNullOrEmpty(entity.Master))
            { }
            if (entity.Containers != null && entity.Containers.Count() > 0)
            { }
            var response = new Shipments()
            {
                ShipmentList = shipments.Select(x => new ShipmentResponseItem()
                {
                    ShipmentNumber = x.ShipmentNumber,
                    MasterShipmentNumber = x.ShipmentMasterData != null ? x.ShipmentMasterData.Master : null,
                    CreateDate = x.CreateDateTime
                }).ToList(),
            };

            return response;
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
