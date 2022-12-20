using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
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
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var screenQueryService = new DigitalPortalScreenQueryService(tenant);
                var digitalPortalScreens = screenQueryService.GetDigitalPortalScreensQuery(tenant, objectTableId, screenCode);
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
        [Route("DigitalTextCode/UpdateDigitalPortalScreen")]
        public HttpResponseMessage UpdateDigitalPortalScreen(DigitalPortalScreenUpdateModel digitalTextCodeUpdateModel)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                digitalTextCodeUpdateModel.Tenant = tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var digitalPreDefinedComponentQueryService = new DigitalPortalScreenQueryService(tenant);

                var tenantDigitalPortalScreen = digitalPreDefinedComponentQueryService.GetDigitalPortalScreensQuery(tenant, digitalTextCodeUpdateModel.ObjectTableId, digitalTextCodeUpdateModel.ScreenCode)
                                                                                      .FirstOrDefault();

                if (tenantDigitalPortalScreen == null)
                {
                    tenantDigitalPortalScreen = new DigitalPortalScreenList
                    {
                        Name = digitalTextCodeUpdateModel.Name,
                        Content = digitalTextCodeUpdateModel.IsDraft ? "" : digitalTextCodeUpdateModel.Content,
                        DraftContent = digitalTextCodeUpdateModel.DraftContent,
                        Tenant = tenant,
                        ScreenCode = digitalTextCodeUpdateModel.ScreenCode,
                        ObjectTableId = digitalTextCodeUpdateModel.ObjectTableId,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow
                    };
                }
                else
                {
                    if (digitalTextCodeUpdateModel.IsDraft)
                    {
                        tenantDigitalPortalScreen.DraftContent = digitalTextCodeUpdateModel.DraftContent;
                        tenantDigitalPortalScreen.UpdateDate = DateTime.UtcNow;
                    }
                    else
                    {
                        tenantDigitalPortalScreen.Content = digitalTextCodeUpdateModel.Content;
                        tenantDigitalPortalScreen.DraftContent = digitalTextCodeUpdateModel.Content;
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
    }
}