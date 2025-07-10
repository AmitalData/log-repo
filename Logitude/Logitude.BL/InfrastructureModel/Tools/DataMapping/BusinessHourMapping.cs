using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

            entityPOCO.MondayFromHour = entityPM.MondayFromHour ?? (entityPM.MondayFromHourDate !=null? entityPM.MondayFromHourDate.Value.TimeOfDay: (TimeSpan?)null);
            entityPOCO.TuesdayFromHour = entityPM.TuesdayFromHour ??(entityPM?.TuesdayFromHourDate != null ? entityPM.TuesdayFromHourDate.Value.TimeOfDay : (TimeSpan?)null);
            entityPOCO.WednesdayFromHour = entityPM.WednesdayFromHour ??( entityPM?.WednesdayFromHourDate!=null ? entityPM.WednesdayFromHourDate.Value.TimeOfDay : (TimeSpan?)null);
            entityPOCO.ThursdayFromHour = entityPM.ThursdayFromHour ?? (entityPM?.ThursdayFromHourDate!=null ? entityPM.ThursdayFromHourDate.Value.TimeOfDay : (TimeSpan?)null); 
            entityPOCO.FridayFromHour = entityPM.FridayFromHour ?? (entityPM?.FridayFromHourDate!=null ? entityPM.FridayFromHourDate.Value.TimeOfDay : (TimeSpan?)null); 
            entityPOCO.SaturdayFromHour = entityPM.SaturdayFromHour ??( entityPM?.SaturdayFromHourDate!=null? entityPM.SaturdayFromHourDate.Value.TimeOfDay : (TimeSpan?)null);
            entityPOCO.SundayFromHour = entityPM.SundayFromHour ??( entityPM?.SundayFromHourDate!=null ? entityPM.SundayFromHourDate.Value.TimeOfDay : (TimeSpan?)null);

            entityPOCO.MondayToHour = entityPM.MondayToHour ?? (entityPM?.MondayToHourDate!=null ? entityPM.MondayToHourDate.Value.TimeOfDay : (TimeSpan?)null);
            entityPOCO.TuesdayToHour = entityPM.TuesdayToHour ?? (entityPM?.TuesdayToHourDate!=null ? entityPM.TuesdayToHourDate.Value.TimeOfDay : (TimeSpan?)null);
            entityPOCO.WednesdayToHour = entityPM.WednesdayToHour ?? (entityPM?.WednesdayToHourDate!=null ? entityPM.WednesdayToHourDate.Value.TimeOfDay : (TimeSpan?)null);
            entityPOCO.ThursdayToHour = entityPM.ThursdayToHour ??( entityPM?.ThursdayToHourDate!=null ? entityPM.ThursdayToHourDate.Value.TimeOfDay : (TimeSpan?)null);
            entityPOCO.FridayToHour = entityPM.FridayToHour ??( entityPM?.FridayToHourDate!=null ? entityPM.FridayToHourDate.Value.TimeOfDay : (TimeSpan?)null);
            entityPOCO.SaturdayToHour = entityPM.SaturdayToHour ?? (entityPM?.SaturdayToHourDate!=null ? entityPM.SaturdayToHourDate.Value.TimeOfDay : (TimeSpan?)null);
            entityPOCO.SundayToHour = entityPM.SundayToHour ?? (entityPM?.SundayToHourDate!=null ? entityPM.SundayToHourDate.Value.TimeOfDay : (TimeSpan?)null);
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
