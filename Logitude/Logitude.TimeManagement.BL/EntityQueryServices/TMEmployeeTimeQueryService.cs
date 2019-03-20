using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagement.BL.EntityQueryServices
{
    public partial class TMEmployeeTimeQueryService
    {
        public List<TMEmployeeTimePM> GetWeeklyTimeSheetList(int tenant, string employeeId, string locationCode)
        {
            ITimeManagementContext context = MainContext as TimeManagementContext;
            List<TMEmployeeTimePM> results = (from a in context.TMEmployeeTimes
                                              where a.Tenant == tenant && a.EmployeeUserId == employeeId && a.LocationCode == locationCode
                                              select new TMEmployeeTimePM()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  CreateDate = a.CreateDate,
                                                  CreatedByUserId = a.CreatedByUserId,
                                                  UpdateDate = a.UpdateDate,
                                                  UpdatedByUserId = a.UpdatedByUserId,
                                                  EmployeeUserId = a.EmployeeUserId,
                                                  DateOfWork = a.DateOfWork,
                                                  Description = a.Description,
                                                  TimeInMinutes = a.TimeInMinutes,
                                                  WINumber = a.WINumber,
                                                  ProjectId = a.ProjectId,
                                                  LocationCode = a.LocationCode,
                                                  //ProjectName = a.Project != null ? a.Project.Name : "",
                                                  //ProjectDescription = a.Project != null ? a.Project.Description : "",
                                              }).ToList();




            return results;
        }
        public List<TMEmployeeTimePM> GetTimeSheetByWINumberList(string id, string wiNumber, int tenant)
        {
            ITimeManagementContext context = MainContext as TimeManagementContext;
            List<TMEmployeeTimePM> results = new List<TMEmployeeTimePM>();
            results = (from a in context.TMEmployeeTimes
                       where a.Id != id && a.Tenant == tenant && a.WINumber == wiNumber
                       select new TMEmployeeTimePM()
                       {
                           Id = a.Id,
                           Tenant = a.Tenant,
                           CreateDate = a.CreateDate,
                           CreatedByUserId = a.CreatedByUserId,
                           UpdateDate = a.UpdateDate,
                           UpdatedByUserId = a.UpdatedByUserId,
                           EmployeeUserId = a.EmployeeUserId,
                           DateOfWork = a.DateOfWork,
                           Description = a.Description,
                           TimeInMinutes = a.TimeInMinutes,
                           WINumber = a.WINumber,
                           ProjectId = a.ProjectId,
                           LocationCode = a.LocationCode,
                       }).ToList();
            return results;
        }
    }
}
