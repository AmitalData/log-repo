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
                    ITimeManagementContext currentContext = TimeManagementContext.GetContext(0);
                    SprintRepository sprintRepository = new SprintRepository(currentContext);
                    TMEmployeeTimeRepository tMEmployeeTimeRepository = new TMEmployeeTimeRepository(currentContext);
                    IQueryable<TMEmployeeTime> iQueryable_All = (from d in currentContext.TMEmployeeTimes select d);
                    int tenant = iQueryable_All.FirstOrDefault().Tenant;
                    IQueryable<TMEmployeeTime> iQueryable_NeedsProrating = iQueryable_All.Where(d => d.NeedsProrating == true);
                    IQueryable<TMProject> allProjects = (from d in currentContext.TMProjects where d.Tenant == tenant select d);

                    var groupedItems_NeedsProrating = (from d in iQueryable_NeedsProrating
                                                       group d by new { d.EmployeeUserId, d.SprintId } into g
                                                       select new
                                                       {
                                                           EmployeeUserId = g.Key.EmployeeUserId,
                                                           SprintId = g.Key.SprintId,
                                                       });
                    foreach (var item in groupedItems_NeedsProrating)
                    {
                        List<TMEmployeeTime> employeeTasks = iQueryable_All.Where(a => a.EmployeeUserId == item.EmployeeUserId && a.SprintId == item.SprintId).ToList();
                        List<TMEmployeeTime> employeeTasks_Needsprorating = (from myTMEmployeeTime in employeeTasks
                                                                             join db_Projects in allProjects
                                                                             on myTMEmployeeTime.ProjectId equals db_Projects.Id into joinedData
                                                                             from myProjct in joinedData
                                                                             where myTMEmployeeTime.Tenant == tenant
                                                                             && myProjct.Tenant == tenant
                                                                             && myProjct.IsProrated == true
                                                                             select myTMEmployeeTime).ToList();

                        double totalProratingHours = Math.Round((employeeTasks_Needsprorating.ToList().Sum(a => a.TimeInMinutes)) / 60.0, 2);
                        if (!Double.IsNaN(totalProratingHours) && totalProratingHours > 0)
                        {
                            List<TMEmployeeTime> employeeTasks_NotNeedsprorating = (from myTMEmployeeTime in employeeTasks
                                                                                    join db_Projects in allProjects
                                                                                    on myTMEmployeeTime.ProjectId equals db_Projects.Id into joinedData
                                                                                    from myProjct in joinedData
                                                                                    where myTMEmployeeTime.Tenant == tenant
                                                                                    && myProjct.Tenant == tenant
                                                                                    && myProjct.IsProrated == false
                                                                                    select myTMEmployeeTime).ToList();

                            double total_NotProratingHours = Math.Round((employeeTasks_NotNeedsprorating.ToList().Sum(a => a.TimeInMinutes)) / 60.0, 2);
                            foreach (var item_M in employeeTasks_NotNeedsprorating)
                            {
                                TMEmployeeTime itemPOCO = tMEmployeeTimeRepository.GetSingle(item_M.Id, tenant);
                                double wIWorkedHours_Employee = item_M.TimeInMinutes;
                                double wIWorkedHours_Employee_Prorated = (wIWorkedHours_Employee / total_NotProratingHours) * totalProratingHours;
                                wIWorkedHours_Employee = wIWorkedHours_Employee + wIWorkedHours_Employee_Prorated;

                                itemPOCO.ProratedDuration = !Double.IsNaN(wIWorkedHours_Employee_Prorated) ? wIWorkedHours_Employee_Prorated : 0;
                                itemPOCO.FullDuration = !Double.IsNaN(wIWorkedHours_Employee) ? wIWorkedHours_Employee : 0;
                                itemPOCO.NeedsProrating = false;
                            }

                            foreach (var item_U in employeeTasks_Needsprorating)
                            {
                                TMEmployeeTime itemPOCO = tMEmployeeTimeRepository.GetSingle(item_U.Id, tenant);
                                itemPOCO.NeedsProrating = false;
                            }

                            currentContext.SaveChanges();
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
