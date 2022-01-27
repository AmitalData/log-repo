using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CustomerTeamMapping
    {
        public static void MapEntity(CustomerTeamPM entityPM, CustomerTeam entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                MapNewEntityFields(entityPM, entityPOCO);
            }

            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.LocalName = entityPM.LocalName;
            entityPOCO.InActive = entityPM.InActive;
            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void MapNewEntityFields(CustomerTeamPM entityPM, CustomerTeam entityPOCO)
        {
            entityPOCO.Id = entityPM.Id = entityPM.Id;
            entityPOCO.Tenant = entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
        }

        private static void BuildSearchFields(CustomerTeamPM entityPM, CustomerTeam entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
