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
            this.RunTask();
            //this.CalculatingTheProratingProjectsTime();
        }

        private void RunTask()
        {
            try
            {
                int tenant = 0;

                ITimeManagementContext iContext = TimeManagementContext.GetContext(tenant);
                TMEmployeeTimeRepository iRepository = new TMEmployeeTimeRepository(iContext);

                var dataGroups = (from d in iContext.TMEmployeeTimes
                                  where
                                  d.NeedsProrating == true
                                  group d by new { d.EmployeeUserId, d.SprintId } into g
                                  select new
                                  {
                                      SprintId = g.Key.SprintId,
                                      EmployeeUserId = g.Key.EmployeeUserId,
                                  }).ToList();

                if (dataGroups.Count > 0)
                {
                    foreach (var itemGroup in dataGroups)
                    {
                        IQueryable<TMEmployeeTime> iQueryable = iRepository.GetAllWithoutTenant();
                        iQueryable = iQueryable.Where(d => d.SprintId == itemGroup.SprintId && d.EmployeeUserId == itemGroup.EmployeeUserId);

                        List<TMEmployeeTime> itemsProrated =
                            (from EmployeeTimes in iQueryable
                             join Projects in iContext.TMProjects on EmployeeTimes.ProjectId equals Projects.Id
                             where EmployeeTimes.ProjectId != null && EmployeeTimes.ProjectId != "" && Projects.IsProrated == true && !Projects.ExcludeFromProrating
                             select EmployeeTimes).ToList();

                        if (itemsProrated.Count > 0)
                        {
                            double itemsProratedMinutes = itemsProrated.Sum(s => s.TimeInMinutes);

                            if (itemsProratedMinutes > 0)
                            {
                                List<TMEmployeeTime> itemsNotProrated
                                    = (
                                    (from EmployeeTimes in iQueryable
                                     join Projects in iContext.TMProjects on EmployeeTimes.ProjectId equals Projects.Id
                                     where EmployeeTimes.ProjectId != null && EmployeeTimes.ProjectId != ""
                                     && Projects.IsProrated == false
                                     select EmployeeTimes)

                                     .Union

                                     (from EmployeeTimes in iQueryable
                                      where EmployeeTimes.ProjectId == null || EmployeeTimes.ProjectId == ""
                                      select EmployeeTimes)
                                      ).ToList();

                                double itemsNotProratedMinutes = itemsNotProrated.Sum(s => s.TimeInMinutes);

                                foreach (TMEmployeeTime item in itemsNotProrated)
                                {
                                    double iProratedDuration = item.TimeInMinutes / itemsNotProratedMinutes * itemsProratedMinutes;
                                    double iFullDuration = item.TimeInMinutes + iProratedDuration;

                                    item.ProratedDuration = Math.Round(iProratedDuration, 2);
                                    item.FullDuration = Math.Round(iFullDuration, 2);
                                    item.NeedsProrating = false;

                                    iRepository.Update(item);
                                }

                                foreach (TMEmployeeTime item in itemsProrated)
                                {
                                    item.ProratedDuration = 0;
                                    item.FullDuration = item.TimeInMinutes;
                                    item.NeedsProrating = false;
                                    iRepository.Update(item);
                                }

                                iRepository.SubmitChanges();
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
            }
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
