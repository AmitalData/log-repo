using Logitude.BL.InvoiceModel.CustomFilters;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using WebFreight.Web.Controllers.InvoiceModel.ApiHelpers;
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

        public DigitalExportResult ExportQueryDataToStorage(DigitalExportQueryToExcelArgs args)
        {
            SecurityUtility.IsWorkerRoleCall = Logitude.BL.Security.SecurityUtility.IsWorkerRoleCall = args.IsWorkerRoleCall;
            var res = GetFileInfo(args);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(res.Data, res.BlobFileInfo);
            return new DigitalExportResult() { FileName = res.BlobFileInfo.FileName };
        }

        #region private


        private string GetLoggedUserEmail()
        {
            if (HttpContext.Current == null || HttpContext.Current.User == null || HttpContext.Current.User.Identity == null || string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) || string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name)) return null;
            return HttpContext.Current.User.Identity.Name;


        }


        private User GetLoggedUserId(int tenant)
        {
            string loggedUserEmail = GetLoggedUserEmail();
            string loggedSystemEmail = "system@tenant" + tenant + ".com";
            User loggedUser = null;
            UserRepository userRep = new UserRepository(tenant);

            if (!string.IsNullOrEmpty(loggedUserEmail))
            {
                loggedUser = userRep.GetSingleUserByEmail(loggedUserEmail, tenant, true);
            }

            if (loggedUser == null)
            {
                loggedUser = userRep.GetSingleUserByEmail(loggedSystemEmail, tenant, true);
            }

            return loggedUser;

        }


        private DigitalExportResult AddQueryExportExecutionLog(GeneralFilters queryFilters)
        {
            int tenant = (int)queryFilters.Tenant;

            User loggedUser = GetLoggedUserId(tenant);

            if (loggedUser == null)
            {
                throw new AutenticationException("Sorry! this user is not authorized!");
            }

            var myTenantRepository = new TenantRepository(tenant);
            var tenantData = myTenantRepository.GetSingleTenantWithOutIncluded(tenant);
            
            var reportExecutionLogRepository = new QueryExportExecutionLogRepository(tenant);
            var logId = IdCounter.GetNumber("QueryExportExecutionLog", tenant);

            var executionLog = new QueryExportExecutionLog
            {
                Id = logId,
                CreateDate = DateTime.Now,
                CreatedByUserId = loggedUser.Id,
                QueryFilterXML = LogitudeXmlSerializer.SerializeObjectToXmlString(queryFilters),
                Tenant = tenant,
                StatusCode = "W"
            };

            reportExecutionLogRepository.Add(executionLog);
            reportExecutionLogRepository.SubmitChanges();

            string ObjectTableName = queryFilters.ObjectTableName.Replace("Customs.", "");

            string mappedFile = "";

            if (ObjectTableName.Equals("DigitalShipmentsView", StringComparison.InvariantCultureIgnoreCase))
            {
                mappedFile = $"DigitalPortal_{tenantData.Company}_ShipmentList";
            }
            else if (ObjectTableName.Equals("DigitalInvoice", StringComparison.InvariantCultureIgnoreCase))
            {
                mappedFile = $"DigitalPortal_{tenantData.Company}_InvoiceList";
            }

            string fileName = GetOutpuFileName(mappedFile);

            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("DigitalPortalQueryExportExecutionQueue", executionLog.Tenant);

            queueservice.Send(new Dictionary<string, string>() {
                    { "LogId", executionLog.Id},
                    { "Tenant", executionLog.Tenant.ToString()},
                    { "FileName", fileName },
                    { "LoggedUserEmail", loggedUser.Contact.Email},
                }, tenant, null, null, null, null);

            return new DigitalExportResult() { ExecutionLogId = logId, FileName = fileName, IsWorkerRole = true };
        }

        private static string GetOutpuFileName(string mappedFileName)
        {
            return mappedFileName + "_" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
        }

        private byte[] DigitalPortalShipmentExportToExcel(List<DigitalShipmentList> digitalShipmentLists)
        {
            var excelEngine = new ExcelEngine();
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet1 = workbook.Worksheets[0];
            sheet1.Name = "Shipments";

            sheet1.Range["A1:D1"].Merge();
            sheet1.Range["A2:D2"].Merge();
            sheet1.Range["E1:S1"].Merge();
            sheet1.Range["E2:S2"].Merge();
            sheet1.Range["E1"].CellStyle.Font.Bold = true;
            sheet1.Range["E1"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
            sheet1.Range["E1"].VerticalAlignment = ExcelVAlign.VAlignTop;
            sheet1.Range["E1"].Text = "Shipments List";
            sheet1.Range["E1"].CellStyle.Font.Size = 14;

            sheet1.Range["E2"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
            sheet1.Range["E2"].VerticalAlignment = ExcelVAlign.VAlignCenter;
            sheet1.Range["E2"].Text = $"Created Date: {DateTime.UtcNow:dd MMM yyyy}";
            sheet1.Range["E2"].CellStyle.Font.Size = 12;

            sheet1.Range["A3:S3"].CellStyle.Color = Color.LightGray;
            sheet1.Range["A3:S3"].RowHeight = 25;

            sheet1.Range["A3:S3"].CellStyle.Font.Bold = true;
            sheet1.Range["A3:S3"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
            sheet1.Range["A3:S3"].VerticalAlignment = ExcelVAlign.VAlignCenter;
            sheet1.Range["A3:S3"].CellStyle.Font.Size = 11;
            sheet1.Range["A3:S3"].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range["A3:S3"].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range["A3:S3"].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range["A3:S3"].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;

            sheet1.Range["A3:S3"].AutofitRows();
            sheet1.Range["A3:S3"].AutofitColumns();

            // Build excel headers 
            var table = new DataTable();
            table.Columns.Add("Shipment No.");
            table.Columns.Add("Transport Mode");
            table.Columns.Add("Direction");
            table.Columns.Add("Routing");
            table.Columns.Add("MainCarriage ATD");
            table.Columns.Add("MainCarriage ATA");
            table.Columns.Add("Master No.");
            table.Columns.Add("Shipper");
            table.Columns.Add("Consignee");
            table.Columns.Add("Shipment Type");
            table.Columns.Add("Status");
            table.Columns.Add("TruckContainer Numbers");
            table.Columns.Add("No of Package");
            table.Columns.Add("Gross Weight");
            table.Columns.Add("Chargable Weight");
            table.Columns.Add("Incoterm");
            table.Columns.Add("Volume");
            table.Columns.Add("Goods Value");
            table.Columns.Add("Goods Description");

            var index = 4;
            foreach (var item in digitalShipmentLists)
            {
                sheet1.Range[$"A{index}:S{index}"].CellStyle.Font.Size = 10;
                sheet1.Range[$"A{index}:S{index}"].ColumnWidth = 25;
                sheet1.Range[$"A{index}:S{index}"].WrapText = true;
                sheet1.Range[$"A{index}:S{index}"].AutofitRows();
                sheet1.Range[$"A{index}:S{index}"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
                sheet1.Range[$"A{index}:S{index}"].VerticalAlignment = ExcelVAlign.VAlignCenter;

                sheet1.Range[$"E{index}:F{index}"].NumberFormat = "dd MMM yyyy";

                sheet1.Range[$"M{index}:O{index}"].NumberFormat = "#,##0.00";
                sheet1.Range[$"Q{index}:Q{index}"].NumberFormat = "#,##0.00";
                sheet1.Range[$"S{index}:S{index}"].NumberFormat = "#,##0.00";

                DataRow row = table.NewRow();
                row[0] = item.ShipmentNumber;
                row[1] = item.TransportModeName;
                row[2] = item.DirectionName;
                row[3] = item.MainCarriageFromPortName + ", " + item.MainCarriageToPortName;
                row[4] = item.MainCarriageATD?.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                row[5] = item.MainCarriageATA?.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                row[6] = item.Master;
                row[7] = item.ShipperName;
                row[8] = item.ConsigneeName;
                row[9] = item.ShipmentTypeName;
                row[10] = item.StatusName;
                row[11] = item.TruckContainerNumber;
                row[12] = item.NumberOfPackages;
                row[13] = item.GrossWeight;
                row[14] = item.ChargeableWeight;
                row[15] = item.IncotermCode;
                row[16] = item.Volume;
                row[17] = item.ValueOfGoods;
                row[18] = item.DescriptionOfGoods;

                table.Rows.Add(row);
                index++;
            }

            sheet1.Range["A4"].FreezePanes();
            sheet1.ImportDataTable(table, true, 3, 1);
            workbook.Version = ExcelVersion.Excel2007;
            MemoryStream memory = new MemoryStream();
            workbook.SaveAs(memory);
            workbook.Close();
            excelEngine.Dispose();
            return memory.ToArray();
        }

        private byte[] DigitalPortalInvoiceExportToExcel(List<ARInvoiceList> invoices)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet1 = workbook.Worksheets[0];

            sheet1.Name = "Invoices";
            sheet1.Range["A1:L1"].Merge();
            sheet1.Range["A2:L2"].Merge();
            sheet1.Range["A1"].CellStyle.Font.Bold = true;
            sheet1.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
            sheet1.Range["A1"].VerticalAlignment = ExcelVAlign.VAlignTop;
            sheet1.Range["A1"].Text = "Invoices List";
            sheet1.Range["A1"].CellStyle.Font.Size = 14;

            sheet1.Range["A2"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
            sheet1.Range["A2"].VerticalAlignment = ExcelVAlign.VAlignCenter;
            sheet1.Range["A2"].Text = $"Created Date: {DateTime.UtcNow:dd MMM yyyy}";
            sheet1.Range["A2"].CellStyle.Font.Size = 12;

            sheet1.Range["A3:L3"].CellStyle.Color = Color.LightGray;
            sheet1.Range["A3:S3"].RowHeight = 25;

            sheet1.Range["A3:L3"].CellStyle.Font.Bold = true;
            sheet1.Range["A3:L3"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
            sheet1.Range["A3:L3"].VerticalAlignment = ExcelVAlign.VAlignCenter;
            sheet1.Range["A3:L3"].CellStyle.Font.Size = 11;
            sheet1.Range["A3:L3"].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range["A3:L3"].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range["A3:L3"].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range["A3:L3"].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;

            sheet1.Range["A3:L3"].AutofitRows();
            sheet1.Range["A3:L3"].AutofitColumns();

            // Build excel headers 
            var table = new DataTable();
            table.Columns.Add("Invoice Number");
            table.Columns.Add("Invoice Type");
            table.Columns.Add("My Reference");
            table.Columns.Add("Your Reference");
            table.Columns.Add("Invoice Date");
            table.Columns.Add("Invoice Status");
            table.Columns.Add("Payment Term");
            table.Columns.Add("Invoice Currency");
            table.Columns.Add("Total Amount");
            table.Columns.Add("Open Amount ");
            table.Columns.Add("Due Date");
            table.Columns.Add("Print Notes");

            var index = 4;

            foreach (var item in invoices)
            {
                sheet1.Range[$"A{index}:L{index}"].CellStyle.Font.Size = 10;
                sheet1.Range[$"A{index}:L{index}"].ColumnWidth = 25;
                sheet1.Range[$"A{index}:L{index}"].WrapText = true;
                sheet1.Range[$"A{index}:L{index}"].AutofitRows();
                sheet1.Range[$"A{index}:L{index}"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
                sheet1.Range[$"A{index}:L{index}"].VerticalAlignment = ExcelVAlign.VAlignCenter;

                sheet1.Range[$"E{index}:E{index}"].NumberFormat = "dd MMM yyyy";
                sheet1.Range[$"K{index}:K{index}"].NumberFormat = "dd MMM yyyy";

                sheet1.Range[$"I{index}:J{index}"].NumberFormat = "#,##0.00";

                DataRow row = table.NewRow();

                row[0] = item.InvoiceNumber;
                row[1] = item.ARInvoiceTypeName;
                row[2] = item.MainEntityReference;
                row[3] = item.CustomerRef;
                row[4] = $"{item.CreateDate?.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}";
                row[5] = item.StatusName;
                row[6] = item.PaymentTermName;
                row[7] = item.InvoiceCurrencyCode;
                row[8] = item.AmountInInvoiceCurrency;
                row[9] = item.AmountDue;
                row[10] = $"{item.DueDate?.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}";
                row[11] = item.PrintNotes;
                table.Rows.Add(row);
                index++;
            }

            sheet1.Range["A4"].FreezePanes();
            sheet1.ImportDataTable(table, true, 3, 1);
            workbook.Version = ExcelVersion.Excel2007;
            MemoryStream memory = new MemoryStream();
            workbook.SaveAs(memory);
            workbook.Close();
            excelEngine.Dispose();
            return memory.ToArray();
        }

        private List<ARInvoiceList> GetInvoicesByFilters(GeneralFilters newFilters)
        {
            var myTenantRepository = new TenantRepository(newFilters.Tenant);
            var myTenant = myTenantRepository.GetSingleTenant(newFilters.Tenant);

            var filters = new ApiQueryFilters()
            {
                Filter1Value = newFilters.CardId,
                Filter2Value = newFilters.CardType
            };

            var queryOperations = new QueryOperations()
            {
                ObjectTableName = "ARInvoice",
                PageIndex = newFilters.PageIndex,
                PageSize = newFilters.PageSize,
                QuerySection = "ARInvoices",
                SortByColumnName = newFilters.SortBy,
                SortDirectin = newFilters.SortDirection
            };

            queryOperations.SetFilter("IsPrinted", true, false, "Equals", null, false);
            queryOperations.SetFilter("IsConstituentInvoice", false, false, "Equals", null, false);

            var cardFilterValues = newFilters.CardId;

            if (!string.IsNullOrWhiteSpace(cardFilterValues))
            {
                var cardBillToId = GetCardBillToId(newFilters.CardId, newFilters.Tenant);
                if (!string.IsNullOrWhiteSpace(cardBillToId))
                {
                    cardFilterValues = cardFilterValues + "," + cardBillToId;
                    queryOperations.SetFilter("PartnerId", newFilters.CardId, false, "Equals", null, false);
                }

                queryOperations.SetFilter("BillToId", cardFilterValues, false, "InList", null, false);
            }

            var ARInvoiceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableNameWithNoIncludes("ARInvoice", newFilters.Tenant);

            if (newFilters.AdditionalFilters.Any())
            {
                foreach (var filter in newFilters.AdditionalFilters)
                {
                    var field = ARInvoiceObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);

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
            }

            ARInvoiceAPiHelper.AddFilters(queryOperations, newFilters.Tenant);
            var genericFilter = new GenericFilter();
            var MyContext = InvoiceContext.GetContext(newFilters.Tenant);

            var nonListQueryOperation = new QueryOperations
            {
                QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList()
            };

            var listQueryOperation = new QueryOperations
            {
                QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList()
            };

            var aRInvoiceRepository = new ARInvoiceRepository(MyContext);
            var aRInvoiceQuery = new ARInvoiceQuery(aRInvoiceRepository);

            var entityPocos = aRInvoiceRepository.GetARInvoices(newFilters.Tenant);

            entityPocos = aRInvoiceRepository.FilterInvoicesStatusesForList(entityPocos);

            var customfilters = new ARInvoiceCustomFilter(newFilters.Tenant);
            entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos);
            entityPocos = ARInvoiceAPiHelper.ApplyFilters(entityPocos, newFilters.Tenant);
            entityPocos = genericFilter.GetFilteredQuery(nonListQueryOperation, entityPocos);

            var entityLists = aRInvoiceQuery.GetIQueryableEntityList(entityPocos);
            entityLists = genericFilter.GetFilteredQuery(listQueryOperation, entityLists);

            if (!string.IsNullOrWhiteSpace(queryOperations.SortByColumnName) && !string.IsNullOrWhiteSpace(queryOperations.SortDirectin))
            {
                ObjectField objectField = ARInvoiceObjectFields.FirstOrDefault(a => a.FieldName == queryOperations.SortByColumnName);

                if (objectField != null)
                {
                    var sortClass = new GenericSort();

                    if (!objectField.IsCustom)
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "text":
                                {
                                    entityLists = sortClass.GetSorterQuery<ARInvoiceList, string>(queryOperations, entityLists);
                                    break;
                                }
                            case "double":
                                {
                                    entityLists = sortClass.GetSorterQuery<ARInvoiceList, double>(queryOperations, entityLists);
                                    break;
                                }
                            case "datetime":
                                {
                                    entityLists = sortClass.GetSorterQuery<ARInvoiceList, DateTime>(queryOperations, entityLists);
                                    break;
                                }
                            case "integer":
                                {
                                    entityLists = sortClass.GetSorterQuery<ARInvoiceList, int>(queryOperations, entityLists);
                                    break;
                                }
                            case "lookup":
                                {
                                    entityLists = sortClass.GetSorterQuery<ARInvoiceList, string>(queryOperations, entityLists);
                                    break;
                                }
                            case "boolean":
                                {
                                    entityLists = sortClass.GetSorterQuery<ARInvoiceList, bool>(queryOperations, entityLists);
                                    break;
                                }
                            default:
                                {
                                    entityLists = entityLists.OrderByDescending(d => d.InvoiceDate);
                                    break;
                                }
                        }
                    }
                    else
                    {
                        entityLists = sortClass.GetSorterQuery<ARInvoiceList, string>(queryOperations, entityLists);
                    }
                }
            }
            else
            {
                entityLists = entityLists.OrderByDescending(d => d.InvoiceDate);
            }

            var res = entityLists.ToList();
            return res;
        }

        private List<DigitalShipmentList> GetShipmentsByFilter(GeneralFilters newFilters, int tenant)
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

            shipments = Logitude.BL.ShipmentsModel.CustomFilters.DigitalPortalCustomFilter.GetDigtalFilteredQuery(queryOperations, shipments, shipmentRepository, tenant);

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

            entityLists = entityLists.OrderByDescending(d => d.CreateDateTime);

            return entityLists?.ToList();
        }

        private string GetCardBillToId(string cardId, int tenant)
        {
            CardRepository cardRepository = new CardRepository(tenant);
            var cardBillToId = cardRepository.GetBillToCardById(cardId, tenant);
            return cardBillToId;
        }

        private DigitalXslExportResult GetFileInfo(DigitalExportQueryToExcelArgs args)
        {
            var queryFilters = args.QueryFilters;
            int tenant = (int)queryFilters.Tenant;
            string ObjectTableName = queryFilters.ObjectTableName.Replace("Customs.", "");

            string fileName = !string.IsNullOrEmpty(args.OutputFileName)
                              ? args.OutputFileName
                              : GetOutpuFileName(ObjectTableName);

            byte[] data = null;

            switch (ObjectTableName)
            {
                case "DigitalShipmentsView":
                    var shipmentQuery = new ShipmentQuery(args.QueryFilters.Tenant);
                    var shipmentData = shipmentQuery.GetByFilters(args.QueryFilters).ToList();
                    FillContainerNumbers(shipmentData);
                    data = DigitalPortalShipmentExportToExcel(shipmentData);
                    break;
                case "DigitalInvoice":
                    var aRInvoiceQuery = new ARInvoiceQuery(args.QueryFilters.Tenant);
                    var invoiceData = aRInvoiceQuery.GetByFilters(args.QueryFilters).ToList();
                    data = DigitalPortalInvoiceExportToExcel(invoiceData);
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

        private void FillContainerNumbers(List<DigitalShipmentList> listQuery)
        {
            listQuery.ForEach(shipment =>
            {
                bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");
                if (shipment.TransportModeId != "O" || !string.IsNullOrEmpty(shipment.ContainersNumbersandTypesArray))
                {
                    shipment.TruckContainerNumber = shipment.TransportModeId == "O"
                                                    ? Regex.Replace(shipment.ContainersNumbersandTypesArray, "(\\[.*?\\])", "")
                                                    : isInlandDomesticShipment
                                                        ? shipment.TruckNumber
                                                        : shipment.CarrierNumber;
                }
            });
        }

        #endregion private
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