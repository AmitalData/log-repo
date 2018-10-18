using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Ransom;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    public class VAL_NG_8227_MSG_520_RequiredDocumentMessageAnalyzerService : MessageAnalyzerServiceBase<
        GenericRequestParams, 
        INF_MSG_GenericResponseData,
        VAL_NG_8227_MSG_520_RequiredDocumentMessage,
        VAL_NG_8227_MSG_520_RequiredDocumentMessageResponseService>, IMessageAnalyzerService
    {
        public VAL_NG_8227_MSG_520_RequiredDocumentMessageAnalyzerService()
            : base("UnifreightIIG.Common.MessageLib.Ransom.VAL_NG_8227_MSG_520_RequiredDocumentMessage.xsd")
        {

        }

        public override INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            VAL_NG_8227_MSG_520_RequiredDocumentMessage addAttachmentMessage = _CustomResponse;
            var reData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();

            var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams();
            requestParams.Tenant = ResolveTenant();

            /*var context = CustomContext.GetContext(requestParams.Tenant); // to check what to do? how can i know witch record in CustomsDocumentPointers ???
            var myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(context);
            var customsDocumentPM = myCustomsDocumentPointerQueryService.GetSingle(addAttachmentMessage.RequiredDocumentDetails.DocumentID.ToString(), true, false);
            requestParams.AppicationId = customsDocumentPM.DocumentInId;*/
            VAL_NG_8227_MSG_520_RequiredDocumentMessageResponseService customResponseService = new VAL_NG_8227_MSG_520_RequiredDocumentMessageResponseService();
            customResponseService.Update(addAttachmentMessage, requestParams);
            if (customResponseService.MyResponseData == null)
            {
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
            return "Customs.CustomsDocument";
        }

        public override string GetLoggingEntityReference()
        {
            return _CustomResponse.RequiredDocumentDetails.documentID.ToString();
        }


    }
}
