using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                try
                {
                    var tenants = new List<int>();
                    var tenantRepo = new TenantRepository(0);
                    // tenants = tenantRepo.All().Select(r => r.Id).ToList();
                    tenants = new List<int>() { 1 };
                    foreach (var tenant in tenants)
                    {
                        ITimeManagementContext currentContext = TimeManagementContext.GetContext(tenant);
                        SprintRepository sprintRepository = new SprintRepository(currentContext);
                        TMEmployeeTimeRepository tMEmployeeTimeRepository = new TMEmployeeTimeRepository(currentContext);
                        IQueryable<TMEmployeeTime> iQueryable = (from d in currentContext.TMEmployeeTimes where d.Tenant == tenant select d);
                        IQueryable<TMEmployeeTime> iQueryable_NeedsProrating = iQueryable.Where(d => d.NeedsProrating == true);
                        IQueryable<TMProject> allProjects = (from d in currentContext.TMProjects where d.Tenant == tenant select d);

                        if (iQueryable != null)
                        {
                            var groupedItems = (from d in iQueryable_NeedsProrating
                                                group d by new { d.EmployeeUserId, d.SprintId, d.WINumber, d.Id } into g
                                                select new
                                                {
                                                    Id = g.Key.Id,
                                                    EmployeeUserId = g.Key.EmployeeUserId,
                                                    SprintId = g.Key.SprintId,
                                                    WINumber = g.Key.WINumber
                                                });

                            foreach (var item in groupedItems)
                            {
                                var sprint = sprintRepository.GetSingle(item.SprintId, tenant);
                                if (sprint != null)
                                {
                                    var itemQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(sprint.FromDate));
                                    itemQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(sprint.ToDate));

                                    IQueryable<TMEmployeeTime> projectsProrating = (from myTMEmployeeTime in itemQueryable
                                                                                    join db_Projects in allProjects
                                                                                    on myTMEmployeeTime.ProjectId equals db_Projects.Id into joinedData
                                                                                    from myProjct in joinedData
                                                                                    where myTMEmployeeTime.Tenant == tenant
                                                                                    && myProjct.Tenant == tenant
                                                                                    && myProjct.IsProrated == true
                                                                                    select myTMEmployeeTime);

                                    TMEmployeeTime itemPOCO = tMEmployeeTimeRepository.GetSingle(item.Id, tenant);
                                    double totalProratingHours = Math.Round((projectsProrating.ToList().Sum(a => a.TimeInMinutes)) / 60.0, 2);
                                    double totalNotProratingHours = Math.Round((iQueryable_NeedsProrating.ToList().Sum(a => a.TimeInMinutes)) / 60.0, 2);

                                    List<TMEmployeeTime> itemGrouplist = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) == System.Data.Entity.DbFunctions.TruncateTime(itemPOCO.DateOfWork) && d.EmployeeUserId == itemPOCO.EmployeeUserId && d.ProjectId == itemPOCO.ProjectId && d.WINumber == item.WINumber && d.Description == itemPOCO.Description).ToList();

                                    double wIWorkedHours_Employee = Math.Round((itemGrouplist.Sum(a => a.TimeInMinutes)) / 60.0, 2);
                                    double wIWorkedHours_Employee_Prorated = (wIWorkedHours_Employee / totalNotProratingHours) * totalProratingHours;
                                    wIWorkedHours_Employee = wIWorkedHours_Employee + wIWorkedHours_Employee_Prorated;
                                    itemPOCO.ProratedDuration = !Double.IsNaN(wIWorkedHours_Employee_Prorated) ? wIWorkedHours_Employee_Prorated : 0;
                                    itemPOCO.FullDuration = !Double.IsNaN(wIWorkedHours_Employee) ? wIWorkedHours_Employee : 0;
                                }
                                currentContext.SaveChanges();
                            }
                        }
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
