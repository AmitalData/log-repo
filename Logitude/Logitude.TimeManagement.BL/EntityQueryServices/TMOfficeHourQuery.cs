using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.TimeManagement.BL.EntityQueryServices
{
    public class TMOfficeHourQuery
    {
        ITimeManagementContext myContext;
            public TMOfficeHourQuery()
        {
        }

        public TMOfficeHourQuery(int tenant)
        {
            myContext =  TimeManagementContext.GetContext(tenant);
        }
     
        public IQueryable<TMOfficeHourPM> GetTMOfficeHoursByUserIdAndDate(string employeeUserId, DateTime? FromDate, DateTime? ToDate, int tenant)
        {
            ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);

            IQueryable<TMOfficeHourPM> TMOfficeHours = from a in myContext.TMOfficeHours.Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                                                       where a.Tenant == tenant && a.UserId == employeeUserId && a.WorkDate >= FromDate && a.WorkDate <= ToDate
                                                       select new TMOfficeHourPM()
                                                       {
                                                           Id = a.Id,
                                                           CreateDate = a.CreateDate,
                                                           CreatedByUserId = a.CreatedByUserId,
                                                           Description = a.Description,
                                                           UpdateDate = a.UpdateDate,
                                                           UpdatedByUserId = a.UpdatedByUserId,
                                                           UpdatedByUserName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact.EnglishName,
                                                           Inactive = a.Inactive,
                                                           Tenant = a.Tenant,
                                                           WorkDate = a.WorkDate,
                                                           EntryTime = a.EntryTime,
                                                           ExitTime = a.ExitTime,
                                                           RecordedEntryTime = a.RecordedEntryTime,
                                                           RecordedExitTime = a.RecordedExitTime,
                                                           UserId = a.UserId,
                                                       };
            return TMOfficeHours;
        }




    }
}
