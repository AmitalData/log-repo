using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        internal static void MapShipmentCarrierStatus(ShipmentCarrierStatusPM itemPM, ShipmentCarrierStatus itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            itemPoco.Weight = itemPM.Weight;
            itemPoco.Details = itemPM.Details;
            itemPoco.EventDate = itemPM.EventDate;
            itemPoco.FlightNumber = itemPM.FlightNumber;
            itemPoco.FromPortId = itemPM.FromPortId;
            itemPoco.Partial = itemPM.Partial;
            itemPoco.Pieces = itemPM.Pieces;
            itemPoco.ReceivingDate = itemPM.ReceivingDate;
            itemPoco.RecordHash = itemPM.RecordHash;
            itemPoco.Status = itemPM.Status;
            itemPoco.ToPortId = itemPM.ToPortId;
            itemPoco.Location = itemPM.Location;

            itemPoco.DepartureDate = itemPM.DepartureDate;
            itemPoco.ArrivalDate = itemPM.ArrivalDate;
            itemPoco.TimeOfDepartureInfo = itemPM.TimeOfDepartureInfo;
            itemPoco.TimeOfArrivalInfo = itemPM.TimeOfArrivalInfo;
        }
    }
}