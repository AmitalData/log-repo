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
    public class CompetitorMapping
    {
        public static void MapEntity(CompetitorPM entityPM, Competitor entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.AddressId = entityPM.AddressId;
            }

            entityPOCO.Name = entityPM.Name;
            entityPOCO.Website = entityPM.Website;
            entityPOCO.Strengths = entityPM.Strengths; ;
            entityPOCO.Weaknesses = entityPM.Weaknesses;
            entityPOCO.Opportunity = entityPM.Opportunity;
            entityPOCO.Threat = entityPM.Threat;
            entityPOCO.InActive = entityPM.InActive;

            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void BuildSearchFields(CompetitorPM entityPM, Competitor entityPOCO)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.Name))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Name : mySearchFields + "," + entityPM.Name;
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}