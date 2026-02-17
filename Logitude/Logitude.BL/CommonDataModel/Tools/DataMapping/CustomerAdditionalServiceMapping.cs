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
    public class CustomerAdditionalServiceMapping
    {
        public static void MapEntity(CustomerAdditionalServicePM entityPM, CustomerAdditionalService entityPoco, bool isNewState)
        {
            if (isNewState)
            {
                entityPoco.Tenant = entityPM.Tenant;
                entityPoco.CustomerId = entityPM.CustomerId;
                entityPoco.AdditionalServiceId = entityPM.AdditionalServiceId;
            }

            entityPoco.Potential = entityPM.Potential;
            entityPoco.Notes = entityPM.Notes;
            entityPoco.NotesRightToLeft = entityPM.NotesRightToLeft;
        }
    }
}
