using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
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
using WebFreight.Web.SystemLogsModel.EntityPMs;
using WebFreight.Web.SystemLogsModel.Queries;
using WebFreight.Web.SystemLogsModel.Tools.EntityService;

namespace WebFreight.Web.Controllers.SystemLogsModel
{
    public class ErrorLogsController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                SecurityUtility.CheckContactFeature("ErrorLog", "READ", authToken.Tenant);
                ErrorLogsQuery errorLogQuery = new ErrorLogsQuery(authToken.Tenant);
                ErrorLogPM errorLogPM = errorLogQuery.GetSinglePM(id);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, errorLogPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage Post(ErrorLogPM entityPM)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                   // SecurityUtility.AuthenticationOnEntityTenant("ErrorLog", entityPM.Tenant, authToken.Tenant);
                    //SecurityUtility.CheckContactFeature("ErrorLog", "NEW", authToken.Tenant);

                    ErrorLog errorLogs = new ErrorLog();

                    ISystemLogContext systemLogContext = SystemLogContext.GetContext();

                    ErrorLogRepository  errorLogRepository = new ErrorLogRepository(systemLogContext);
                    if (entityPM.Id == null)
                    {
                        entityPM.Id = Guid.NewGuid().ToString();
                    }

                    try
                    {
                        if (!systemLogContext.ErrorLogs.Where(a => a.Id == entityPM.Id).Any())
                        {  
                            MapErrorLogsErrorLogsPM(entityPM, errorLogs);
                            ErrorsLogger.AddErrorLog(errorLogs);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.Message.Contains("Violation of PRIMARY KEY constraint") || ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                            {
                                try
                                {
                                    entityPM.Id = Guid.NewGuid().ToString();
                                    errorLogRepository.SubmitChanges();
                                }
                                catch (Exception)
                                {  
                                } 
                            }
                        }
                        //else
                        //    throw ex;
                        //Cannot insert duplicate key in object 

                    }
                     scope.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private void MapErrorLogsErrorLogsPM(ErrorLogPM errorLogPm, ErrorLog errorLog)
        {


            errorLog.Tenant = errorLogPm.Tenant;
            errorLog.UserName = errorLogPm.UserName;
            errorLog.LogDate = errorLogPm.LogDate;
            errorLog.ClientDate = errorLogPm.ClientDate;
            errorLog.Tier = errorLogPm.Tier;
            errorLog.Exception = errorLogPm.Exception;
            errorLog.StackTrace = errorLogPm.StackTrace;
            errorLog.IP = errorLogPm.IP;
            errorLog.SearchFields = errorLogPm.Tier + "," + errorLogPm.UserName + "," + errorLogPm.Exception + "," + errorLogPm.Tenant + "," + errorLogPm.IP;

            if (!string.IsNullOrEmpty(errorLog.Exception) && errorLog.Exception.Length > 7000)
            {
                errorLog.Exception = errorLog.Exception.Substring(0, 7000);
            }
            if (!string.IsNullOrEmpty(errorLog.StackTrace) && errorLog.StackTrace.Length > 7000)
            {
                errorLog.StackTrace = errorLog.StackTrace.Substring(0, 7000);
            }
            if (!string.IsNullOrEmpty(errorLog.SearchFields) && errorLog.SearchFields.Length > 8000)
            {
                errorLog.SearchFields = errorLog.SearchFields.Substring(0, 8000);
            }

        }
        public HttpResponseMessage Put(ErrorLogPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("ErrorLog", entityPM.Tenant, authToken.Tenant);
                        SecurityUtility.CheckContactFeature("ErrorLog", "UPDATE", authToken.Tenant);

                        string entityName = "ErrorLog" + entityPM.Id + entityPM.Tenant;
                        string entityPmName = "ErrorLogPM" + entityPM.Id + entityPM.Tenant;
                        if (CacheManager.CacheWrapper.Get(entityName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityName);
                        }
                        if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityPmName);
                        }

                        ISystemLogContext myContext = SystemLogContext.GetContext();
                        ErrorLogService service = new ErrorLogService(myContext);

                        service.Update(entityPM);

                        //ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
                        //ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("ErrorLog", 0, true);
                        //string email = HttpContext.Current.User.Identity.Name;
                        //ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                        //Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
                        //if (loggedContact != null)
                        //{
                        //   ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
                        //}

                        TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "ErrorLog");

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