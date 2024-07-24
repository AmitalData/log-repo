
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.CargoTracking.Data;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.EntityListQueryServices;
using Simplog.Server.Infrastructure;
using System.Data.Common;
using System.Data.Entity;
using System.Configuration;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Services;
using System.Linq.Expressions;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Newtonsoft.Json;
using Microsoft.TeamFoundation.Common;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{


    public partial class QueueMessagesViewsController : ApiController
    {



        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("QueueMessage", "READ", authToken.Tenant);

                IWebFreightContext MyContext = WebFreightContext.GetContext(authToken.Tenant);
                QueueMessageRepository QueueMessageRepository = new QueueMessageRepository(MyContext);
                QueueMessageList entityList = null;
                QueueMessage entityPoco = QueueMessageRepository.GetSingleQueueMessage(id);

                if (entityPoco != null)
                {
                    List<QueueMessage> singleEntityList = new List<QueueMessage>();
                    singleEntityList.Add(entityPoco);

                    QueueMessageQuery QueueMessageQuery = new QueueMessageQuery(QueueMessageRepository);
                    IQueryable<QueueMessage> iQueryable = singleEntityList.AsQueryable();
                    IQueryable<QueueMessageList> iQueryableEntityList = QueueMessageQuery.GetIQueryableEntityList(iQueryable);
                    entityList = iQueryableEntityList.FirstOrDefault();

                }

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, entityList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetAll()
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("QueueMessage", "READ", authToken.Tenant);


                IWebFreightContext MyContext = WebFreightContext.GetContext(authToken.Tenant);
                QueueMessageRepository QueueMessageRepository = new QueueMessageRepository(MyContext);
                IQueryable<QueueMessage> entityPocos = QueueMessageRepository.GetQueueMessages();

                QueueMessageQuery QueueMessageQuery = new QueueMessageQuery(QueueMessageRepository);
                IQueryable<QueueMessageList> entityLists = QueueMessageQuery.GetIQueryableEntityList(entityPocos);
                entityLists = entityLists.OrderBy(d => d.Id);
                List<QueueMessageList> listResult = entityLists.ToList();
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, listResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }




        [HttpGet]
        public HttpResponseMessage GetByFilters([FromUri] string additionalFilters)
        {
            try
            {
                List<FilterCriteria> filterCriteria = JsonConvert.DeserializeObject<List<FilterCriteria>>(additionalFilters);

                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                SecurityUtility.CheckContactFeature("QueueMessage", "READ", authToken.Tenant);

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "QueueMessage",
                    PageIndex = 0,
                    PageSize = 100,
                    QuerySection = "QueueMessage",
                    SortByColumnName = "CreateDateTime",
                    SortDirectin = "ASC",
                    GetAll = false,
                };

                List<ObjectField> QueueMessageObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QueueMessages", tenant);
                QueueMessageRepository queueMessageRepository = new QueueMessageRepository();

                QueueMessagesService queueMessagesService = new QueueMessagesService();
                List<QueueMessage> resultList = queueMessagesService.FilteredQuery(queueMessageRepository, filterCriteria, tenant);
                ServiceResponse response = new ServiceResponse()
                {
                    Result = resultList,
                    Count = resultList.Count,
                };

                HttpResponseMessage responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return responseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }
}
