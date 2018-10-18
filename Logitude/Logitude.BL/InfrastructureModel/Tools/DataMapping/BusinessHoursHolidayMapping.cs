using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class BusinessHoursHolidayMapping
    {
        public static void MapEntity(BusinessHoursHolidayPM entityPM, BusinessHoursHoliday entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Id = entityPM.Id;
            }

            entityPOCO.BusinessHourId = entityPM.BusinessHourId;
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
