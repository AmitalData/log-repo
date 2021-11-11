using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityListQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class BIReportsExtendedController : ApiController
    {
        public HttpResponseMessage GetReportExist(string name, string folderId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                BIReportQueryService bIReportQueryService = new BIReportQueryService(authToken.Tenant);
                bool reportExist = bIReportQueryService.DoesReportExist(name, folderId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, reportExist);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTenantReports([FromUri] ApiQueryFilters filters, int copyFromTenant)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                if(authToken.Tenant!=0 && copyFromTenant != 0)
                {
                    throw new Exception("Sorry! you have no permission to do this operation on Tenant:" + copyFromTenant + ". Please contact your administrator.");
                }

                SecurityUtility.AuthenticationOnTenant(copyFromTenant);

                SecurityUtility.CheckContactFeature("BIReport", "READ", authToken.Tenant);

                //int tenant = authToken.Tenant;

                TenantQuery tenantQuery = new TenantQuery(authToken.Tenant);
                bool tenantExist = tenantQuery.TenantExist(authToken.Tenant, copyFromTenant);

                if (!tenantExist)
                {
                    ServiceResponse emptyResponseMessage = new ServiceResponse();
                    emptyResponseMessage.Result = new List<BIReportList>();
                    emptyResponseMessage.Count = 0;
                    return Request.CreateResponse(HttpStatusCode.OK, emptyResponseMessage);
                }

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "BIReport",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "BIReports",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };


                List<ObjectField> BIReportObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("BIReport", copyFromTenant);
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
                        ObjectField field = BIReportObjectFields.FirstOrDefault(f => f.FieldName == filterName);
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
                        ObjectField field = BIReportObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
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

                IInfrastructureContext MyContext = InfrastructureContext.GetContext(copyFromTenant);
                BIReportListQueryService bIReportQuery = new BIReportListQueryService(MyContext);
                List<BIReportList> entityLists;
                if (filters.Filter1Value == "true")
                {
                    entityLists = bIReportQuery.GetAllLists(queryOperations, copyFromTenant);
                }
                else
                {
                    entityLists = bIReportQuery.GetList(queryOperations, copyFromTenant);
                }

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = 0;
                    if (filters.Filter1Value == "true")
                    {
                        count = bIReportQuery.GetAllListsCount(queryOperations, copyFromTenant);
                    }
                    else
                    {
                        count = bIReportQuery.GetListCount(queryOperations, copyFromTenant);
                    }
                    response.Count = count;
                }

                response.Result = entityLists;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PutWithoutAGGridXML(BIReportPM entityPM)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("BIReport", "UPDATE", authToken.Tenant);
                    SecurityUtility.AuthenticationOnEntityTenant("BIReport", entityPM.Tenant, authToken.Tenant);

                    BIReportQueryService query = new BIReportQueryService(entityPM.Tenant);
                    BIReportPM oldEntityPM = query.GetSingle(entityPM.Id, false, false);
                    entityPM.AGGridOptionsXML = oldEntityPM.AGGridOptionsXML;
                    IInfrastructureContext MyContext = InfrastructureContext.GetContext(entityPM.Tenant);
                    BIReportUpdateService service = new BIReportUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    service.InitializeEntityPM(entityPM);
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
    }
}