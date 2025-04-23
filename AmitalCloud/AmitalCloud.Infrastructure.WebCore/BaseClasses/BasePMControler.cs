using AmitalCloud.Infrastructure.Application.BaseClasses;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Domain.Enums;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace AmitalCloud.Infrastructure.Web.BaseClasses
{
    public abstract class BasePMControler<TService, TUpdateService, TEntityPM, TEntityPOCO> : ControllerBase
        where TService : class, IBaseEntityQueryService<TEntityPM, TEntityPOCO>
        where TUpdateService : class, IBaseEntityUpdateService<TEntityPM>
        where TEntityPM : class, IEntityPM, new()
        where TEntityPOCO : IEntity
    {
        private protected string ObjectTableName;
        private protected bool EnableSecurity;
        private protected bool HasTenant;
        private protected bool SaveHistory;
        #region Constructors
        protected BasePMControler(string objectTableName, bool enableSecurity = false, bool hasTenant = true, bool saveHistory = false)
        {
            ObjectTableName = objectTableName;
            EnableSecurity = enableSecurity;
            HasTenant = hasTenant;
            SaveHistory = saveHistory;

        }
        protected BasePMControler()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Public Methods
        public HttpResponseMessage GetSingle()
        {
            string logKey = PerformanceLogger.LogCurrentTime();
            try
            {
                var tenant = AuthenticationToken("READ");
                return Request.CreateResponse(HttpStatusCode.OK, GetResult(tenant, Request.GetQueryNameValuePairs()));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
            finally
            {
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
            }

        }
        public HttpResponseMessage Post(IEntityPM entityPM)
        {
            if (ModelState.IsValid)
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                try
                {
                    var tenant = AuthenticationToken("NEW", entityPM.Tenant);
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        SaveEntity(entityPM, ChangeSetOperation.Insert);
                        if (SaveHistory) TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, ObjectTableName);
                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
                }
                finally
                {
                    PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
        public HttpResponseMessage Put(IEntityPM entityPM)
        {
            if (ModelState.IsValid)
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        var tenant = AuthenticationToken("UPDATE", entityPM.Tenant);
                        SaveEntity(entityPM, ChangeSetOperation.Update);
                        //TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "AWBAdditionalHandlingInfo");

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
                }
                finally
                {
                    PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        private object GetResult(int tenant, IEnumerable<KeyValuePair<string, string>> paramList) => GetService(tenant).GetSingle(paramList, true, false);
        private void SaveEntity(IEntityPM entityPM, ChangeSetOperation changeSetOperation)
        {
            if (!HasTenant) return;
            IBaseEntityUpdateService<TEntityPM> service = GetUpdateService(entityPM.Tenant);
            if (changeSetOperation == ChangeSetOperation.Update) service.InitializeEntityPM((TEntityPM)entityPM);
            entityPM.ChangeSetOp = changeSetOperation;
            service.Update((TEntityPM)entityPM, true);
        }
        #endregion
        private int AuthenticationToken(string mode, int tenant = 0)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            AmitalCloudSecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            if (EnableSecurity)
            {
                AmitalCloudSecurityUtility.CheckContactFeature(ObjectTableName, mode, authToken.Tenant);
                if (HasTenant)
                {
                    AmitalCloudSecurityUtility.AuthenticationOnEntityTenant(ObjectTableName, tenant, authToken.Tenant);
                }
            }
            return authToken.Tenant;
        }
        private IBaseEntityQueryService<TEntityPM, TEntityPOCO> GetService(int tenant)
        {
            return (IBaseEntityQueryService<TEntityPM, TEntityPOCO>)typeof(TService).GetConstructor(new Type[] { typeof(int) }).Invoke(null, new object[] { tenant });
        }
        private IBaseEntityUpdateService<TEntityPM> GetUpdateService(int tenant)
        {
            return (IBaseEntityUpdateService<TEntityPM>)typeof(TUpdateService).GetConstructor(new Type[] { typeof(int) }).Invoke(null, new object[] { tenant });
        }

    }
}
