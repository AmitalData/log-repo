using ICSharpCode.SharpZipLib.BZip2;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.ShipmentsModel.DigitalModels;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;
using WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Marvin.JsonPatch;
using Marvin.JsonPatch.Exceptions;
using static Dropbox.Api.Sharing.ListFileMembersIndividualResult;
using WebFreight.Web.WebServices;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.Controllers.ShipmentsModel.Generated.PMControllers
{
    public class ShipmentController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(id, tenant);

                //DateTime completionTime = DateTime.Now;
                //int executionTime = (int)((completionTime.Ticks - callTime.Ticks) / TimeSpan.TicksPerMillisecond);
                //HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers", "ServerTime, X-Custom");
                //HttpContext.Current.Response.Headers.Add("ServerTime", executionTime.ToString());

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);


                return Request.CreateResponse(HttpStatusCode.OK, shipmentPM); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingleByForwarderShipmentNumber(string fsn)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM shipmentPM = shipmentQuery.GetSingleShipmentPMByForwarderNumber(fsn, tenant, 0); // Check the 0

                //DateTime completionTime = DateTime.Now;
                //int executionTime = (int)((completionTime.Ticks - callTime.Ticks) / TimeSpan.TicksPerMillisecond);
                //HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers", "ServerTime, X-Custom");
                //HttpContext.Current.Response.Headers.Add("ServerTime", executionTime.ToString());

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);


                return Request.CreateResponse(HttpStatusCode.OK, shipmentPM); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage Post(ShipmentPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;

                        SecurityUtility.AuthenticationOnTenant(tenant);
                        SecurityUtility.CheckContactFeature("Shipment", "NEW", tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("Shipment", entityPM.Tenant, authToken.Tenant);

                        IShipmentsContext objectContext = ShipmentsContext.GetContext(entityPM.Tenant);
                        ShipmentService service = new ShipmentService(objectContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                        service.Create();

                        IShipmentsContext updatedEntityContext = ShipmentsContext.GetContext(tenant);
                        ShipmentRepository updatedEntityRepository = new ShipmentRepository(updatedEntityContext);
                        ShipmentQuery updatedShipmentQuery = new ShipmentQuery(updatedEntityRepository);
                        entityPM = updatedShipmentQuery.GetSinglePM(entityPM.Id, entityPM.Tenant);

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


        public HttpResponseMessage Put(ShipmentPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;

                        SecurityUtility.AuthenticationOnTenant(tenant);
                        SecurityUtility.CheckContactFeature("Shipment", "UPDATE", tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("Shipment", entityPM.Tenant, authToken.Tenant);

                        IShipmentsContext objectContext = ShipmentsContext.GetContext(entityPM.Tenant);
                        ShipmentService service = new ShipmentService(objectContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                        service.Update(true);

                        IShipmentsContext updatedEntityContext = ShipmentsContext.GetContext(tenant);
                        ShipmentRepository updatedEntityRepository = new ShipmentRepository(updatedEntityContext);
                        ShipmentQuery updatedShipmentQuery = new ShipmentQuery(updatedEntityRepository);
                        entityPM = updatedShipmentQuery.GetSinglePM(entityPM.Id, entityPM.Tenant);

                        if (service.DummyIdGuidPackages != null)
                        {
                            foreach (var item in service.DummyIdGuidPackages)
                            {
                                ShipmentPackagePM itemPM = entityPM.ShipmentPackages.Where(d => d.Id == item.Key).FirstOrDefault();
                                if (itemPM != null)
                                {
                                    itemPM.DummyIdGuid = item.Value;
                                }
                            }
                        }

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

        // PATCH api/shipment?id={shipmentId}
        public HttpResponseMessage Patch(string id, JsonPatchDocument<ShipmentPM> shipmentJsonPatch)
        {
            try
            {
                using (TransactionScope transactionScope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authenticationToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authenticationToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("Shipment", "UPDATE", tenant);

                    IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
                    ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
                    ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                    ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(id, tenant);

                    if (shipmentPM == null) { throw new Exception("Cannot find the shipment"); }

                    SecurityUtility.AuthenticationOnEntityTenant("Shipment", shipmentPM.Tenant, tenant);

                    shipmentJsonPatch.ApplyTo(shipmentPM);

                    ShipmentService shipmentService = new ShipmentService(shipmentsContext, shipmentPM, SecurityUtility.GetAuthenticatedUser());
                    shipmentService.Update(true, isPatchUpdate: true);

                    ShipmentPM updatedShipmentPM = shipmentQuery.GetSinglePM(shipmentPM.Id, shipmentPM.Tenant);

                    transactionScope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, updatedShipmentPM);
                }
            }
            catch (JsonPatchException exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildJsonPatchException(exception, id, shipmentJsonPatch));
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(exception));
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        public HttpResponseMessage GetSingleByCustomerReference1(string CustomerReference1, bool IsForwarderShipment)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM shipmentPM = shipmentQuery.GetSinglePMByCustomerReference1(CustomerReference1, tenant, IsForwarderShipment);

                //DateTime completionTime = DateTime.Now;
                //int executionTime = (int)((completionTime.Ticks - callTime.Ticks) / TimeSpan.TicksPerMillisecond);
                //HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers", "ServerTime, X-Custom");
                //HttpContext.Current.Response.Headers.Add("ServerTime", executionTime.ToString());

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);


                return Request.CreateResponse(HttpStatusCode.OK, shipmentPM); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetByCustomerReferences1or3(string CustomerReference1, bool IsForwarderShipment)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM shipmentPM = shipmentQuery.GetByCustomerReferences1or3(CustomerReference1, tenant, IsForwarderShipment);

                //DateTime completionTime = DateTime.Now;
                //int executionTime = (int)((completionTime.Ticks - callTime.Ticks) / TimeSpan.TicksPerMillisecond);
                //HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers", "ServerTime, X-Custom");
                //HttpContext.Current.Response.Headers.Add("ServerTime", executionTime.ToString());

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);


                return Request.CreateResponse(HttpStatusCode.OK, shipmentPM); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetByCustomerReferences1or3ForUpdate(string CustomerReference1, string ShipmentId, bool IsForwarderShipment)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM shipmentPM = shipmentQuery.GetByCustomerReferences1or3ForUpdate(CustomerReference1, ShipmentId, tenant, IsForwarderShipment);

                //DateTime completionTime = DateTime.Now;
                //int executionTime = (int)((completionTime.Ticks - callTime.Ticks) / TimeSpan.TicksPerMillisecond);
                //HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers", "ServerTime, X-Custom");
                //HttpContext.Current.Response.Headers.Add("ServerTime", executionTime.ToString());

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);


                return Request.CreateResponse(HttpStatusCode.OK, shipmentPM); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetArchiveShipments([FromUri] List<string> Ids)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);

                foreach (var item in Ids)
                {

                    ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                    ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(item, tenant);
                    shipmentPM.IsOperationalClosed = true;
                    ShipmentService service = new ShipmentService(objectContext, shipmentPM, SecurityUtility.GetAuthenticatedUser());
                    service.Update(true);

                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }




        }

        [HttpGet]
        public HttpResponseMessage GetArchiveAllShipments([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", authToken.Tenant);
                int tenant = authToken.Tenant;

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

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }


                ShipmentAPiHelper.AddFilters(queryOperations, tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);


                GenericFilter genericFilter = new GenericFilter();
                GenericSort sortClass = new GenericSort();
                ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);
                IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);
                shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                shipments = genericFilter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);


                ShipmentQuery myShipmentQuery = new ShipmentQuery(shipmentRepository);

                var entityLists = myShipmentQuery.GetIQueryableShipmentList(shipments, tenant);

                entityLists = genericFilter.GetFilteredQuery<ShipmentList>(listQueryOperation, entityLists);
                var Ids = (from a in entityLists select a.Id).ToList();

                IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);

                foreach (var item in Ids)
                {

                    ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                    ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(item, tenant);
                    shipmentPM.IsOperationalClosed = true;
                    ShipmentService service = new ShipmentService(objectContext, shipmentPM, SecurityUtility.GetAuthenticatedUser());
                    service.Update(true);

                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetTop100ShipmentIds([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", authToken.Tenant);
                int tenant = authToken.Tenant;

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

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }


                ShipmentAPiHelper.AddFilters(queryOperations, tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);


                GenericFilter genericFilter = new GenericFilter();
                GenericSort sortClass = new GenericSort();
                ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);
                IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);
                shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                shipments = genericFilter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);


                ShipmentQuery myShipmentQuery = new ShipmentQuery(shipmentRepository);

                var entityLists = myShipmentQuery.GetIQueryableShipmentList(shipments, tenant);

                entityLists = genericFilter.GetFilteredQuery<ShipmentList>(listQueryOperation, entityLists);
                var Ids = (from a in entityLists select a.Id).Take(100).ToList();

                //IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);

                //foreach (var item in Ids)
                //{

                //    ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                //    ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(item, tenant);
                //    shipmentPM.IsOperationalClosed = true;
                //    ShipmentService service = new ShipmentService(objectContext, shipmentPM, SecurityUtility.GetAuthenticatedUser());
                //    service.Update(true);

                //}

                return Request.CreateResponse(HttpStatusCode.OK, Ids);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSingleBySecurityKeyWithoutToken(string key, int? tenant = null)
        {
            try
            {
                ShipmentQuery shipmentQuery;
                ShipmentAdditionalCloudCustomData CustomData;
                const string testKey = "d5e6d15f4cb24f12a8ac9c5e8c54a06d";
                //string logKey = PerformanceLogger.LogCurrentTime();

                //string token = HttpContext.Current.Request.Headers["Token"];
                //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //int tenant = authToken.Tenant;

                //SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);
                if (key == testKey)
                {
                    shipmentQuery = new ShipmentQuery(0);
                    CustomData = shipmentQuery.GetSingleShipmentAdditionalCloudCustomDataTest();
                    tenant = CustomData.Tenant;
                }
                else
                {
                    shipmentQuery = new ShipmentQuery(tenant.Value);
                    CustomData = shipmentQuery.GetSingleShipmentAdditionalCloudCustomData(key, tenant.Value);//GetSingleShipmentPMBySecurityKeyTenant(key,tenant); // Check the 0
                }
                if (CustomData != null && !string.IsNullOrEmpty(CustomData.PaymentRequestXML))
                {
                   
                    AddWhatsAppMessagingPhoneNumberToResponseHeader(tenant.Value);
                    TenantAdditionalDataRepository TADR = new TenantAdditionalDataRepository(tenant.Value);
                    var MyAdditionalData = TADR.GetSingleTenantAdditionalData(tenant.Value);
                    if (MyAdditionalData != null)
                    {
                        var MyPaymentData = LogitudeXmlSerializer.DeserializeObject<RequestPayment>(CustomData.PaymentRequestXML);
                        CustomData.RequestPaymentData = MyPaymentData;
                        //var MyPaymentData = LogitudeXmlSerializer.DeserializeObject<RequestPayment>(data.PaymentRequestXML);
                        var myId = Path.GetRandomFileName().Replace("&", "").Replace(".", "").Replace("=", "");
                        string entityReference = CustomData.ShipmentNumber + (!string.IsNullOrEmpty(MyPaymentData.ProformaInvoiceNumber) ? ("," + MyPaymentData.ProformaInvoiceNumber) : "");
                        //"sum=199.9&supplier=amitaltest&TranzilaPW=4Jwdsb&currency=1&op=1&DCdisable="
                        var MyConString = MyAdditionalData.PaymentGatewayConnectionString;
                        MyConString = MyConString.Replace("*sum*", MyPaymentData.TotalChargesInNIS);
                        MyConString = MyConString.Replace("*DCdisable*", entityReference);
                        MyConString = MyConString.Replace("*DclickTK*", myId);
                        string myParams = MyConString;// "sum=" + MyPaymentData.TotalChargesInNIS + "&supplier=amitaltest&TranzilaPW=4Jwdsb&currency=1&op=1&DCdisable=" + myId + "&DclickTK=" + myId;
                        Dictionary<string, string> dict = GetParamsAsDict(myParams);
                        string result = "";
                        var success = GetRequestToken(dict, out result, tenant.Value);
                        if (success)
                        {
                            CustomData.PaymentData = ForwardToPaymentLink(result, myParams);
                        }
                        else
                        {
                            CustomData.PaymentData = new PaymentData();
                            CustomData.PaymentData.currency = dict["currency"];
                            CustomData.PaymentData.sum = dict["sum"];
                            CustomData.PaymentData.op = dict["op"];
                            CustomData.PaymentData.DCdisable = dict["DCdisable"];
                            CustomData.PaymentData.DclickTK = dict["DclickTK"];
                            //CustomData.PaymentData.thtk = dict["thtk"];
                            CustomData.PaymentData.TargetEnv = dict["TargetEnv"];
                            CustomData.PaymentData.u71 = dict["u71"];

                        }
                        //CustomData.PaymentData.UseTestLink = FeatureToggleHelper.HasFeatureToggle("CTT", tenant);
                    }

                }



                return Request.CreateResponse(HttpStatusCode.OK, CustomData); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static void AddWhatsAppMessagingPhoneNumberToResponseHeader(int tenant)
        {
            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);
            if (tenantManagementPM != null && !string.IsNullOrEmpty(tenantManagementPM.WhatsAppMessagingPhoneNumber))
            {
                HttpContext.Current.Response.Headers.Add("WhatsAppMessagingPhoneNumber", tenantManagementPM.WhatsAppMessagingPhoneNumber);
                HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers", "WhatsAppMessagingPhoneNumber");
            }
        }

        public HttpResponseMessage GetTenantBySecurityKeyWithoutToken(string securityKey)
        {
            try
            {
                ShipmentQuery shipmentQuery = new ShipmentQuery(0);
                int? tenantNumber = shipmentQuery.GetTenantBySecurityKey(securityKey);

                return Request.CreateResponse(HttpStatusCode.OK, tenantNumber);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetLogoAndUrlWithoutToken(string securityKey)
        {
            int? tenant = new ShipmentQuery(0).GetTenantBySecurityKey(securityKey);
            if (tenant == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "tenant not found");

            var tenantManagement = new TenantManagementQuery().GetSinglePM(tenant.Value);
            byte[] filedata = new Uploader().DownloadFile("smalllogo" + tenant.Value, "jpg", "logos", tenant.Value);
            string logo = "data:image/jpg;base64," + Convert.ToBase64String(filedata);

            return Request.CreateResponse(new { url = tenantManagement.LogoURL, logo = logo, serviceAgreementURL = tenantManagement.ServiceAgreementURL });
        }

        private static void AddDataToResponseHeader(int tenant)
        {
            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);
            if (tenantManagementPM != null && !string.IsNullOrEmpty(tenantManagementPM.WhatsAppMessagingPhoneNumber))
            {
                HttpContext.Current.Response.Headers.Add("TranzilaPaymentWithBit", tenantManagementPM.TranzilaPaymentWithBit ? "1" : "0");
                HttpContext.Current.Response.Headers.Add("WhatsAppMessagingPhoneNumber", tenantManagementPM.WhatsAppMessagingPhoneNumber);
                HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers", "WhatsAppMessagingPhoneNumber, TranzilaPaymentWithBit");
            }
        }

        private PaymentData ForwardToPaymentLink(string thtk, string MyParams)
        {
            PaymentData MyPaymentData = new PaymentData();

            string postbackUrl = @"https://direct.tranzila.com/amitaltest/";

            Dictionary<string, string> dict = GetParamsAsDict(MyParams + "&" + thtk);
            MyPaymentData.currency = dict["currency"];
            MyPaymentData.sum = dict["sum"];
            MyPaymentData.op = dict["op"];
            MyPaymentData.u71 = dict["u71"];
            MyPaymentData.DCdisable = dict["DCdisable"];
            MyPaymentData.DclickTK = dict["DclickTK"];
            MyPaymentData.thtk = dict["thtk"];
            MyPaymentData.TargetEnv = dict["TargetEnv"];
            return MyPaymentData;

        }

        private Dictionary<string, string> GetParamsAsDict(string text)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            var list = text.Split('&');
            foreach (var item in list)
            {
                dict.Add(item.Split('=')[0], item.Split('=')[1]);
            }
            return (dict);
        }

        private static readonly HttpClient client = new HttpClient();

        private bool GetRequestToken(Dictionary<string, string> myDict, out string result, int tenant)
        {
            SetServicePointManagerSecurityProtocol(tenant);
            var content = new FormUrlEncodedContent(myDict);
            string Uri = GetSecureTranzilaURI(tenant);
            var response = client.PostAsync(Uri, content);
            var httpResponse = response.Result.Content.ReadAsStringAsync();// .Content.ReadAsStringAsync();
            result = httpResponse.Result;
            return (result.Contains("thtk") ? true : false);
        }

        private string GetSecureTranzilaURI(int tenant)
        {
            if (false) //FeatureToggleHelper.HasFeatureToggle("CTT", tenant)
            {
                return "https://secure2.tranzila.com/cgi-bin/tranzila71dt.cgi";
            }
            else
            {
                return "https://secure5.tranzila.com/cgi-bin/tranzila71dt.cgi";
            }
        }

        private static void SetServicePointManagerSecurityProtocol(int tenant)
        {
            if (true) //FeatureToggleHelper.HasFeatureToggle("OT2", tenant)
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }
            else
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            }
        }

        [HttpGet]
        public HttpResponseMessage RemoveShipmentTasks(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "UPDATE", tenant);

                IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);

                ShipmentComputedFieldsRepository shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(tenant);
                DocumentsFilingRepository DocRepo = new DocumentsFilingRepository(tenant);
                var entityComputedFields = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(id, tenant);
                var ShipmentDocs = DocRepo.GetRequestedDocumentsFilingPMsByEntityId(id, tenant);
                foreach (var Doc in ShipmentDocs)
                {
                    Doc.IsRequested = false;
                    Doc.IsDigitalSignRequired = false;
                    DocRepo.Update(Doc);
                    DocRepo.SubmitChanges();
                }
                entityComputedFields.IsDigitalSignRequired = false;
                entityComputedFields.IsRequestedDocuments = false;
                entityComputedFields.RequestedDocumentsCount = 0;
                entityComputedFields.IsDepositionRequired = false;
                ShipmentComputedFieldsHelper shipmentComputedFieldsHelper = new ShipmentComputedFieldsHelper();
                shipmentComputedFieldsHelper.UpdateShipmentComputedFields(entityComputedFields, shipmentComputedFieldsRepository.context);


                //shipmentComputedFieldsRepository.Update(entityComputedFields);
                //  shipmentComputedFieldsRepository.SubmitChanges();
                //ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                //ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(id, tenant);
                //shipmentPM.IsOperationalClosed = true;
                //ShipmentService service = new ShipmentService(objectContext, shipmentPM, SecurityUtility.GetAuthenticatedUser());
                //service.Update(true);



                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }




        }


        public HttpResponseMessage getSingleByShipmentNumber(string number)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM shipmentPM = shipmentQuery.GetSinglePMByShipmentNumber(number, tenant);
                string Id = "";
                if (shipmentPM != null)
                    Id = shipmentPM.Id;
                return Request.CreateResponse(HttpStatusCode.OK, Id); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetUserIdDetailsByShipmentSecurityKeyWithoutToken(string key, int? tenant = null)
        {
            try
            {
                ShipmentQuery shipmentQuery;
                ShipmentPM RequestedShipment;
                const string testKey = "d5e6d15f4cb24f12a8ac9c5e8c54a06d";
                if (key == testKey)
                {
                    shipmentQuery = new ShipmentQuery(0);
                    RequestedShipment = shipmentQuery.GetSingleShipmentPMBySecurityKeyTenantTest();
                }
                else
                {

               
                    shipmentQuery = new ShipmentQuery(tenant.Value);
                    RequestedShipment = shipmentQuery.GetSingleShipmentPMBySecurityKeyTenant(key, tenant.Value);
                }
                var MyData = LogitudeXmlSerializer.DeserializeObject<UserIdNumberRequestPM>(RequestedShipment.UserIdNumberXMLData);
                MyData.Id = RequestedShipment.Id;
                MyData.IsUserIDNumberRequired = RequestedShipment.IsUserIDNumberRequired;
                MyData.UserIdNumberUpdateDate = RequestedShipment.UserIdNumberUpdateDate;
                MyData.UserIdNumber = RequestedShipment.UserIdNumber;
                return Request.CreateResponse(HttpStatusCode.OK, MyData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage CheckIsCFSShipmentById(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                bool isBonded = shipmentQuery.CheckIsCFSShipmentById(shipmentId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, isBonded);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetShipmentPMByShipmentNumber(string number)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM shipmentPM = shipmentQuery.GetSinglePMByShipmentNumber(number, tenant, true);

                return Request.CreateResponse(HttpStatusCode.OK, shipmentPM); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetShipmentPMByShipmentNumberWithoutComposition(string number)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM shipmentPM = shipmentQuery.GetSinglePMByShipmentNumber(number, tenant, false);

                return Request.CreateResponse(HttpStatusCode.OK, shipmentPM); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetShipmentsAdditionalFields(string shipmentIds)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            var shipmentsAdditionalFields = shipmentQuery.GetShipmentsAdditionalFields(shipmentIds, tenant);

            return Request.CreateResponse(HttpStatusCode.OK, shipmentsAdditionalFields);
        }

        public HttpResponseMessage GetSingleShipmentsAdditionalFields(string shipmentId)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            var shipmentsAdditionalFields = shipmentQuery.GetSingleShipmentsAdditionalFields(shipmentId, tenant);

            return Request.CreateResponse(HttpStatusCode.OK, shipmentsAdditionalFields);
        }

        public HttpResponseMessage GetDigitalFiltersCounts(string CustomerId, int leastStatusWeight, int greatestStatusWeight)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            var digitalFiltersCounts = shipmentQuery.GetDigitalFiltersCounts(tenant, CustomerId, leastStatusWeight, greatestStatusWeight);

            return Request.CreateResponse(HttpStatusCode.OK, digitalFiltersCounts);
        }
    }
}