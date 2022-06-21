using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class HorseMapping
    {
        public static void MapEntity(HorsePM entityPM, Horse entityPOCO, bool isNewState, string loggedContactId)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPOCO.CreatedByUserId = loggedContactId;
            }

            entityPOCO.Name = entityPM.Name;
            entityPOCO.YearOfBirth = entityPM.YearOfBirth;
            entityPOCO.Color = entityPM.Color;
            entityPOCO.Breed = entityPM.Breed;
            entityPOCO.Discipline = entityPM.Discipline;
            entityPOCO.TravelBehavior = entityPM.TravelBehavior;
            entityPOCO.MicochipNumber = entityPM.MicochipNumber;
            entityPOCO.PassportNumber = entityPM.PassportNumber;
            entityPOCO.CountryOfBirthId = entityPM.CountryOfBirthId;
            entityPOCO.CurrentStable = entityPM.CurrentStable;
            entityPOCO.Owner = entityPM.Owner;
            entityPOCO.Remarks = entityPM.Remarks;
            entityPOCO.Inactive = entityPM.Inactive;
            entityPOCO.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPOCO.UpdatedByUserId = loggedContactId;
            entityPOCO.GenderCode = entityPM.GenderCode;

            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void BuildSearchFields(HorsePM entityPM, Horse entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.MicochipNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PassportNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Owner);

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
