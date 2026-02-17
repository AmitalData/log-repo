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
    public class VatTypePercentageMapping
    {
        public static void MapEntity(VatTypePercentagePM entityPM, VatTypePercentage vatTypePercentage, bool isNewEntity)
        {
            if (isNewEntity)
            {
                vatTypePercentage.Id = entityPM.Id;
                vatTypePercentage.Tenant = entityPM.Tenant;
            }

            vatTypePercentage.Percentage = entityPM.Percentage;

            DateTime? myFromDate = null;
            if(entityPM.FromDate != null)
            {
                myFromDate = entityPM.FromDate.Value.Date;
            }

            vatTypePercentage.FromDate = myFromDate;
        }
    }
}