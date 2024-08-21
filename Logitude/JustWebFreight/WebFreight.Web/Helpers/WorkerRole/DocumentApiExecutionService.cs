using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.SystemLogs;
using WebFreight.Web.DataContracts;
using Logitude.Server.Tools.Helpers;
using System.Threading.Tasks;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System.Data.Entity.Core.Objects;
using System.Text;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.BL.CommonDataModel.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.Helpers.WorkerRoleHelpers
{
    public class DocumentApiExecutionService
    {

		static string Address = string.Empty;//"https://customs.amital.co.il/amitalapi";
		static string Token = string.Empty;//"367ae8b2-7d11-4332-b8b5-eeccf3443d3c";
		static string x_functions_key = string.Empty;//"FnDaCtzap5CTgEBFTDXJAKBiRqL29Xnj2HGpSju3PfKtAzFu9Q7CEA==";
		public  DocumentApiExecutionService(string courierDocURL, string courierDocKey)
        {	
			Address = courierDocURL;
			x_functions_key = courierDocKey;		
		}
		public async static Task<(bool success, string filepath, string message)> DownloadFile(AzureQueueMessageApi queueMessage)
		{
			Token = CustomsSettingQueryService.GetSettingByTenant(Convert.ToInt32(queueMessage.Tenant)).CourierDocToken;
			string filepath = "";
			try
			{
				var client = new HttpClient();
				var request = new HttpRequestMessage(HttpMethod.Post, Path.Combine(Address, "GetDocument"));
				request.Headers.Add("token", Token);
				request.Headers.Add("x-functions-key", x_functions_key);
				var content = new StringContent("{\"RequestFileId\":\"" + queueMessage.RequestFileId + "\", \"Tenant\": \"" + queueMessage.Tenant + "\"}", null, "application/json");
				request.Content = content;
				var response = await client.SendAsync(request);
				response.EnsureSuccessStatusCode();
				string blobfilename = queueMessage.BlobFilename;
				filepath = GetTempDirectory();
				filepath = Path.Combine(filepath, blobfilename);
				using (FileStream outputFileStream = new FileStream(filepath, FileMode.Create))
				{
					var res = response.Content.CopyToAsync(outputFileStream);
					res.Wait();
				}

				return (true, filepath, "");
			}
			catch (Exception ex)
			{
				return (false, "", $"Failed -> DownloadFile: RequestFileId = {queueMessage.RequestFileId} -  {ex.Message}{Environment.NewLine}{ex.ToString()}");
			}
		}
		public async static Task UpdateParcelStatus(AzureQueueMessageApi queueMessage, bool success, string message)
		{
			try
			{
				var client = new HttpClient();
				var request = new HttpRequestMessage(HttpMethod.Post, Path.Combine(Address, "UpdateParcelStatus"));
				request.Headers.Add("token", Token);
				request.Headers.Add("x-functions-key", x_functions_key);
				var item = new
				{
					RequestFileId = queueMessage.RequestFileId,
					Tenant = queueMessage.Tenant,
					DownloadAck = success ? 1 : -1,
					DownloadDate = DateTimeOffset.Now,
					DownloadMess = message
				};
				var list = new[]
				{
					new
					{
						RequestFileId = queueMessage.RequestFileId,
						Tenant = queueMessage.Tenant,
						DownloadAck = success ? 1 : -1,
						DownloadDate = DateTimeOffset.Now,
						DownloadMess = message
					}
				};
				var jsonData = JsonConvert.SerializeObject(list);
				var content = new StringContent(jsonData, null, "application/json");
				request.Content = content;
				var response = await client.SendAsync(request);
			}
			catch (Exception ex)
			{
				//Logger.LogMe($"Failed --> UpdateParcelStatus: {ex.Message}{Environment.NewLine}{ex.ToString()}", true);
			}

		}

		static object obj = new object();
		static string ProjectPrefix = "API";
		internal static string GetTempDirectory()
		{
			string threadtempfolder = Path.Combine(Path.GetTempPath(), "SharOlami", ProjectPrefix,
				String.Format("{0:yyyyMMdd}", DateTime.Now) + "_" + Thread.CurrentThread.ManagedThreadId);
			if (Directory.Exists(threadtempfolder))
				return threadtempfolder;
			lock (obj)
			{
				string baseDir = Path.Combine(Path.GetTempPath(), "SharOlami");
				if (!Directory.Exists(baseDir))
					Directory.CreateDirectory(baseDir);
				string baseDir2 = Path.Combine(baseDir, ProjectPrefix);
				if (!Directory.Exists(baseDir2))
					Directory.CreateDirectory(baseDir2);
				if (!Directory.Exists(threadtempfolder))
					Directory.CreateDirectory(threadtempfolder);
				return threadtempfolder;
			}
		}
		public static string AddCommunicationLog(AzureQueueMessageApi queueMessage,string queueId)
		{   
			int tenant = Convert.ToInt32(queueMessage.Tenant);
			byte[] logXML = Encoding.UTF8.GetBytes(queueId);// LogitudeXmlSerializer.SerializeObject(queueMessage);
			ObjectTableQuery tablesQuery = new ObjectTableQuery(Convert.ToInt32(queueMessage.Tenant));
			string hawb = queueMessage.BlobFilename?.Split('_')[2]?.Split('.')[0];

			CommunicationsParams logParams = new CommunicationsParams()
			{
				Tenant = Convert.ToInt32(queueMessage.Tenant),
				From = queueMessage.PartnerName,
				To = "amital",
				CommunicationLogTypeCode = "A",
				InOut = "O",
				Status = "P",
				Subject = "Courier Document API",
				ByteData = logXML,
				LoggingEntityReference = hawb,
				Logs = "Filename: " + queueMessage.BlobFilename,
				FolderName = "CourierDocumentAPI"
			};
			string communicationLogId = Communications.AddCommunicationLog(logParams);
			return communicationLogId;
		}
		public void UpdateCommunicationLog(string communicationLogId, int tenant, string logs,string entityId)
		{
			ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
			CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
			CommunicationLogQuery communicationLogQuery = new CommunicationLogQuery(communicationLogRepository);
			CommunicationLog commLog = communicationLogRepository.GetSingleCommunicationLog(communicationLogId, tenant);
			ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);
			CommunicationLogService communicationLogService = new CommunicationLogService(commonContext, tenant);
			commLog.Logs += logs;
			commLog.CommunicationStatusTypeCode = "D";
			commLog.EntityId = entityId;
			commLog.ObjectTableId = tablesQuery.GetObjectTableIdByName("DocumentsFiling");
			communicationLogRepository.Update(commLog);
			communicationLogRepository.SubmitChanges();
		}
		public static string GetFormatedElapsedTime(TimeSpan timeSpan)
		{
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
				timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
				timeSpan.Milliseconds / 10);
			return elapsedTime;
		}
		public string GetComputingPartnerCodeTranslation(string logitudeCode, string computingPartner, string objectTableName, int tenant)
		{
			ICommonDataContext context;
			ObjectTableRepository myObjectTabelRepository;
			ComputingPartnerQuery computingPartnerQuery;
			ComputingPartnerTranslationQuery computingPartnerTranslationQuery;
			context = CommonDataContext.GetContext(tenant);
			myObjectTabelRepository = new ObjectTableRepository(tenant);
			computingPartnerQuery = new ComputingPartnerQuery(new ComputingPartnerRepository(context));
			computingPartnerTranslationQuery = new ComputingPartnerTranslationQuery(new ComputingPartnerTranslationRepository(context));

			ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
			ComputingPartnerPM partner = computingPartnerQuery.GetSinglePMByCode(computingPartner, tenant);
			if (partner == null)
			{
				partner = computingPartnerQuery.GetSinglePMByCode(computingPartner, 0);
			}

			string partnerCode = null;
			if (partner != null && objectTable != null)
			{
				partnerCode = computingPartnerTranslationQuery.GetPartnerCodeTranslation(logitudeCode, partner.Id, objectTable.Id, tenant);
			}

			return partnerCode;
		}


		
	}
	public class AzureQueueMessageApi
	{
		public string TaskName { get; set; }
		public string TaskRef { get; set; }
		public string ClientPartnerApiId { get; set; }
		public string PartnerName { get; set; }
		public string ClientName { get; set; }
		public string BlobFilename { get; set; }
		public string Tenant { get; set; }
		public string ApiQueueId { get; set; }
		public string RequestFileId { get; set; }
		public int Priority { get; set; }

		public IDictionary<string, string> Params { get; set; } = new Dictionary<string, string>();

		public override string ToString()
		{
			return $"Tenant: {Tenant}" +
				$"RequestFileId: {RequestFileId}";
		}
	}
}