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
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class ShipmentCustomsValidator
    {
        public static void ValidateUpdate(ShipmentPM shipmentPM)
        {
            if (string.IsNullOrEmpty(shipmentPM.ShipmentNumber)) throw new ApplicationException("No shipment number found");
            //IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(shipmentPM.Tenant);
            //ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            //Shipment entityPoco = shipmentRepository.GetSingleShipmentByShipmentNumber(shipmentPM.ShipmentNumber, shipmentPM.Tenant);
            //if (entityPoco == null) throw new ApplicationException("No shipment with such shipment number");

            //CustomsQueryService mappingService = new CustomsQueryService(shipmentPM.Tenant);
            //mappingService.CustomsCustomDataMappingAndValidatin(entity, shipmentPM.Tenant);
        }
    }
}