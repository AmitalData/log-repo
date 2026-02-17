using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class TarrifChargeMapping
    {
        public static void MapEntity(TarrifChargePM tarrifChargePm, TarrifCharge tarrifCharge, bool isNewState)
        {
            tarrifCharge.ChargesTypeId = tarrifChargePm.ChargesTypeId;
            tarrifCharge.TarrifHeaderId = tarrifChargePm.TarrifHeaderId;
            tarrifCharge.Tenant = tarrifChargePm.Tenant;
            tarrifCharge.CurrencyId = tarrifChargePm.CurrencyId;
            tarrifCharge.MaxPrice = tarrifChargePm.MaxPrice;
            tarrifCharge.MeasurementId = tarrifChargePm.MeasurementId;
            tarrifCharge.MinPrice = tarrifChargePm.MinPrice;
            tarrifCharge.UnitPrice = tarrifChargePm.UnitPrice;
        }
    }
}