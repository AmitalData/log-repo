
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Utils;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.BL.Messaging.U2L.DeclarationDocuments;
using Logitude.AmitalMessaging.Utils;
using Logitude.Server.Tools.Utils;
using System.Configuration;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCustomBatchSend2715Bonded_MsgResponseService : 
        ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCSBondedWithResponseContentHeader, GenericRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(DCAInUCSBondedWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCSBondedWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            this.MyResponseData = new INF_MSG_GenericResponseData();
            bool lockit = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("Singleton.CRS:2715/UDLT"));
            string key = ProcessLockTableUtil.Instance.GetKey4DocumentsFilingId(customResponse.DocumentsFilingId, requestParams.Tenant);

            using (var processLockTableDisposable = ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, lockit, key, "CRS:2715/UDLT"))
            {
                RealUpdate(customResponse, requestParams);
            }
        }


        private void RealUpdate(
            DCAInUCSBondedWithResponseContentHeader customResponse, 
            GenericRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var myCustomsDocumentQueryService = new CustomsDocumentQueryService(dbContext);
            var customsDocumentPM = myCustomsDocumentQueryService.GetSingle(customResponse.DocumentsFilingId, true, false);
            if(customsDocumentPM!= null)
            customsDocumentPM.DeclarationId = customResponse?.DeclarationId;
			(new Send2715Bonded()).Send(
                   requestParams.Tenant,
                   customResponse.DocumentsFilingId,
                   customsDocumentPM,
                   customResponse.DocumentTypeCode, customResponse.IsSendFromAutoClosing);


            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            //this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;// (unifreightGenericService.MyGenericResponseObj.StatusType == AmitalMessaging.Infrastructure.GenericResponseObj.StatusEnum.Success);
        }

    }
}
