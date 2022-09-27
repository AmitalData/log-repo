using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Helpers
{
    public class DigitalPortalQueryToExcelExportService
    {
        public DigitalExportResult ExportQueryDataToExcel(GeneralFilters queryFilters)
        {
            return AddQueryExportExecutionLog(queryFilters);
        }

        private DigitalExportResult AddQueryExportExecutionLog(GeneralFilters queryFilters)
        {
            int tenant = (int)queryFilters.Tenant;
            var reportExecutionLogRepository = new QueryExportExecutionLogRepository(tenant);
            var logId = IdCounter.GetNumber("QueryExportExecutionLog", tenant);
            var loggedEmail = SecurityUtility.GetAuthenticatedUser();

            var executionLog = new QueryExportExecutionLog
            {
                Id = logId,
                CreateDate = DateTime.Now,
                CreatedByUserId = SecurityUtility.GetLoggedUserId(loggedEmail, tenant),
                QueryFilterXML = LogitudeXmlSerializer.SerializeObjectToXmlString(queryFilters),
                Tenant = tenant,
                StatusCode = "W"
            };

            reportExecutionLogRepository.Add(executionLog);
            reportExecutionLogRepository.SubmitChanges();

            string ObjectTableName = queryFilters.ObjectTableName.Replace("Customs.", "");
            string fileName = GetOutpuFileName(ObjectTableName);


            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("DigitalPortalQueryExportExecutionQueue", executionLog.Tenant);

            queueservice.Send(new Dictionary<string, string>() {
                    { "LogId", executionLog.Id },
                    { "Tenant", executionLog.Tenant.ToString() },
                    { "FileName", fileName },
                    { "LoggedUserEmail", loggedEmail },
                }, tenant, null, null, null, null);

            return new DigitalExportResult() { ExecutionLogId = logId, FileName = fileName, IsWorkerRole = true };
        }

        private static string GetOutpuFileName(string ObjectTableName)
        {
            return ObjectTableName + "_" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
        }

        private byte[] DigitalPortalShipmentExportToExcel(List<DigitalShipmentList> digitalShipmentLists)
        {
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            ExcelEngine excelEngine = new ExcelEngine();
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet1 = workbook.Worksheets[0];

            // Build excel headers 
            var table = new DataTable();
            table.Columns.Add("Shipment Number");
            table.Columns.Add("Transport Mode Name");
            table.Columns.Add("Direction Name");
            table.Columns.Add("Org");
            table.Columns.Add("Dest");
            table.Columns.Add("ATD");
            table.Columns.Add("ATA");
            table.Columns.Add("Master Number");
            table.Columns.Add("Shipper");
            table.Columns.Add("Consignee");
            table.Columns.Add("Shipment Type");
            table.Columns.Add("Status");
            table.Columns.Add("Nof Package");
            table.Columns.Add("G. Weight");
            table.Columns.Add("CH. Weight");
            table.Columns.Add("Incoterm");
            table.Columns.Add("Volume");
            table.Columns.Add("Goods Description");
            table.Columns.Add("Goods Value");

            foreach (var item in digitalShipmentLists)
            {
                DataRow row = table.NewRow();
                row[0] = item.ShipmentNumber;
                row[1] = item.TransportModeName;
                row[2] = item.DirectionName;
                row[4] = item.MainCarriageFromPortName;
                row[5] = item.MainCarriageToPortName;
                row[6] = item.MainCarriageATD.HasValue ? item.MainCarriageATD : null;
                row[7] = item.MainCarriageATA.HasValue ? item.MainCarriageATA : null;
                row[8] = item.Master;
                row[9] = item.ShipperName;
                row[10] = item.ConsigneeName;
                row[11] = item.ShipmentTypeName;
                row[12] = item.StatusName;
                row[13] = item.NumberOfPackages.HasValue ? item.NumberOfPackages : 0;
                row[14] = item.ChargeableWeight.HasValue ? item.NumberOfPackages : 0;
                row[15] = item.IncotermCode ;
                row[16] = item.Volume.HasValue ? item.Volume : 0;
                row[17] = item.DescriptionOfGoods;
                row[18] = item.ValueOfGoods.HasValue ? item.ValueOfGoods : 0;
                table.Rows.Add(row);
            }

            sheet1.ImportDataTable(table, true, 1, 1);
            workbook.Version = ExcelVersion.Excel2007;
            workbook.SaveAs(memory);
            return memory.ToArray();
        }

        private List<DigitalShipmentList> GetByShipmentsByFilter(GeneralFilters newFilters, int tenant)
        {
            var myTenantRepository = new TenantRepository(tenant);
            var myTenant = myTenantRepository.GetSingleTenant(tenant);

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

            var ShipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableNameWithNoIncludes("Shipment", tenant);

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

            ShipmentAPiHelper.AddFilters(queryOperations, tenant);
            var shipmentRepository = new ShipmentRepository(tenant);

            var customfilters = new ShipmentCustomFilter(tenant);

            IQueryable<DigitalShipmentsDataView> shipments = shipmentRepository.GetDigitalShipmentViewsByTenant(tenant);

            shipments = DigitalPortalCustomFilter.GetDigtalFilteredQuery(queryOperations, shipments, shipmentRepository, tenant);

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
            var entityLists = myShipmentQuery.GetDigitalIQueryableShipmentList(shipments, tenant);
            entityLists = genericFilter.GetFilteredQuery(listQueryOperation, entityLists);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                var propInfo = typeof(DigitalShipmentList).GetProperty(queryOperations.SortByColumnName);
                var shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant).ToList();

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

            return entityLists?.ToList();
        }

        public DigitalExportResult ExportQueryDataToStorage(DigitalExportQueryToExcelArgs args)
        {
            SecurityUtility.IsWorkerRoleCall = Logitude.BL.Security.SecurityUtility.IsWorkerRoleCall = args.IsWorkerRoleCall;
            var res = GetFileInfo(args);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(res.Data, res.BlobFileInfo);
            return new DigitalExportResult() { FileName = res.BlobFileInfo.FileName };
        }

        private DigitalXslExportResult GetFileInfo(DigitalExportQueryToExcelArgs args)
        {
            var queryFilters = args.QueryFilters;
            int tenant = (int)queryFilters.Tenant;
            string ObjectTableName = queryFilters.ObjectTableName.Replace("Customs.", ""); ;

            string fileName = !string.IsNullOrEmpty(args.OutputFileName)
                              ? args.OutputFileName
                              : GetOutpuFileName(ObjectTableName);

            byte[] data = null;

            switch (ObjectTableName)
            {
                case "DigitalShipmentsView":
                    var res = GetByShipmentsByFilter(args.QueryFilters, args.QueryFilters.Tenant.Value);
                    data = DigitalPortalShipmentExportToExcel(res);
                    break;
                default:
                    break;
            };

            var fileInfo = new BlobFileInfo
            {
                FileName = fileName,
                FolderName = "others",
                Extension = "xlsx",
                Tenant = tenant,
                FileSize = data.Length
            };

            return new DigitalXslExportResult
            {
                BlobFileInfo = fileInfo,
                Data = data
            };
        }
    }

    public class DigitalXslExportResult
    {
        public BlobFileInfo BlobFileInfo { get; set; }
        public byte[] Data { get; set; }
    }

    public class DigitalExportQueryToExcelArgs
    {
        public GeneralFilters QueryFilters { get; set; }
        public bool IsWorkerRoleCall { get; set; }
        public string OutputFileName { get; set; }
        public string LoggedUserEmail { get; set; }
    }

    public class DigitalExportResult
    {
        public string FileName { get; set; }
        public bool IsWorkerRole { get; set; }
        public string ExecutionLogId { get; set; }
    }
}