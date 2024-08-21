using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Common.RequestParams;
using UnifreightIIG.Common.LogisticActionRequestMessageServiceReference;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using UnifreightIIG.Common.CertificateOfOriginRequestServiceReference;
using Logitude.Customs.BL.BL;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using System.Diagnostics;
using System.Linq;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.BL.TraceEvents;
using System;

using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.CustomsMessaging.Helpers;
using Simplog.Data.CommonDataModel;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Attachment = UnifreightIIG.Common.CertificateOfOriginRequestServiceReference.Attachment;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Customs.Data.EntityMapping;
using System.Data.Entity;
using Logitude.Customs.Data.Repsitories;
using SupplierInvoiceQueryService = Logitude.Customs.BL.EntityQueryServices.SupplierInvoiceQueryService;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using UnifreightIIG.Common.DeclarationPrintServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class PC_NG_2280_MSG01_CertificateOfOriginRequestResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback, CertificateOfOriginRequestRequestParams>
    {
		private DateTime _TransmitionDateTime = DateTime.Now;
		private ICustomContext dbContext;
		public override Action<PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback> GetActionShrinkCustomResponse()
		{
			return new Action<PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback>(this.ShrinkCustomResponse);
		}
		void ShrinkCustomResponse(PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback customResponse)
		{
			if (customResponse == null) return;
			if (customResponse.Attachment == null) return;

			foreach (var item in customResponse.Attachment)
			{			
			   var MD5Hash = MD5HashUtil.GetMD5Hash(item.content);
			   item.content = System.Text.UTF8Encoding.UTF8.GetBytes(MD5Hash);		
			}
		}
		public override INF_MSG_GenericResponseData GetResponse(PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback customResponse, CertificateOfOriginRequestRequestParams requestParams) =>
            this.MyResponseData;        

        public override void Update(PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback customResponse, CertificateOfOriginRequestRequestParams requestParams)
        {

			this.MyResponseData = new INF_MSG_GenericResponseData();

            dbContext = CustomContext.GetContext(requestParams.Tenant);
			CertificateOfOriginUpdateService certificateOfOriginUpdateService = new CertificateOfOriginUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);

			CertificateOfOriginQueryService certificateOfOriginQueryService = new CertificateOfOriginQueryService(requestParams.Tenant);
			var certificateOfOriginPM = certificateOfOriginQueryService.GetSingle(requestParams.CertificateOfOriginId, true, false);

			if (certificateOfOriginPM == null)
			{
				  certificateOfOriginPM = certificateOfOriginQueryService.GetCertificateOfOriginByCounter(customResponse?.CertificateOfOriginRequestFeedback?.internalApplication, requestParams.Tenant);

				if (certificateOfOriginPM == null)
				{
					LogMessagingUtil.Instance.AppendLine("Can not find certificateOfOrigin" + requestParams.CertificateOfOriginId);
					this.MyResponseData.ApplicationID = requestParams.CertificateOfOriginId;
					this.MyResponseData.Succeeded = true;
					this.MyResponseData.UserMessage = "Can not find certificateOfOrigin" + requestParams.CertificateOfOriginId;
					return;
				}
			}

			if (this.MyRequestSheetParam == null)
			{
				this.MyRequestSheetParam = new RequestSheetParam();
			}

			this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
			this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationId;
			this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.CertificateOfOrigin");
			this.MyRequestSheetParam.EntityId2 = requestParams.CertificateOfOriginId;
			this.MyRequestSheetParam.CustomFileNo = requestParams.CustomFileNo;
			this.MyRequestSheetParam.RequestDescription = (requestParams.RequestReasonCode == 13 ? "משוב לסטטוס תעודת מקור: " : "משוב תעודת מקור: ") + certificateOfOriginPM.Counter;

			if (customResponse.ResponseContentHeader?.Exception != null)
			{
				certificateOfOriginPM.ErrXml = XmlGenericUtil<UnifreightIIG.Common.CertificateOfOriginRequestServiceReference.Exception[]>.SerializeObject(customResponse.ResponseContentHeader.Exception);
				certificateOfOriginPM.ChangeSetOp = ChangeSetOperation.Update;
				certificateOfOriginUpdateService.Update(certificateOfOriginPM, true);

				this.MyResponseData.ApplicationID = requestParams.CertificateOfOriginId;
				this.MyResponseData.Succeeded = true;
				this.MyResponseData.UserMessage = "התקבלו שגיאות במסר תעודת מקור";
				this.MyResponseData.HasException = true;
				// return;
			}
			else certificateOfOriginPM.ErrXml = null;
			if (customResponse.CertificateOfOriginRequestFeedback.certificateID != null)
				certificateOfOriginPM.COONumber = customResponse.CertificateOfOriginRequestFeedback.certificateID;
			
			certificateOfOriginPM.CooStatusCode = customResponse.CertificateOfOriginRequestFeedback.certificateOfOriginStatusCode.ToString();
			certificateOfOriginPM.FeedbackRemark = customResponse.CertificateOfOriginRequestFeedback.FeedbackRemark;
			certificateOfOriginPM.RejectCancelReason = customResponse.CertificateOfOriginRequestFeedback.rejectCancelReason;
			certificateOfOriginPM.QueryUrl = customResponse.CertificateOfOriginRequestFeedback.QueryURL;
			certificateOfOriginPM.IssueDateIfReleased = customResponse.CertificateOfOriginRequestFeedback.IssueDateIfReleased;

			if(requestParams.RequestReasonCode == 10 && certificateOfOriginPM.COONumber != null)
				certificateOfOriginPM.RequestReasonCode = "12";


            if (requestParams.RequestReasonCode == 1)
			  certificateOfOriginPM.IsSubmitted = true;

			DeclarationQueryService declarationQueryService = new DeclarationQueryService(certificateOfOriginPM.Tenant);
			var declarationPM = declarationQueryService.GetSingle(certificateOfOriginPM.DeclarationId, false, false);

			if (customResponse.Attachment != null)
			{

				var declarationPmOrg = declarationPM;
				if (!string.IsNullOrEmpty(declarationPM.AmendmentOriginalDeclartation))
				{
					declarationPmOrg = declarationQueryService.GetSingle(declarationPM.AmendmentOriginalDeclartation, false, false);

				}
				foreach (var attachment in customResponse.Attachment)
				{
					AnalyzeCertificateOfOriginDocument(attachment, requestParams, declarationPmOrg, certificateOfOriginPM);
				}

				RaiseEvent(certificateOfOriginPM, declarationPM, requestParams.LoggingUserId, "COO", _TransmitionDateTime);
			}

			var certificateOfOrigins = certificateOfOriginQueryService.GetCertificateOfOriginsByDeclarationId(declarationPM.Id, declarationPM.AmendmentOriginalDeclartation, requestParams.Tenant);
			if (certificateOfOrigins != null && certificateOfOrigins.Count() == 1) 
			{ 

			   LogicCooNumberInDeclaration(requestParams.DeclarationId, requestParams.Tenant, certificateOfOriginPM);
			}
			certificateOfOriginPM.ChangeSetOp = ChangeSetOperation.Update;
			certificateOfOriginUpdateService.Update(certificateOfOriginPM,true);

            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.ApplicationID = requestParams.CertificateOfOriginId; 
            this.MyResponseData.UserMessage =  "המסר התקבל בהצלחה במכס";

		}
		private void AnalyzeCertificateOfOriginDocument(Attachment attachment, CertificateOfOriginRequestRequestParams requestParams, DeclarationPM declarationPM, CertificateOfOriginPM certificateOfOriginPM)
		{
			ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
			var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant,
				new CustomDocumentsFilingParams() { MainInterfaceCode = "2280", IsCourier = false });
			var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);
			var documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
			DocumentsFilingPM documentsFilingPM = null;

			if (attachment == null | declarationPM == null)
			{
				return;
			}

			GDMFILINGQueryService uniGDMFILINGQueryService = null;
			if (CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant).IsConnectedToUniFreight)
			{
				uniGDMFILINGQueryService = new GDMFILINGQueryService(AmitalContext.GetContext(requestParams.Tenant));
			}
			//Check if file already exists
			var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
			var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("COOE", requestParams.Tenant);
            if (documentType == null)
            {
                this.MyResponseData.ApplicationID = requestParams.CertificateOfOriginId;
                this.MyResponseData.Succeeded = true;
				this.MyResponseData.UserMessage = "Can not find COOE documentType";
                return;
            }
            var documentsFilingPMList = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(declarationPM.Id, certificateOfOriginPM.Id, objectTableId, "I", requestParams.Tenant);
			foreach (var documentItem in documentsFilingPMList)
			{
				if (documentItem.DocumentTypeId == documentType?.Id)
				{
					if (uniGDMFILINGQueryService != null)
					{
						var gdmfiling = uniGDMFILINGQueryService.GetSingle(documentItem.Id, true);
						if (gdmfiling?.DELETED == "T")
						{
							// uniface deleted!!
							continue;
						}
					}
					documentsFilingPM = documentItem;
					break;
				}
			}

			if (documentsFilingPM == null)
			{
				CreateCertificateOfOriginDocument(attachment, declarationPM, requestParams, certificateOfOriginPM);

			}
			else
			{

				UpdateCertificateOfOriginDocument(documentsFilingPM, attachment, declarationPM, requestParams, certificateOfOriginPM);
			}


		}
		private DocumentsFilingPM CreateCertificateOfOriginDocument(Attachment attachment, DeclarationPM declarationPM, CertificateOfOriginRequestRequestParams requestParams, CertificateOfOriginPM certificateOfOriginPM)
		{
			ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
			var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "2280", IsCourier = false },"1");
			var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);

			var documentsFilingPM = new DocumentsFilingPM();
			documentsFilingPM.Tenant = requestParams.Tenant;
			var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("COOE", requestParams.Tenant);
			documentsFilingPM.DocumentTypeId = documentType.Id;
			documentsFilingPM.Name = certificateOfOriginPM.COONumber + "-1" + " :תעודת מקור";
			documentsFilingPM.EntityId = declarationPM.Id;
			documentsFilingPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
			documentsFilingPM.ChildEntityId = certificateOfOriginPM.Id;
			documentsFilingPM.ChildObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CertificateOfOrigin");
			documentsFilingPM.CreatedByUserId = requestParams.LoggingUserId;
			documentsFilingPM.OwnerId = requestParams.LoggingUserId;
			documentsFilingPM.UpdatedByUserId = requestParams.LoggingUserId;
			documentsFilingPM.ReceivedByUserId = requestParams.LoggingUserId;
			documentsFilingPM.DirectionCode = "I";
			// I/O  - only  !!  -   documentsFilingPM.DirectionCode = this._MyDeclarationPM.Direction;
			documentsFilingPM.Description = certificateOfOriginPM.COONumber + "-1" + " :תעודת מקור";
			documentsFilingPM.ExternalEntityName = declarationPM.Direction == "E" ? "BFIFILE" : "CFIFILEM";
			documentsFilingPM.ExternalEntityReference = declarationPM.CustomFileNo;
			documentsFilingPM.FileExtension = "PDF";
			documentsFilingPM.LastVersion = 1;
			documentsFilingService.Create(documentsFilingPM, attachment.content, requestParams.LoggingUserId);
			LogMessagingUtil.Instance.AppendLine("Filed document " + documentsFilingPM.Code + "Created For certificateOfOrigin " + certificateOfOriginPM.COONumber + " documentsFilingPM.ID= " + documentsFilingPM.Id);
			return documentsFilingPM;

		}
		private void UpdateCertificateOfOriginDocument(DocumentsFilingPM documentsFilingPM, Attachment attachment, DeclarationPM declarationPM, CertificateOfOriginRequestRequestParams requestParams, CertificateOfOriginPM certificateOfOriginPM)
		{
			var newVersion = ++documentsFilingPM.LastVersion;

			ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
			var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "2280", IsCourier = false }, newVersion.ToString());

			documentsFilingPM.Description = certificateOfOriginPM.COONumber + "-" + newVersion + " :תעודת מקור";
			documentsFilingPM.Name = certificateOfOriginPM.COONumber + "-" + newVersion + " :תעודת מקור";
			documentsFilingPM.UpdatedByUserId = requestParams.LoggingUserId;
			documentsFilingPM.LastVersion = newVersion;

			documentsFilingService.Update(documentsFilingPM, attachment.content, requestParams.LoggingUserId);
			LogMessagingUtil.Instance.AppendLine("File document " + documentsFilingPM.Code + " Updated For certificateOfOrigin " + certificateOfOriginPM.COONumber + " documentsFilingPM.ID= " + documentsFilingPM.Id);

		}

		private static void RaiseEvent(CertificateOfOriginPM certificateOfOriginPM, DeclarationPM declarationPM, string loggingUserId, string status_id, DateTime? status_DateTime)
		{

			string primary_number = $"{declarationPM.CustomFileNo},EFIFILEM";
			if (declarationPM.TransportModeId != "A")
			{
				primary_number = $"{declarationPM.CustomFileNo},MFIFILEM";
			}

			var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
			{
				Tenant = certificateOfOriginPM.Tenant,
				objectTableName = "Customs.Declaration",
				EventCode = "COO",
				notes = certificateOfOriginPM.COONumber + " " + certificateOfOriginPM.CooTypeCodeName,
				CommunicationLoggingEntityReference = declarationPM.DeclarationNumber,
				EntityId = declarationPM.Id,
				UserId = loggingUserId,

				CommunicationSubject = "FU Status " + status_id + " from logitude",
				MyFUStatus = new AmitalEventTracerModel.FUStatus()
				{
					entname = "BFIFILE",
					primary_number = primary_number,
					status = "new",
					xml_status = "new",
					status_id = "COO",
					status_DateTime = status_DateTime ?? DateTime.Now,
					comments = certificateOfOriginPM.COONumber + " " + certificateOfOriginPM.CooTypeCodeName,
				}
			};

			AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, suppress_RAISE_EVENT: true);


		}



		private void LogicCooNumberInDeclaration(string declarationId, int tenant, CertificateOfOriginPM certificateOfOriginPM)
		{			
			DeclarationQueryService declarationQueryService = new DeclarationQueryService(tenant);
			var declarationPM = declarationQueryService.GetSingle(declarationId, false, false);
			if (declarationPM != null)
			{
				bool IsUpdated = UpdateCooNumberInDeclaration(declarationId, certificateOfOriginPM, Convert.ToBoolean(declarationPM.IsSubmitDeclaration));
		
				if (IsUpdated)
				{
					certificateOfOriginPM.UpdateDeclaration = "A";

					if (declarationPM.IsSubmitDeclaration != true) {
						SendDeclaration(declarationPM, certificateOfOriginPM.Id);
					}
				}
			}
		}
		public bool UpdateCooNumberInDeclaration(string declarationId, CertificateOfOriginPM certificateOfOriginPM,bool IsSubmitDeclaration = false)
		{
			int tenant = certificateOfOriginPM.Tenant;
			SupplierInvoiceQueryService supplierInvoiceQueryService = new SupplierInvoiceQueryService(tenant);
			dbContext = CustomContext.GetContext(tenant);

			SupplierInvoiceItemUpdateService supplierInvoiceItemUpdateService = new SupplierInvoiceItemUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);

			var supplierInvoices = supplierInvoiceQueryService.GetSupplierInvoicesForDeclaration(declarationId, tenant, true);

			var IsUpdated = false;
			var InvoiceItems = certificateOfOriginPM.CertificateOriginInvoiceItems.FindAll(x => x.IsInvoicesForPrint == true);
			foreach (var supplierInvoice in supplierInvoices)
			{
				if (InvoiceItems.Any(x => x.InvoicesIdUry == supplierInvoice.SequenceNumeric))
				{
					foreach (var supplierInvoiceItem in supplierInvoice.SupplierInvoiceItems)
					{
						if (supplierInvoiceItem.PreferenceDocumentNumber != certificateOfOriginPM.COONumber && (string.IsNullOrEmpty(supplierInvoiceItem.OriginCountryCode) || supplierInvoiceItem.OriginCountryCode == "IL"))
						{
							if (!IsSubmitDeclaration) 
							{
							   supplierInvoiceItem.PreferenceDocumentNumber = certificateOfOriginPM.COONumber;
							   supplierInvoiceItem.ChangeSetOp = ChangeSetOperation.Update;
							   supplierInvoiceItemUpdateService.Update(supplierInvoiceItem, true);
							}
							IsUpdated = true;
						}
					}
				}
			}
			return IsUpdated;
		}
		public INF_MSG_GenericResponseData SendDeclaration(DeclarationPM decPm, string certificateOfOriginId)
		{
			var requestParamsData = new GenericRequestParams();

			var objecttableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
			var loggedUserId = AuthenticationUtil.ResolveUserId(decPm.Tenant);

			requestParamsData.Tenant = decPm.Tenant;
			requestParamsData.AppicationId = decPm.Id;
			requestParamsData.LoggingEnabled = true;
			requestParamsData.LoggingEntityId = decPm.Id;
			requestParamsData.LoggingEntityReference = decPm.DeclarationNumber;
			requestParamsData.LoggingObjectTableId = objecttableId;
			requestParamsData.LoggingUserId = loggedUserId;
			requestParamsData.RequestName = "Declaration Request";
			requestParamsData.ResponseName = "Declaration Response";
			requestParamsData.RequestVIA = SendRequestVIA.WebServiceBatch;
			requestParamsData.ForcePersonalSign = false;
			requestParamsData.LoggingEntityReference = certificateOfOriginId;


			INF_MSG_GenericResponseData responseData = decPm.DeclarationTypeCode == "3" ?
					  new SaveDF_MSG2751_2757_TransshipmentDeclarationRequestMessagingService().Send(requestParamsData) :
					  new DF_NG_2751_MSG10000_ExportDeclarationMessagingService().Send(requestParamsData);

			return responseData;
		}
		public INF_MSG_GenericResponseData SendAmendmentDeclaration(DeclarationPM decPm, string LoggingUserId = "")
		{
			var requestParamsData = new GenericRequestParams();

			var objecttableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
			var loggedUserId = !string.IsNullOrEmpty(LoggingUserId) ? LoggingUserId : AuthenticationUtil.ResolveUserId(decPm.Tenant);

			requestParamsData.Tenant = decPm.Tenant;
			requestParamsData.AppicationId = decPm.Id;
			requestParamsData.LoggingEnabled = true;
			requestParamsData.LoggingEntityId = decPm.Id;
			requestParamsData.LoggingEntityReference = decPm.DeclarationNumber;
			requestParamsData.LoggingObjectTableId = objecttableId;
			requestParamsData.LoggingUserId = loggedUserId;
			requestParamsData.RequestName = (decPm.DeclarationTypeCode == "3" ? "Transshipment" : "Export") + " Amendment Declaration Request";
			requestParamsData.ResponseName = "Amendment Declaration Response";
			requestParamsData.RequestVIA = SendRequestVIA.WebServiceInteractive;
			requestParamsData.ForcePersonalSign = false;



			var serializedParent = JsonConvert.SerializeObject(requestParamsData);
			AmendmentRequestParams requestParams = JsonConvert.DeserializeObject<AmendmentRequestParams>(serializedParent);

			INF_MSG_GenericResponseData responseData = decPm.DeclarationTypeCode == "3" ?
					  new DF_MSG8235_TransshipmentDeclarationAmendmentMessagingService().Send(requestParams) :
					  new DF_MSG8235_ExportDeclarationAmendmentMessagingService().Send(requestParams);
					
			return responseData;
		}
	}
}
