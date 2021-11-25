using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Helpers;
using System.Data;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BookingLib.Data.Repositories;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Data.Entity.Core;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.QuoteModel;

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class ShipmentHouseValidator
    {
        public static void ValidateUpdate(ShipmentPM shipmentPM)
        {
            if (shipmentPM.IsOperationalClosed) throw new ApplicationException("Can't update operationally closed shipments");
            if (shipmentPM.IsCancelled) throw new ApplicationException("Can't update cancelled shipments");
            if (!string.IsNullOrEmpty(shipmentPM.MasterShipmentDataId)) throw new ApplicationException("Can't update house connected to master");

            ShipmentMainCarriageLegsValidator shipmentMainCarriageLegsValidator = new ShipmentMainCarriageLegsValidator(shipmentPM);
            shipmentMainCarriageLegsValidator.ValidateRoutingsSeriesDates();

            ValidateCustomerData(shipmentPM);
        }

        private static void ValidateCustomerData(ShipmentPM shipmentPM)
        {
            if (string.IsNullOrEmpty(shipmentPM.CustomerId)) ValidateCustomerId(shipmentPM);
            else if (!IsSentCustomerAShipmentPatrner(shipmentPM)) throw new ApplicationException("The sent customer is not one of the sent partners");
        }

        private static void ValidateCustomerId(ShipmentPM shipmentPM)
        {
            string shipmentCustomerId;
            if (shipmentPM.DirectionId == "I") shipmentCustomerId = shipmentPM.ConsigneeId;
            else shipmentCustomerId = shipmentPM.ShipperId;

            if (string.IsNullOrEmpty(shipmentCustomerId)) throw new ApplicationException("The customer is required");
        }

        private static bool IsSentCustomerAShipmentPatrner(ShipmentPM shipmentPM)
        {
            return shipmentPM.CustomerId == shipmentPM.ShipperId
                   || shipmentPM.CustomerId == shipmentPM.ConsigneeId
                   || shipmentPM.CustomerId == shipmentPM.ShipperNotExporterId
                   || shipmentPM.CustomerId == shipmentPM.AgentId
                   || shipmentPM.CustomerId == shipmentPM.CustomAgentImportId
                   || shipmentPM.CustomerId == shipmentPM.ReleasingAgentId
                   || shipmentPM.CustomerId == shipmentPM.FreightForwarderId;
        }
    }
}