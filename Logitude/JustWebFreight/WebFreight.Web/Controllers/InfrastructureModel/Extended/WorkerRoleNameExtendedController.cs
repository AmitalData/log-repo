using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityListQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class WorkerRoleNamesExtendedController : ApiController
    {
        //public HttpResponseMessage Post(WorkerRoleNamePM entityPM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            string logKey = PerformanceLogger.LogCurrentTime();
        //            using (TransactionScope scope = TransactionFactory.GetTransaction())
        //            {
        //                string token = HttpContext.Current.Request.Headers["Token"];
        //                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        //                WorkerRoleNameQuery workerRoleNameQuery = new WorkerRoleNameQuery(authToken.Tenant);
        //                int latestWaitingStatus = workerRoleNameQuery.GetLatestWaitingStatusAfterCheckIfNotExist(entityPM.Name);
        //                if (latestWaitingStatus != 1)
        //                {
        //                    IWebFreightContext MyContext = WebFreightContext.GetContext(authToken.Tenant);
        //                    WorkerRoleNameService service = new WorkerRoleNameService(MyContext);
        //                    entityPM.WaitingStatus = latestWaitingStatus + 1;
        //                    service.Create(entityPM);
        //                }

        //                scope.Complete();
        //                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

        //                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
        //    }
        //}



    }
}