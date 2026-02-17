using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.ShipmentsModel.EntityLists;
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
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShipmentsModel.Generated.ListControllers
{
    public partial class ContainerFollowUpViewsController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("ContainerFollowUp", "READ", authToken.Tenant);

                IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);

                string myPackageId = id.Split(':')[0];

                ContainerFollowUpList entityList = (from f in myContext.ShipmentPackages.Include("PackageType").Include("DeliveryTransportMode").Include("ECRTransportMode")
                                                    join db_Shipments in myContext.Shipments.Include("Direction").Include("TransportMode").Include("ShipmentLevel").Include("ShipmentType").Include("ShipperCard").Include("ConsigneeCard").Include("CustomerCard").Include("CustomerCard.PrimaryContact").Include("ShipmentMasterData").Include("ShipmentMasterData.MainCarriageCarrierCard").Include("ShipmentMasterData.MainCarriageVessel")
                                                    on f.ShipmentId equals db_Shipments.Id into PackagesShipments
                                                    from myShipment in PackagesShipments
                                                    where f.Tenant == tenant && myShipment.Tenant == tenant
                                                    && f.Id == myPackageId
                                                    select new ContainerFollowUpList()
                                                    {
                                                        Id = f.Id + ":" + f.ShipmentId,
                                                        Tenant = f.Tenant,
                                                        ShipmentId = f.ShipmentId,
                                                        ShipperSeal = f.ShipperSeal,
                                                        Volume = f.Volume,
                                                        ContainerNumber = f.ContainerNumber,
                                                        IsDangerous = f.IsDangerous,
                                                        Description = f.Description,
                                                        MarksAndNumbers = f.MarksAndNumbers,
                                                        ContainerTypeName = f.PackageType == null ? null : f.PackageType.EnglishName,
                                                        IsDeliveryFU = f.IsDeliveryFU,
                                                        DeliveryId = f.DeliveryId,
                                                        DeliveryETD = f.DeliveryETD,
                                                        DeliveryATD = f.DeliveryATD,
                                                        DeliveryATA = f.DeliveryATA,
                                                        DeliveryETA = f.DeliveryETA,
                                                        DeliveryFrom = f.DeliveryFrom,
                                                        DeliveryTo = f.DeliveryTo,
                                                        DeliveryDeparture = f.DeliveryATD != null ? f.DeliveryATD : f.DeliveryETD,
                                                        DeliveryArrival = f.DeliveryATA != null ? f.DeliveryATA : f.DeliveryETA,
                                                        IsEmptyContainerReturnFU = f.IsEmptyContainerReturnFU,
                                                        EmptyContainerReturnId = f.EmptyContainerReturnId,
                                                        EmptyContainerReturnETD = f.EmptyContainerReturnETD,
                                                        EmptyContainerReturnATD = f.EmptyContainerReturnATD,
                                                        EmptyContainerReturnETA = f.EmptyContainerReturnETA,
                                                        EmptyContainerReturnATA = f.EmptyContainerReturnATA,
                                                        EmptyContainerReturnFrom = f.EmptyContainerReturnFrom,
                                                        EmptyContainerReturnTo = f.EmptyContainerReturnTo,
                                                        ReturnDeparture = f.EmptyContainerReturnATD != null ? f.EmptyContainerReturnATD : f.EmptyContainerReturnETD,
                                                        ReturnArrival = f.EmptyContainerReturnATA != null ? f.EmptyContainerReturnATA : f.EmptyContainerReturnETA,

                                                        DeliveryTransportModeCode = f.DeliveryTransportModeCode,
                                                        DeliveryTransportModeName = f.DeliveryTransportMode == null ? null : f.DeliveryTransportMode.Name,

                                                        ECRTransportModeCode = f.ECRTransportModeCode,
                                                        ECRTransportModeName = f.ECRTransportMode == null ? null : f.ECRTransportMode.Name,

                                                        DirectionId = myShipment.DirectionId,
                                                        TransportModeId = myShipment.TransportModeId,
                                                        ShipmentTypeId = myShipment.ShipmentTypeId,
                                                        IsCancelled = myShipment.IsCancelled,
                                                        ShipmentNumber = myShipment.ShipmentNumber,
                                                        House = myShipment.House,
                                                        ShipmentLevelCode = myShipment.ShipmentLevelCode,
                                                        StatusId = myShipment.StatusId,
                                                        ConsigneeReference = (myShipment.ConsigneeReference1 == null || myShipment.ConsigneeReference1 == "") ? myShipment.ConsigneeReference2 : ((myShipment.ConsigneeReference2 == null || myShipment.ConsigneeReference2 == "") ? myShipment.ConsigneeReference1 : myShipment.ConsigneeReference1 + "," + myShipment.ConsigneeReference2),
                                                        DirectionName = myShipment.Direction == null ? null : myShipment.Direction.Name,
                                                        TransportModeName = myShipment.TransportMode == null ? null : myShipment.TransportMode.Name,
                                                        ShipmentLevelName = myShipment.ShipmentLevel == null ? null : myShipment.ShipmentLevel.Name,
                                                        ShipmentType = myShipment.ShipmentType == null ? null : myShipment.ShipmentType.Name,
                                                        ShipperName = myShipment.ShipperCard == null ? null : myShipment.ShipperCard.EnglishName,
                                                        ConsigneeName = myShipment.ConsigneeCard == null ? null : myShipment.ConsigneeCard.EnglishName,
                                                        CustomerName = myShipment.CustomerCard == null ? null : myShipment.CustomerCard.EnglishName,
                                                        LongMaster = myShipment.ShipmentMasterData == null ? null : (myShipment.TransportModeId == "A" ? (!string.IsNullOrEmpty(myShipment.ShipmentMasterData.AirlinePrefix) && !string.IsNullOrEmpty(myShipment.ShipmentMasterData.Master) ? myShipment.ShipmentMasterData.AirlinePrefix + "-" + myShipment.ShipmentMasterData.Master : "") : myShipment.ShipmentMasterData.Master),
                                                        CarrierName = myShipment.ShipmentMasterData == null ? null : (myShipment.ShipmentMasterData.MainCarriageCarrierCard == null ? null : myShipment.ShipmentMasterData.MainCarriageCarrierCard.EnglishName),
                                                        CustomerContactName = myShipment.CustomerCard == null ? null : (myShipment.CustomerCard.PrimaryContact == null ? null : myShipment.CustomerCard.PrimaryContact.EnglishName),

                                                        ShipperId = myShipment.ShipperId,
                                                        ConsigneeId = myShipment.ConsigneeId,
                                                        CustomerId = myShipment.CustomerId,
                                                        CarrierId = myShipment.ShipmentMasterData == null ? null : myShipment.ShipmentMasterData.MainCarriageCarrierId,
                                                        SearchFields = myShipment.SearchFields,

                                                        ShipmentNotes = myShipment.Notes,
                                                        VesselName = myShipment.ShipmentMasterData == null ? null : (myShipment.ShipmentMasterData.MainCarriageVessel == null ? null : myShipment.ShipmentMasterData.MainCarriageVessel.EnglishName),

                                                    }).FirstOrDefault();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, entityList);
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
                SecurityUtility.CheckContactFeature("ContainerFollowUp", "READ", authToken.Tenant);

                int tenant = authToken.Tenant;

                if (filters.Tenant != null)
                {
                    tenant = tenant;
                }

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "ContainerFollowUp",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "ContainerFollowUps",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> QuoteObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ContainerFollowUp", tenant);
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

                        ObjectField field = QuoteObjectFields.FirstOrDefault(f => f.FieldName == filterName);
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
                        ObjectField field = QuoteObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {
                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            bool isDisplayInList = field.DisplayInList;
                            switch (field.FieldName)
                            {
                                case "ShipperId":
                                case "ConsigneeId":
                                case "CustomerId":
                                case "CarrierId":
                                    {
                                        isDisplayInList = true;
                                        break;
                                    }
                            }

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, isDisplayInList);
                        }

                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }

                GenericFilter genericFilter = new GenericFilter();
                GenericSort sortClass = new GenericSort();

                IShipmentsContext MyContext = ShipmentsContext.GetContext(tenant);
                ShipmentPackageRepository entityRepository = new ShipmentPackageRepository(MyContext);
                IQueryable<ShipmentPackage> entityPocos = entityRepository.GetShipmentPackages(tenant);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                entityPocos = genericFilter.GetFilteredQuery<ShipmentPackage>(nonListQueryOperation, entityPocos);
                int skippedEntities = queryOperations.PageIndex;

                IQueryable<ContainerFollowUpList> entityLists = (from f in entityPocos.Include("PackageType").Include("DeliveryTransportMode").Include("ECRTransportMode")
                                                                 join db_Shipments in MyContext.Shipments.Include("Direction").Include("TransportMode").Include("ShipmentLevel").Include("ShipmentType").Include("ShipperCard").Include("ConsigneeCard").Include("CustomerCard").Include("CustomerCard.PrimaryContact").Include("ShipmentMasterData").Include("ShipmentMasterData.MainCarriageCarrierCard").Include("ShipmentMasterData.MainCarriageVessel")
                                                                 on f.ShipmentId equals db_Shipments.Id into PackagesShipments
                                                                 from myShipment in PackagesShipments
                                                                 where f.Tenant == tenant && myShipment.Tenant == tenant
                                                                 && myShipment.IsCancelled == false
                                                                 select new ContainerFollowUpList()
                                                                 {
                                                                     Id = f.Id + ":" + f.ShipmentId,
                                                                     Tenant = f.Tenant,
                                                                     ShipmentId = f.ShipmentId,
                                                                     ShipperSeal = f.ShipperSeal,
                                                                     Volume = f.Volume,
                                                                     ContainerNumber = f.ContainerNumber,
                                                                     IsDangerous = f.IsDangerous,
                                                                     Description = f.Description,
                                                                     MarksAndNumbers = f.MarksAndNumbers,
                                                                     ContainerTypeName = f.PackageType == null ? null : f.PackageType.EnglishName,
                                                                     IsDeliveryFU = f.IsDeliveryFU,
                                                                     DeliveryId = f.DeliveryId,
                                                                     DeliveryETD = f.DeliveryETD,
                                                                     DeliveryATD = f.DeliveryATD,
                                                                     DeliveryATA = f.DeliveryATA,
                                                                     DeliveryETA = f.DeliveryETA,
                                                                     DeliveryFrom = f.DeliveryFrom,
                                                                     DeliveryTo = f.DeliveryTo,
                                                                     DeliveryDeparture = f.DeliveryATD != null ? f.DeliveryATD : f.DeliveryETD,
                                                                     DeliveryArrival = f.DeliveryATA != null ? f.DeliveryATA : f.DeliveryETA,
                                                                     IsEmptyContainerReturnFU = f.IsEmptyContainerReturnFU,
                                                                     EmptyContainerReturnId = f.EmptyContainerReturnId,
                                                                     EmptyContainerReturnETD = f.EmptyContainerReturnETD,
                                                                     EmptyContainerReturnATD = f.EmptyContainerReturnATD,
                                                                     EmptyContainerReturnETA = f.EmptyContainerReturnETA,
                                                                     EmptyContainerReturnATA = f.EmptyContainerReturnATA,
                                                                     EmptyContainerReturnFrom = f.EmptyContainerReturnFrom,
                                                                     EmptyContainerReturnTo = f.EmptyContainerReturnTo,
                                                                     ReturnDeparture = f.EmptyContainerReturnATD != null ? f.EmptyContainerReturnATD : f.EmptyContainerReturnETD,
                                                                     ReturnArrival = f.EmptyContainerReturnATA != null ? f.EmptyContainerReturnATA : f.EmptyContainerReturnETA,

                                                                     DeliveryTransportModeCode = f.DeliveryTransportModeCode,
                                                                     DeliveryTransportModeName = f.DeliveryTransportMode == null ? null : f.DeliveryTransportMode.Name,

                                                                     ECRTransportModeCode = f.ECRTransportModeCode,
                                                                     ECRTransportModeName = f.ECRTransportMode == null ? null : f.ECRTransportMode.Name,
                                                                     
                                                                     DirectionId = myShipment.DirectionId,
                                                                     TransportModeId = myShipment.TransportModeId,
                                                                     ShipmentTypeId = myShipment.ShipmentTypeId,
                                                                     IsCancelled = myShipment.IsCancelled,
                                                                     ShipmentNumber = myShipment.ShipmentNumber,
                                                                     House = myShipment.House,
                                                                     ShipmentLevelCode = myShipment.ShipmentLevelCode,
                                                                     StatusId = myShipment.StatusId,
                                                                     ConsigneeReference = (myShipment.ConsigneeReference1 == null || myShipment.ConsigneeReference1 == "") ? myShipment.ConsigneeReference2 : ((myShipment.ConsigneeReference2 == null || myShipment.ConsigneeReference2 == "") ? myShipment.ConsigneeReference1 : myShipment.ConsigneeReference1 + "," + myShipment.ConsigneeReference2),
                                                                     DirectionName = myShipment.Direction == null ? null : myShipment.Direction.Name,
                                                                     TransportModeName = myShipment.TransportMode == null ? null : myShipment.TransportMode.Name,
                                                                     ShipmentLevelName = myShipment.ShipmentLevel == null ? null : myShipment.ShipmentLevel.Name,
                                                                     ShipmentType = myShipment.ShipmentType == null ? null : myShipment.ShipmentType.Name,
                                                                     ShipperName = myShipment.ShipperCard == null ? null : myShipment.ShipperCard.EnglishName,
                                                                     ConsigneeName = myShipment.ConsigneeCard == null ? null : myShipment.ConsigneeCard.EnglishName,
                                                                     CustomerName = myShipment.CustomerCard == null ? null : myShipment.CustomerCard.EnglishName,
                                                                     LongMaster = myShipment.ShipmentMasterData == null ? null : (myShipment.TransportModeId == "A" ? (!string.IsNullOrEmpty(myShipment.ShipmentMasterData.AirlinePrefix) && !string.IsNullOrEmpty(myShipment.ShipmentMasterData.Master) ? myShipment.ShipmentMasterData.AirlinePrefix + "-" + myShipment.ShipmentMasterData.Master : "") : myShipment.ShipmentMasterData.Master),
                                                                     CarrierName = myShipment.ShipmentMasterData == null ? null : (myShipment.ShipmentMasterData.MainCarriageCarrierCard == null ? null : myShipment.ShipmentMasterData.MainCarriageCarrierCard.EnglishName),
                                                                     CustomerContactName = myShipment.CustomerCard == null ? null : (myShipment.CustomerCard.PrimaryContact == null ? null : myShipment.CustomerCard.PrimaryContact.EnglishName),

                                                                     ShipperId = myShipment.ShipperId,
                                                                     ConsigneeId = myShipment.ConsigneeId,
                                                                     CustomerId = myShipment.CustomerId,
                                                                     CarrierId = myShipment.ShipmentMasterData == null ? null : myShipment.ShipmentMasterData.MainCarriageCarrierId,
                                                                     SearchFields = myShipment.SearchFields,

                                                                     ShipmentNotes = myShipment.Notes,
                                                                     VesselName = myShipment.ShipmentMasterData == null ? null : (myShipment.ShipmentMasterData.MainCarriageVessel == null ? null : myShipment.ShipmentMasterData.MainCarriageVessel.EnglishName),
                                                                 });


                ContainerFollowUpCustomFilter customfilters = new ContainerFollowUpCustomFilter(tenant);
                entityLists = customfilters.GetFilteredQuery(queryOperations, entityLists);

                entityLists = genericFilter.GetFilteredQuery<ContainerFollowUpList>(listQueryOperation, entityLists);

                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    PropertyInfo propInfo = typeof(ContainerFollowUpList).GetProperty(queryOperations.SortByColumnName);


                    ObjectField objectField = (from a in QuoteObjectFields
                                               where a.FieldName == queryOperations.SortByColumnName
                                               select a).FirstOrDefault();

                    if (objectField != null)
                    {
                        if (objectField.IsCustom)
                        {
                            entityLists = sortClass.GetSorterQuery<ContainerFollowUpList, string>(queryOperations, entityLists);
                        }

                        else
                        {
                            switch (objectField.DataTypeCode.ToLower())
                            {
                                case "ntext":
                                case "text":
                                case "lookup":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ContainerFollowUpList, string>(queryOperations, entityLists);
                                        break;
                                    }

                                case "sigdouble":
                                case "double":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ContainerFollowUpList, double>(queryOperations, entityLists);
                                        break;
                                    }
                                case "date":
                                case "datetime":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ContainerFollowUpList, DateTime>(queryOperations, entityLists);
                                        break;
                                    }
                                case "unsinteger":
                                case "integer":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ContainerFollowUpList, int>(queryOperations, entityLists);
                                        break;
                                    }
                                case "boolean":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ContainerFollowUpList, bool>(queryOperations, entityLists);
                                        break;
                                    }
                                case "unsdecimal":
                                case "decimal":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ContainerFollowUpList, decimal>(queryOperations, entityLists);
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

                List<ContainerFollowUpList> listResult = entityLists.ToList();

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