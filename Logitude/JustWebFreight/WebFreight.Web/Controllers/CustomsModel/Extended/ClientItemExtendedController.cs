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

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class ClientItemExtendedControllerExtendedController : ApiController
    {

        public HttpResponseMessage GetClientItem(string itemCode, string exporterCode, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                ClientItemQueryService clientItemQueryService = new ClientItemQueryService(tenant);
                ClientItemPM clientItem = clientItemQueryService.GetSingleWithTenant(itemCode, exporterCode, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, clientItem);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
     

        public HttpResponseMessage GetClientItemDescription(string itemDescription, string exporterCode, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                ClientItemQueryService clientItemQueryService = new ClientItemQueryService(tenant);
                ClientItemPM clientItem = clientItemQueryService.GetSingleWithTenantDescription(itemDescription, exporterCode, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, clientItem);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
     

    }
}