using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;


namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated

{
    public partial class InternalBorderSiteTypesController : ApiController
    {
        public HttpResponseMessage Put(InternalBorderSiteTypePM entityPM)
        {
            if (ModelState.IsValid)
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
                        var us = new InternalBorderSiteTypeUpdateService(MyContext, new System.Collections.Generic.Dictionary<string, IContext>(), authToken.Tenant);

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
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
    }
}