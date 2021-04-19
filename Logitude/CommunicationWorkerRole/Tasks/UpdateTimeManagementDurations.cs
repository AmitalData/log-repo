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
        private ITimeManagementContext timeManagementContext;
        private TMEmployeeTimeRepository employeeTimeRepository;
        private IQueryable<TMEmployeeTime> allEmployeeTimes;
        private List<TMEmployeeTime> itemsProrated;
        private List<TMEmployeeTime> itemsExcludedFromProrated;

        public UpdateTimeManagementDurations(string Id, int tenant)
            : base(Id, tenant)
        {
        }

        public override void StartTask()
        {
            this.RunUpdateEmployeeTimeWithProratingTask();
        }
        private void RunUpdateEmployeeTimeWithProratingTask()
        {
            try
            {
                InitializeDefaults();
                var dataGroups = GetEmployeeTimesGroupBySprintAndUserId();
                UpdateEmployeeTimeWithProrating(dataGroups);
            }

            catch (Exception exception)
            {
                PrintException(exception);
            }
        }
        private void InitializeDefaults()
        {
            int tenant = 0;
            timeManagementContext = TimeManagementContext.GetContext(tenant);
            employeeTimeRepository = new TMEmployeeTimeRepository(timeManagementContext);
        }
        private dynamic GetEmployeeTimesGroupBySprintAndUserId()
        {
            var employeeTimesGroups = (from d in timeManagementContext.TMEmployeeTimes
                              where
                              d.NeedsProrating == true
                              group d by new { d.EmployeeUserId, d.SprintId } into g
                              select new
                              {
                                  SprintId = g.Key.SprintId,
                                  EmployeeUserId = g.Key.EmployeeUserId,
                              }).ToList();

            return employeeTimesGroups;
        }
        private void UpdateEmployeeTimeWithProrating(dynamic dataGroups)
        {
            if (dataGroups.Count > 0)
            {
                foreach (var itemGroup in dataGroups)
                {
                    SetProratedQueries(itemGroup);
                    if (itemsProrated.Count > 0)
                    {
                        double itemsProratedMinutes = itemsProrated.Sum(s => s.TimeInMinutes);

                        if (itemsProratedMinutes > 0)
                        {
                            UpdateItemsNotProrated(itemsProratedMinutes);
                            UpdateItemsProrated(itemsProrated);
                            UpdateItemsExcludedProrated(itemsExcludedFromProrated);
                            employeeTimeRepository.SubmitChanges();
                        }
                    }
                }
            }
        }
        private void UpdateItemsNotProrated(double itemsProratedMinutes)
        {
            List<TMEmployeeTime> itemsNotProrated
                                = (
                                (from EmployeeTimes in allEmployeeTimes
                                 join Projects in timeManagementContext.TMProjects on EmployeeTimes.ProjectId equals Projects.Id
                                 where EmployeeTimes.ProjectId != null
                                 && Projects.IsProrated == false && !Projects.ExcludeFromProrating
                                 select EmployeeTimes)

                                 .Union

                                 (from EmployeeTimes in allEmployeeTimes
                                  where EmployeeTimes.ProjectId == null
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
                employeeTimeRepository.Update(item);
            }
        }
        private void UpdateItemsProrated(List<TMEmployeeTime> itemsProrated)
        {
            foreach (TMEmployeeTime item in itemsProrated)
            {
                item.ProratedDuration = 0;
                item.FullDuration = item.TimeInMinutes;
                item.NeedsProrating = false;
                employeeTimeRepository.Update(item);
            }
        }
        private void UpdateItemsExcludedProrated(List<TMEmployeeTime> itemsExcludedFromProrated)
        {
            foreach (TMEmployeeTime item in itemsExcludedFromProrated)
            {
                item.ProratedDuration = 0;
                item.FullDuration = item.TimeInMinutes;
                item.NeedsProrating = false;
                employeeTimeRepository.Update(item);
            }
        }
        private void SetProratedQueries(dynamic itemGroup)
        {
            allEmployeeTimes = employeeTimeRepository.GetAllWithoutTenant();
            string sprintId = itemGroup.SprintId;
            string employeeUserId = itemGroup.EmployeeUserId;
            allEmployeeTimes = allEmployeeTimes.Where(d => d.SprintId == sprintId && d.EmployeeUserId == employeeUserId);

            itemsProrated =
                (from EmployeeTimes in allEmployeeTimes
                 join Projects in timeManagementContext.TMProjects on EmployeeTimes.ProjectId equals Projects.Id
                 where EmployeeTimes.ProjectId != null && Projects.IsProrated == true && !Projects.ExcludeFromProrating
                 select EmployeeTimes).ToList();

            itemsExcludedFromProrated =
                 (from EmployeeTimes in allEmployeeTimes
                  join Projects in timeManagementContext.TMProjects on EmployeeTimes.ProjectId equals Projects.Id
                  where EmployeeTimes.ProjectId != null && Projects.ExcludeFromProrating
                  select EmployeeTimes).ToList();
        }
        private void PrintException(Exception exception)
        {
            string errorMessage = new StringBuilder().Append("Exception Message: ").AppendLine().Append(exception.Message).AppendLine().ToString();
            errorMessage += new StringBuilder().Append("Stack Trace:").AppendLine().Append(exception.StackTrace).AppendLine().ToString();
            throw new Exception(errorMessage);
        }
    }
}
