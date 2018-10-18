using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.RTGSInfoServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class TSH_WEB8289_9060_RTGSInfoQueryRequestService
        : RequestServiceBase<TSH_NG_8289_Web05_CreditQuery, CreditQueryRequestParams>
    {
        public override TSH_NG_8289_Web05_CreditQuery GetRequest(CreditQueryRequestParams requestParams)
        {
            var myTSH_NG_8289_Web05_CreditQuery = new TSH_NG_8289_Web05_CreditQuery();
            myTSH_NG_8289_Web05_CreditQuery.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myTSH_NG_8289_Web05_CreditQuery.TSHIRequestorParams = new TSH_NG_8289_Web05_CreditQueryTSHIRequestorParams();

            myTSH_NG_8289_Web05_CreditQuery.TSHIRequestorParams.DateFrom = requestParams.DateFrom;
            myTSH_NG_8289_Web05_CreditQuery.TSHIRequestorParams.DateTo = requestParams.DateTo;
            if (!string.IsNullOrWhiteSpace(requestParams.AgentExternalId))
            {
                long AgentExternalIdLong;
                if (long.TryParse(requestParams.AgentExternalId, out AgentExternalIdLong))
                {
                    myTSH_NG_8289_Web05_CreditQuery.TSHIRequestorParams.AgentExternalID = AgentExternalIdLong;
                    myTSH_NG_8289_Web05_CreditQuery.TSHIRequestorParams.AgentExternalIDSpecified = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(requestParams.AgentID))
            {
                myTSH_NG_8289_Web05_CreditQuery.TSHIRequestorParams.AgentID = requestParams.AgentID;
            }

            if (!string.IsNullOrWhiteSpace(requestParams.ExtertnalID))
            {
                myTSH_NG_8289_Web05_CreditQuery.TSHIRequestorParams.ExtertnalID = requestParams.ExtertnalID;
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "שאילתא לתקרת זהב";

            return myTSH_NG_8289_Web05_CreditQuery;

        }
    }
}