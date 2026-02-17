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
    public class CustomerSalesNoteMapping
    {
        public static void MapEntity(CustomerSalesNotePM entityPM, CustomerSalesNote poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Id = entityPM.Id;              
                poco.Tenant = entityPM.Tenant;
                poco.CustomerId = entityPM.CustomerId;
                poco.CreateDate = entityPM.CreateDate;
                poco.CreatedByUserId = entityPM.CreatedByUserId;
            }

            poco.Notes = entityPM.Notes;
            poco.UpdateDate = entityPM.UpdateDate;
            poco.UpdatedByUserId = entityPM.UpdatedByUserId;
        }
    }
}