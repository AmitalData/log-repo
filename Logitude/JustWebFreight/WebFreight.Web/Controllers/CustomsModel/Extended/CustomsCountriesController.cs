using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Transactions;
using Logitude.BL.Helpers;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.BL;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure.Helpers;
using System;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    public partial class CustomsCountriesController : ApiController
    {
        public HttpResponseMessage Put(CustomsCountryPM entityPM)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    ICustomContext MyContext = CustomContext.GetContext(authToken.Tenant);
                    var us = new CustomsCountryUpdateService(MyContext, new System.Collections.Generic.Dictionary<string, IContext>(), authToken.Tenant);

                    entityPM.ChangeSetOp = ChangeSetOperation.Update;
                    us.Update(entityPM, true);

                    scope.Complete();
                    PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}