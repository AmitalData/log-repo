using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Web;
using Simplog.Server.Infrastructure.DataContracts.Models;
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
            if (HttpContext.Current == null 
                || HttpContext.Current.User == null 
                || HttpContext.Current.User.Identity == null 
                || string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) 
                || string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
            {
                return null;
            }

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
            else
            {
                mappedFile = ObjectTableName;
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

        private byte[] DigitalPortalShipmentExportToExcel(List<dynamic> digitalShipmentLists, bool showMultiUnitsOfMeasurements, Dictionary<string, string> textCodeObjects)
        {
            var helper = new DigitalFieldSecuritesHelper();
            var excelEngine = new ExcelEngine();
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet1 = workbook.Worksheets[0];
            sheet1.Name = "Shipments";
            var table = new DataTable();

            if (showMultiUnitsOfMeasurements)
            {
                sheet1.Range["A1:D1"].Merge();
                sheet1.Range["A2:D2"].Merge();

                sheet1.Range["E1:V1"].Merge();
                sheet1.Range["E2:V2"].Merge();
                sheet1.Range["E1"].CellStyle.Font.Bold = true;
                sheet1.Range["E1"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
                sheet1.Range["E1"].VerticalAlignment = ExcelVAlign.VAlignTop;
                sheet1.Range["E1"].Text = "Shipments List";
                sheet1.Range["E1"].CellStyle.Font.Size = 14;

                sheet1.Range["E2"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
                sheet1.Range["E2"].VerticalAlignment = ExcelVAlign.VAlignCenter;
                sheet1.Range["E2"].Text = $"Created Date: {DateTime.UtcNow:dd MMM yyyy}";
                sheet1.Range["E2"].CellStyle.Font.Size = 12;

                sheet1.Range["A3:V3"].CellStyle.Color = Color.LightGray;
                sheet1.Range["A3:V3"].RowHeight = 25;

                sheet1.Range["A3:V3"].CellStyle.Font.Bold = true;
                sheet1.Range["A3:V3"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
                sheet1.Range["A3:V3"].VerticalAlignment = ExcelVAlign.VAlignCenter;
                sheet1.Range["A3:V3"].CellStyle.Font.Size = 11;
                sheet1.Range["A3:V3"].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet1.Range["A3:V3"].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet1.Range["A3:V3"].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet1.Range["A3:V3"].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;

                sheet1.Range["A3:V3"].AutofitRows();
                sheet1.Range["A3:V3"].AutofitColumns();

                // Build excel headers 

                table.Columns.Add(textCodeObjects["Shipment.F.ShipmentNumber"]);
                table.Columns.Add(textCodeObjects["Shipment.G.TransportMode"]);
                table.Columns.Add(textCodeObjects["Shipment.G.DirectionName"]);
                table.Columns.Add(textCodeObjects["Shipment.F.Routing"]);
                table.Columns.Add(textCodeObjects["Shipment.G.ATD"]);
                table.Columns.Add(textCodeObjects["Shipment.F.MainCarriageATA"]);
                table.Columns.Add(textCodeObjects["Shipment.O.Master"]);
                table.Columns.Add(textCodeObjects["Shipment.F.ShipperName"]);
                table.Columns.Add(textCodeObjects["Shipment.F.ConsigneeName"]);
                table.Columns.Add(textCodeObjects["Shipment.G.ShipmentTypeName"]);
                table.Columns.Add(textCodeObjects["Shipment.G.ShipmentStatus"]);
                table.Columns.Add(textCodeObjects["Shipment.F.TruckContainerNumber"]);
                table.Columns.Add(textCodeObjects["Shipment.F.NumberOfPackages"]);

                if (showMultiUnitsOfMeasurements)
                {
                    table.Columns.Add(textCodeObjects["Shipment.F.GrossWeightInKG"]);
                    table.Columns.Add(textCodeObjects["Shipment.G.GrossWeightInLB"]);
                }
                else
                {
                    table.Columns.Add(textCodeObjects["Shipment.F.GrossWeightInKG"]);
                }

                if (showMultiUnitsOfMeasurements)
                {
                    table.Columns.Add(textCodeObjects["Shipment.F.ChargeableWeightInKG"]);
                    table.Columns.Add(textCodeObjects["Shipment.G.ChargeableWeightInLB"]);
                }
                else
                {
                    table.Columns.Add(textCodeObjects["Shipment.F.ChargeableWeightInKG"]);
                }

                table.Columns.Add(textCodeObjects["Shipment.F.IncotermCode"]);
                if (showMultiUnitsOfMeasurements)
                {
                    table.Columns.Add(textCodeObjects["Shipment.G.VolumeInCBF"]);
                    table.Columns.Add(textCodeObjects["Shipment.F.VolumeInCBM"]);
                }
                else
                {
                    table.Columns.Add(textCodeObjects["Shipment.F.VolumeInCBM"]);
                }

                table.Columns.Add(textCodeObjects["Shipment.F.ValueOfGoods"]);
                table.Columns.Add(textCodeObjects["Shipment.G.DescriptionOfGoods"]);

                var index = 4;
                foreach (var item in digitalShipmentLists)
                {
                    sheet1.Range[$"A{index}:V{index}"].CellStyle.Font.Size = 10;
                    sheet1.Range[$"A{index}:V{index}"].ColumnWidth = 25;
                    sheet1.Range[$"A{index}:V{index}"].WrapText = true;
                    sheet1.Range[$"A{index}:V{index}"].AutofitRows();
                    sheet1.Range[$"A{index}:V{index}"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
                    sheet1.Range[$"A{index}:V{index}"].VerticalAlignment = ExcelVAlign.VAlignCenter;

                    sheet1.Range[$"E{index}:F{index}"].NumberFormat = "dd MMM yyyy";

                    sheet1.Range[$"M{index}:Q{index}"].NumberFormat = "#,##0.00";
                    sheet1.Range[$"S{index}:T{index}"].NumberFormat = "#,##0.00";
                    sheet1.Range[$"V{index}:V{index}"].NumberFormat = "#,##0.00";

                    DataRow row = table.NewRow();

                    row[0] = helper.DoesPropertyExistInDynamic(item, "ShipmentNumber") ? item.ShipmentNumber : null;
                    row[1] = helper.DoesPropertyExistInDynamic(item, "TransportModeName") ? item.TransportModeName : null;
                    row[2] = helper.DoesPropertyExistInDynamic(item, "DirectionName") ? item.DirectionName : null;
                    row[3] = helper.DoesPropertyExistInDynamic(item, "MainCarriageFromPortName" ) ? $"{item.MainCarriageFromPortName}, " : "" + helper.DoesPropertyExistInDynamic(item, "MainCarriageToPortName") ? item.MainCarriageToPortName : "";
                    row[4] = helper.DoesPropertyExistInDynamic(item, "MainCarriageATD") ? item.MainCarriageATD?.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) : null;
                    row[5] = helper.DoesPropertyExistInDynamic(item, "MainCarriageATA") ? item.MainCarriageATA?.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) : null; ;
                    row[6] = helper.DoesPropertyExistInDynamic(item, "Master") ? item.Master : null;
                    row[7] = helper.DoesPropertyExistInDynamic(item, "ShipperName") ? item.ShipperName : null;
                    row[8] = helper.DoesPropertyExistInDynamic(item, "ConsigneeName") ? item.ConsigneeName : null;
                    row[9] = helper.DoesPropertyExistInDynamic(item, "ShipmentTypeName") ? item.ShipmentTypeName : null;
                    row[10] = helper.DoesPropertyExistInDynamic(item, "StatusName") ? item.StatusName : null;
                    row[11] = helper.DoesPropertyExistInDynamic(item, "TruckContainerNumber") ? item.TruckContainerNumber : null;
                    row[12] = helper.DoesPropertyExistInDynamic(item, "NumberOfPackages") ? item.NumberOfPackages : null;
                    row[13] = helper.DoesPropertyExistInDynamic(item, "GrossWeightInKG") ? item.GrossWeightInKG : null;
                    row[14] = helper.DoesPropertyExistInDynamic(item, "GrossWeightInKG") ? ShipmentMapping.GetWeightInLB(item.GrossWeightInKG) : null;
                    row[15] = helper.DoesPropertyExistInDynamic(item, "ChargeableWeightInKG") ? item.ChargeableWeightInKG : null;
                    row[16] = helper.DoesPropertyExistInDynamic(item, "ChargeableWeightInKG") ? ShipmentMapping.GetWeightInLB(item.ChargeableWeightInKG) : null;
                    row[17] = helper.DoesPropertyExistInDynamic(item, "IncotermCode") ? item.IncotermCode : null;
                    row[18] = helper.DoesPropertyExistInDynamic(item, "VolumeInCBM") ? ShipmentMapping.GetVolumeInCBF(item.VolumeInCBM) : null;
                    row[19] = helper.DoesPropertyExistInDynamic(item, "VolumeInCBM") ? item.VolumeInCBM : null;
                    row[20] = helper.DoesPropertyExistInDynamic(item, "ValueOfGoods") ? item.ValueOfGoods : null;
                    row[21] = helper.DoesPropertyExistInDynamic(item, "DescriptionOfGoods") ? item.DescriptionOfGoods : null;

                    table.Rows.Add(row);
                    index++;
                }
            }
            else
            {
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
                table.Columns.Add(textCodeObjects["Shipment.F.ShipmentNumber"]);
                table.Columns.Add(textCodeObjects["Shipment.G.TransportMode"]);
                table.Columns.Add(textCodeObjects["Shipment.G.DirectionName"]);
                table.Columns.Add(textCodeObjects["Shipment.F.Routing"]);
                table.Columns.Add(textCodeObjects["Shipment.G.ATD"]);
                table.Columns.Add(textCodeObjects["Shipment.F.MainCarriageATA"]);
                table.Columns.Add(textCodeObjects["Shipment.O.Master"]);
                table.Columns.Add(textCodeObjects["Shipment.F.ShipperName"]);
                table.Columns.Add(textCodeObjects["Shipment.F.ConsigneeName"]);
                table.Columns.Add(textCodeObjects["Shipment.G.ShipmentTypeName"]);
                table.Columns.Add(textCodeObjects["Shipment.G.ShipmentStatus"]);
                table.Columns.Add(textCodeObjects["Shipment.F.TruckContainerNumber"]);
                table.Columns.Add(textCodeObjects["Shipment.F.NumberOfPackages"]);
                table.Columns.Add(textCodeObjects["Shipment.F.GrossWeightInKG"]);
                table.Columns.Add(textCodeObjects["Shipment.F.ChargeableWeightInKG"]);
                table.Columns.Add(textCodeObjects["Shipment.F.IncotermCode"]);
                table.Columns.Add(textCodeObjects["Shipment.F.VolumeInCBM"]);
                table.Columns.Add(textCodeObjects["Shipment.F.ValueOfGoods"]);
                table.Columns.Add(textCodeObjects["Shipment.G.DescriptionOfGoods"]);

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
                    row[0] = helper.DoesPropertyExistInDynamic(item, "ShipmentNumber") ? item.ShipmentNumber : null;
                    row[1] = helper.DoesPropertyExistInDynamic(item, "TransportModeName") ? item.TransportModeName : null;
                    row[2] = helper.DoesPropertyExistInDynamic(item, "DirectionName") ? item.DirectionName : null;
                    row[3] = helper.DoesPropertyExistInDynamic(item, "MainCarriageFromPortName") ? $"{item.MainCarriageFromPortName}, " : "" + helper.DoesPropertyExistInDynamic(item, "MainCarriageToPortName") ? item.MainCarriageToPortName : "";
                    row[4] = helper.DoesPropertyExistInDynamic(item, "MainCarriageATD") ? item.MainCarriageATD?.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) : null;
                    row[5] = helper.DoesPropertyExistInDynamic(item, "MainCarriageATA") ? item.MainCarriageATA?.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) : null; ;
                    row[6] = helper.DoesPropertyExistInDynamic(item, "Master") ? item.Master : null;
                    row[7] = helper.DoesPropertyExistInDynamic(item, "ShipperName") ? item.ShipperName : null;
                    row[8] = helper.DoesPropertyExistInDynamic(item, "ConsigneeName") ? item.ConsigneeName : null;
                    row[9] = helper.DoesPropertyExistInDynamic(item, "ShipmentTypeName") ? item.ShipmentTypeName : null;
                    row[10] = helper.DoesPropertyExistInDynamic(item, "StatusName") ? item.StatusName : null;
                    row[11] = helper.DoesPropertyExistInDynamic(item, "TruckContainerNumber") ? item.TruckContainerNumber : null;
                    row[12] = helper.DoesPropertyExistInDynamic(item, "NumberOfPackages") ? item.NumberOfPackages : null;
                    row[13] = helper.DoesPropertyExistInDynamic(item, "GrossWeight") ? item.GrossWeight : null;
                    row[14] = helper.DoesPropertyExistInDynamic(item, "ChargeableWeight") ? item.ChargeableWeight : null;
                    row[15] = helper.DoesPropertyExistInDynamic(item, "IncotermCode") ? item.IncotermCode : null;
                    row[16] = helper.DoesPropertyExistInDynamic(item, "Volume") ? item.Volume : null;
                    row[17] = helper.DoesPropertyExistInDynamic(item, "ValueOfGoods") ? item.ValueOfGoods : null;
                    row[18] = helper.DoesPropertyExistInDynamic(item, "DescriptionOfGoods") ? item.DescriptionOfGoods : null;

                    table.Rows.Add(row);
                    index++;
                }
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

        private byte[] DigitalTextCodeExportDataToExcel(Dictionary<string, List<DigitalTextCodeList>> englishTextobjects,
                                                        Dictionary<string, List<DigitalTextCodeList>> foreignTextObjects,
                                                        string languageCode)
        {
            var excelEngine = new ExcelEngine();
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet1 = workbook.Worksheets[0];
            sheet1.Name = "Label translation";
            var table = new DataTable();

            sheet1.Range["A1:F1"].Merge();
            sheet1.Range["A2:F2"].Merge();
            sheet1.Range["A1"].CellStyle.Font.Bold = true;
            sheet1.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
            sheet1.Range["A1"].VerticalAlignment = ExcelVAlign.VAlignCenter;
            sheet1.Range["A1"].Text = "Digital Portal Translation";
            sheet1.Range["A1"].CellStyle.Font.Size = 14;
            sheet1.Range["A1"].CellStyle.Font.Size = 14;

            sheet1.Range["A2"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
            sheet1.Range["A2"].VerticalAlignment = ExcelVAlign.VAlignCenter;
            sheet1.Range["A2"].Text = $"Created Date: {DateTime.UtcNow:dd MMM yyyy}";
            sheet1.Range["A2"].CellStyle.Font.Size = 12;

            sheet1.Range["A3:F3"].CellStyle.Color = Color.LightGray;
            sheet1.Range["A3:F3"].RowHeight = 25;
                             
            sheet1.Range["A3:F3"].CellStyle.Font.Bold = true;
            sheet1.Range["A3:F3"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
            sheet1.Range["A3:F3"].VerticalAlignment = ExcelVAlign.VAlignCenter;
            sheet1.Range["A3:F3"].CellStyle.Font.Size = 11;
            sheet1.Range["A3:F3"].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range["A3:F3"].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range["A3:F3"].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range["A3:F3"].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                             
            sheet1.Range["A3:F3"].AutofitRows();
            sheet1.Range["A3:F3"].AutofitColumns();
            sheet1.Range["A3:F3"].WrapText = true;

            table.Columns.Add("Text Code");
            table.Columns.Add("Field Code");
            table.Columns.Add("English Default Text");
            table.Columns.Add("Spanish Default Text");
            table.Columns.Add("Profile");
            table.Columns.Add("Entity");

            var index = 4;
            foreach (var profilesLables in englishTextobjects)
            {
                foreach (var item in profilesLables.Value)
                {
                    sheet1.Range[$"A{index}:F{index}"].CellStyle.Font.Size = 10;
                    sheet1.Range[$"A{index}:F{index}"].ColumnWidth = 40;
                    sheet1.Range[$"A{index}:F{index}"].WrapText = true;
                    sheet1.Range[$"A{index}:F{index}"].AutofitRows();
                    sheet1.Range[$"A{index}:F{index}"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
                    sheet1.Range[$"A{index}:F{index}"].VerticalAlignment = ExcelVAlign.VAlignCenter;

                    var englishLables = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(item.Labels);
                    var foreignLables = new List<DigitalTextCodeObject>();

                    if (foreignTextObjects.ContainsKey(profilesLables.Key))
                    {
                        var foreignTextCode = foreignTextObjects[profilesLables.Key].FirstOrDefault(a => a.LanguageCode == languageCode);

                        if (foreignTextCode != null)
                        {
                            foreignLables = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(foreignTextCode.Labels);
                        }
                    }

                    foreach (var code in englishLables)
                    {
                        if (string.IsNullOrWhiteSpace(code.TextCode))
                        {
                            continue;
                        }

                        var foreignLanguageTextCode = foreignLables.Any()
                                                      ? foreignLables.FirstOrDefault(a => a.TextCode.Equals(code.TextCode, StringComparison.InvariantCultureIgnoreCase))?.DefaultText
                                                      : "";

                        DataRow row = table.NewRow();
                        row[0] = code.TextCode;
                        row[1] = !string.IsNullOrWhiteSpace(code.FieldCode) ? code.FieldCode : "";
                        row[2] = code.DefaultText;
                        row[3] = foreignLanguageTextCode;
                        row[4] = item.ProfileCode;
                        row[5] = item.ObjectTableName;
                        table.Rows.Add(row);
                        index++;
                    }
                }
            }

            sheet1.Range["A4"].FreezePanes();
            sheet1.ImportDataTable(table, true, 3, 1);
            workbook.Version = ExcelVersion.Excel2007;
            var memory = new MemoryStream();
            workbook.SaveAs(memory);
            workbook.Close();
            excelEngine.Dispose();
            return memory.ToArray();
        }

        private DigitalXslExportResult GetFileInfo(DigitalExportQueryToExcelArgs args)
        {
            var helper = new DigitalFieldSecuritesHelper();
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
                    var tenantData = new TenantQuery(args.QueryFilters.Tenant);
                    var tenantObject = tenantData.GetSinglePM(args.QueryFilters.Tenant);
                    var showMultiUnitsOfMeasurements = false;

                    if (tenantObject != null)
                    {
                        showMultiUnitsOfMeasurements = tenantObject.ShowMultiUnitsOfMeasurements;
                    }

                    var shipmentData = shipmentQuery.GetByFilters(args.QueryFilters);

                    var allowedShipmentsFieldSecurites = helper.GitDigitalSecuritesFeilds(args.QueryFilters.ObjectTableId, args.QueryFilters.ProfileCode, tenant, false)
                                                               .Where(a => a.HasPermission)
                                                               .Select(a => a.FieldCode.Replace($"Shipment.{tenant}.", ""))
                                                               .Select(a => a.Replace("Shipment.", ""))
                                                               .ToList();

                    var shipmentsFields = string.Join(",", allowedShipmentsFieldSecurites);
                    var shipmentsDynamicData = shipmentData.Select("new { " + shipmentsFields + " }").ToDynamicList();
                    FillContainerNumbers(shipmentsDynamicData, helper);
                    var textCodeObjects = helper.GetDigitalTextCodeObjects(tenant, 
                                                                           args.QueryFilters.ObjectTableId,
                                                                           args.QueryFilters.ProfileCode,
                                                                           true, args.QueryFilters.LanguageCode)
                                                .ToDictionary(x => x.TextCode, y => y.DefaultText);
                    data = DigitalPortalShipmentExportToExcel(shipmentsDynamicData, showMultiUnitsOfMeasurements, textCodeObjects);
                    break;
                case "DigitalInvoice":
                    var aRInvoiceQuery = new ARInvoiceQuery(args.QueryFilters.Tenant);
                    var invoiceData = aRInvoiceQuery.GetByFilters(args.QueryFilters).ToList();
                    data = DigitalPortalInvoiceExportToExcel(invoiceData);
                    break;
                case "DigitalLabelTranslations":
                    var service = new DigitalTextCodeQueryService(0);
                    var tenant0Objects = service.GetDigitalTextCodesTenant0();
                    var englishObjects = tenant0Objects.Where(a => a.LanguageCode == "EN")
                                                       .GroupBy(a => a.ObjectTableId)
                                                       .ToDictionary(a => a.Key, x => x.ToList());
                    var foreignObjects = tenant0Objects.Where(a => a.LanguageCode == args.QueryFilters.LanguageCode)
                                                       .GroupBy(a => a.ObjectTableId)
                                                       .ToDictionary(a => a.Key, x => x.ToList());
                    data = DigitalTextCodeExportDataToExcel(englishObjects, foreignObjects, args.QueryFilters.LanguageCode);
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

        private void FillContainerNumbers(List<dynamic> listQuery, DigitalFieldSecuritesHelper helper)
        {
            listQuery.ForEach(shipment =>
            {
                bool isInlandDomesticShipment = helper.DoesPropertyExistInDynamic(shipment, "DirectionId") 
                                                && helper.DoesPropertyExistInDynamic(shipment, "TransportModeId") 
                                                ? (shipment.DirectionId == "D" && shipment.TransportModeId == "I")
                                                : false;

                if (helper.DoesPropertyExistInDynamic(shipment, "ContainersNumbersandTypesArray") 
                    && helper.DoesPropertyExistInDynamic(shipment, "TransportModeId")
                    && helper.DoesPropertyExistInDynamic(shipment, "TruckContainerNumber")
                    && (shipment.TransportModeId != "O" || !string.IsNullOrEmpty(shipment.ContainersNumbersandTypesArray)))
                {
                    shipment.TruckContainerNumber = shipment.TransportModeId == "O"
                                                    ? Regex.Replace(shipment.ContainersNumbersandTypesArray, "(\\[.*?\\])", "")
                                                    : isInlandDomesticShipment
                                                        ? helper.DoesPropertyExistInDynamic(shipment, "TruckNumber") ? shipment.TruckNumber : ""
                                                        : helper.DoesPropertyExistInDynamic(shipment, "CarrierNumber") ? shipment.CarrierNumber : "";
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