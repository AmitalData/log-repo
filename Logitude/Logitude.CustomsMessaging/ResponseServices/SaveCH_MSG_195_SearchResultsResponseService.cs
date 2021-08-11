using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SearchResultsServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class SaveCH_MSG_195_SearchResultsResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, INF_MSG_Generic, GenericRequestParams>
    {


        public override void OnRequestFail(INF_MSG_Generic customResponse, GenericRequestParams requestParams)
        {
            base.OnRequestFail(customResponse, requestParams);
        }

        public override INF_MSG_GenericResponseData GetResponse(INF_MSG_Generic customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(INF_MSG_Generic customResponse, GenericRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
         
            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.ApplicationID = requestParams.AppicationId;
             if (customResponse.ResponseContentHeader.Exception != null)
            {
                 this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;

                return;

            }

            PhysicalCheckQueryService physicalCheckQueryService = new PhysicalCheckQueryService(requestParams.Tenant);
            PhysicalCheckUpdateService physicalCheckUpdateService = new PhysicalCheckUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var physicalCheck = physicalCheckQueryService.GetSingle(requestParams.LoggingEntityId, false, false);

            physicalCheck.CheckAnwserStatus = 1;
            physicalCheck.ChangeSetOp = ChangeSetOperation.Update;

            physicalCheckUpdateService.Update(physicalCheck, true);

            this.MyResponseData.UserMessage = "תוצאות בדיקה התקבלו במכס";
         
        }
    }
}
