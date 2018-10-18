//using Logitude.CRM.Data;
//using Logitude.CRM.Data.EntityListQueryServices;
//using Logitude.CRM.Data.EntityLists;
//using Logitude.Server.Tools.Helpers;
//using Simplog.Data.CommonDataModel;
//using Simplog.Data.CommonDataModel.EntityPOCOs;
//using Simplog.Data.CommonDataModel.Repositories;
//using Simplog.Server.Infrastructure.DataContracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Reflection;
//using System.Web;
//using System.Web.Http;
//using System.Web.Script.Serialization;
//using WebFreight.Web.DataContracts;
//using WebFreight.Web.Helpers;
//using WebFreight.Web.Security;

//namespace WebFreight.Web.App_Code.AngularJS_App_Code
//{
//    public class EmployeeGroupViewsController : ApiController
//    {

//        public HttpResponseMessage GetSingle(string id)
//        {
//            try
//            {
//                string token = HttpContext.Current.Request.Headers["Token"];
//                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
//                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
//                SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", authToken.Tenant);

//                ICRMContext MyContext = CRMContext.GetContext(1);
//                EmployeeGroupListQueryService listService = new EmployeeGroupListQueryService(MyContext);

//                var result = listService.GetSingle(id);
//                return Request.CreateResponse(HttpStatusCode.OK, result);
//            }
//            catch (Exception ex)
//            {
//                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
//            }

//        }

//        public HttpResponseMessage GetEmployeeGroupLists(int tenant)
//        {
//            try
//            {
//                string token = HttpContext.Current.Request.Headers["Token"];
//                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
//                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
//                SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", authToken.Tenant);

//                ICRMContext MyContext = CRMContext.GetContext(authToken.Tenant);
//                EmployeeGroupListQueryService listService = new EmployeeGroupListQueryService(MyContext);

//                List<EmployeeGroupList> result = listService.GetList(1);
//                return Request.CreateResponse(HttpStatusCode.OK, result);
//            }
//            catch (Exception ex)
//            {
//                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
//            }
//        }
//        [HttpGet]
//        public HttpResponseMessage GetEmployeeGroupsFilters([FromUri] ApiQueryFilters filters)
//        {
//            try
//            {
//                string token = HttpContext.Current.Request.Headers["Token"];
//                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
//                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
//                SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", authToken.Tenant);

//                QueryOperations queryOperations = new QueryOperations()
//                {
//                    ObjectTableName = "EmployeeGroup",
//                    PageIndex = filters.PageIndex,
//                    PageSize = filters.PageSize,
//                    QuerySection = "EmployeeGroups",
//                    SortByColumnName = filters.SortBy,
//                    SortDirectin = filters.SortDirection,

//                };

//                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
//                for (int i = 1; i <= 10; i++)
//                {
//                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
//                    object filterValue = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
//                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);

//                    if (filterNameProp != null)
//                    {
//                        string filterName = filterNameProp.ToString();
//                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";

//                        //ToDo: Get object field by name and set the remained filter properties
//                        queryOperations.SetFilter(filterName, filterValue, false, filterOperator, null, true);
//                    }



//                }

//                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
//                {
//                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
//                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

//                    foreach (QueryFilterItem filter in filters_list)
//                    {
//                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);

//                    }
//                }

//                ICRMContext MyContext = CRMContext.GetContext(1);
//                EmployeeGroupListQueryService listService = new EmployeeGroupListQueryService(MyContext);

//                List<EmployeeGroupList> result = listService.GetList(queryOperations, 1);

//                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, result);

//                if (filters.GetCount)
//                {
//                    int count = listService.GetListCount(queryOperations, 1);
//                    reponseMessage.Headers.Add("TotalCount", count.ToString());
//                }

//                return reponseMessage;
//            }
//            catch (Exception ex)
//            {
//                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
//            }

//        }

//        //List<PropertyInfo> filterProp = filters.GetType().GetProperties().Where(p=>p.Name.Contains("Filter")).ToList();
//        //foreach (var prop in filterProp)
//        //{
//        //    //PropertyInfo queryProp = queryOperations.GetType().GetProperty(prop.Name);
//        //   // queryProp.SetValue(queryOperations, queryProp.GetValue(filters));
//        //    var filterValue = prop.GetValue(filters);
//        //    queryOperations.SetFilter(prop.Name, filterValue, false, "Equals", null, true);

//        //}

//        //public HttpResponseMessage GetEmployeeGroupFiltersCount([FromUri]QueryOperations queryOperations)
//        //{
//        //    try
//        //    {
//        //        //SecurityUtility.AuthenticationOnTenant(tenant);
//        //        //SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", tenant);

//        //        ICRMContext MyContext = CRMContext.GetContext(1);
//        //        EmployeeGroupListQueryService queryService = new EmployeeGroupListQueryService(MyContext);

//        //        int result = queryService.GetListCount(queryOperations, 1);
//        //        return Request.CreateResponse(HttpStatusCode.OK, result);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
//        //    }

//        //}


//    }
//}

