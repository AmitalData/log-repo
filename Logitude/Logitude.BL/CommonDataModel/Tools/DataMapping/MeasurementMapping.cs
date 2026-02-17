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
    public class MeasurementMapping
    {
        public static void MapEntity(MeasurementPM entityPM, Measurement entityPOCO, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewState)
            {
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.IsContainer = entityPM.IsContainer;
            entityPOCO.IsContainerMeasurement = entityPM.IsContainerMeasurement;
            entityPOCO.Code = entityPM.Code;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.ShortName = entityPM.ShortName;
            entityPOCO.LocalName = entityPM.LocalName;
            
            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void BuildSearchFields(MeasurementPM entityPM, Measurement entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShortName);
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
