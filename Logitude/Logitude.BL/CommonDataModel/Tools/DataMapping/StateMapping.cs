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
    public class StateMapping
    {
        public static void MapEntity(StatePM entityPM, State entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Code = entityPM.Code;
            }

            entityPOCO.AddedManually = entityPM.AddedManually;
            entityPOCO.CountryId = entityPM.CountryId;
            entityPOCO.EnglishName = entityPM.EnglishName;
            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.LocalName = entityPM.LocalName;
            entityPOCO.Notes = entityPM.Notes;
            entityPOCO.QBOTransactionLocationCode = entityPM.QBOTransactionLocationCode;
        entityPOCO.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;

        }
    }
}