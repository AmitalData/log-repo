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
using UnifreightIIG.Common.GlobalScannedAttachmentToEntityServiceReference;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    public class D_NG_2716_MSG22001_AddAttachmentAnalyzerService : MessageAnalyzerServiceBase<
        GenericRequestParams,INF_MSG_GenericResponseData,
        D_NG_2716_MSG22001_AddAttachmentResponse, D_NG_2716_MSG22001_AddAttachmentResponseService>, IMessageAnalyzerService

    {
        public D_NG_2716_MSG22001_AddAttachmentAnalyzerService()
            : base("UnifreightIIG.Common.GlobalScannedAttachmentToEntityServiceReference.D_NG_2716_MSG22001_AddAttachmentResponse.xsd")
        {
            
        }

        public override INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            D_NG_2716_MSG22001_AddAttachmentResponse addAttachmentMessage = null;
            addAttachmentMessage = _CustomResponse;
            var tenant = 1;
            var reData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();

            var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams();
            requestParams.Tenant = ResolveTenant();

            var context = CustomContext.GetContext(requestParams.Tenant);
            var myCustomsDocumentQueryService = new CustomsDocumentQueryService(context);
            var customsDocumentPM = myCustomsDocumentQueryService.GetSingle(addAttachmentMessage.externalAttachmentID.ToString(), true, false);
            requestParams.AppicationId = customsDocumentPM.DocumentsFilingId;
            D_NG_2716_MSG22001_AddAttachmentResponseService customResponseService = new D_NG_2716_MSG22001_AddAttachmentResponseService();
            customResponseService.Update(addAttachmentMessage, requestParams);
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
            return "Customs.CustomsDocument";
        }

        public override string GetLoggingEntityReference()
        {
            return _CustomResponse.ResponseContentHeader.ApplicationID.ToString();
        }

    }
}
