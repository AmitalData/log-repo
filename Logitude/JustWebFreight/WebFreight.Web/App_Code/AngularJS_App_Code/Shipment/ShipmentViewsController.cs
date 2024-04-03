using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.Helpers;
using System.Web.Script.Serialization;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataContracts;
using Simplog.Data.ShipmentsModel;
using WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using WebFreight.Web.ShipmentsModel.DomainServices;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class ShipmentViewsController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", authToken.Tenant);
                int tenant = authToken.Tenant;
                bool isFullTextSearch = false;
                TenantRepository myTenantRepository = new TenantRepository(tenant);
                Tenant myTenant = myTenantRepository.GetSingleTenant(tenant);
                if (myTenant != null)
                {
                    isFullTextSearch = myTenant.IsFullTextSearchEnabled;
                }
                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Shipment",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Shipments",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    QueryFilterItems = new List<QueryFilterItem>(),
                };


                List<ObjectField> ShipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant);
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
                        //if (filterValue1 != null && filterValue1.GetType() == typeof(string))
                        //{
                        //string[] values = filterValue1.ToString().Split(',');
                        //if (values.Count() > 1)
                        //{
                        //filterValue1 = values[0];
                        //filterValue2 = values[1];
                        //}
                        //}
                        //ToDo: Get object field by name and set the remained filter properties
                        ObjectField field = ShipmentObjectFields.FirstOrDefault(f => f.FieldName == filterName);
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
                        ObjectField field = ShipmentObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {


                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList,field.IsCustom,field.DataTypeCode);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }
                string SearchFilterAsWhere = "";
                List<SqlParameter> parameters = new List<SqlParameter>();
                string loggedUserEmail = authToken.Email;
                string loggedContactId = null;
                //ContactQuery contactQuery = new ContactQuery(tenant);
                //ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                //if (loggedContact != null)
                //{
                //    loggedContactId = loggedContact.Id;
                //}
                if (isFullTextSearch)//Ayman,Ihab and Rabaia
                {
                    var SearchFilter = queryOperations.QueryFilterItems.Where(a => a.FieldName == "SearchFields").FirstOrDefault();
                    List<string> ShipmentIds = new List<string>();

                    parameters.Add(new SqlParameter("@Tenant", tenant));
                    if (SearchFilter != null)
                    {
                        SearchFilterAsWhere = "Id in (SELECT Id FROM Shipments Where Tenant = @p__linq__0 and Contains(SearchFields,@SearchFields))";
                        parameters.Add(new SqlParameter("@SearchFields", "\"" + SearchFilter.FieldValue + "*\""));
                        queryOperations.QueryFilterItems.Remove(SearchFilter);
                    }
                }

                

                ShipmentAPiHelper.AddFilters(queryOperations, tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                if (FeatureToggleHelper.HasFeatureToggle("SCD", tenant))
                {
                    shipmentRepository.SetSecondDBforContext(tenant);
                }
                TenantQuery tenantQuery = new TenantQuery(tenant);
                TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);
                GenericFilter genericFilter = new GenericFilter();
                GenericSort sortClass = new GenericSort();
                ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);
                IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);
                var MySearchFilter = queryOperations.QueryFilterItems.Where(a => a.FieldName == "SearchFields").FirstOrDefault();

                if (MySearchFilter != null)
                {
                    var SearchTerm = MySearchFilter.FieldValue.ToString();
                    shipments = shipments.Where(a => a.SearchFields.Contains(SearchTerm));
                    queryOperations.QueryFilterItems.Remove(MySearchFilter);
                }
                shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                shipments = genericFilter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);
                int skippedShipments = queryOperations.PageIndex;

                ShipmentQuery myShipmentQuery = new ShipmentQuery(shipmentRepository);
                var sss = IQueryableExtensions.ToTraceString(shipments);
                var entityLists = myShipmentQuery.GetIQueryableShipmentList(shipments, tenant);

                entityLists = genericFilter.GetFilteredQuery<ShipmentList>(listQueryOperation, entityLists);
                //if (ShipmentIds != null && ShipmentIds.Count > 0)
                //{
                //    entityLists = entityLists.Where(a => ShipmentIds.Contains(a.Id));
                //}
                
               // var MySql = ((System.Data.Objects.ObjectQuery)entityLists).ToTraceString();
                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    PropertyInfo propInfo = typeof(ShipmentList).GetProperty(queryOperations.SortByColumnName);
                    List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant).ToList();

                    ObjectField objectField = (from a in shipmentObjectFields
                                               where a.FieldName == queryOperations.SortByColumnName
                                               select a).FirstOrDefault();

                    if (objectField != null)
                    {
                        if (!objectField.IsCustom)
                        {
                            switch (objectField.DataTypeCode.ToLower())
                            {
                                case "text":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "double":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ShipmentList, double>(queryOperations, entityLists);
                                        break;
                                    }
                                case "datetime":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ShipmentList, DateTime>(queryOperations, entityLists);
                                        break;
                                    }
                                case "integer":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ShipmentList, int>(queryOperations, entityLists);
                                        break;
                                    }
                                case "lookup":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "boolean":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ShipmentList, bool>(queryOperations, entityLists);
                                        break;
                                    }
                                default:
                                    {
                                        entityLists = entityLists.OrderByDescending(d => d.CreateDateTime);
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            entityLists = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, entityLists);
                        }
                    }
                }
                else
                {
                    entityLists = entityLists.OrderByDescending(d => d.CreateDateTime);
                }

                ServiceResponse response = new ServiceResponse();
                
                int count=0;
                if (filters.GetCount && string.IsNullOrEmpty(SearchFilterAsWhere))
                {
                    response.Count = entityLists.Count();
                }
                entityLists = System.Data.Entity.QueryableExtensions.Skip(entityLists,()=> skippedShipments);
                entityLists = System.Data.Entity.QueryableExtensions.Take(entityLists,() => queryOperations.PageSize);
                //entityLists = entityLists.Skip(skippedShipments);
                //entityLists = entityLists.Take(queryOperations.PageSize);
                List<ShipmentList> listQuery;
                //string loggedUserEmail = authToken.Email;
                //string loggedContactId = null;
                //ContactQuery contactQuery = new ContactQuery(tenant);
                //ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                //if (loggedContact != null)
                //{
                //    loggedContactId = loggedContact.Id;
                //} 
                if (isFullTextSearch)//Ayman,Ihab and Rabaia
                {
                    TraceStringValues MySql;
                    MySql = IQueryableExtensions.ToTraceString<ShipmentList>(entityLists);//.ToString().Replace("\r\n", "").ToLower();
                    if (!string.IsNullOrEmpty(SearchFilterAsWhere) && MySql.TSQL.ToLower().Contains("where"))
                    {
                        var regex = new Regex(Regex.Escape("WHERE"), RegexOptions.IgnoreCase);
                        MySql.TSQL = regex.Replace(MySql.TSQL, "WHERE " + SearchFilterAsWhere + " AND ", 1);

                        var regex1 = new Regex(Regex.Escape("WHERE [Project1].[row_number] > "), RegexOptions.IgnoreCase);
                        MySql.TSQL = regex1.Replace(MySql.TSQL, "WHERE [Project1].[row_number] > 0 --", 1);
                        //MySql = MySql.Replace("where ", SearchFilterAsWhere + " and ");
                    }
                    //parameters.Concat();
                    foreach (var item in MySql.TSQLParams)
                    {
                        parameters.Add(new SqlParameter(item.Name, item.Value));
                    }
                    IShipmentsContext context = ShipmentsContext.GetContext(tenant);
                    ShipmentsContext activeContext = context.GetActiveDbContext() as ShipmentsContext;
                    //var mylistQuery = activeContext.Database.SqlQuery<ShipmentDataView>(MySql.TSQL, parameters.ToArray()).AsQueryable();
                    listQuery = activeContext.Database.SqlQuery<ShipmentList>(MySql.TSQL, parameters.ToArray()).ToList();
                    //listQuery = myShipmentQuery.GetIQueryableShipmentList(mylistQuery1.AsQueryable(), tenant).ToList(); 
                }
                else
                {
                    listQuery = entityLists.ToList();
                }

                response.Result = listQuery;
                if (filters.GetCount && !string.IsNullOrEmpty(SearchFilterAsWhere))
                {
                    response.Count = listQuery.Count();
                }
                CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
                customFieldResolver.SetCustomFieldsValues("Shipment", tenant, listQuery.Cast<object>().ToList());
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                if (filters.GetCount)
                {
                    reponseMessage.Headers.Add("TotalCount", count.ToString());

                }

                return reponseMessage;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
         
        public HttpResponseMessage GetSingle(string Id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", authToken.Tenant);

                IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                ShipmentRepository shipmentRepository = new ShipmentRepository(MyContext);
                ShipmentQuery myShipmentQuery = new ShipmentQuery(shipmentRepository);

                ShipmentDataView f = shipmentRepository.GetSingleShipmentDataView(Id, authToken.Tenant);

                ShipmentList myResult = myShipmentQuery.GetSingleShipmentList(f, authToken.Tenant);

                ServiceResponse response = new ServiceResponse();
                response.Result = myResult;

                return Request.CreateResponse(HttpStatusCode.OK, myResult); 
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}