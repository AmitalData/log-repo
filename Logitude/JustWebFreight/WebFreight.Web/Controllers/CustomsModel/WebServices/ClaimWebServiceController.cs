using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class ClaimWebServiceController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage CheckIfCorporationNameExists(int tenant)
        {
            try
            {
                bool exists = false;
                string claimSubmiterNumber = "";
                var setting = CustomsSettingQueryService.GetSettingByTenant(tenant);

                claimSubmiterNumber = setting.CustomsAgentId.Length <= 9 ? setting.CustomsAgentId : null;
                if (!string.IsNullOrEmpty(claimSubmiterNumber))
                {
                    ClientQueryService clientQueryService = new ClientQueryService(tenant);
                    ClientPM clientPM = clientQueryService.GetClientByCode(claimSubmiterNumber, tenant);
                    if (clientPM != null && !string.IsNullOrWhiteSpace(clientPM.LocalCorporationName))
                    {
                        exists = true;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, exists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRequiredFieldsForClaim(string claimId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CustomsRequiredFieldErrors errors = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForClaim(claimId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, errors);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostSendClaimRequest(CLAIM_2340_ClaimRequestRequestParams requestParamsData)
        {
            try
            {
                ClaimAnswerResponseData responseData;
                var messagingService = new CLAIM_2340_ClaimRequestMessagingService();
                responseData = messagingService.Send(requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostSendContinuousRequestOnClaim(ContinuousRequestOnClaimFileRequestParams requestParamsData)
        {
            try
            {
                ContinuousResponseOnClaimFileResponseData responseData;
                var messagingService = new CLAIM_5005_ContinuousRequestOnClaimFileMessagingService();
                responseData = messagingService.Send(requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

    }
}