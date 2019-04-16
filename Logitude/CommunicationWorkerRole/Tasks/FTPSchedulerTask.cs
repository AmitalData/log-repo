using Logitude.BL.InfrastructureModel.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.FTP;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
	public class FTPSchedulerTask : TaskManagerBase
	{
		TasksSchedulerPM ftpTask = null;
		public FTPSchedulerTask(string Id, int tenant)
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
				//try
				//{
				SchedulerDetails schedulerDetails = LogitudeXmlSerializer.DeserializeObject<SchedulerDetails>(ftpTask.SchedulerDetailsXML);

				FTPService ftpService = new FTPService(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password);
				var directoryFiles = ftpService.DirectoryListSimple(schedulerDetails.FTPDetails.Folder).ToList();
				if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Extension))
				{
					directoryFiles = directoryFiles.Where(f => (
					Path.GetExtension(f)
					.Equals(schedulerDetails.FTPDetails.Extension, StringComparison.CurrentCultureIgnoreCase)))
					.ToList();
				}

				if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Prefix))
				{
					directoryFiles = directoryFiles.Where(f => (
					Path.GetExtension(f)
					.StartsWith(schedulerDetails.FTPDetails.Prefix, StringComparison.CurrentCultureIgnoreCase)))
					.ToList();
				}

				foreach (string fileName in directoryFiles)
				{
					string extention = Path.GetExtension(fileName);
					if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(extention))
					{
						string p_message = "";
						var filePath = fileName;
						if (!string.IsNullOrEmpty(schedulerDetails.FTPDetails.Folder) && !fileName.Contains(schedulerDetails.FTPDetails.Folder))
						{
							filePath = schedulerDetails.FTPDetails.Folder + "/" + fileName;
						}

						byte[] fileData = ftpService.Download(fileName, out p_message);

						AddToAnalyzeQueu(fileName, fileData, schedulerDetails, ftpTask.Tenant);

						ftpService.Delete(fileName);
					}
				}
				//}

				//catch (Exception ex)
				//{
				//	ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "FTP To AnalyzeQueue WorkerRole", ex.Message, null);
				//}
			}

		}

		private void AddToAnalyzeQueu(string fileName, byte[] fileData, SchedulerDetails schedulerDetails , int tenant)
		{
			fileName = fileName.Split('/')[fileName.Split('/').Length - 1].ToLower();
			AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();

			AnalyzeQueue analyzeQueue = new AnalyzeQueue()
			{
				Subject = schedulerDetails.FTPDetails.Subject,
				CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
				From = schedulerDetails.FTPDetails.From,
				Id = IdCounter.GetNumber("AnalyzeQueue", 0),
				MessageBody = fileData,
				Status = "W",
				Retries = 0,
				ConnectedToEntity = false,
				ConnectedToTenant = true,
				Tenant = tenant,
				FileSize = fileData.Length,
				FileName = fileName,
			};

			analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
			analyzeQueueReposiory.Add(analyzeQueue);
			analyzeQueueReposiory.SubmitChanges();
		}


	}
}