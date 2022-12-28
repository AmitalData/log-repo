using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalCustomizationController : ApiController
    {
        [HttpGet]
        [Route("DigitalCustomization/GetDigitalPortalScreenNames")]
        public HttpResponseMessage GetDigitalPortalScreenNames()
        {
            int tenant = 0;
            string email = "";
            try
            {
                var screenQueryService = new DigitalPortalScreenQueryService(tenant);
                var digitalPortalScreens = screenQueryService.GetDigitalPortalScreenNamesQuery(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, digitalPortalScreens);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalCustomization/GetDigitalPortalScreens")]
        public HttpResponseMessage GetDigitalPortalScreens(string objectTableId, string screenCode = "")
        {
            int tenant = 0;
            string email = "";
            try
            {
                string securityKey = HttpContext.Current.Request.Headers["securitykey"];

                if (!string.IsNullOrWhiteSpace(securityKey))
                {
                    var authenticationHelper = new DigitalPortalAuthenticationHelper();
                    var shipmentIdAndTenant = authenticationHelper.GetShipmentBySecurityKey(securityKey);
                    if (shipmentIdAndTenant == null)
                    {
                        throw new AutenticationException("Sorry! this user is not authorized!");
                    }

                    tenant = shipmentIdAndTenant.Item2;
                }
                else
                {
                    var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                    email = authToken.Email;
                    tenant = authToken.Tenant;
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                }

                var screenQueryService = new DigitalPortalScreenQueryService(tenant);
                var digitalPortalScreens = screenQueryService.GetDigitalPortalScreensQuery(tenant, objectTableId, screenCode);
                var data = digitalPortalScreens.FirstOrDefault(a => a.Tenant == tenant);

                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }

                return Request.CreateResponse(HttpStatusCode.OK, digitalPortalScreens.FirstOrDefault());
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalCustomization/GetDigitalPreDefinedComponents")]
        public HttpResponseMessage GetDigitalPreDefinedComponents(string objectTableId, string name = "")
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var preDefinedComponentQueryService = new DigitalPreDefinedComponentQueryService(tenant);
                var digitalPreDefinedComponents = preDefinedComponentQueryService.GetDigitalPreDefinedComponentQuery(tenant, objectTableId, name);
                return Request.CreateResponse(HttpStatusCode.OK, digitalPreDefinedComponents);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [Route("DigitalCustomization/UpdateDigitalPortalScreen")]
        public HttpResponseMessage UpdateDigitalPortalScreen(DigitalPortalScreenUpdateModel digitalPortalScreenUpdateModel)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                digitalPortalScreenUpdateModel.Tenant = tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var digitalPreDefinedComponentQueryService = new DigitalPortalScreenQueryService(tenant);

                var tenantDigitalPortalScreen = digitalPreDefinedComponentQueryService.GetDigitalPortalScreensQuery(tenant, digitalPortalScreenUpdateModel.ObjectTableId, digitalPortalScreenUpdateModel.ScreenCode)
                                                                                      .FirstOrDefault(a => a.Tenant == tenant);

                DigitalPortalScreenList defaultTenantDigitalPortalScreen = null;

                if (digitalPortalScreenUpdateModel.IsDraft && string.IsNullOrEmpty(digitalPortalScreenUpdateModel.Content))
                {
                     defaultTenantDigitalPortalScreen = digitalPreDefinedComponentQueryService.GetDigitalPortalScreensQuery(tenant, digitalPortalScreenUpdateModel.ObjectTableId, digitalPortalScreenUpdateModel.ScreenCode)
                                                                      .FirstOrDefault(a => a.Tenant == 0);
                }


                if (tenantDigitalPortalScreen == null)
                {
                    tenantDigitalPortalScreen = new DigitalPortalScreenList
                    {
                        Name = digitalPortalScreenUpdateModel.Name,
                        Content = digitalPortalScreenUpdateModel.IsDraft  
                                    && string.IsNullOrEmpty(digitalPortalScreenUpdateModel.Content) 
                                 ? defaultTenantDigitalPortalScreen.Content 
                                 : digitalPortalScreenUpdateModel.Content,
                        DraftContent =  digitalPortalScreenUpdateModel.DraftContent,
                        Tenant = tenant,
                        ScreenCode = digitalPortalScreenUpdateModel.ScreenCode,
                        ObjectTableId = digitalPortalScreenUpdateModel.ObjectTableId,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow
                    };
                }
                else
                {
                    if (digitalPortalScreenUpdateModel.IsDraft)
                    {
                        tenantDigitalPortalScreen.DraftContent = digitalPortalScreenUpdateModel.DraftContent;
                        tenantDigitalPortalScreen.UpdateDate = DateTime.UtcNow;
                    }
                    else
                    {
                        tenantDigitalPortalScreen.Content = digitalPortalScreenUpdateModel.Content;
                        tenantDigitalPortalScreen.DraftContent = digitalPortalScreenUpdateModel.DraftContent;
                        tenantDigitalPortalScreen.UpdateDate = DateTime.UtcNow;
                    }
                }

                digitalPreDefinedComponentQueryService.UpdateDigitalPortalScreen(tenantDigitalPortalScreen);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalCustomization/GetDefaultScreenLayout")]
        public HttpResponseMessage GetDefaultScreenLayout(string objectTableId, string screenCode)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var digitalPreDefinedComponentQueryService = new DigitalPortalScreenQueryService(tenant);

                var allScreens = digitalPreDefinedComponentQueryService.GetDigitalPortalScreensQuery(tenant, objectTableId, screenCode);
                var defaultDisgitalPortalScreen = allScreens.FirstOrDefault(a => a.Tenant == 0);
                var tenantDigitalPortalScreen = allScreens.FirstOrDefault(a => a.Tenant == tenant);

                if (tenantDigitalPortalScreen != null)
                {
                    tenantDigitalPortalScreen.Content = tenantDigitalPortalScreen.Content;
                    tenantDigitalPortalScreen.DraftContent = defaultDisgitalPortalScreen.DraftContent;
                    tenantDigitalPortalScreen.UpdateDate = DateTime.UtcNow;
                    digitalPreDefinedComponentQueryService.UpdateDigitalPortalScreen(tenantDigitalPortalScreen);
                }
                else
                {
                    tenantDigitalPortalScreen = defaultDisgitalPortalScreen;
                }

                return Request.CreateResponse(HttpStatusCode.OK, tenantDigitalPortalScreen);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}