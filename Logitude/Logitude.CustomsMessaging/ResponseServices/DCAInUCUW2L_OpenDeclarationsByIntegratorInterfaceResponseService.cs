
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
using Logitude.CustomsMessaging.U2L.CommDec;
using Unifreight.Data.AmitalModel.Repsitories;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCUW2LResponseContentHeader, DCAInUCUW2LRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(DCAInUCUW2LResponseContentHeader customResponse, DCAInUCUW2LRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCUW2LResponseContentHeader customResponse, DCAInUCUW2LRequestParams requestParams)
        {
            this.MyResponseData = new INF_MSG_GenericResponseData();


            bool lockit = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("Singleton.CRS:UCUW2L"));
          // string key = ProcessLockTableUtil.Instance.GetKey4DocumentsFilingId(customResponse.CustomFileNo, requestParams.Tenant);

           // using (var processLockTableDisposable = ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, lockit, key, "CRS:UCUW2L"))
           // {
                CommDecService CommDecService = new CommDecService();
                try {
                    string error = "";
                    string moreParams = customResponse.MoreParams;
                    CommDecService.ProccessGenericRequestReal(customResponse.LOGICOMMDEC, ref moreParams, out error);


                    this.MyResponseData.ApplicationID = requestParams.AppicationId;
                    this.MyResponseData.HasException = false;
                    this.MyResponseData.Succeeded = true;
                    }
                catch (Exception ex)
                {
                    this.MyResponseData.ApplicationID = requestParams.AppicationId;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.UserMessage = ex.Message;
                }
           // }
        }


 


    }
}
