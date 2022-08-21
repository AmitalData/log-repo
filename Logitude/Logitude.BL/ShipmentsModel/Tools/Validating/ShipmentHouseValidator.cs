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
        public static void ValidateUpdate(ShipmentPM shipmentPM, ShipmentPM oldShipmentPM)
        {
            //if (oldShipmentPM.IsOperationalClosed) throw new ApplicationException("Can't update operationally closed shipments");
            if (oldShipmentPM.IsCancelled) throw new ApplicationException("Can't update cancelled shipments");
            //if (!string.IsNullOrEmpty(shipmentPM.MasterShipmentDataId)) throw new ApplicationException("Can't update house connected to master");

            ShipmentMainCarriageLegsValidator shipmentMainCarriageLegsValidator = new ShipmentMainCarriageLegsValidator(shipmentPM);
            shipmentMainCarriageLegsValidator.ValidateRoutingsSeriesDates();

            //ValidateCustomerData(shipmentPM);
        }

        public static void HandleHouseConnectedToMasterShipmentValidation(object entityPM, AutomationSetValue item)
        {
            ShipmentPM shipmentPM = (ShipmentPM)entityPM;
            if (shipmentPM.ShipmentLevelCode != "H") return;
            if (string.IsNullOrEmpty(shipmentPM.MasterShipmentDataId)) return;

            List<string> objectFieldsCodesNotAllowedToUpdate = GetObjectFieldsCodesNotAllowedToUpdate();
            if (objectFieldsCodesNotAllowedToUpdate.Contains(item.ObjectFieldCode))
            {
                throw new ApplicationException("Can't update house connected to master");
            }
        }

        public static void HandleOperationallyClosedShipmentValidation(object entityPM, AutomationSetValue item)
        {
            ShipmentPM oldShipmentPM = (ShipmentPM)entityPM;

            const string isOperationalClosedShipmentObjectFieldCode = "Shipment.IsOperationalClosed";

            if (oldShipmentPM.IsOperationalClosed && !IsCustomField(item, oldShipmentPM.Tenant) && item.FieldName != "IsAccountingClosed" && item.ObjectFieldCode != isOperationalClosedShipmentObjectFieldCode)
            {
                throw new ApplicationException("Can't update operationally closed shipments");
            }
        }

        private static bool IsCustomField(AutomationSetValue item, int tenant)
        {
            return item.ObjectFieldCode.Contains("." + tenant + ".Field");
        }

        private static List<string> GetObjectFieldsCodesNotAllowedToUpdate()
        {
            List<string> objectFieldsCodesNotAllowedToUpdate = new List<string>
            {
                "ShipmentComputedFields.MainCarriageATA",
                "Container.MainCarriageATA",
                "Shipment.MainCarriageATA",
                "ShipmentComputedFields.MainCarriageATD",
                "Shipment.MainCarriageATD",
                "Master.MainCarriageATD",
                "Shipment.MainCarriageCarrierId",
                "Master.MainCarriageCarrierId",
                "ShipmentComputedFields.MainCarriageETA",
                "Shipment.MainCarriageETA",
                "Master.MainCarriageETA",
                "ShipmentComputedFields.MainCarriageETD",
                "Shipment.MainCarriageETD",
                "Master.MainCarriageETD",
                "Shipment.MainCarriageFromPortId",
                "Master.MainCarriageFromPortId",
                "Shipment.MainCarriageFromPortName",
                "Shipment.MainCarriageToPortId",
                "Master.MainCarriageToPortId",
                "Shipment.OnCarriageETA",
                "Master.OnCarriageETA",
                "Shipment.OnCarriageETD",
                "Master.OnCarriageETD",
                "Shipment.PreCarriageETA",
                "Master.PreCarriageETA",
                "Shipment.PreCarriageETD",
                "Master.PreCarriageETD",
                "Shipment.Transshipment1ATA",
                "Master.Transshipment1ATA",
                "Shipment.Transshipment1ATD",
                "Master.Transshipment1ATD",
                "Shipment.Transshipment1ETA",
                "Master.Transshipment1ETA",
                "Shipment.Transshipment1ETD",
                "Master.Transshipment1ETD",
                "Shipment.Transshipment1FromPortId",
                "Master.Transshipment1FromPortId",
                "Shipment.Transshipment1ToPortId",
                "Master.Transshipment1ToPortId",
                "Shipment.Transshipment2FromPortId",
                "Master.Transshipment2FromPortId",
                "Shipment.Transshipment2ToPortId",
                "Master.Transshipment2ToPortId",
                "Shipment.Transshipment3FromPortId",
                "Master.Transshipment3FromPortId",
                "Shipment.Transshipment3ToPortId",
                "Master.Transshipment3ToPortId"
            };

            return objectFieldsCodesNotAllowedToUpdate;
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