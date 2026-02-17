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
    public class CountryCityMapping
    {
        public static void MapEntity(CountryCityPM entityPM, CountryCity entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.Code = entityPM.Code;
            entityPOCO.AddedManually = entityPM.AddedManually;
            entityPOCO.CountryId = entityPM.CountryId;
            entityPOCO.EnglishName = entityPM.EnglishName;
            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.LocalName = entityPM.LocalName;
            entityPOCO.Notes = entityPM.Notes;
            entityPOCO.StateId = entityPM.StateId;

            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void BuildSearchFields(CountryCityPM entityPM, CountryCity entityPOCO)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.Code))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Code : mySearchFields + "," + entityPM.Code;
            }

            if (!string.IsNullOrEmpty(entityPM.EnglishName))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.EnglishName : mySearchFields + "," + entityPM.EnglishName;
            }

            if (!string.IsNullOrEmpty(entityPM.LocalName))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.LocalName : mySearchFields + "," + entityPM.LocalName;
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}