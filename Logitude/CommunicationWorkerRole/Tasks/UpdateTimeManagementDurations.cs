using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class UpdateTimeManagementDurations : TaskManagerBase
    {
        public UpdateTimeManagementDurations(string Id, int tenant)
            : base(Id, tenant)
        {
        }

        public override void StartTask()
        {
            this.CalculatingTheProratingProjectsTime();

        }

        private void CalculatingTheProratingProjectsTime()
        {
            //string sprintId = entityPM.SprintId;
            //SprintRepository sprintRepository = new SprintRepository(entityPM.Tenant);
            //Sprint sprint = sprintRepository.GetSingle(sprintId, entityPM.Tenant);
            //if (sprint != null)
            //{
            //    DateTime? fromDate = sprint.FromDate;
            //    DateTime? toDate = sprint.ToDate;

            //    ITimeManagementContext currentContext = TimeManagementContext.GetContext(entityPM.Tenant);

            //    IQueryable<TMEmployeeTime> iQueryable = (from d in currentContext.TMEmployeeTimes where d.Tenant == entityPM.Tenant select d);
            //    IQueryable<TMProject> allProjects = (from d in currentContext.TMProjects where d.Tenant == entityPM.Tenant select d);

            //    if (fromDate != null && toDate != null)
            //    {
            //        iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
            //        iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
            //    }

            //    IQueryable<TMEmployeeTime> projectsProrating = (from myTMEmployeeTime in iQueryable
            //                                                    join db_Projects in allProjects on myTMEmployeeTime.ProjectId equals db_Projects.Id into joinedData
            //                                                    from myProjct in joinedData
            //                                                    where myTMEmployeeTime.Tenant == entityPM.Tenant
            //                                                    && myProjct.Tenant == entityPM.Tenant
            //                                                    && myProjct.IsProrated == true
            //                                                    select myTMEmployeeTime);

            //    double totalProratingHours = Math.Round((projectsProrating.ToList().Sum(a => a.TimeInMinutes)) / 60.0, 2);
            //    double totalNotProratingHours = Math.Round((iQueryable.ToList().Sum(a => a.TimeInMinutes)) / 60.0, 2);

            //    List<TMEmployeeTime> itemGrouplist = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) == System.Data.Entity.DbFunctions.TruncateTime(entityPM.DateOfWork) && d.EmployeeUserId == entityPM.EmployeeUserId && d.ProjectId == entityPM.ProjectId && d.WINumber == entityPM.WINumber && d.Description == entityPM.Description).ToList();

            //    var wIWorkedHours_Employee = Math.Round((itemGrouplist.Sum(a => a.TimeInMinutes)) / 60.0, 2);
            //    var wIWorkedHours_Employee_Prorated = (wIWorkedHours_Employee / totalNotProratingHours) * totalProratingHours;
            //    wIWorkedHours_Employee = wIWorkedHours_Employee + wIWorkedHours_Employee_Prorated;

            //    entityPM.ProratedDuration = wIWorkedHours_Employee_Prorated;
            //    entityPM.FullDuration = wIWorkedHours_Employee;
            //}
        }
    }
}
