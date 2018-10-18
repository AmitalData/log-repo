using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class BusinessHourMapping
    {
        public static void MapEntity(BusinessHourPM entityPM, BusinessHour entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.Code = entityPM.Code;

            entityPOCO.Name = entityPM.Name;
            entityPOCO.Description = entityPM.Description;

            entityPOCO.Is247 = entityPM.Is247;

            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;

            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;

            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.UpdateDate = entityPM.UpdateDate;

            entityPOCO.IsMondayEnabeled = entityPM.IsMondayEnabeled;
            entityPOCO.IsTuesdayEnabeled = entityPM.IsTuesdayEnabeled;
            entityPOCO.IsWednesdayEnabeled = entityPM.IsWednesdayEnabeled;
            entityPOCO.IsThursdayEnabeled = entityPM.IsThursdayEnabeled;
            entityPOCO.IsFridayEnabeled = entityPM.IsFridayEnabeled;
            entityPOCO.IsSaturdayEnabeled = entityPM.IsSaturdayEnabeled;
            entityPOCO.IsSundayEnabeled = entityPM.IsSundayEnabeled;

            entityPOCO.MondayFromHour = entityPM.MondayFromHour;
            entityPOCO.TuesdayFromHour = entityPM.TuesdayFromHour;
            entityPOCO.WednesdayFromHour = entityPM.WednesdayFromHour;
            entityPOCO.ThursdayFromHour = entityPM.ThursdayFromHour;
            entityPOCO.FridayFromHour = entityPM.FridayFromHour;
            entityPOCO.SaturdayFromHour = entityPM.SaturdayFromHour;
            entityPOCO.SundayFromHour = entityPM.SundayFromHour;

            entityPOCO.MondayToHour = entityPM.MondayToHour;
            entityPOCO.TuesdayToHour = entityPM.TuesdayToHour;
            entityPOCO.WednesdayToHour = entityPM.WednesdayToHour;
            entityPOCO.ThursdayToHour = entityPM.ThursdayToHour;
            entityPOCO.FridayToHour = entityPM.FridayToHour;
            entityPOCO.SaturdayToHour = entityPM.SaturdayToHour;
            entityPOCO.SundayToHour = entityPM.SundayToHour;
            entityPOCO.SearchFields = entityPM.SearchFields;
            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void BuildSearchFields(BusinessHourPM entityPM, BusinessHour entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }

        public static void MapBusinessHoursHolidayEntity(BusinessHoursHolidayPM entityPM, BusinessHoursHoliday entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.BusinessHourId = entityPM.BusinessHourId;
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.Day = entityPM.Day;
            entityPOCO.Month = entityPM.Month;
            entityPOCO.Year = entityPM.Year;

            entityPOCO.HolidayName = entityPM.HolidayName;

            entityPOCO.IsRecurring = entityPM.IsRecurring;

            entityPOCO.Inactive = entityPM.Inactive;

            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.UpdateDate = entityPM.UpdateDate;

            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;

            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;

        }
    }
}
