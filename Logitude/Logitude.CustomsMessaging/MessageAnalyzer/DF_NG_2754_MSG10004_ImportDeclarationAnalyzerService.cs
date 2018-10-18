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
using UnifreightIIG.Common.ImportDeclarationServiceReference;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    public class DF_NG_2754_MSG10004_ImportDeclarationAnalyzerService : MessageAnalyzerServiceBase<
        GenericRequestParams,INF_MSG_GenericResponseData,
        DF_NG_2754_MSG10004_ImportDeclarationResponse, DF_NG_2754_MSG10004_ImportDeclarationResponseService>, IMessageAnalyzerService

    {
        public DF_NG_2754_MSG10004_ImportDeclarationAnalyzerService()
            : base("UnifreightIIG.Common.MessageLib.Declaration.DF_NG_2754_MSG10004_ImportDeclarationResponse.xsd")
        {

        }

        public override INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            DF_NG_2754_MSG10004_ImportDeclarationResponse importDeclarationMessage = null;
            importDeclarationMessage = _CustomResponse;
            var tenant = 1;
            var reData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();

            var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams();
            requestParams.Tenant = ResolveTenant();

            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            requestParams.AppicationId = myDeclarationQueryService.GetIdByDeclarationNumber(importDeclarationMessage.Response.Declaration.ID.Value,requestParams.Tenant);
            
            DF_NG_2754_MSG10004_ImportDeclarationResponseService customResponseService = new DF_NG_2754_MSG10004_ImportDeclarationResponseService();
            customResponseService.Update(importDeclarationMessage, requestParams);
            if (customResponseService.MyResponseData == null)
            {
                //??
                return null;
            }
            reData.ExceptionMessage = customResponseService.MyResponseData.ExceptionMessage;
            reData.ApplicationID = customResponseService.MyResponseData.ApplicationID;
            reData.HasException = customResponseService.MyResponseData.HasException;
            reData.Succeeded = customResponseService.MyResponseData.Succeeded;
            return reData;
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
            return _CustomResponse.Response.Declaration.ID.Value;
        }

    }
}
