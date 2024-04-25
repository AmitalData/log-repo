
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

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{


    public partial class QueueMessagesIncrementalStatViewsController : ApiController
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
        public HttpResponseMessage GetByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                SecurityUtility.CheckContactFeature("QueueMessage", "READ", authToken.Tenant);

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "QueueMessage",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "QueueMessage",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> QueueMessageObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QueueMessage", tenant);
                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                    object filterValue2 = null;

                    if (filterNameProp != null)
                    {
                        string filterName = filterNameProp.ToString();
                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";
                        ObjectField field = QueueMessageObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            //queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                        }
                        else
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                    }



                }

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = QueueMessageObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {


                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            //queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }


                GenericFilter genericFilter = new GenericFilter();
                GenericSort sortClass = new GenericSort();

                IWebFreightContext MyContext = WebFreightContext.GetContext(tenant);
                QueueMessageRepository QueueMessageRepository = new QueueMessageRepository(MyContext);
                IQueryable<QueueMessage> entityPocos = QueueMessageRepository.GetQueueMessages();

                QueueMessageQuery QueueMessageQuery = new QueueMessageQuery(QueueMessageRepository);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                entityPocos = genericFilter.GetFilteredQuery<QueueMessage>(nonListQueryOperation, entityPocos);
                int skippedEntities = queryOperations.PageIndex;
                IQueryable<QueueMessageList> entityLists = QueueMessageQuery.GetIQueryableEntityList(entityPocos);

                entityLists = genericFilter.GetFilteredQuery<QueueMessageList>(listQueryOperation, entityLists);


                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    PropertyInfo propInfo = typeof(QueueMessageList).GetProperty(queryOperations.SortByColumnName);


                    ObjectField objectField = (from a in QueueMessageObjectFields
                                               where a.FieldName == queryOperations.SortByColumnName
                                               select a).FirstOrDefault();

                    if (objectField != null)
                    {
                        if (objectField.IsCustom)
                        {
                            entityLists = sortClass.GetSorterQuery<QueueMessageList, string>(queryOperations, entityLists);
                        }
                        else
                        {
                            switch (objectField.DataTypeCode.ToLower())
                            {
                                case "ntext":
                                case "text":
                                case "lookup":
                                    {
                                        entityLists = sortClass.GetSorterQuery<QueueMessageList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "sigdouble":
                                case "double":
                                    {
                                        entityLists = sortClass.GetSorterQuery<QueueMessageList, double>(queryOperations, entityLists);
                                        break;
                                    }
                                case "date":
                                case "datetime":
                                    {
                                        entityLists = sortClass.GetSorterQuery<QueueMessageList, DateTime>(queryOperations, entityLists);
                                        break;
                                    }
                                case "unsinteger":
                                case "integer":
                                    {
                                        entityLists = sortClass.GetSorterQuery<QueueMessageList, int>(queryOperations, entityLists);
                                        break;
                                    }
                                case "boolean":
                                    {
                                        entityLists = sortClass.GetSorterQuery<QueueMessageList, bool>(queryOperations, entityLists);
                                        break;
                                    }
                                case "unsdecimal":
                                case "decimal":
                                    {
                                        entityLists = sortClass.GetSorterQuery<QueueMessageList, decimal>(queryOperations, entityLists);
                                        break;
                                    }
                                default:
                                    {
                                        entityLists = entityLists.OrderBy(d => d.Id);
                                        break;
                                    }
                            }
                        }
                    }
                }
                else
                {
                    entityLists = entityLists.OrderBy(d => d.Id);
                }

                ServiceResponse response = new ServiceResponse();

                if (filters.GetCount)
                {
                    response.Count = entityLists.Count();
                }
                if (!queryOperations.GetAll)
                {

                    entityLists = entityLists.Skip(skippedEntities);
                    entityLists = entityLists.Take(queryOperations.PageSize);

                }
                List<QueueMessageList> listResult = entityLists.ToList();

                response.Result = listResult;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



    }
}
