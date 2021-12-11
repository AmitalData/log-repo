using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.Repsitories;
using Microsoft.Practices.Unity;
using Logitude.CustomsMessaging.Utils;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UpdateAllCertificateBatchUCB_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBUpdateAllCertificateResponseContentHeader, GenericRequestParams>
    {

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBUpdateAllCertificateResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBUpdateAllCertificateResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var customContext = CustomContext.GetContext(requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();
            long lCUSTOMFILENO;
            if (!long.TryParse(customResponse.CustomFileNo, out lCUSTOMFILENO))
            {
                throw new BusinessErrorException("DeclarationCustomFileNo could not convert to long ");
            }
            var myCCUFILEMRepository = new CCUFILEMRepository(requestParams.Tenant);
            var ccufilem = myCCUFILEMRepository.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);
            var myCCUQUELOCKRepository = new CCUQUELOCKRepository(requestParams.Tenant);
            var isUpdate = true;
            try
            {
                var cculock = myCCUQUELOCKRepository.GetSingleGeneralLockNOWAIT("CCUFILEM", ccufilem.ToString());
            }
            catch (System.Exception)
            {
                LogMessagingUtil.Instance.AppendLine($"GetSingleGeneralLockNOWAIT(CCUFILEM, {customResponse.CustomFileNo}) ==> Already Lock => try later (*5) ");
                isUpdate = false;
            }
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyRequestSheetParam.CustomFileNo = customResponse.CustomFileNo;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.ApplicationID = customResponse.Declarationid;
            if (isUpdate)
            {
                SupplierInvioceItemCertificatUpdateService updateService = new SupplierInvioceItemCertificatUpdateService(customContext);
                var count = updateService.UpdateAllCertificateWithoutResponse(customResponse.Declarationid, requestParams.Tenant);
                this.MyResponseData.UserMessage = "עודכנו " + count + " אישורים";
            }

        }

    }

}
