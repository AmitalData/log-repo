using CommunicationWorkerRole.Services;
using Logitude.BL.InfrastructureModel.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.IntegrationTests.FTP
{
	[TestClass]
	public class FTPSchedulerTaskTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			//	< SchedulerDetails xmlns: i = "http://www.w3.org/2001/XMLSchema-instance" >
			//< FTPDetails >
			//< Extension />
			//< Folder > DocumentsBackup </ Folder >
			//< From > Islam </ From >
			//< Host > 192.168.1.26 </ Host >
			//< Password > !I123456 </ Password >
			//< Prefix />
			//< Subject > test </ Subject >
			//< Suffix />
			//< UserName > islam </ UserName >
			//</ FTPDetails >
			//</ SchedulerDetails >

			SchedulerDetails schedulerDetails = new SchedulerDetails()
			{
				Tenant = 1,
				FTPDetails = new FTPSchedulerDetails()
				{
					Extension = "pdf",
					Folder = "DocumentsBackup",
					From = "My FTP Server",
					Host = "192.168.1.26",
					Password = "!I123456",
					Prefix = "is",
					Subject = "test",
					Suffix = "",
					UserName = "Islam",
				}
			};
			FTPSchedulerTaskService fTPSchedulerTaskService = new FTPSchedulerTaskService(schedulerDetails);
			fTPSchedulerTaskService.ReadFTPFilesBySchedulerDetailsToAnalyzeQueue();
		}
	}
}
