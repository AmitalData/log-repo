using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.BL.Messaging.ILOVS;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Data.EntityListQueryServices;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Reflection;
using Logitude.Customs.Data.EntityLists;
using WebFreight.Web.CustomWebServices.BL.XLSExport;
using System.IO;
using System.Net.Http.Headers;
using Logitude.Customs.BL.Messaging.ILSWS;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Customs.Data.EntityPOCOs;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class LogisticActionRequestController : ApiController
    {
        
        public HttpResponseMessage GetIfLogisticActionRequestExists(string id, string cargoIdentifierKey1, string cargoIdentifierKey2, string cargoIdentifierKey3, string cargoIdentifierType)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                LogisticActionRequestRepository rep = new LogisticActionRequestRepository(authToken.Tenant);
                bool exist = rep.GetExistByCargoKey(id, cargoIdentifierKey1, cargoIdentifierKey2, cargoIdentifierKey3, cargoIdentifierType);
                
                return Request.CreateResponse(HttpStatusCode.OK, exist);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

   }
}