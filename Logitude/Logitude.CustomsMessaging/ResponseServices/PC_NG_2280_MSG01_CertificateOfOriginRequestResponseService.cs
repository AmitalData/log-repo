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
			this.MyRequestSheetParam.RequestDescription = "משוב לתעודת מקור : " + certificateOfOriginPM.COONumber;

			if (customResponse.ResponseContentHeader.Exception != null)
			{
					string userMessage = "";

				foreach (UnifreightIIG.Common.CertificateOfOriginRequestServiceReference.Exception exception in customResponse.ResponseContentHeader.Exception)
				{
					if (!string.IsNullOrWhiteSpace(userMessage))
					{
						userMessage = userMessage + @"";
					}
					userMessage = userMessage + exception.ExeptionDescription;
					var ErrorXml = XmlGenericUtil<UnifreightIIG.Common.CertificateOfOriginRequestServiceReference.Exception>.SerializeObject(exception);

					if (string.IsNullOrEmpty(certificateOfOriginPM.ErrXml))
					{
						certificateOfOriginPM.ErrXml = ErrorXml;
					}
					else
					{
						certificateOfOriginPM.ErrXml = string.Concat(certificateOfOriginPM.ErrXml, ErrorXml);
					}
				}
				certificateOfOriginPM.ChangeSetOp = ChangeSetOperation.Update;
				certificateOfOriginUpdateService.Update(certificateOfOriginPM, true);

				this.MyResponseData.ApplicationID = requestParams.CertificateOfOriginId;
				this.MyResponseData.Succeeded = true;
				this.MyResponseData.UserMessage = userMessage;
				this.MyResponseData.HasException = true;

					return;
			}
				
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

			}

			certificateOfOriginPM.ChangeSetOp = ChangeSetOperation.Update;
			certificateOfOriginUpdateService.Update(certificateOfOriginPM,true);

			this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.ApplicationID = requestParams.CertificateOfOriginId; 
            this.MyResponseData.UserMessage =  "המסר התקבל בהצלחה במכס";

		}
    }
}
