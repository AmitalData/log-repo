using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.CommonDataModel;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalShipmentController : ApiController
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
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                return Request.CreateResponse(HttpStatusCode.OK, shipmentPM); ;
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


                string shipmentLevelCodeValue = "";
                string partnerTypeName = "";
                var partnerTypeValue = filters.Filter2Value;
                var partnerTypeId = filters.Filter1Value;
                if (partnerTypeValue == "null" || partnerTypeValue == "undefined") partnerTypeValue = null;
                if (partnerTypeId == "null" || partnerTypeId == "undefined") partnerTypeId = null;

                if (partnerTypeValue == "CS")
                {
                    shipmentLevelCodeValue = "D,H,A"; 
                    partnerTypeName = "CustomerId";
                }
                else if (partnerTypeValue == "AG")
                {
                    shipmentLevelCodeValue = "D,C";
                    partnerTypeName = "AgentId";
                }

                if (!string.IsNullOrEmpty(partnerTypeId))
                    queryOperations.SetFilter(partnerTypeName, partnerTypeId, false, "Equals", null, false);
                if (!string.IsNullOrEmpty(shipmentLevelCodeValue))
                    queryOperations.SetFilter("ShipmentLevelCode", shipmentLevelCodeValue, false, "InList", null, false);

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
                string SearchFilterAsWhere = "";
                List<SqlParameter> parameters = new List<SqlParameter>();
                string loggedUserEmail = authToken.Email;
                string loggedContactId = null;
                if (isFullTextSearch)
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
                var entityLists = myShipmentQuery.GetIQueryableShipmentList(shipments, tenant);
                entityLists = genericFilter.GetFilteredQuery<ShipmentList>(listQueryOperation, entityLists);
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
                int count = 0;
                if (filters.GetCount && string.IsNullOrEmpty(SearchFilterAsWhere))
                {
                    response.Count = entityLists.Count();
                }
                entityLists = System.Data.Entity.QueryableExtensions.Skip(entityLists, () => skippedShipments);
                entityLists = System.Data.Entity.QueryableExtensions.Take(entityLists, () => queryOperations.PageSize);
                List<ShipmentList> listQuery;
                if (isFullTextSearch)
                {
                    TraceStringValues MySql;
                    MySql = IQueryableExtensions.ToTraceString<ShipmentList>(entityLists);
                    if (!string.IsNullOrEmpty(SearchFilterAsWhere) && MySql.TSQL.ToLower().Contains("where"))
                    {
                        var regex = new Regex(Regex.Escape("WHERE"), RegexOptions.IgnoreCase);
                        MySql.TSQL = regex.Replace(MySql.TSQL, "WHERE " + SearchFilterAsWhere + " AND ", 1);

                        var regex1 = new Regex(Regex.Escape("WHERE [Project1].[row_number] > "), RegexOptions.IgnoreCase);
                        MySql.TSQL = regex1.Replace(MySql.TSQL, "WHERE [Project1].[row_number] > 0 --", 1);
                    }

                    foreach (var item in MySql.TSQLParams)
                    {
                        parameters.Add(new SqlParameter(item.Name, item.Value));
                    }
                    IShipmentsContext context = ShipmentsContext.GetContext(tenant);
                    ShipmentsContext activeContext = context.GetActiveDbContext() as ShipmentsContext;
                    listQuery = activeContext.Database.SqlQuery<ShipmentList>(MySql.TSQL, parameters.ToArray()).ToList();
                }
                else
                {
                    listQuery = entityLists.ToList();
                }

                this.BuildShipmentListWithTimeLine(listQuery, tenant);
                response.Result = listQuery;
                if (filters.GetCount && !string.IsNullOrEmpty(SearchFilterAsWhere))
                {
                    response.Count = listQuery.Count();
                }
                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
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

        ICommonDataContext commonContext;
        AddressRepository addressRepository;
        IShipmentsContext shipmentContext;
        private void BuildShipmentListWithTimeLine(List<ShipmentList> entityLists, int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
            commonContext = CommonDataContext.GetContext(tenant);
            addressRepository = new AddressRepository(commonContext);
            foreach (var item in entityLists)
            {
                this.FillShipmnetTimeLine(item, tenant);
            }
        }

        private void FillShipmnetTimeLine(ShipmentList shipment, int tenant)
        {
            var shipmentPickUpDeliveries = (from a in shipmentContext.ShipmentPickUpDeliveries where a.ShipmentId == shipment.Id select a);
            TimeLineData timeLineData = new TimeLineData();
            timeLineData.Stops = new List<TimeLineStop>();
            this.FillMainCarraigeFromTimeLine(timeLineData, shipment);
            this.FillMainCarraigeToTimeLine(timeLineData, shipment);
            this.FillPickUpTimeLine(timeLineData, shipment, shipmentPickUpDeliveries);
            this.FillDeliveryTimeLine(timeLineData, shipment, shipmentPickUpDeliveries);
            shipment.TimeLineData = timeLineData;
        }

        private void FillMainCarraigeFromTimeLine(TimeLineData timeLineData, ShipmentList shipment)
        {

            var mainCarriageFrom = new TimeLineStop()
            {
                LegName = "MainCarriageFrom",
                City = shipment.MainCarriageFromCity,
                CountryCode = shipment.MainCarriageFromCountryCode,
                Date = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD,
                DateType = shipment.MainCarriageATD != null ? "Actual" : "Estimated",
            };
            timeLineData.Stops.Add(mainCarriageFrom);
        }
        private void FillMainCarraigeToTimeLine(TimeLineData timeLineData, ShipmentList shipment)
        {
            var mainCarriageTo = new TimeLineStop()
            {
                LegName = "MainCarriageTo",
                City = shipment.MainCarriageToCity,
                CountryCode = shipment.MainCarriageToCountryCode,
                Date = shipment.MainCarriageATA != null ? shipment.MainCarriageATA : shipment.MainCarriageETA,
                DateType = shipment.MainCarriageATA != null ? "Actual" : "Estimated",
            };

            timeLineData.Stops.Add(mainCarriageTo);
        }
        private void FillPickUpTimeLine(TimeLineData timeLineData, ShipmentList item, IQueryable<ShipmentPickUpDelivery> shipmentPickUpDeliveries)
        {
            var firstPickup = shipmentPickUpDeliveries?.Where(d => d.PickUpDeliveryTypeCode == "PICK").OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();
            if (firstPickup == null)
            {
                return;
            }
            int tenant = firstPickup.Tenant;
            var pickup = new TimeLineStop()
            {
                LegName = "Pickup",
                City = "",
                CountryCode = "",
                Date = firstPickup.ATD != null ? firstPickup.ATD : firstPickup.ETD,
                DateType = firstPickup.ATD != null ? "Actual" : "Estimated",
            };
            this.FillPickUpCityAndCountry(pickup, firstPickup, tenant);
            timeLineData.Stops.Add(pickup);
        }
        private void FillPickUpCityAndCountry(TimeLineStop timeLineData, ShipmentPickUpDelivery firstPickup, int tenant)
        {
            switch (firstPickup.PickUpDeliveryFromTypeCode)
            {
                case "PART":
                    {
                        if (!string.IsNullOrEmpty(firstPickup.FromPartnerCardId))
                        {
                            if (!string.IsNullOrEmpty(firstPickup.FromAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(firstPickup.FromAddressId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    timeLineData.City = myPartnerAddress.City;
                                    timeLineData.CountryCode = myPartnerAddress.Country?.Code;
                                }
                            }
                            else
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(firstPickup.FromPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    timeLineData.City = myPartnerAddress.City;
                                    timeLineData.CountryCode = myPartnerAddress.Country?.Code;
                                }
                            }
                        }
                        break;
                    }

                case "PORT":
                    {
                        if (!string.IsNullOrEmpty(firstPickup.FromPortId))
                        {
                            PortPM myPort = PortQuery.GetSinglePort(tenant, firstPickup.FromPortId, true);
                            if (myPort != null)
                            {
                                timeLineData.City = myPort.EnglishName;
                                timeLineData.CountryCode = myPort.CountryCode;
                            }
                        }
                        break;
                    }

                case "CASL":
                    {
                        timeLineData.City = firstPickup.FromAddressCity;
                        timeLineData.CountryCode = firstPickup.FromAddressCountry?.Code;
                        break;
                    }
            }
        }
        private void FillDeliveryTimeLine(TimeLineData timeLineData, ShipmentList item, IQueryable<ShipmentPickUpDelivery> shipmentPickUpDeliveries)
        {
            var finalDelivery = shipmentPickUpDeliveries?.Where(d => d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
            if (finalDelivery == null)
            {
                return;
            }
            int tenant = finalDelivery.Tenant;
            var delivery = new TimeLineStop()
            {
                LegName = "Delivery",
                City = "",
                CountryCode = "",
                Date = finalDelivery.ATD != null ? finalDelivery.ATD : finalDelivery.ETD,
                DateType = finalDelivery.ATD != null ? "Actual" : "Estimated",
            };
            this.FillDeliveryCityAndCountry(delivery, finalDelivery, tenant);
            timeLineData.Stops.Add(delivery);

        }
        private void FillDeliveryCityAndCountry(TimeLineStop timeLineData, ShipmentPickUpDelivery finalDelivery, int tenant)
        {
            switch (finalDelivery.PickUpDeliveryToTypeCode)
            {
                case "PART":
                    {
                        if (!string.IsNullOrEmpty(finalDelivery.ToPartnerCardId))
                        {
                            if (!string.IsNullOrEmpty(finalDelivery.ToAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(finalDelivery.ToAddressId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    timeLineData.City = myPartnerAddress.City;
                                    timeLineData.CountryCode = myPartnerAddress.Country?.Code;
                                }
                            }
                            else
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(finalDelivery.ToPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    timeLineData.City = myPartnerAddress.City;
                                    timeLineData.CountryCode = myPartnerAddress.Country?.Code;
                                }
                            }
                        }

                        break;
                    }

                case "PORT":
                    {
                        if (!string.IsNullOrEmpty(finalDelivery.ToPortId))
                        {
                            PortPM myPort = PortQuery.GetSinglePort(tenant, finalDelivery.ToPortId, true);
                            if (myPort != null)
                            {
                                timeLineData.City = myPort.EnglishName;
                                timeLineData.CountryCode = myPort.CountryCode;
                            }
                        }

                        break;
                    }

                case "CASL":
                    {
                        timeLineData.City = finalDelivery.ToAddressCity;
                        timeLineData.CountryCode = finalDelivery.ToAddressCountry?.Code;
                        break;
                    }
            }
        }

        [HttpGet]
        public HttpResponseMessage GetShipmentActiveStatuses()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                EntityStatusQuery entityStatusQuery = new EntityStatusQuery(tenant);
                var digitalPortalActiveStatuses = entityStatusQuery.GetDigitalPortalActiveStatuses(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, digitalPortalActiveStatuses);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public List<TraceEventPM> GetEntityEvents(string entityId, string objectTableName, string partnerType, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<TraceEventPM> result = new List<TraceEventPM>();

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            if (objectTable != null)
            {
                string objectTableId = objectTable.Id;

                TraceEventRepository traceEventsRepository = new TraceEventRepository(tenant);
                TraceEventQuery traceEventQuery = new TraceEventQuery(traceEventsRepository);

                List<TraceEventPM> data = traceEventQuery.GetTraceEventPMsByTenantByEntityId(tenant, entityId, objectTableId).ToList();

                if (partnerType == "AG")
                {
                    result = data.Where(d => d.IsAgentView).ToList();
                }

                else
                {
                    result = data.Where(d => d.IsCustomerView).ToList();
                }

            }

            return result.OrderByDescending(s => s.EventDateTime).ToList();
        }

    }
}

