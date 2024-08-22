using Azure.Messaging.ServiceBus;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.ExternalServices;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebFreight.Web.Helpers.WorkerRoleHelpers;
using WebFreight.Web.WcfApi;
using Logitude.Customs.Def.EntityPMs;

using System.Diagnostics;
using Simplog.Global.Data.GlobalModel.Repositories;


namespace CustomsWorkerRole
{
	public class DocumentAzureQueueWR : CustomsWorkerEntryPoint
	{

		//DbQueueService queueService;
		static AzureQueueMessageApi AzureQueueMessageApi;
		DocumentApiExecutionService DocumentApiExecutionService;
		UnifreightFillingService UnifreightFillingService;
		static string connectionString;
		static string queueName;
		static string logs;
		int tenant = 0;
		public DocumentAzureQueueWR()
		{

		}

		public override bool OnStart()
		{
			ThreadId = Guid.NewGuid().ToString();
			DoneItemsInRange = new Dictionary<DateTime, int>();
			ConnectClient();
			return base.OnStart();
		}


		public override void Run()
		{
			while (true)
			{
				if (!General.IsUpdating())
				{
					try
					{
						WorkOnce();
						Thread.Sleep(TimeSpan.FromSeconds(1));

					}
					catch (Exception exception)
					{
						Thread.Sleep(new TimeSpan(0, 0, 1));
					}
				}
				else Thread.Sleep(new TimeSpan(0, 0, 1));
			}
		}

		private void ConnectClient()
		{
			try
			{
				var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(1);
				CustomsEnvironmentSettingPM customsEnvironmentSettingPM = customsEnvironmentSettingQueryService.GetEnvironmentSettingPM() ?? new CustomsEnvironmentSettingPM();

				DocumentApiExecutionService = new DocumentApiExecutionService(customsEnvironmentSettingPM.CourierDocURL, customsEnvironmentSettingPM.CourierDocKey);
				UnifreightFillingService = new UnifreightFillingService();
				connectionString = customsEnvironmentSettingPM.CourierDocQueueConn;// "Endpoint=sb://amitalqueue-api-10.servicebus.windows.net/;SharedAccessKeyName=tenant5113;SharedAccessKey=ykVKzEXbrsD2+06Y3FgcSpiL6Qj0wL/Mj+ASbCbnt6E=;EntityPath=api_document_tenant5113";
				queueName = customsEnvironmentSettingPM.CourierDocQueueName;//"api_document_tenant5113";
			}
			catch (Exception ex)
			{
			}
		}

		public override void WorkOnce()
		{
			while (!WorkerRoleServiceLocator.PleaseShutDown)
			{
				try
				{
					ExecuteQueue();
				}
				catch (Exception exception)
				{
					Thread.Sleep(new TimeSpan(0, 0, 1));
				}

			}
		}
		public  ServiceBusProcessor ExecuteQueue()
		{

			ServiceBusProcessor processor = new ServiceBusClient(connectionString).CreateProcessor(queueName, new ServiceBusProcessorOptions()
			{
				AutoCompleteMessages = false,
				MaxConcurrentCalls = 1,
				ReceiveMode = ServiceBusReceiveMode.PeekLock,
				MaxAutoLockRenewalDuration = TimeSpan.FromSeconds(90),
			});

			processor.ProcessMessageAsync += async (args) =>
			{
				string communicationLogId = string.Empty;
				logs = string.Empty;
				Response response = new Response();
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				try
				{
					#region  get data from queue           
					string queueId = args.Message.Body.ToString();
					AzureQueueMessageApi = JsonConvert.DeserializeObject<AzureQueueMessageApi>(queueId);
					TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();				
					tenant = tenantManagementRepository.GetTenantManagementByExportTenant(Convert.ToInt32(AzureQueueMessageApi.Tenant)).Id;
					#endregion

					communicationLogId = DocumentApiExecutionService.AddCommunicationLog(AzureQueueMessageApi, queueId, tenant);
					#region Download file
					Task<(bool success, string filepath, string message)> res = DocumentApiExecutionService.DownloadFile(AzureQueueMessageApi,tenant);
					logs += "download file Result.success: " + res?.Result.success + " message: "+ res?.Result.message + DateTime.Now.ToString();
					#endregion
					if (res.Result.success)
					{
						
						#region Filing the document in the filing system by CreateNewFiling
						string filePath = res.Result.filepath;
						Dictionary<string, string> outParams; 
		                bool fatal_error = false;
						string message = string.Empty;

						Dictionary<string, string> inParams = new Dictionary<string, string>();
						inParams.Add("REMARKS", "document from api");
						inParams.Add("base64data", "true");
						var filedata = Convert.ToBase64String(File.ReadAllBytes(filePath));
						logs += "before CreateNewFiling " + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
						UnifreightFillingService.CreateNewFiling(inParams, filedata, tenant, out outParams, out fatal_error, out message);
						logs += "after CreateNewFiling  fatal_error: " + fatal_error.ToString() + " message: " + message + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
                        #endregion
						if (!fatal_error) 
						{
							#region Save document and metadata
							logs += "before SaveDocument" + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
							response = SaveDocument(filePath);
							logs += "after SaveDocument HasError: " + response?.HasError + "ErrorMessage: " + response.ErrorMessage + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
							#endregion
							if (!response.HasError) {
							    #region Update on the receipt and filing of the document
							    logs += "before UpdateParcelStatus" + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
							   var r =  DocumentApiExecutionService.UpdateParcelStatus(AzureQueueMessageApi, res.Result.success, res.Result.message);
							    logs += "after UpdateParcelStatus" + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
								#endregion
								DocumentApiExecutionService.UpdateCommunicationLog(communicationLogId, tenant, logs, response?.Result, "D");
								await args.CompleteMessageAsync(args.Message);
							}
							else
							{
								logs += " dont success SaveDocument";
								DocumentApiExecutionService.UpdateCommunicationLog(communicationLogId, tenant, logs, response?.Result, "F");
							}
						}
						else
						{
							logs += " dont success CreateNewFiling";
							DocumentApiExecutionService.UpdateCommunicationLog(communicationLogId, tenant, logs, response?.Result, "F");
						}
					}
					else
					{
						logs += " dont success DownloadFile";
						DocumentApiExecutionService.UpdateCommunicationLog(communicationLogId, tenant, logs, response?.Result, "F");
					}								
				}
				catch (Exception e)
				{
					if(!string.IsNullOrEmpty(communicationLogId))
					{
						logs += "exption" + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
						Communications.UpdateCommunicationLogStatus(communicationLogId, tenant, null, "F", logs, e.Message.ToString());
					}
				}
				finally
				{
					stopwatch.Stop();
				}
			};
			processor.ProcessErrorAsync += (args) =>
			{
				return Task.CompletedTask;
			};

			processor.StartProcessingAsync().GetAwaiter().GetResult();

			return processor;
		}

		public void DebugStep()
		{		
			ConnectClient();
			ExecuteQueue();
		}

		private  Response SaveDocument(string filePath)
		{
			string fileName = Path.GetFileName(filePath);
			long fileSize = new System.IO.FileInfo(filePath).Length;
			string fileExtension = Path.GetExtension(fileName).Substring(1);
			string customsDocumentTypeCode = fileName?.Split('_')[1];
			string hawb = fileName?.Split('_')[2].Split('.')[0];
			string PartnerCode = AzureQueueMessageApi.PartnerName;
			string code = CodeCounter.GetNumber("DocumentsFiling", tenant, false).ToString();//> CUS - 26043 </ Code >  //TODO 

			var guid = Guid.NewGuid();
			var documentsFilingId = Convert.ToBase64String(guid.ToByteArray()).ToLower();
			documentsFilingId = documentsFilingId.Substring(0, 22);
			documentsFilingId = documentsFilingId.Replace("/", "_");
			documentsFilingId = documentsFilingId.Replace("+", "-");

			UserQuery userQuery = new UserQuery(tenant);
			CardRepository cardRepository = new CardRepository(tenant);
			DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
			DeclarationQueryService declarationQuery = new DeclarationQueryService(tenant);
			ComputingPartnerTranslationHelper computingPartnerTranslationHelper = new ComputingPartnerTranslationHelper(tenant);
			BranchRepository branchRepository = new BranchRepository(tenant);

			string userEmail = computingPartnerTranslationHelper.GetLogitudeCodeTranslation(PartnerCode, "CourierDec", "User");
			string cardCode = computingPartnerTranslationHelper.GetLogitudeCodeTranslation(PartnerCode, "CourierDec", "Card");

			UserPM user = userQuery.GetSingleUserPMByEmail(userEmail, tenant, true);
			Card card = cardRepository.GetSingleCardByCodeAndType(cardCode, "CS", tenant, true);
			DocumentType documentType = documentTypeQuery.GetDocumentTypeByCode(customsDocumentTypeCode, tenant);
			DeclarationPM declaration = declarationQuery.GetDeclarationsByHawbAndIntegratore(tenant,hawb, card?.Id);

			DocumentsFilingPM documentsFilingPM = new DocumentsFilingPM();

			documentsFilingPM.BackedupExternally = false;
			documentsFilingPM.BranchId = branchRepository.GetSingleBranch(user?.BranchId,tenant)?.Code;
			documentsFilingPM.BufferNumber = 0;
			documentsFilingPM.CancellSignRequest = false;
			documentsFilingPM.Code = code;
			documentsFilingPM.CreateDate = DateTime.Now;
			documentsFilingPM.CreatedByUserCode = user.Code;
			documentsFilingPM.CreatedByUserId = user.Code;
			documentsFilingPM.CreatedByUserName = user?.EnglishName;
			documentsFilingPM.CustomerTenantNumber = null;
			documentsFilingPM.CustomsDocumentTypeCode = customsDocumentTypeCode;
			documentsFilingPM.CustomsDocumentTypeName = documentType.Name;
			documentsFilingPM.DeleteDateTime = null;
			documentsFilingPM.DepartmentId = null;
			documentsFilingPM.Description = "ECOM DOC " + customsDocumentTypeCode;
			documentsFilingPM.DirectionCode = "I";
			documentsFilingPM.DocumentId = null;
			documentsFilingPM.DocumentTypeCode = customsDocumentTypeCode;
			documentsFilingPM.DocumentTypeId = documentType?.Code;
			documentsFilingPM.DocumentTypeName = documentType?.Name;
			documentsFilingPM.DocumentsFilingMetaDataValues = GetMapDocumentsFilingMetaDataValue(tenant, hawb, card?.Id);//> integrator + hawb
			documentsFilingPM.DontAddToQueue = false;
			documentsFilingPM.DontDeleteRealFile = false;
			documentsFilingPM.DoucmentTypeTemplateFormatCode = "P";
			documentsFilingPM.EntityId = declaration?.Id;
			documentsFilingPM.EntityNumber = declaration?.CustomFileNo;
			documentsFilingPM.ExternalEntityName = declaration?.CustomFileNo != null ? "CFIFILEM" : null;
			documentsFilingPM.ExternalEntityReference = declaration?.CustomFileNo;
			documentsFilingPM.FileExtension = fileExtension;
			documentsFilingPM.FileName = fileName;
			documentsFilingPM.FileSize = fileSize;
			documentsFilingPM.Folder = "docsin";
			documentsFilingPM.FollowUpCount = 0;
			documentsFilingPM.FromCTool = false;
			documentsFilingPM.HasCopies = false;
			documentsFilingPM.HasFile = true;
			documentsFilingPM.HasFollowUp = false;
			documentsFilingPM.Id = documentsFilingId;
			documentsFilingPM.IsAgentSharedInDirect = false;
			documentsFilingPM.IsAgentSharedInHouse = false;
			documentsFilingPM.IsAgentSharedInMaster = false;
			documentsFilingPM.IsAgentView = true;
			documentsFilingPM.IsApprovalRequired = false;
			documentsFilingPM.IsAttachSelect = false;
			documentsFilingPM.IsAttachment = false;
			documentsFilingPM.IsCustomReference = false;
			documentsFilingPM.IsCustomerUploadPermission = false;
			documentsFilingPM.IsCustomerView = true;
			documentsFilingPM.IsDeleted = false;
			documentsFilingPM.IsDigitalSignRequired = false;
			documentsFilingPM.IsDigitallySigned = false;
			documentsFilingPM.IsFromDigital = false;
			documentsFilingPM.IsFromUnifreightPodMobile = false;
			documentsFilingPM.IsHybrid = false;
			documentsFilingPM.IsMetaDataReady = false;
			documentsFilingPM.IsRequested = false;
			documentsFilingPM.IsSharedIn = false;
			documentsFilingPM.IsSharedOut = false;
			documentsFilingPM.IsSharedWithCustomer = false;
			documentsFilingPM.IsSharedWithForwarder = false;
			documentsFilingPM.IsTransferdToQBO = true;
			documentsFilingPM.IsUoloadedField = false;
			documentsFilingPM.IsUpdateSharedDocument = false;
			documentsFilingPM.LastShareDate = null;
			documentsFilingPM.LastVersion = 1;//> after filing service
			documentsFilingPM.Name = documentType.Name;
			documentsFilingPM.NoAddToTasksQueue = false;
			documentsFilingPM.ObjectTableId = "Customs.Declaration";
			documentsFilingPM.ObjectTableName = "Customs.Declaration";
			documentsFilingPM.OwnerId = user.Code;
			documentsFilingPM.OwnerUserCode = user.Code;
			documentsFilingPM.Received = true;
			documentsFilingPM.ReceivedDate = null;
			documentsFilingPM.SearchFields = code + "," + declaration?.CustomFileNo + "," + documentType?.Code + "," + documentType?.Name + "," + user?.EnglishName + "," + user?.LocalName;//add DESCREPTION
		    documentsFilingPM.SecurityId = "";//ask tomer
			documentsFilingPM.SentSize = 0;
			documentsFilingPM.SignDueDate = null;
			documentsFilingPM.Tenant = tenant;
			documentsFilingPM.UpdateDate = null;
			documentsFilingPM.UpdatedByUserCode = user.Code;
			documentsFilingPM.UpdatedByUserId = user.Code;
			
			DocumentInWcfService documentInWcfService = new DocumentInWcfService();
		    var res = documentInWcfService.Upsert(documentsFilingPM, false);
			return res;
		}
		public static List<DocumentsFilingMetaDataValuePM> GetMapDocumentsFilingMetaDataValue(int tenant, string hawb,string IntegratorCode)
		{
			List<DocumentsFilingMetaDataValuePM> DocumentsFilingMetaDataValuelist = new List<DocumentsFilingMetaDataValuePM>();

			DocumentsFilingMetaDataValuePM DocumentsFilingMetaDataValue = new DocumentsFilingMetaDataValuePM();
			DocumentsFilingMetaDataValue.ChangeSetOp = ChangeSetOperation.None;
			DocumentsFilingMetaDataValue.Tenant = tenant;
			DocumentsFilingMetaDataValue.DocumentsMetaDataTypeId = "CARFI";
			DocumentsFilingMetaDataValue.MetaDataValue = hawb;
			DocumentsFilingMetaDataValuelist.Add(DocumentsFilingMetaDataValue);

			DocumentsFilingMetaDataValue = new DocumentsFilingMetaDataValuePM();
			DocumentsFilingMetaDataValue.ChangeSetOp = ChangeSetOperation.None;
			DocumentsFilingMetaDataValue.Tenant = tenant;
			DocumentsFilingMetaDataValue.DocumentsMetaDataTypeId = "INTGR_R";
			DocumentsFilingMetaDataValue.MetaDataValue = IntegratorCode;
			DocumentsFilingMetaDataValuelist.Add(DocumentsFilingMetaDataValue);

			return DocumentsFilingMetaDataValuelist;
		}

	}



}
