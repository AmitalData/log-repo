using CommunicationWorkerRole.Services;
using Logitude.BL.InfrastructureModel.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel.Repositories;

namespace CommunicationWorkerRole.Tasks
{
    public class QueryReport : TaskManagerBase
    {
        TasksSchedulerPM reportTask = null;
        public QueryReport(string Id, int tenant)
                   : base(Id, tenant)
        {
            if (!string.IsNullOrWhiteSpace(Id))
            {
                TasksSchedulerRepository tasksSchedulerRepository = new TasksSchedulerRepository(tenant);
                TasksSchedulerQuery tasksSchedulerQuery = new TasksSchedulerQuery(tasksSchedulerRepository);
                reportTask = tasksSchedulerQuery.GetSingleTasksSchedulerPM(Id);
            }
        }

        public override void StartTask()
        {
            if (reportTask != null && !string.IsNullOrEmpty(reportTask.SchedulerDetailsXML))
            {
				QueryReportService queryReportServiceService = new QueryReportService(this);
				queryReportServiceService.RunTask(reportTask);
            }

        }
    }
}