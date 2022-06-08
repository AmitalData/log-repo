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
    public class ShipmentMasterValidator
    {
        public static void ValidateUpdate(ShipmentPM shipmentPM, ShipmentPM oldShipmentPM)
        {
            //if (oldShipmentPM.IsOperationalClosed)
            //    throw new ApplicationException("Can't update operationally closed shipments");

            if (oldShipmentPM.IsCancelled)
                throw new ApplicationException("Can't update cancelled shipments");

            ShipmentMainCarriageLegsValidator shipmentMainCarriageLegsValidator = new ShipmentMainCarriageLegsValidator(shipmentPM);
            shipmentMainCarriageLegsValidator.ValidateMainCarriageLegs();
            shipmentMainCarriageLegsValidator.ValidateRoutingsSeriesDates();
            shipmentMainCarriageLegsValidator.ValidateActualDates();

            //ShipmentAccountingValidator shipmentAccountingValidator = new ShipmentAccountingValidator(shipmentPM);
            //shipmentAccountingValidator.ValidateAccountingClosed();
        }
    }
}