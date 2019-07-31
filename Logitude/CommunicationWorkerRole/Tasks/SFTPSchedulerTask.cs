using CommunicationWorkerRole.Services;
using Logitude.BL.InfrastructureModel.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class SFTPSchedulerTask : TaskManagerBase
    {
        TasksSchedulerPM ftpTask = null;
        public SFTPSchedulerTask(string Id, int tenant)
                   : base(Id, tenant)
        {
            if (!string.IsNullOrWhiteSpace(Id))
            {
                TasksSchedulerRepository tasksSchedulerRepository = new TasksSchedulerRepository(tenant);
                TasksSchedulerQuery tasksSchedulerQuery = new TasksSchedulerQuery(tasksSchedulerRepository);
                ftpTask = tasksSchedulerQuery.GetSingleTasksSchedulerPM(Id);
            }
        }

        public override void StartTask()
        {
            if (ftpTask != null && !string.IsNullOrEmpty(ftpTask.SchedulerDetailsXML))
            {

                SchedulerDetails schedulerDetails = LogitudeXmlSerializer.DeserializeObject<SchedulerDetails>(ftpTask.SchedulerDetailsXML);
                schedulerDetails.Tenant = ftpTask.Tenant;
                SFTPSchedulerTaskService fTPSchedulerTaskService = new SFTPSchedulerTaskService();
                fTPSchedulerTaskService.ReadSFTPFilesBySchedulerDetailsToAnalyzeQueue(schedulerDetails);

                foreach(var warning in fTPSchedulerTaskService.WarningsList)
                {
                    this.Logwarning(warning);
                }

                foreach (var message in fTPSchedulerTaskService.MessagesList)
                {
                    this.LogInfo(message);
                }
            }

        }

        

    }
}