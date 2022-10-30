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

        private DigitalExportResult AddQueryExportExecutionLog(GeneralFilters queryFilters)
        {
            int tenant = (int)queryFilters.Tenant;

            var myTenantRepository = new TenantRepository(tenant);
            var tenantData = myTenantRepository.GetSingleTenantWithOutIncluded(tenant);
            
            var reportExecutionLogRepository = new QueryExportExecutionLogRepository(tenant);
            var logId = IdCounter.GetNumber("QueryExportExecutionLog", tenant);
            var loggedEmail = SecurityUtility.GetAuthenticatedUser();

            var userId = SecurityUtility.GetLoggedUserId(loggedEmail, tenant);
            if(userId == null)
            {
                userId = GetSystemUser(tenant);
            }

            var executionLog = new QueryExportExecutionLog
            {
                Id = logId,
                CreateDate = DateTime.Now,
                CreatedByUserId = userId,
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
                    { "LoggedUserEmail", loggedEmail},
                }, tenant, null, null, null, null);

            return new DigitalExportResult() { ExecutionLogId = logId, FileName = fileName, IsWorkerRole = true };
        }

        private string GetSystemUser(int tenant)
        {
            var loggedEmail = "system@tenant" + tenant + ".com";
            var userId = SecurityUtility.GetLoggedUserId(loggedEmail, tenant);
            return userId;
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