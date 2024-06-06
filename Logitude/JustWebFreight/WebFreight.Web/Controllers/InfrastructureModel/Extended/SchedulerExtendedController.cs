using Logitude.BL.InfrastructureModel.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
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

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class SchedulerExtendedController : ApiController
    {
        public HttpResponseMessage GetSchedulerDetailsById(string schedulerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                SecurityUtility.CheckContactFeature("TasksScheduler", "READ", authToken.Tenant);
                TasksSchedulerQuery tasksSchedulerQuery = new TasksSchedulerQuery(tenant);
                string schedulerDetailsXmal = tasksSchedulerQuery.GetSchedulerDetailsXmalById(schedulerId, tenant);
                SchedulerDetails schedulerDetails = new SchedulerDetails();
                if (!string.IsNullOrEmpty(schedulerDetailsXmal))
                {
                    schedulerDetails = LogitudeXmlSerializer.DeserializeObject<SchedulerDetails>(schedulerDetailsXmal);
                }

                return Request.CreateResponse(HttpStatusCode.OK, schedulerDetails);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSchedulerHistoryLogs(string historyId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                SecurityUtility.CheckContactFeature("TasksScheduler", "READ", authToken.Tenant);
                //SchedulerLogsQuery LogsQuery = new SchedulerLogsQuery(tenant);
                //var HistoryLogs = LogsQuery.GetSchedulerLogsByHistory(historyId);


                return Request.CreateResponse(HttpStatusCode.OK);// HistoryLogs);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage Post(TasksSchedulerPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("TasksScheduler", entityPM.Tenant, authToken.Tenant);

                        SecurityUtility.CheckContactFeature("TasksScheduler", "NEW", authToken.Tenant);
                        IWebFreightContext MyContext = WebFreightContext.GetContext(entityPM.Tenant);
                        TasksSchedulerService service = new TasksSchedulerService(MyContext, entityPM.Tenant);


                        if (entityPM.SchedulerDetailsData != null)
                        {
                            if (entityPM.SchedulerDetailsData.FTPDetails != null && !string.IsNullOrEmpty(entityPM.SchedulerDetailsData.FTPDetails.Extension))
                            {
                                entityPM.SchedulerDetailsData.FTPDetails.Extension = entityPM.SchedulerDetailsData.FTPDetails.Extension.TrimStart('.');
                            }

                            entityPM.SchedulerDetailsData.Tenant = entityPM.Tenant;
                            
                            System.Type type1 = typeof(FTPSchedulerDetails);
                            System.Type type2 = "string".GetType();
                            System.Type type3 = typeof(ReportSchedulerDetails);
                            System.Type type4 = typeof(ReportSchedulerRecepients);
                            System.Type type5 = typeof(DWObjectFieldsDetails);
                            System.Type type6 = typeof(MultiSelectedValue);
                            System.Type type7 = typeof(ValueDetails);
                            System.Type type8 = typeof(ObjectFieldOperator);
                            System.Type[] types = new System.Type[8];
                            types[0] = type1;
                            types[1] = type2;
                            types[2] = type3;
                            types[3] = type4;
                            types[4] = type5;
                            types[5] = type6;
                            types[6] = type7;
                            types[7] = type8;

                            entityPM.SchedulerDetailsXML = LogitudeXmlSerializer.SerializeObjectToElementString(entityPM.SchedulerDetailsData, types);

                        }


                        service.Create(entityPM);

                        entityPM.SchedulerDetailsXML = null;

                        scope.Complete();
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

        public HttpResponseMessage Put(TasksSchedulerPM entityPM)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.AuthenticationOnEntityTenant("TasksScheduler", entityPM.Tenant, authToken.Tenant);
                    SecurityUtility.CheckContactFeature("TasksScheduler", "UPDATE", authToken.Tenant);


                    IWebFreightContext MyContext = WebFreightContext.GetContext(entityPM.Tenant);
                    TasksSchedulerService service = new TasksSchedulerService(MyContext, entityPM.Tenant);

                    if (entityPM.SchedulerDetailsData != null)
                    {
                        if (entityPM.SchedulerDetailsData.FTPDetails != null && !string.IsNullOrEmpty(entityPM.SchedulerDetailsData.FTPDetails.Extension))
                        {
                            entityPM.SchedulerDetailsData.FTPDetails.Extension = entityPM.SchedulerDetailsData.FTPDetails.Extension.TrimStart('.');
                        }

                        entityPM.SchedulerDetailsData.Tenant = entityPM.Tenant;

                        System.Type type1 = typeof(FTPSchedulerDetails);
                        System.Type type2 = "string".GetType();
                        System.Type type3 = typeof(ReportSchedulerDetails);
                        System.Type type4 = typeof(ReportSchedulerRecepients);
                        System.Type[] types = new System.Type[4];
                        types[0] = type1;
                        types[1] = type2;
                        types[2] = type3;
                        types[3] = type4;

                        entityPM.SchedulerDetailsXML = LogitudeXmlSerializer.SerializeObjectToElementString(entityPM.SchedulerDetailsData, types);

                    }


                    service.Update(entityPM);
                    entityPM.SchedulerDetailsXML = null;
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetIsExceedsScheduledTasksLimitPerReport(int tenant, string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                IWebFreightContext MyContext = WebFreightContext.GetContext(tenant);

                SecurityUtility.CheckContactFeature("TasksScheduler", "READ", authToken.Tenant);
                TasksSchedulerService service = new TasksSchedulerService(MyContext,tenant);
                TasksSchedulerQuery tasksSchedulerQuery = new TasksSchedulerQuery(tenant);

                bool isExceedsScheduledTasksLimitPerReport = service.isExceedsScheduledTasksLimitPerReport(tenant, entityId);
                if (!isExceedsScheduledTasksLimitPerReport)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, isExceedsScheduledTasksLimitPerReport);

                }
                else
                {
                    throw new ArgumentException("You have exceeded the defined quota. Contact your account manager if you need to add more!");
                }

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetIsEntityHasScheduler(string entityId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("TasksScheduler", authToken.Tenant, tenant);

                TasksSchedulerQuery tasksSchedulerQuery = new TasksSchedulerQuery(tenant);
                bool hasScheduler = tasksSchedulerQuery.GetIsEntityHasScheduler(entityId, tenant);
                
                return Request.CreateResponse(HttpStatusCode.OK, hasScheduler);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
		public HttpResponseMessage GetProceduresBySchema(string schemaId)
		{
			try
			{
				string token = HttpContext.Current.Request.Headers["Token"];
				AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
				SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
				int tenant = authToken.Tenant;
				
				TasksSchedulerQuery tasksSchedulerQuery = new TasksSchedulerQuery(tenant);
				List<Procedure> procedures = tasksSchedulerQuery.GetProceduresBySchema(schemaId, tenant);

				return Request.CreateResponse(HttpStatusCode.OK, procedures);
			}

			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}
		}
	}
}