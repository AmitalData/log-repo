using ICSharpCode.SharpZipLib.BZip2;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.CustomFilters;
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
                ShipmentPM shipmentPM = shipmentQuery.GetSingleShipmentPMByForwarderNumber(fsn, tenant,0); // Check the 0

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

        public HttpResponseMessage GetByCustomerReference1(string CustomerReference1, bool IsForwarderShipment)
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
                ShipmentPM shipmentPM = shipmentQuery.GetByCustomerReference1(CustomerReference1, tenant, IsForwarderShipment);

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

        public HttpResponseMessage GetByCustomerReference1ForUpdate(string CustomerReference1,string ShipmentId, bool IsForwarderShipment)
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
                ShipmentPM shipmentPM = shipmentQuery.GetByCustomerReference1ForUpdate(CustomerReference1, ShipmentId, tenant, IsForwarderShipment);

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

        public HttpResponseMessage GetSingleBySecurityKeyWithoutToken(string key, int tenant)
        {
            try
            {
                //string logKey = PerformanceLogger.LogCurrentTime();

                //string token = HttpContext.Current.Request.Headers["Token"];
                //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //int tenant = authToken.Tenant;

                //SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentAdditionalCloudCustomData CustomData = shipmentQuery.GetSingleShipmentAdditionalCloudCustomData(key, tenant);//GetSingleShipmentPMBySecurityKeyTenant(key,tenant); // Check the 0
                if (CustomData != null && !string.IsNullOrEmpty(CustomData.PaymentRequestXML))
                {
                    TenantAdditionalDataRepository TADR = new TenantAdditionalDataRepository(tenant);
                    var MyAdditionalData = TADR.GetSingleTenantAdditionalData(tenant);
                    if (MyAdditionalData != null)
                    {
                        var MyPaymentData = LogitudeXmlSerializer.DeserializeObject<RequestPayment>(CustomData.PaymentRequestXML);
                        CustomData.RequestPaymentData = MyPaymentData;
                        //var MyPaymentData = LogitudeXmlSerializer.DeserializeObject<RequestPayment>(data.PaymentRequestXML);
                        var myId = Path.GetRandomFileName().Replace("&", "").Replace(".", "").Replace("=", "");
                        var ShipmentNumber = CustomData.ShipmentNumber;
                        //"sum=199.9&supplier=amitaltest&TranzilaPW=4Jwdsb&currency=1&op=1&DCdisable="
                        var MyConString = MyAdditionalData.PaymentGatewayConnectionString;
                        MyConString = MyConString.Replace("*sum*", MyPaymentData.TotalChargesInNIS);
                        MyConString = MyConString.Replace("*DCdisable*", ShipmentNumber);
                        MyConString = MyConString.Replace("*DclickTK*", myId);
                        string myParams = MyConString;// "sum=" + MyPaymentData.TotalChargesInNIS + "&supplier=amitaltest&TranzilaPW=4Jwdsb&currency=1&op=1&DCdisable=" + myId + "&DclickTK=" + myId;
                        Dictionary<string, string> dict = GetParamsAsDict(myParams);
                        string result = "";
                        var success = GetRequestToken(dict, out result);
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
                        }
                    }

                }
                 


                return Request.CreateResponse(HttpStatusCode.OK, CustomData); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
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

        private bool GetRequestToken(Dictionary<string, string> myDict, out string result)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var content = new FormUrlEncodedContent(myDict);
            var response = client.PostAsync("https://secure5.tranzila.com/cgi-bin/tranzila71dt.cgi", content);
            var httpResponse = response.Result.Content.ReadAsStringAsync();// .Content.ReadAsStringAsync();
            result = httpResponse.Result;
            return (result.Contains("thtk") ? true : false);
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
                shipmentComputedFieldsHelper.UpdateShipmentComputedFields(entityComputedFields);


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

       
    }
}