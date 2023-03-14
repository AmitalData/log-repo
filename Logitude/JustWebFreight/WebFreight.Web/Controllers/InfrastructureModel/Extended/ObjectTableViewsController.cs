using Logitude.BL.InfrastructureModel.CustomFilters;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class ObjectTableViewsController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                ObjectTableQuery objectTableQuery = new ObjectTableQuery(authToken.Tenant);


                ObjectTableList objecTtableList = objectTableQuery.GetObjectTableList(id, authToken.Tenant);


                //IWebFreightContext MyContext = WebFreightContext.GetContext(authToken.Tenant);
                //ObjectTableRepository objectTableRepository = new ObjectTableRepository(MyContext);
                //ObjectTableList entityList = null;
                //ObjectTable entityPoco = objectTableRepository.GetSingleObjectTable(id, authToken.Tenant , true);

                //if (entityPoco != null)
                //{
                //    List<ObjectTable> singleEntityList = new List<ObjectTable>();
                //    singleEntityList.Add(entityPoco);

                //    ObjectTableQuery objectTableQuery = new ObjectTableQuery(objectTableRepository);
                //    IQueryable<ObjectTable> iQueryable = singleEntityList.AsQueryable();
                //    IQueryable<ObjectTableList> iQueryableEntityList = objectTableQuery.GetIQueryableEntityList(iQueryable);
                //    entityList = iQueryableEntityList.FirstOrDefault();

                //}


                return Request.CreateResponse(HttpStatusCode.OK, objecTtableList);
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
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                IWebFreightContext MyContext = WebFreightContext.GetContext(authToken.Tenant);
                ObjectTableRepository objectTableRepository = new ObjectTableRepository(MyContext);
                IQueryable<ObjectTable> entityPocos = objectTableRepository.GetObjects();

                ObjectTableQuery chargesGroupQuery = new ObjectTableQuery(objectTableRepository);
                IQueryable<ObjectTableList> entityLists = chargesGroupQuery.GetIQueryableEntityList(entityPocos);
                entityLists = entityLists.OrderBy(d => d.Name);

                return Request.CreateResponse(HttpStatusCode.OK, entityLists);
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
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                if (filters.Tenant != null)
                    tenant = tenant;

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "ObjectTable",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "ObjectTables",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> ChargesGroupObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ObjectTable", tenant);
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
                 
                        ObjectField field = ChargesGroupObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
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
                        ObjectField field = ChargesGroupObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {


                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
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
                ObjectTableRepository objectTableRepository = new ObjectTableRepository(MyContext);
                IQueryable<ObjectTable> entityPocos = objectTableRepository.GetObjects();
                entityPocos = entityPocos.Where(entity => entity.Tenant == 0 || entity.Tenant == tenant);

                ObjectTableQuery objectTableQuery = new ObjectTableQuery(objectTableRepository);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                ObjectTableCustomFilter customfilters = new ObjectTableCustomFilter(tenant);
                entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos);

                entityPocos = genericFilter.GetFilteredQuery<ObjectTable>(nonListQueryOperation, entityPocos);
                int skippedEntities = queryOperations.PageIndex;
                IQueryable<ObjectTableList> entityLists = objectTableQuery.GetIQueryableEntityList(entityPocos);

                entityLists = genericFilter.GetFilteredQuery<ObjectTableList>(listQueryOperation, entityLists);


                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    PropertyInfo propInfo = typeof(ObjectTableList).GetProperty(queryOperations.SortByColumnName);


                    ObjectField objectField = (from a in ChargesGroupObjectFields
                                               where a.FieldName == queryOperations.SortByColumnName
                                               select a).FirstOrDefault();

                    if (objectField != null)
                    {
                        if (objectField.IsCustom)
                        {
                            entityLists = sortClass.GetSorterQuery<ObjectTableList, string>(queryOperations, entityLists);
                        }
                        else
                        {
                            switch (objectField.DataTypeCode.ToLower())
                            {
                                case "ntext":
                                case "text":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ObjectTableList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "sigdouble":
                                case "double":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ObjectTableList, double>(queryOperations, entityLists);
                                        break;
                                    }
                                case "date":
                                case "datetime":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ObjectTableList, DateTime>(queryOperations, entityLists);
                                        break;
                                    }
                                case "unsinteger":
                                case "integer":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ObjectTableList, int>(queryOperations, entityLists);
                                        break;
                                    }
                                case "boolean":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ObjectTableList, bool>(queryOperations, entityLists);
                                        break;
                                    }
                                case "unsdecimal":
                                case "decimal":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ObjectTableList, decimal>(queryOperations, entityLists);
                                        break;
                                    }
                                default:
                                    {
                                        entityLists = entityLists.OrderBy(d => d.Name);
                                        break;
                                    }
                            }
                        }
                    }
                }
                else
                {
                    entityLists = entityLists.OrderBy(d => d.Name);
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

                response.Result = entityLists;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);


                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }






    }
}


 
