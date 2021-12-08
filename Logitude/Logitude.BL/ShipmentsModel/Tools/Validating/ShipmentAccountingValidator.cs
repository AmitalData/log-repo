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
using Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours.Validators;

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class ShipmentAccountingValidator
    {
        private int tenant;
        private ShipmentPM shipmentPM;
        private AccountingSettingRepository accountingSettingRepository;
        public ShipmentAccountingValidator(ShipmentPM shipmentPM)
        {
            this.shipmentPM = shipmentPM;
            this.tenant = shipmentPM.Tenant;
            accountingSettingRepository = new AccountingSettingRepository(tenant);
        }

        public void ValidateAccountingClosed()
        {
            if (!shipmentPM.IsAccountingClosed) return;

            bool hasOpenReceivables = CheckIfHasOpenReceivables();
            bool hasOpenPayables = CheckIfHasOpenPayables();

            if (hasOpenPayables || hasOpenReceivables)
            {
                throw new ApplicationException("can’t close for accounting if there are any open payables/receivables.");
            }

            if (!shipmentPM.IsOperationalClosed)
            {
                throw new ApplicationException("Shipment shoud be closed operationally");
            }
        }

        private bool CheckIfHasOpenPayables()
        {
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountingSetting(shipmentPM.Tenant);

            return accountingSetting != null && !accountingSetting.AllowClosureWithoutPayables && shipmentPM.ShipmentPayables.Count() > 0 && shipmentPM.ShipmentPayables.Where(p => p.ExpectedAmount != null && p.ExpectedAmount != 0).Any();
        }

        private bool CheckIfHasOpenReceivables()
        {
            return shipmentPM.ShipmentReceivables.Count() > 0 && shipmentPM.ShipmentReceivables.Where(p => p.TotalAmount != null && p.TotalAmount != 0).Any();
        }
    }
}