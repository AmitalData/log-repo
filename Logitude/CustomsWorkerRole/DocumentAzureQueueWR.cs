using Azure.Messaging.ServiceBus;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.ExternalServices;
using Newtonsoft.Json;
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
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.Tools.HybridMapping;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using WebFreight.Web.Security;
using System.Transactions;
using Logitude.Server.Tools.Utils;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;


namespace CustomsWorkerRole
{
	public class DocumentAzureQueueWR : CustomsWorkerEntryPoint
	{

		AzureQueueMessageApi AzureQueueMessageApi;
		DocumentApiExecutionService DocumentApiExecutionService;
		UnifreightFillingService UnifreightFillingService;
		static string connectionString;
		static string queueName;
		static string logs;
		int tenant = 0;
		bool _OnStartDone = false;
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
						NetCommonHelper.Logger.DevLog.Instance.WriteError(exception.Message.ToString());

					}
				}
				else Thread.Sleep(new TimeSpan(0, 0, 1));
			}
		}

		private void ConnectClient()
		{
			try
			{
				int tenantConfig = SettingUtil.GetTenantDBFromConfig();
				var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(tenantConfig);
				CustomsEnvironmentSettingPM customsEnvironmentSettingPM = customsEnvironmentSettingQueryService.GetEnvironmentSettingPM(tenantConfig) ?? new CustomsEnvironmentSettingPM();

				DocumentApiExecutionService = new DocumentApiExecutionService(customsEnvironmentSettingPM.CourierDocURL, customsEnvironmentSettingPM.CourierDocKey);
				UnifreightFillingService = new UnifreightFillingService();
				connectionString = customsEnvironmentSettingPM.CourierDocQueueConn;// "Endpoint=sb://amitalqueue-api-10.servicebus.windows.net/;SharedAccessKeyName=tenant5113;SharedAccessKey=ykVKzEXbrsD2+06Y3FgcSpiL6Qj0wL/Mj+ASbCbnt6E=;EntityPath=api_document_tenant5113";
				queueName = customsEnvironmentSettingPM.CourierDocQueueName;//"api_document_tenant5113";
			}
			catch (Exception ex)
			{
				NetCommonHelper.Logger.DevLog.Instance.WriteError(ex.Message.ToString());

			}
		}

		public override void WorkOnce()
		{
			try
			{
				if (_OnStartDone) return ;
				_OnStartDone = true;
				OnStart();
				ExecuteQueue();
			}
			catch (Exception exception)
			{
				Thread.Sleep(new TimeSpan(0, 0, 1));
				_OnStartDone = false;
				NetCommonHelper.Logger.DevLog.Instance.WriteError(exception.Message.ToString());
			}

		}
		public ServiceBusProcessor ExecuteQueue()
		{

			ServiceBusProcessor processor = new ServiceBusClient(connectionString).CreateProcessor(queueName, new ServiceBusProcessorOptions()
			{
				AutoCompleteMessages = false,
				MaxConcurrentCalls = 1,
				ReceiveMode = ServiceBusReceiveMode.PeekLock,
				MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(3),
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
					Task<(bool success, string filepath, string message)> res = DocumentApiExecutionService.DownloadFile(AzureQueueMessageApi, tenant);
					logs += "download file Result.success: " + res?.Result.success + " message: " + res?.Result.message + DateTime.Now.ToString();
					#endregion
					if (res.Result.success)
					{
						string filePath = res.Result.filepath;
						byte[] filedataByte = File.ReadAllBytes(filePath);

						if (CheckIsDocumentPDF(filedataByte))
						{

						    #region Filing the document in the filing system by CreateNewFiling
						     Dictionary<string, string> outParams = new Dictionary<string, string>();
						     outParams.Add("COM_ID", string.Empty);
						     
						     bool fatal_error = false;
						     string message = string.Empty;
						     
						     Dictionary<string, string> inParams = new Dictionary<string, string>();
						     inParams.Add("REMARKS", "document from api");
						     inParams.Add("base64data", "true");
						
							string filedata = Convert.ToBase64String(filedataByte);
							if (CustomsSettingQueryService.GetSettingByTenant(tenant).IsConnectedToUniFreight)
							{
								logs += "before CreateNewFiling " + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
								UnifreightFillingService.CreateNewFiling(inParams, filedata, tenant, out outParams, out fatal_error, out message,isFromCloud: true);
								logs += "after CreateNewFiling  fatal_error: " + fatal_error.ToString() + " message: " + message + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
							}
							#endregion
							if (!fatal_error)
							{
								#region Save document and metadata
								logs += "before SaveDocument" + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
								response = SaveDocument(filePath, outParams["COM_ID"], filedataByte);
								logs += "after SaveDocument HasError: " + response?.HasError + "ErrorMessage: " + response.ErrorMessage + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
								#endregion
								if (!response.HasError)
								{
									#region Update on the receipt and filing of the document
									logs += "before UpdateParcelStatus" + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
									var r = DocumentApiExecutionService.UpdateParcelStatus(AzureQueueMessageApi, res.Result.success, res.Result.message);
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
							logs += "dont success document from  DownloadFile is not pdf type";
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
					if (!string.IsNullOrEmpty(communicationLogId))
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
		private bool CheckIsDocumentPDF(byte[] fileBytes)
		{
			try
			{
				if (fileBytes.Length >= 5 &&
					fileBytes[0] == 0x25 && // %
					fileBytes[1] == 0x50 && // P
					fileBytes[2] == 0x44 && // D
					fileBytes[3] == 0x46 && // F
					fileBytes[4] == 0x2D)   // -
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (FormatException)
			{
				return false;
			}
		}

		public void DebugStep()
		{
			ConnectClient();
			ExecuteQueue();
		}

		private Response SaveDocument(string filePath, string commId, byte[] filedataByte)
		{
			string fileName = Path.GetFileName(filePath);
			long fileSize = new System.IO.FileInfo(filePath).Length;
			string fileExtension = Path.GetExtension(fileName).Substring(1);
			string customsDocumentTypeCode = AzureQueueMessageApi.Params?.TryGetValue("documentType", out var val) == true && !string.IsNullOrWhiteSpace(val) ? val : fileName?.Split('_')[1];
			string hawb = AzureQueueMessageApi.Params?.TryGetValue("parcelTrackingNumber", out var val1) == true && !string.IsNullOrWhiteSpace(val1) ? val1 : fileName?.Split('_')[2].Split('.')[0];
			string PartnerCode = AzureQueueMessageApi.PartnerName;
			string code = CodeCounter.GetNumber("DocumentsFiling", tenant, false).ToString();//> CUS - 26043 </ Code >  //TODO 


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
			DeclarationPM declaration = declarationQuery.GetDeclarationsByHawbAndIntegratore(tenant, hawb, card?.Id);

			DocumentsFilingPM documentsFilingPM = new DocumentsFilingPM();

			documentsFilingPM.BackedupExternally = false;
			documentsFilingPM.BranchId = branchRepository.GetSingleBranch(user?.BranchId, tenant)?.Code;
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
			documentsFilingPM.Id = commId;
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
			documentsFilingPM.SecurityId = "";
			documentsFilingPM.SentSize = 0;
			documentsFilingPM.SignDueDate = null;
			documentsFilingPM.Tenant = tenant;
			documentsFilingPM.UpdateDate = null;
			documentsFilingPM.UpdatedByUserCode = user.Code;
			documentsFilingPM.UpdatedByUserId = user.Code;
			documentsFilingPM.IsFromCloud = true;
			documentsFilingPM.FileData = filedataByte;

			var res = UpsertDocumentData(documentsFilingPM);
			return res;
		}
		public Response UpsertDocumentData(DocumentsFilingPM entityPM)
		{

			Response response = new Response();
			TransactionScope scope = null;
			try

			{
				CheckLock(entityPM.Id);
				using (scope = new TransactionScope(TransactionScopeOption.Required,
							new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
				{

					ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
					IWebFreightContext webFreightContext = WebFreightContext.GetContext(entityPM.Tenant);
					DocumentsFilingService service = new DocumentsFilingService(commonContext, entityPM.Tenant);
					DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(commonContext);
					DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);

					entityPM.IsHybrid = true;
					if (string.IsNullOrEmpty(entityPM.FileExtension))
					{
						response.HasError = true;
						response.ErrorMessage += "FileExtension field is required" + Environment.NewLine;

					}
					response = DocumentsFilingHybridMapping.MapEntityToLogitude(entityPM);

					if (!response.HasError)
					{
						if (string.IsNullOrEmpty(entityPM.Folder))
						{
							entityPM.Folder = "docsin";
						}

						entityPM.HasFile = true;
						entityPM.ReceivedDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
						entityPM.Received = true;

						DocumentsFilingPM documentInPM = documentsFilingQuery.GetSinglePM(entityPM.Id, entityPM.Tenant);

						if (documentInPM == null)
						{
							service.SetChangeSet(entityPM.DocumentsFilingMetaDataValues);
							service.Create(entityPM, entityPM.FileData, null, entityPM.FileData == null ? true : false);
							commonContext.SaveChanges();
						}
						response.Result = entityPM.Id;
						response.Result2 = entityPM.SecurityId;
					}
					scope.Complete();

					return response;
				}
			}
			catch (System.Data.Entity.Validation.DbEntityValidationException e)
			{
				string Error = "";
				foreach (var eve in e.EntityValidationErrors)
				{
					Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
						eve.Entry.Entity.GetType().Name, eve.Entry.State);
					foreach (var ve in eve.ValidationErrors)
					{
						Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
							ve.PropertyName, ve.ErrorMessage);

						Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
					}
				}

				response.HasError = true;
				response.ErrorMessage = Error;

				return response;
			}
			catch (Exception ex)
			{
				response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
				response.HasError = true;
				response.ErrorMessage = ex.Message;
				response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
				if (!string.IsNullOrEmpty(ex.StackTrace))
				{
					response.ErrorMessage += Environment.NewLine + ex.StackTrace;
				}
				return response;
			}
			finally
			{
			}

		}

		public void CheckLock(string id)
		{
			string key = ProcessLockTableUtil.Instance.GetKey4UCBUD2LT(id, tenant);
			var repo = new GeneralLockRepository(tenant);
			var lockPoco = repo.GetSingleGeneralLockNOWAIT(key, tenant);
			if (lockPoco == null)
			{
				using (var scope = TransactionFactory.GetNewTransaction())
				{
					repo.Add(new GeneralLock()
					{
						Tenant = tenant,
						GeneralKey = key,
						CreatedAt = TenantServerConfigration.GetCurrentDateTime(tenant)
					});
					repo.SubmitChanges();
					scope.Complete();
				}


			}
		}



		public static List<DocumentsFilingMetaDataValuePM> GetMapDocumentsFilingMetaDataValue(int tenant, string hawb, string IntegratorCode)
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
