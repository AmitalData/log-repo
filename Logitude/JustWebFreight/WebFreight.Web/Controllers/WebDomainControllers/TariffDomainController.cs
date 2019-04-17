using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.TariffModule.BL.DataContracts;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityQueryServices;
using Logitude.TariffModule.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Linq;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class TariffDomainController : ApiController
    {
        public HttpResponseMessage GetTariffsCounts()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);

                string loggedContactId = null;
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (loggedContact != null)
                {
                    loggedContactId = loggedContact.Id;
                }


                TariffQueryService tariffQueryService = new TariffQueryService(tenant);

                TariffsSummary myResult = tariffQueryService.GetCount(tenant);


                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTenantTariffSetting()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ITariffModuleContext iContext = TariffModuleContext.GetContext(authToken.Tenant);
                TariffSettingPM entityPM = (from d in iContext.TariffSettings
                                            where d.Tenant == authToken.Tenant
                                            select new TariffSettingPM()
                                            {
                                                Id = d.Id,
                                                Tenant = d.Tenant,
                                                DefaultPriceSteps = d.DefaultPriceSteps,
                                            }).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}


