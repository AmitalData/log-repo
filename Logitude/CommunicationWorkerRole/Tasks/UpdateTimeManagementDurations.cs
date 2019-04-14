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
                             where EmployeeTimes.ProjectId != null && EmployeeTimes.ProjectId != null && Projects.IsProrated == true && !Projects.ExcludeFromProrating
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
                                     where EmployeeTimes.ProjectId != null && EmployeeTimes.ProjectId != null
                                     && Projects.IsProrated == false && !Projects.ExcludeFromProrating
                                     select EmployeeTimes)

                                     .Union

                                     (from EmployeeTimes in iQueryable
                                      where EmployeeTimes.ProjectId == null || EmployeeTimes.ProjectId == null
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
    }
}
