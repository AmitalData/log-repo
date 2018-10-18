using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Storage;
//using UnifreightIIG.Common.StorageResponseFromWarehouseServiceReference;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    public class ST_NG_70_MSG3_StorageResponseFromWarehouseAnalyzerService : MessageAnalyzerServiceBase<
        GenericRequestParams,INF_MSG_GenericResponseData,
        ST_NG_70_MSG3_StorageResponseFromWarehouse, ST_NG_70_MSG3_StorageResponseFromWarehouseResponseService>, IMessageAnalyzerService

    {

        public ST_NG_70_MSG3_StorageResponseFromWarehouseAnalyzerService()
            : base("UnifreightIIG.Common.StorageResponseFromWarehouseServiceReference.ST_NG_70_MSG3_StorageResponseFromWarehouse.xsd")
        {

        }
        public override INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            ST_NG_70_MSG3_StorageResponseFromWarehouse importDeclarationMessage = null;
            importDeclarationMessage = _CustomResponse;
            var responseData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();

            var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams();
            requestParams.Tenant = ResolveTenant();
            

            ST_NG_70_MSG3_StorageResponseFromWarehouseResponseService customResponseService = new ST_NG_70_MSG3_StorageResponseFromWarehouseResponseService();
            customResponseService.Update(importDeclarationMessage, requestParams);
            if (customResponseService.MyResponseData == null)
            {
                //??
                return null;
            }
            responseData.ExceptionMessage = customResponseService.MyResponseData.ExceptionMessage;
            responseData.ApplicationID = customResponseService.MyResponseData.ApplicationID;
            responseData.HasException = customResponseService.MyResponseData.HasException;
            responseData.Succeeded = customResponseService.MyResponseData.Succeeded;
            return responseData;
        }

        public override int ResolveTenant()
        {
            var tenant = 1;
            return tenant;
        }

        public override string GetObjectTableName()
        {
            return "Customs.Declaration";
        }

        public override string GetLoggingEntityReference()
        {
            return _CustomResponse.StorageResponseFromWarehouse.declerationNumber;
        }
    }
}
