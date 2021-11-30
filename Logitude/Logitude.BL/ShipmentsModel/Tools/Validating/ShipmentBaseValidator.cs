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
    public class ShipmentBaseValidator
    {
        public static void ValidateUpdate(object entityPM)
        {
            ShipmentPM shipmentPM = (ShipmentPM)entityPM;
            string shipmentLevelCode = shipmentPM.ShipmentLevelCode;

            switch (shipmentLevelCode)
            {
                case "D": ShipmentDirectValidator.ValidateUpdate(shipmentPM);
                    break;
                case "H": ShipmentHouseValidator.ValidateUpdate(shipmentPM);
                    break;
                case "C": ShipmentMasterValidator.ValidateUpdate(shipmentPM);
                    break;
                case "A": ShipmentCustomsValidator.ValidateUpdate(shipmentPM);
                    break;
            }
        }
    }
}