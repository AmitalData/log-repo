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
    public class TarrifFromToMapping
    {
        public static void MapEntity(TarrifFromToPM tarrifFromToPm, TarrifFromTo tarrifFromTo, bool isNewState)
        {
            tarrifFromTo.CountryId = tarrifFromToPm.CountryId;
            tarrifFromTo.TarrifHeaderId = tarrifFromToPm.TarrifHeaderId;
            tarrifFromTo.Tenant = tarrifFromToPm.Tenant;
            tarrifFromTo.PortId = tarrifFromToPm.PortId;
            tarrifFromTo.TarrifFromToTypeCode = tarrifFromToPm.TarrifFromToTypeCode;
        }
    }
}