
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Transactions;
using Logitude.Server.Tools;
 using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    
    public class CB_PreferenceController : ApiController
    {
		public HttpResponseMessage GetCB_PreferenceByUserIdAndTenant(string userId, int tenant = 0)
		{
			try
			{
                CB_PreferenceQueryService preferenceQueryServiceQueryService =  new CB_PreferenceQueryService(tenant);
                var CBPreferences = preferenceQueryServiceQueryService.GetCB_PreferenceByUserIdAndTenant(userId, tenant);
				return Request.CreateResponse(HttpStatusCode.OK, CBPreferences);
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}
		}
        public HttpResponseMessage AddNewCB_Preference(CB_PreferencePM entityPM)
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

                        ICustomContext MyContext = CustomContext.GetContext(entityPM.Tenant);
                        CB_PreferenceUpdateService service = new CB_PreferenceUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        service.Update(entityPM, true);

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
         public HttpResponseMessage AddNewAllCB_Preferences(List<CB_PreferencePM> entityPMs)
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
                        CB_PreferenceUpdateService service = new CB_PreferenceUpdateService(MyContext, new Dictionary<string, IContext>(), authToken.Tenant);
                        foreach (var entityPM in entityPMs)
                        {
                            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                            service.Update(entityPM, true);
                        }
                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                        return Request.CreateResponse(HttpStatusCode.OK, entityPMs);
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

        public HttpResponseMessage EditCB_Preference(CB_PreferencePM entityPM)
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


                        ICustomContext MyContext = CustomContext.GetContext(entityPM.Tenant);
                        CB_PreferenceUpdateService service = new CB_PreferenceUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        service.Update(entityPM, true);

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
        public HttpResponseMessage EditAllCB_Preferences(List<CB_PreferencePM> entityPMs)
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
                        CB_PreferenceUpdateService service = new CB_PreferenceUpdateService(MyContext, new Dictionary<string, IContext>(), authToken.Tenant);
                        foreach (var entityPM in entityPMs)
                        {
                            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                            service.Update(entityPM, true);
                        }
                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK, entityPMs);
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

        [HttpPost]
        public HttpResponseMessage DeleteCB_Preference(CB_PreferencePM entityPM)
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


                        ICustomContext MyContext = CustomContext.GetContext(entityPM.Tenant);
                        CB_PreferenceUpdateService service = new CB_PreferenceUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                        service.Update(entityPM, true);

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

        [HttpPost]
        public HttpResponseMessage DeleteAllCB_Preferences(List<CB_PreferencePM> entityPMs)
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
                        CB_PreferenceUpdateService service = new CB_PreferenceUpdateService(MyContext, new Dictionary<string, IContext>(), authToken.Tenant);
                        foreach (var entityPM in entityPMs)
                        {
                            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            service.Update(entityPM, true);
                        }
                        entityPMs.Clear();
                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK, entityPMs);
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