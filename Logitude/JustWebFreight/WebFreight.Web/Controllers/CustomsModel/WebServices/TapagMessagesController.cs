using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.CustomWebServices;
using WebFreight.Web.Helpers;
namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class TapagMessagesController : ApiController
    {
        public HttpResponseMessage GetSingleTapagList(string id, int tenant)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);

                TapagListQueryService tapagListQueryService = new TapagListQueryService(customContext);
                TapagList tapagList = tapagListQueryService.GetSingle(id);

                return Request.CreateResponse(HttpStatusCode.OK, tapagList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostGuaranteeCertificateRequest(GuaranteeCertificateRequestParams requestParams)
        {
            try
            {
                GuaranteeCertificateResponseData responseData = null;


                // use messageing service

                var service = new TPG_NG_8306_Web07_GuaranteeCertificateMessagingService();
                responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostFaultQueryRequest(FaultProceduralRequestParams requestParams)
        {
            try
            {
                FaultProceduralResponseData responseData = null;
                var service = new DF_NG_Web8332_FaultProceduralParamMessagingService();
                responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostGuaranteeFileFilterQueryRequest(GuaranteeRequestParams requestParams)
        {
            try
            {
                GuaranteeResponseData responseData = null;

                var service = new TPG_NG_8305_Web05_GuaranteeFileFilterParamMessagingService();
                responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDeclarationTapagsLists(string declarationId, int tenant)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);
                TapagQueryService queryService = new TapagQueryService(customContext);
                List<TapagList> tapagList = queryService.GetDeclarationTapags(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, tapagList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDepositPMByPaymentOrderNumberOrTapagId(string paymentNumber, string tapagId, int tenant)
        {
            if (paymentNumber == "" || paymentNumber == "null") paymentNumber = null;
            if (tapagId == "" || tapagId == "null") tapagId = null;

            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);

                DepositQueryService depositQueryService = new DepositQueryService(customContext);
                DepositPM depositPM = depositQueryService.GetDepositByPaymentOrderNumberOrTapagId(paymentNumber, tapagId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, depositPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDeficitPMByPaymentOrderNumberOrTapagId(string paymentNumber, string tapagId, int tenant)
        {
            if (paymentNumber == "" || paymentNumber == "null") paymentNumber = null;
            if (tapagId == "" || tapagId == "null") tapagId = null;

            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);

                DeficitQueryService deficitQueryService = new DeficitQueryService(customContext);
                DeficitPM deficitPM = deficitQueryService.GetDeficitByPaymentOrderNumberOrTapagId(paymentNumber, tapagId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, deficitPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetGuaranteeByTapagId(string tapagId, int tenant)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);

                GuaranteeQueryService guaranteeQueryService = new GuaranteeQueryService(customContext);
                GuaranteePM guaranteePM = guaranteeQueryService.GetGuaranteeByTapagId(tapagId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, guaranteePM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDeficitConnectedFileParagraphTypeList(string declarationId, string deficitId, int tenant)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);

                DeficitConnFileParagraphTypeQueryService deficitConnFileParagraphTypeQueryService = new DeficitConnFileParagraphTypeQueryService(customContext);
                List<DeficitConnFileParagraphTypeList> deficitConnFileParagraphTypeList = deficitConnFileParagraphTypeQueryService.GetDeficitConnectedFileParagraphTypesByDeclarationId(declarationId, deficitId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, deficitConnFileParagraphTypeList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage PostDeclarationFilterRequestParams(DeclarationFilterRequestParams requestParams)
        {
            try
            {
                

                var messagingService = new TPG_NG_8307_Web09_DeclarationFilterMessagingService();
                var responseData = messagingService.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostBankAccountToRefundQueryRequest(BankAccountToRefundRequestParams requestParams)
        {
            try
            {
                INF_MSG_GenericResponseData responseData = null;
                var service = new TPG_NG_2018_BankAccountToRefundUpdateReplayMessagingService();
                responseData = service.Send(requestParams);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

}