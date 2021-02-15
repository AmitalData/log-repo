using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.Helpers;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class FollowUpsViewsController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetByFilters([FromUri] ApiQueryFilters filters)
        {
            //try
            //{
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

                        queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                    }
                    else
                    {
                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                    }
                }
            }

            ShipmentAPiHelper.AddFilters(queryOperations, tenant);

            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);

            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);
            GenericFilter genericFilter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            FollowUpsCustomFilter customfilters = new FollowUpsCustomFilter(tenant);
            IQueryable<ShipmentFollowUpDataView> shipments = shipmentRepository.GetShipmentFollowUpDataViewByTenant(tenant);
            shipments = customfilters.GetShipmentFollowUpFilteredQuery(queryOperations, shipments);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            shipments = genericFilter.GetFilteredQuery<ShipmentFollowUpDataView>(nonListQueryOperation, shipments);
            int skippedShipments = queryOperations.PageIndex;

            var entityLists = from f in shipments
                              select new ShipmentList()
                              {
                                  FNAReason = f.FNAReason,
                                  CarrierLastStatusDate = f.CarrierLastStatusDate,
                                  CarrierLastStatusName = f.CarrierLastStatusName,
                                  CarrierLastStatusCode = f.CarrierLastStatusCode,
                                  ShipmentViewId = f.Id + f.FollowUpId,
                                  Id = f.Id,
                                  Shipper = f.ShipperName,
                                  Consignee = f.ConsigneeName,
                                  DirectionId = f.DirectionId,
                                  DirectionName = f.DirectionName,
                                  TransportModeName = f.TransportModeName,
                                  MasterShipmentNumber = f.MasterShipmentNumber,
                                  House = f.House,
                                  CreateDateTime = f.CreateDateTime,
                                  ShipmentNumber = f.ShipmentNumber,
                                  ShipmentType = !string.IsNullOrEmpty(f.ShipmentTypeName) ? f.ShipmentTypeName + " " + f.ShipmentLevelName : f.ShipmentLevelName,
                                  TransportModeId = f.TransportModeId,
                                  Field1 = f.Field1,
                                  Field2 = f.Field2,
                                  Field3 = f.Field3,
                                  Field4 = f.Field4,
                                  Field5 = f.Field5,
                                  Field6 = f.Field6,
                                  Field7 = f.Field7,
                                  Field9 = f.Field9,
                                  Field8 = f.Field8,
                                  Field10 = f.Field10,
                                  Field11 = f.Field11,
                                  Field12 = f.Field12,
                                  Field13 = f.Field13,
                                  Field14 = f.Field14,
                                  Field15 = f.Field15,
                                  Field16 = f.Field16,
                                  Field17 = f.Field17,
                                  Field18 = f.Field18,
                                  Field19 = f.Field19,
                                  Field20 = f.Field20,
                                  Field21 = f.Field21,
                                  Field22 = f.Field22,
                                  Field23 = f.Field23,
                                  Field24 = f.Field24,
                                  Field25 = f.Field25,
                                  Field26 = f.Field26,
                                  Field27 = f.Field27,
                                  Field28 = f.Field28,
                                  Field29 = f.Field29,
                                  Field30 = f.Field30,
                                  Field31 = f.Field31,
                                  Field32 = f.Field32,
                                  Field33 = f.Field33,
                                  Field34 = f.Field34,
                                  Field35 = f.Field35,
                                  Field36 = f.Field36,
                                  Field37 = f.Field37,
                                  Field38 = f.Field38,
                                  Field39 = f.Field39,
                                  Field40 = f.Field40,
                                  ChargeableWeightInKG = f.ChargeableWeightInKG,
                                  ChargeableWeight = f.ChargeableWeight,
                                  GrossWeight = f.GrossWeight,
                                  ShipperReference1 = f.ShipperReference1/*, Master = f.Master*/,
                                  Master = f.Master,
                                  OpenReceivablesInLocalCurrency = f.OpenReceivablesInLocalCurrency,
                                  OpenReceivablesInProfitCurrency = f.OpenReceivablesInProfitCurrency,
                                  AccountedReceivablesInLocalCurrency = f.AccountedReceivablesInLocalCurrency,
                                  OpenPayablesInLocalCurrency = f.OpenPayablesInLocalCurrency,
                                  ShipmentPayableStatusCode = f.ShipmentPayableStatusCode,
                                  ShipmentReceivableStatusCode = f.ShipmentReceivableStatusCode,
                                  ShipmentPayableStatusName = f.ShipmentPayableStatusName,
                                  ShipmentReceivableStatusName = f.ShipmentReceivableStatusName,
                                  ProfitInLocalCurrency = f.ProfitInLocalCurrency,
                                  ProfitInProfitCurrency = f.ProfitInProfitCurrency,
                                  EstimateProfitInLocalCurrency = f.EstimateProfitInLocalCurrency,
                                  EstimateProfitInProfitCurrency = f.EstimateProfitInProfitCurrency,
                                  BranchId = f.BranchId,
                                  DepartmentId = f.DepartmentId,
                                  MainCarriageETA = f.MainCarriageETA,
                                  MainCarriageATD = f.MainCarriageATD,
                                  LocalCurrencyCode = currentTenant.CurrencyCode,
                                  NextETA = f.NextETA,
                                  NextETD = f.NextETD,
                                  NextLegName = f.NextLegName,
                                  Routing = f.Routing,//(f.DirectionId!="D"&&f.TransportModeId!="I")?((f.PreCarriageFromPortCode != null ? f.PreCarriageFromPortCode + " > " : "") + (f.MasterShipmentDataId != null ? (f.MainCarriageFromPortCode != null ? f.MainCarriageFromPortCode + " > " : "") + (f.MainCarriageFinalDestinationPortCode != null ? (f.OnCarriageToPortCode != null ? f.MainCarriageFinalDestinationPortCode + " > " + f.OnCarriageToPortCode : f.MainCarriageFinalDestinationPortCode) : "") : ((f.FromPortCode != null ? f.FromPortCode + " > " : "") + (f.ToPortCode != null ? (f.OnCarriageToPortCode != null ? f.ToPortCode + " > " + f.OnCarriageToPortCode : f.ToPortCode) : "")))):(""),
                                  FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                                  ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
                                  FromPort = !string.IsNullOrEmpty(f.MainCarriageFromPortCode) ? f.MainCarriageFromPortCode : f.FromPortCode,
                                  FromPortName = !string.IsNullOrEmpty(f.MainCarriageFromPortName) ? f.MainCarriageFromPortName : f.FromPortName,
                                  FromPortCountry = f.MainCarriageFromPortCountryName,

                                  // Column: To
                                  ToPort = (f.TransportModeId == "I" && f.DirectionId == "D") ? f.MainCarriageToCity : f.ToPortName,

                                  ToPortName = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortName) ? f.MainCarriageFinalDestinationPortName : f.ToPortName,
                                  ToPortCountry = f.MainCarriageToPortCountryName,
                                  MasterShipmentDataId = f.MasterShipmentDataId,
                                  BranchName = f.BranchName,
                                  CustomerName = f.CustomerName,
                                  GrossWeightInKG = f.GrossWeightInKG,
                                  ShipmentLevelCode = f.ShipmentLevelCode,
                                  ShipmentLevelName = f.ShipmentLevelName,
                                  VolumetricWeight = f.VolumetricWeight,
                                  AirlinePrefix = f.AirlinePrefix,
                                  AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                                  AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                                  AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                                  MainCarriageFromPortId = f.MainCarriageFromPortId,

                                  // Column: Origin
                                  MainCarriageFromPortName = (f.TransportModeId == "I" && f.DirectionId == "D") ? f.MainCarriageFromCity : f.MainCarriageFromPortName,

                                  MainCarriageATA = f.MainCarriageATA,
                                  MainCarriageETD = f.MainCarriageETD,
                                  IncotermId = f.IncotermId,
                                  OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                                  CustomerReference1 = f.CustomerReference1,
                                  CustomerReference2 = f.CustomerReference2,
                                  IssuingCarrierAgentId = f.IssuingCarrierAgentId,
                                  IncotermCode = f.IncotermCode,
                                  AsAgreedFreight = f.AsAgreedFreight,
                                  AsAgreedOtherCharges = f.AsAgreedOtherCharges,
                                  AccountNumber = f.AccountNumber,
                                  FollowUpDate = f.FollowUpDate,
                                  FollowUpId = f.FollowUpId,
                                  FollowUpNotes = f.FollowUpNotes,
                                  FollowUpOwner = f.FollowUpOwner,
                                  FollowUpOwnerId = f.FollowUpOwnerId,
                                  FollowUpType = f.FollowUpType,
                                  FollowUpTypeId = f.FollowUpTypeId,
                                  VolumeInCBM = f.VolumeInCBM,
                                  AgentId = f.AgentId,
                                  ARInvoiceIssued = f.ARInvoiceIssued,
                                  CreditNoteIssued = f.CreditNoteIssued,
                                  FreightForwarderId = f.FreightForwarderId,
                                  FreightForwarderName = f.FreightForwarderName,
                                  ProductCode = f.ProductCode,
                                  IsAccountingClosed = f.IsAccountingClosed,
                                  IsOperationalClosed = f.IsOperationalClosed,
                                  OperationalCloseDate = f.OperationalCloseDate,
                                  AccountingCloseDate = f.AccountingCloseDate,
                                  PackagesQuantity = f.PackagesQuantity,
                                  FHLStatusCode = f.FHLStatusCode,
                                  FHLStatusName = f.FHLStatusName,
                                  FHLStatusDate = f.FHLStatusDate,
                                  FWBStatusCode = f.FWBStatusCode,
                                  FWBStatusName = f.FWBStatusName,
                                  FWBStatusDate = f.FWBStatusDate,
                                  CargonautFHLStatusCode = f.CargonautFHLStatusCode,
                                  CargonautFHLStatusName = f.CargonautFHLStatusName,
                                  CargonautFHLStatusDate = f.CargonautFHLStatusDate,
                                  CargonautFWBStatusCode = f.CargonautFWBStatusCode,
                                  CargonautFWBStatusName = f.CargonautFWBStatusName,
                                  CargonautFWBStatusDate = f.CargonautFWBStatusDate,
                                  NumberOfInsidePackages = f.NumberOfInsidePackages,
                                  NumberOfInsidePackagesDetails = f.NumberOfInsidePackagesDetails,
                                  ConsolidatorId = f.ConsolidatorId,
                                  ConsolidatorName = f.ConsolidatorName,
                                  ConsolidatorNote = f.ConsolidatorNote,
                                  ConsolidatorAddressId = f.ConsolidatorAddressId,
                                  ConsolidatorContactId = f.ConsolidatorContactId,
                                  ConsolidatorReference = f.ConsolidatorReference,
                                  ManifestReason = f.ManifestReason,
                                  ManifestStatusCode = f.ManifestStatusCode,
                                  FromPortCountryCode = f.FromPortCountryCode,
                                  ToPortCountryCode = f.ToPortCountryCode,
                                  StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                                  StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                                  StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                                  StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                                  LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                                  CustomsDeclarationNumber = f.CustomsDeclarationNumber,
                                  OperationalDate = f.OperationalDate,
                                  CutoffDate = f.CutoffDate,
                                  NumberOfHouses = f.NumberOfHouses,
                                  ValueOfGoods = f.ValueOfGoods,
                                  ISFDate = f.ISFDate,
                                  ISFNumber = f.ISFNumber,
                                  ITDate = f.ITDate,
                                  ITNumber = f.ITNumber,
                                  FreightRelease = f.FreightRelease,
                                  TerminalAvailable = f.TerminalAvailable,
                                  OBLTypeCode = f.OBLTypeCode,
                                  DocumentsClosingDate = f.DocumentsClosingDate,
                                  ENSNumber = f.ENSNumber,
                                  ENSDate = f.ENSDate,
                                  RegistryDate = f.RegistryDate,
                                  INTTRASIStatusName = f.INTTRASIStatusName,
                                  INTTRALastStatusDate = f.INTTRALastStatusDate,
                                  Notify1Reference = f.Notify1Reference,
                                  Notify2Reference = f.Notify2Reference,
                                  ShipperNotExporterReference = f.ShipperNotExporterReference,
                                  ConsigneeNotImporterReference = f.ConsigneeNotImporterReference,
                                  MoveTypeName = f.MoveTypeName,
                                  ContainerLastStatusDate = f.ContainerLastStatusDate,
                                  BookingConfirmationNumber = f.BookingConfirmationNumber,
                                  DeclarationDate = f.DeclarationDate,
                                  DeclarationNumber = f.DeclarationNumber,
                                  ARInvoices = f.ARInvoices,
                                  Notes = f.Notes,
                                  WarehouseLegLastFreeDate = f.WarehouseLegLastFreeDate,
                                  WarehouseLegTerminalName = f.WarehouseLegTerminalName,
                                  LastFinalDestination = f.LastFinalDestination,
                                  EstimatedFinalArrivalDate = f.EstimatedFinalArrivalDate,
                                  NotInvoicedReceivablesAmount = f.NotInvoicedReceivablesAmount,
                                  ProjectNumber = f.ProjectNumber,
                                  MainCarriageVesselName = f.MainCarriageVesselName,
                              };

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
            if (filters.GetCount)
            {
                response.Count = entityLists.Count();
            }

            entityLists = entityLists.Skip(skippedShipments);
            entityLists = entityLists.Take(queryOperations.PageSize);

            List<ShipmentList> listQuery = entityLists.ToList();
            response.Result = listQuery;
            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("Shipment", tenant, listQuery.Cast<object>().ToList());
            HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            if (filters.GetCount)
            {
                reponseMessage.Headers.Add("TotalCount", count.ToString());

            }
            return reponseMessage;
            //}
            //catch (Exception ex)
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            //}

        }

        public HttpResponseMessage GetSingle(string Id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", authToken.Tenant);

                 
                ShipmentRepository shipmentRepository = new ShipmentRepository(authToken.Tenant);
                ShipmentQuery myShipmentQuery = new ShipmentQuery(shipmentRepository);

                ShipmentFollowUpDataView f = shipmentRepository.GetSingleShipmentFollowupDataView(Id, authToken.Tenant);

                ShipmentList myResult = myShipmentQuery.GetSingleShipmentListForFollowup(f, authToken.Tenant);

                ServiceResponse response = new ServiceResponse();
                response.Result = myResult;

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}