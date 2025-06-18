using AmitalCloud.Infrastructure.Application.EntityListQueryServices;
using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Model.EntityClasses ;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using AmitalCloud.Infrastructure.Application.EntityListQueryServices;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class InfrastructureDomainController : ApiController
    {
        public HttpResponseMessage GetAllowedFeaturesForLoggedUser()
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                string token = HttpContext.Current.Request.Headers["Token"];
                string loggedUserEmail = AuthenticationTokenRepository.GetSingleTokenFromCache(token).Email;

                ContactQuery contactQuery = new ContactQuery(tenant);
                string loggedUserId = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant)?.Id;

                FeatureQuery featureQuery = new FeatureQuery(tenant);
                LoggedUserFeatures loggedUserFeatures = featureQuery.GetAllowedFeaturesForLoggedUser(loggedUserId, tenant);
                List<FeaturePM> myResult1 = loggedUserFeatures.Features;
                List<FeaturePM> myResult = new List<FeaturePM>();
                List<string> toggleCodes = myResult1.Where(d => !string.IsNullOrEmpty(d.ToggleCode)).Select(s => s.ToggleCode).ToList();
                if (toggleCodes.Count == 0)
                {
                    myResult = myResult1;
                }
                else
                {
                    FeatureToggleRepository featureToggleRepository = new FeatureToggleRepository(0);
                    List<FeatureToggle> featureToggles = featureToggleRepository.GetAllByToggleCodeList(toggleCodes, 0).ToList();
                    foreach (FeaturePM item in myResult1)
                    {
                        if (string.IsNullOrEmpty(item.ToggleCode))
                        {
                            myResult.Add(item);
                        }
                        else
                        {
                            FeatureToggle featureToggle = featureToggles.Where(d => d.ToggleCode == item.ToggleCode && (d.TenantNumber == tenant || (tenant >= d.FromTenantNumber && tenant <= d.ToTenantNumber))).FirstOrDefault();
                            if (featureToggle != null)
                            {
                                myResult.Add(item);
                            }
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetFeatureToggles()
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                List<FeatureToggleList> myResult = new FeatureToggleListQueryService(tenant).GetList(tenant).Where(a => a.Inactive == false).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetLastFilters()
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                string token = HttpContext.Current.Request.Headers["Token"];
                string loggedUserEmail = AuthenticationTokenRepository.GetSingleTokenFromCache(token).Email;
                //ICRMContext crmContext = CRMContext.GetContext(tenant);
                //CRMFilterSettingListQueryService listService = new CRMFilterSettingListQueryService(crmContext);
                //List<CRMFilterSettingList> myResult = listService.GetList(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, new List<string>());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}