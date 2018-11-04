using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class UpdateTimeManagementDurations : TaskManagerBase
    {
        public int Tenant;
        public UpdateTimeManagementDurations(string Id, int tenant)
            : base(Id, tenant)
        {
            this.Tenant = tenant;
        }

        public override void StartTask()
        {
            this.CalculatingTheProratingProjectsTime();

        }

        private void CalculatingTheProratingProjectsTime()
        {
            ITimeManagementContext currentContext = TimeManagementContext.GetContext(this.Tenant);
            SprintRepository sprintRepository = new SprintRepository(currentContext);
            TMEmployeeTimeRepository tMEmployeeTimeRepository = new TMEmployeeTimeRepository(currentContext);
            IQueryable<TMEmployeeTime> iQueryable = (from d in currentContext.TMEmployeeTimes where d.Tenant == this.Tenant && d.NeedsProrating == true select d);
            IQueryable<TMProject> allProjects = (from d in currentContext.TMProjects where d.Tenant == this.Tenant select d);

            if (iQueryable != null)
            {
                var groupedItems = (from d in iQueryable
                                    group d by new { d.EmployeeUserId, d.SprintId, d.WINumber, d.Id } into g
                                    select new
                                    {
                                        Id = g.Key.Id,
                                        EmployeeUserId = g.Key.EmployeeUserId,
                                        SprintId = g.Key.SprintId,
                                        WINumber = g.Key.WINumber
                                    });
                IQueryable<TMEmployeeTime> projectsProrating = (from myTMEmployeeTime in iQueryable
                                                                join db_Projects in allProjects on myTMEmployeeTime.ProjectId equals db_Projects.Id into joinedData
                                                                from myProjct in joinedData
                                                                where myTMEmployeeTime.Tenant == this.Tenant
                                                                && myProjct.Tenant == this.Tenant
                                                                && myProjct.IsProrated == true
                                                                select myTMEmployeeTime);

                foreach (var item in groupedItems)
                {
                    TMEmployeeTime itemPOCO = tMEmployeeTimeRepository.GetSingle(item.Id, this.Tenant);
                    double totalProratingHours = Math.Round((projectsProrating.ToList().Sum(a => a.TimeInMinutes)) / 60.0, 2);
                    double totalNotProratingHours = Math.Round((iQueryable.ToList().Sum(a => a.TimeInMinutes)) / 60.0, 2);
                    List<TMEmployeeTime> itemGrouplist = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) == System.Data.Entity.DbFunctions.TruncateTime(itemPOCO.DateOfWork) && d.EmployeeUserId == itemPOCO.EmployeeUserId && d.ProjectId == itemPOCO.ProjectId && d.WINumber == item.WINumber && d.Description == itemPOCO.Description).ToList();
                    var wIWorkedHours_Employee = Math.Round((itemGrouplist.Sum(a => a.TimeInMinutes)) / 60.0, 2);
                    var wIWorkedHours_Employee_Prorated = (wIWorkedHours_Employee / totalNotProratingHours) * totalProratingHours;
                    wIWorkedHours_Employee = wIWorkedHours_Employee + wIWorkedHours_Employee_Prorated;
                    itemPOCO.ProratedDuration = wIWorkedHours_Employee_Prorated;
                    itemPOCO.FullDuration = wIWorkedHours_Employee;
                }
                currentContext.SaveChanges();
            }
        }
    }
}
