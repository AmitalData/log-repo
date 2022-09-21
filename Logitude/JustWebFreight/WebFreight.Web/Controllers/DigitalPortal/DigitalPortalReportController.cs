using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using System.Data.Entity;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalPortalReportController : ApiController
    {
        [HttpPost]
        [Route("DigitalPortalReport/ExportShipmentDataToExcelFile")]
        public IHttpActionResult ExportShipmentDataToExcelFile(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                var myTenantRepository = new TenantRepository(authToken.Tenant);
                var myTenant = myTenantRepository.GetSingleTenant(authToken.Tenant);

                var filters = new ApiQueryFilters()
                {
                    Filter1Value = newFilters.CardId,
                    Filter2Value = newFilters.CardType
                };

                var queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Shipment",
                    PageIndex = newFilters.PageIndex,
                    PageSize = newFilters.PageSize,
                    QuerySection = "Shipments",
                    SortByColumnName = newFilters.SortBy,
                    SortDirectin = newFilters.SortDirection,
                    QueryFilterItems = new List<QueryFilterItem>(),
                };

                string partnerTypeName = string.Empty;
                string shipmentLevelCodeValue = string.Empty;

                if (newFilters.CardType == "CS")
                {
                    shipmentLevelCodeValue = "D,H,A";
                    partnerTypeName = "CustomerId";
                }
                else if (newFilters.CardType == "AG")
                {
                    shipmentLevelCodeValue = "D,C";
                    partnerTypeName = "AgentId";
                }

                if (!string.IsNullOrEmpty(newFilters.CardId))
                {
                    queryOperations.SetFilter(partnerTypeName, newFilters.CardId, false, "Equals", null, false);
                }

                if (!string.IsNullOrEmpty(shipmentLevelCodeValue))
                {
                    queryOperations.SetFilter("ShipmentLevelCode", shipmentLevelCodeValue, false, "InListExact", null, false);
                }

                var ShipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", authToken.Tenant);

                foreach (var filter in newFilters.AdditionalFilters)
                {
                    var field = ShipmentObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);

                    if (field != null)
                    {
                        string valuestring1 = filter.FieldValue?.ToString();
                        object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
                        string valuestring2 = filter.FieldValue2?.ToString();
                        object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
                        queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                    }
                    else
                    {
                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                    }
                }

                ShipmentAPiHelper.AddFilters(queryOperations, authToken.Tenant);
                var shipmentRepository = new ShipmentRepository(authToken.Tenant);

                var customfilters = new ShipmentCustomFilter(authToken.Tenant);

                IQueryable<DigitalShipmentsDataView> shipments = shipmentRepository.GetDigitalShipmentViewsByTenant(authToken.Tenant);

                shipments = DigitalPortalCustomFilter.GetDigtalFilteredQuery(queryOperations, shipments, shipmentRepository, authToken.Tenant);

                var nonListQueryOperation = new QueryOperations
                {
                    QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList()
                };

                var listQueryOperation = new QueryOperations
                {
                    QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList()
                };

                var genericFilter = new GenericFilter();

                shipments = genericFilter.GetFilteredQuery(nonListQueryOperation, shipments);

                var myShipmentQuery = new ShipmentQuery(shipmentRepository);
                var entityLists = myShipmentQuery.GetDigitalIQueryableShipmentList(shipments, authToken.Tenant);
                entityLists = genericFilter.GetFilteredQuery(listQueryOperation, entityLists);

                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    var propInfo = typeof(DigitalShipmentList).GetProperty(queryOperations.SortByColumnName);
                    var shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", authToken.Tenant).ToList();

                    var objectField = shipmentObjectFields.FirstOrDefault(a => a.FieldName == queryOperations.SortByColumnName);

                    if (objectField != null)
                    {
                        var sortClass = new GenericSort();

                        if (!objectField.IsCustom)
                        {
                            switch (objectField.DataTypeCode.ToLower())
                            {
                                case "text":
                                    {
                                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "double":
                                    {
                                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, double>(queryOperations, entityLists);
                                        break;
                                    }
                                case "datetime":
                                    {
                                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, DateTime>(queryOperations, entityLists);
                                        break;
                                    }
                                case "integer":
                                    {
                                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, int>(queryOperations, entityLists);
                                        break;
                                    }
                                case "lookup":
                                    {
                                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "boolean":
                                    {
                                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, bool>(queryOperations, entityLists);
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
                            entityLists = sortClass.GetSorterQuery<DigitalShipmentList, string>(queryOperations, entityLists);
                        }
                    }
                }
                else
                {
                    entityLists = entityLists.OrderByDescending(d => d.CreateDateTime);
                }

                List<DigitalShipmentList> listQuery = entityLists.ToList();
                ExportToExcelArgs excelArgs = BuildExportToExcelArguments(listQuery, newFilters.QueryCode, authToken.Tenant);
                var excelFile = new ExportToExcelHelper().ExportDataToExcel(excelArgs);
                BlobFileInfo fileInfo = SaveFileToStorage(newFilters, authToken.Tenant, excelFile);
                return Ok(fileInfo?.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        private ExportToExcelArgs BuildExportToExcelArguments(List<DigitalShipmentList> data, string queryCode, int tenant)
        {
            var exportToExcelArgs = new ExportToExcelArgs();
            exportToExcelArgs.Data = data.GetEnumerator();
            exportToExcelArgs.QueryColumns = GetQueryColumnsByQueryCode(queryCode, tenant);
            exportToExcelArgs.Tenant = tenant;
            exportToExcelArgs.QueryPM = new QueryPM()
            {
                ObjectTableName = "Shipment",
                Tenant = tenant,
                DisplayText = "Shipments Report",
            };
            return exportToExcelArgs;
        }

        private List<QueryColumnPM> GetQueryColumnsByQueryCode(string queryCode, int tenant)
        {
            var tenantZero = 0;
            QueryColumnQuery queryColumnQuery = new QueryColumnQuery(tenant);
            return queryColumnQuery.GetQueryColumnsByQueryCode(tenantZero, queryCode).OrderBy(q => q.IndexOrder).ToList();
        }

        private BlobFileInfo SaveFileToStorage(GeneralFilters newFilters, int tenant, byte[] excelFile)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = "Shipments-" + DateTime.Now.ToShortDateString(),
                FolderName = "others",
                Extension = "xls",
                Tenant = tenant,
                FileSize = excelFile.Length,
            };
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(excelFile, fileInfo);
            return fileInfo;
        }
    }

}