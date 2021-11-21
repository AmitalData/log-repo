using Logitude.SystemLogs;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
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
namespace WebFreight.Web.Controllers.SystemLogsModel
{
    public class PerformanceLogsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post(PerformanceLog entity)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(entity.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("ErrorLog", entity.Tenant, authToken.Tenant);
                //SecurityUtility.CheckContactFeature("ErrorLog", "NEW", authToken.Tenant);

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalContext = GlobalContext.GetContext();

                    PerformanceLogRepository performanceLogRepository = new PerformanceLogRepository(globalContext);

                    try
                    {
                        string ip = "";
                        if (HttpContext.Current != null && HttpContext.Current.Request != null)
                        {
                            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                            if (string.IsNullOrEmpty(currentIP))
                            {
                                currentIP = HttpContext.Current.Request.UserHostAddress;
                            }
                            ip = currentIP;
                        }
                        entity.UserIP = ip;

                        entity.LogDateTimeGMT = DateTime.UtcNow;

                        performanceLogRepository.Add(entity);
                        performanceLogRepository.SubmitChanges();
                        scope.Complete();

                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.Message.Contains("Violation of PRIMARY KEY constraint") || ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                            {
                                entity.Id = Guid.NewGuid().ToString();
                                performanceLogRepository.SubmitChanges();
                                scope.Complete();
                            }
                        }
                        else
                            throw ex;
                        //Cannot insert duplicate key in object 

                    }
                    //  scope.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


		public HttpResponseMessage PostLogsList(List<PerformanceLog> logsList)
		{
			try
			{

				string token = HttpContext.Current.Request.Headers["Token"];
				AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
				SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
				//SecurityUtility.CheckContactFeature("ErrorLog", "NEW", authToken.Tenant);

				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{
					IGlobalContext globalContext = GlobalContext.GetContext();

					PerformanceLogRepository performanceLogRepository = new PerformanceLogRepository(globalContext);

					try
					{
						string ip = "";
						if (HttpContext.Current != null && HttpContext.Current.Request != null)
						{
							string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
							if (string.IsNullOrEmpty(currentIP))
							{
								currentIP = HttpContext.Current.Request.UserHostAddress;
							}
							ip = currentIP;
						}
						if (logsList != null && logsList.Count > 0)
						{
							foreach (var entity in logsList)
							{
                                SecurityUtility.AuthenticationOnTenant(entity.Tenant);
                                SecurityUtility.AuthenticationOnEntityTenant("PerformanceLog", entity.Tenant, authToken.Tenant);

                                entity.UserIP = ip;

								entity.LogDateTimeGMT = DateTime.UtcNow;

								performanceLogRepository.Add(entity);
							}

							performanceLogRepository.SubmitChanges();
						}
						scope.Complete();

					}
					catch (Exception ex)
					{
						if (ex.InnerException != null)
						{
							if (ex.InnerException.Message.Contains("Violation of PRIMARY KEY constraint") || ex.Message.Contains("Violation of PRIMARY KEY constraint"))
							{
								//entity.Id = Guid.NewGuid().ToString();
								//performanceLogRepository.SubmitChanges();
								scope.Complete();
							}
						}
						else
							throw ex;
						//Cannot insert duplicate key in object 

					}
					//  scope.Complete();
				}

				return Request.CreateResponse(HttpStatusCode.OK, "");
			}

			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}
		}


		// PUT api/<controller>/5
		public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}