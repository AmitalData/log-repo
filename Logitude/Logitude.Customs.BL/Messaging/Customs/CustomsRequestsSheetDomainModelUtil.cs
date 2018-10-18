using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Utils;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs
{
    public class CustomsRequestsSheetDomainModelUtil
    {

        public static void SetExceptionMessage(int tenant, string CustomsRequestsSheetId, string ExceptionMessage)
        {
            var qs = new CustomsRequestsSheetQueryService(tenant);
            var pm =qs.GetSingle(CustomsRequestsSheetId, false, false);
            var myCommunicationLogRepository = new CommunicationLogRepository(tenant) ;
            var myCommunicationLog = myCommunicationLogRepository.GetSingleCommunicationLog(pm.RequestComminicationId, tenant);
            myCommunicationLog.ExceptionMessage = ExceptionMessage;
            myCommunicationLogRepository.Update(myCommunicationLog);
            myCommunicationLogRepository.SubmitChanges();
 
        }

        public static void ReleaseConcurrentVirtualKey(RequestParamsBase requestParams,bool GetNewTransaction= true)
        {
            string CRSKey = GetCRSVirtualKey(requestParams);
            using (var scope =
                GetNewTransaction ?TransactionFactory.GetNewTransaction(): TransactionFactory.GetTransaction()
                )
            {

                var concurrentKiller1 = new ConcurrentKiller();
                concurrentKiller1.FreeLock(CRSKey, requestParams.Tenant);

                scope.Complete();
            }
        }

        public static void ReleaseConcurrentKey(RequestParamsBase requestParams)
        {
            string CRSKey = GetCRSKey(requestParams.CustomsRequestsSheetId);
            using (var scope = TransactionFactory.GetNewTransaction())
            {

                var concurrentKiller1 = new ConcurrentKiller();
                concurrentKiller1.FreeLock(CRSKey, requestParams.Tenant);

                scope.Complete();
            }
        }
        public static string GetCRSVirtualKey(RequestParamsBase requestParams)
        {
            //if (String.IsNullOrWhiteSpace(reqSheetDetails.CustomFileNo))
            {
                ///return $"CRS:{interfaceTypeCode}/{reqSheetDetails.CustomFileNo}";
            }
            return $"CRSV:{requestParams.InterfaceTypeCode}/({requestParams.LoggingObjectTableId}:{requestParams.LoggingEntityId})";
        }
        public static string GetCRSKey(string CustomsRequestsSheetId)
        {
            //if (String.IsNullOrWhiteSpace(reqSheetDetails.CustomFileNo))
            {
                ///return $"CRS:{interfaceTypeCode}/{reqSheetDetails.CustomFileNo}";
            }
            return $"CRS:{CustomsRequestsSheetId}";
        }

    }
}
