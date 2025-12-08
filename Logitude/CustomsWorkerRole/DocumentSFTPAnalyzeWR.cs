using CustomsWorkerRole;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.Tools.HybridMapping;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Transactions;
using WebFreight.Web.Helpers.WorkerRoleHelpers;
using WebFreight.Web.Security;

namespace CommunicationWorkerRole
{
	public class DocumentSFTPAnalyzeWR : CustomsWorkerEntryPoint
	{
		AnalyzeQueue AnalyzeQueue;
		
		static string logs;
		int tenant = 0;
		bool _OnStartDone = false;

		//DbQueueService queueservice;
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
		public override void WorkOnce()
		{
			try
			{
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
		public void ExecuteQueue()
		{
			AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
			AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueueBySubject("DocumentSFTP");
			if (analyzeQueue != null)
			{
			
			string communicationLogId = string.Empty;
			logs = string.Empty;
			Response response = new Response();
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			try
			{
				#region  get data from queue           
				AnalyzeQueue = analyzeQueue;
				tenant = AnalyzeQueue.Tenant;
				#endregion

				communicationLogId = DocumentApiExecutionService.AddCommunicationLog(AnalyzeQueue, tenant);

				byte[] filedataByte = analyzeQueue.MessageBody;

				if (DocumentAzureQueueWR.CheckIsDocumentPDF(filedataByte))
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
						UnifreightFillingService.CreateNewFiling(inParams, filedata, tenant, out outParams, out fatal_error, out message, isFromCloud: true);
						logs += "after CreateNewFiling  fatal_error: " + fatal_error.ToString() + " message: " + message + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
					}
					else
					{
						var guid = Guid.NewGuid();
						var base64string = Convert.ToBase64String(guid.ToByteArray()).ToLower();
						base64string = base64string.Substring(0, 22);
						base64string = base64string.Replace("/", "_");
						base64string = base64string.Replace("+", "-");
						outParams["COM_ID"] = base64string;
					}
					#endregion
					if (!fatal_error)
					{
						#region Save document and metadata
						logs += "before SaveDocument" + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
						response = SaveDocument(outParams["COM_ID"], filedataByte);
						logs += "after SaveDocument HasError: " + response?.HasError + "ErrorMessage: " + response.ErrorMessage + "take time: " + DocumentApiExecutionService.GetFormatedElapsedTime(stopwatch.Elapsed) + "date: " + DateTime.Now.ToString();
						#endregion
						if (!response.HasError)
						{						
							DocumentApiExecutionService.UpdateCommunicationLog(communicationLogId, tenant, logs, response?.Result, "D");

							analyzeQueue.Status = "D";
							analyzeQueueRepository.Update(analyzeQueue);
							analyzeQueueRepository.SubmitChanges();
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
			}
		}
		private Response SaveDocument(string commId, byte[] filedataByte)
		{

			string fileName = AnalyzeQueue.FileName;
			double fileSize = AnalyzeQueue.FileSize;
			string fileExtension = Path.GetExtension(fileName).Substring(1);
			string customsDocumentTypeCode = string.Empty;
			string hawb = string.Empty;
			DocumentType documentType = null;

			GetcustomsDocumentTypeAndHawbFromFileName(fileName, out customsDocumentTypeCode, out hawb,out documentType);
			string PartnerCode = AnalyzeQueue.From;
			string code = CodeCounter.GetNumber("DocumentsFiling", tenant, false).ToString();//> CUS - 26043 </ Code >  //TODO 


			UserQuery userQuery = new UserQuery(tenant);
			CardRepository cardRepository = new CardRepository(tenant);
			DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
			DeclarationQueryService declarationQuery = new DeclarationQueryService(tenant);
			ComputingPartnerTranslationHelper computingPartnerTranslationHelper = new ComputingPartnerTranslationHelper(tenant);
			BranchRepository branchRepository = new BranchRepository(tenant);

			string userEmail = computingPartnerTranslationHelper.GetLogitudeCodeTranslation(PartnerCode, "CourierDecSFTP", "User");
			string cardCode = computingPartnerTranslationHelper.GetLogitudeCodeTranslation(PartnerCode, "CourierDecSFTP", "Card");

			UserPM user = userQuery.GetSingleUserPMByEmail(userEmail, tenant, true);
			Card card = cardRepository.GetSingleCardByCodeAndType(cardCode, "CS", tenant, true);
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
			documentsFilingPM.DocumentsFilingMetaDataValues = DocumentAzureQueueWR.GetMapDocumentsFilingMetaDataValue(tenant, hawb, card?.Id);//> integrator + hawb
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

		private void GetcustomsDocumentTypeAndHawbFromFileName(string fileName,out string customsDocumentType,out string hawb,out DocumentType documentType)
		{
			customsDocumentType = string.Empty;
			hawb = string.Empty;
			documentType = null;

			try
			{
				if (string.IsNullOrWhiteSpace(fileName))
					return;

				string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
				string[] parts = nameWithoutExtension.Split('_');

				if (parts.Length == 2)
				{
					documentType = new DocumentTypeQuery(tenant).GetDocumentTypeByCode(parts[0], tenant);
					if (documentType != null)
					{
						customsDocumentType = parts[0];
						hawb = parts[1];
						return;
					}

					documentType = new DocumentTypeQuery(tenant).GetDocumentTypeByCode(parts[1], tenant);
					if (documentType != null)
					{
						customsDocumentType = parts[1];
						hawb = parts[0];
						return;
					}
				}
				else if (parts.Length == 3)
				{
					customsDocumentType = "OTH";
					hawb = parts[2];
				}
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex, DateTime.Now, 0, "",
					"WorkerRole", "GetcustomsDocumentTypeAndHawbFromFileName", null);
			}
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
		public override bool OnStart()
		{

			ThreadId = Guid.NewGuid().ToString();
			DoneItemsInRange = new Dictionary<DateTime, int>();

			return base.OnStart();
		}

		
		public void DebugStep()
		{		
			ExecuteQueue();
		}
	}
}
