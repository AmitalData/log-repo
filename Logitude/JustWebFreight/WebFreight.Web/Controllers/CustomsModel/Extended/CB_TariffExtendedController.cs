using Intuit.Ipp.Core.Configuration;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using System.Runtime.Remoting.Contexts;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.PlatformUI;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CB_TariffExtendedController : ApiController
    {

        public HttpResponseMessage GetCustomsBookAgreementLevelData(int customsItemId, int measurementUnitMalamId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                if (token == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(new Exception("Token is missing")));

                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(0);


                CB_TariffQueryService tariffQueryService = new CB_TariffQueryService(0);
                List<CB_TariffList> result = tariffQueryService.GetCustomsBookAgreementLevelData(customsItemId, measurementUnitMalamId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        
        public HttpResponseMessage GetCustomsBookRegularityRequirementData(int customsItemId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                if (token == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(new Exception("Token is missing")));

                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(0);

                CB_RequirementComputedDataQueryService requirementComputedDataQueryService = new CB_RequirementComputedDataQueryService(0);
                List<CB_RequirementComputedDataList> result = requirementComputedDataQueryService.GetCustomsBookRegularityRequirementData(customsItemId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        
        public HttpResponseMessage GetCustomsBookRulesData(int customsItemId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                if (token == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(new Exception("Token is missing")));

                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(0);


                CB_RuleQueryService requirementComputedDataQueryService = new CB_RuleQueryService(0);
                List<CB_RuleList> result = requirementComputedDataQueryService.GetCustomsBookRulesData(customsItemId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCustomsBookTaxRates(int customsItemId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(0);

                CB_TariffQueryService tariffQueryService = new CB_TariffQueryService(0);
                List<CB_TariffList> result = tariffQueryService.GetCustomsBookTaxRates(customsItemId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }

}