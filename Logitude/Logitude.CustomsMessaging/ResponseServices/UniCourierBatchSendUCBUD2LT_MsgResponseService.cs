
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
    public class UniCourierBatchSendUCBUD2LT_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBUD2LTWithResponseContentHeader, GenericRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBUD2LTWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBUD2LTWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            bool lockit = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("Singleton.CRS:2715/UDLT"));
            string key = ProcessLockTableUtil.Instance.GetKey4DocumentsFilingId(customResponse.DocumentsFilingId, requestParams.Tenant);

            using (var processLockTableDisposable = ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, lockit, key, "CRS:2715/UDLT"))
            {
                RealUpdate(customResponse, requestParams);
            }
        }


        private void RealUpdate(DCAInUCBUD2LTWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();
            var xml = XmlGenericUtil<LOGIDOCS>.SerializeObject(
                new LOGIDOCS()
                {
                    LogitudeDocs = new LogitudeDocs[] {
                     new LogitudeDocs(){
                         COM_ID =customResponse.DocumentsFilingId,
                         DOC_ID =customResponse.DocumentTypeCode,
                         Id =customResponse.DeclarationId , Tenant= requestParams.Tenant.ToString()
                     }
                 }
                }
            );


            var unifreightGenericService = new DeclarationDocumentsService();
            string moreParams =
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<ArrayOfEntry xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
 <Entry>
  <Key>MODE</Key>
  <Value></Value>
 </Entry>
 <Entry>
  <Key>TENANT</Key>
  <Value>@TENANT@</Value>
 </Entry>
 <Entry>
  <Key>UNIFREIGHT_USER_ID</Key>
  <Value>@UNIFREIGHT_USER_ID@</Value>
 </Entry>
</ArrayOfEntry>";
            moreParams = moreParams.Replace("@TENANT@", requestParams.Tenant.ToString());
            moreParams = moreParams.Replace("@UNIFREIGHT_USER_ID@", "AMITAL");
            string messageOut = "";
            unifreightGenericService.ProccessGenericRequest(xml, ref moreParams, out messageOut);


            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = (unifreightGenericService.MyGenericResponseObj.StatusType == AmitalMessaging.Infrastructure.GenericResponseObj.StatusEnum.Success);
        }

    }
}
