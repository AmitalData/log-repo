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

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class PC_NG_2280_MSG01_CertificateOfOriginRequestResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback, CertificateOfOriginRequestRequestParams>
    {
		private DateTime _TransmitionDateTime;

        public override INF_MSG_GenericResponseData GetResponse(PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback customResponse, CertificateOfOriginRequestRequestParams requestParams) =>
            this.MyResponseData;        

        public override void Update(PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback customResponse, CertificateOfOriginRequestRequestParams requestParams)
        {
			_TransmitionDateTime = customResponse.ResponseContentHeader.TransmitionDateTime;
			this.MyResponseData = new INF_MSG_GenericResponseData();

            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
			CertificateOfOriginUpdateService certificateOfOriginUpdateService = new CertificateOfOriginUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);

			CertificateOfOriginQueryService certificateOfOriginQueryService = new CertificateOfOriginQueryService(requestParams.Tenant);
			var certificateOfOriginPM = certificateOfOriginQueryService.GetSingle(requestParams.CertificateOfOriginId, true, false);

			if (certificateOfOriginPM == null)
			{
				LogMessagingUtil.Instance.AppendLine("Can not find certificateOfOrigin" + requestParams.CertificateOfOriginId);
				this.MyResponseData.ApplicationID = requestParams.CertificateOfOriginId;
				this.MyResponseData.Succeeded = true;
				this.MyResponseData.UserMessage = "Can not find certificateOfOrigin" + requestParams.CertificateOfOriginId;
				return;
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

			if (customResponse.ResponseContentHeader.Exception != null)
			{
				certificateOfOriginPM.ErrXml = XmlGenericUtil<UnifreightIIG.Common.CertificateOfOriginRequestServiceReference.Exception[]>.SerializeObject(customResponse.ResponseContentHeader.Exception);
				certificateOfOriginPM.ChangeSetOp = ChangeSetOperation.Update;
				certificateOfOriginUpdateService.Update(certificateOfOriginPM, true);

				this.MyResponseData.ApplicationID = requestParams.CertificateOfOriginId;
				this.MyResponseData.Succeeded = true;
				this.MyResponseData.UserMessage = "התקבלו שגיאות במסר תעודת מקור";
				this.MyResponseData.HasException = true;

					return;
			}
			certificateOfOriginPM.ErrXml = null;
			certificateOfOriginPM.COONumber = customResponse.CertificateOfOriginRequestFeedback.certificateID;
			certificateOfOriginPM.CooStatusCode = customResponse.CertificateOfOriginRequestFeedback.certificateOfOriginStatusCode.ToString();
			certificateOfOriginPM.FeedbackRemark = customResponse.CertificateOfOriginRequestFeedback.FeedbackRemark;
			certificateOfOriginPM.RejectCancelReason = customResponse.CertificateOfOriginRequestFeedback.rejectCancelReason;
			certificateOfOriginPM.QueryUrl = customResponse.CertificateOfOriginRequestFeedback.QueryURL;
			certificateOfOriginPM.IssueDateIfReleased = customResponse.CertificateOfOriginRequestFeedback.IssueDateIfReleased;

			if(requestParams.RequestReasonCode == 1)
			  certificateOfOriginPM.IsSubmitted = true;
			if (customResponse.Attachment != null)
			{
				DeclarationQueryService declarationQueryService = new DeclarationQueryService(certificateOfOriginPM.Tenant);
				var declarationPM = declarationQueryService.GetSingle(requestParams.DeclarationId, false, false);
				foreach (var attachment in customResponse.Attachment)
				{
					AnalyzeCertificateOfOriginDocument(attachment, requestParams, declarationPM, certificateOfOriginPM);
				}

				RaiseEvent(certificateOfOriginPM, declarationPM, requestParams.LoggingUserId, "COO", _TransmitionDateTime);
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
			var documentsFilingPMList = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(declarationPM.Id, null, objectTableId, "I", requestParams.Tenant);
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


			//DocumentsMetaDataTypePM verPm= 
			if (documentsFilingPM == null)
			{
				documentsFilingPM = CreateCertificateOfOriginDocument(attachment, declarationPM, requestParams, certificateOfOriginPM);

			}
			else
			{

				UpdateCertificateOfOriginDocument(documentsFilingPM, attachment, declarationPM, requestParams, certificateOfOriginPM);
			}


		}
		private DocumentsFilingPM CreateCertificateOfOriginDocument(Attachment attachment, DeclarationPM declarationPM, CertificateOfOriginRequestRequestParams requestParams, CertificateOfOriginPM certificateOfOriginPM)
		{
			ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
			var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "2280", IsCourier = false });
			var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);

			var documentsFilingPM = new DocumentsFilingPM();
			documentsFilingPM.Tenant = requestParams.Tenant;
			var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("COOE", requestParams.Tenant);
			documentsFilingPM.DocumentTypeId = documentType.Id;
			documentsFilingPM.Name = "תעודת מקור: " + certificateOfOriginPM.COONumber;
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
			documentsFilingPM.Description = "תעודת מקור: " + certificateOfOriginPM.COONumber;
			documentsFilingPM.ExternalEntityName = declarationPM.Direction == "E" ? "BFIFILE" : "CFIFILEM";
			documentsFilingPM.ExternalEntityReference = declarationPM.CustomFileNo;
			documentsFilingPM.FileExtension = "PDF";

			documentsFilingService.Create(documentsFilingPM, attachment.content, requestParams.LoggingUserId);
			LogMessagingUtil.Instance.AppendLine("Filed document " + documentsFilingPM.Code + "Created For certificateOfOrigin " + certificateOfOriginPM.COONumber + " documentsFilingPM.ID= " + documentsFilingPM.Id);
			return documentsFilingPM;

		}
		private void UpdateCertificateOfOriginDocument(DocumentsFilingPM documentsFilingPM, Attachment attachment, DeclarationPM declarationPM, CertificateOfOriginRequestRequestParams requestParams, CertificateOfOriginPM certificateOfOriginPM)
		{
			ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
			var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "2280", IsCourier = false });

			documentsFilingPM.Description = "תעודת מקור: " + certificateOfOriginPM.COONumber;
			documentsFilingPM.Name = "תעודת מקור: " + certificateOfOriginPM.COONumber;
			documentsFilingPM.UpdatedByUserId = requestParams.LoggingUserId;

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

    }
}
