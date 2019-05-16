using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.TariffModule.BL.DataContracts;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityQueryServices;
using Logitude.TariffModule.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Linq;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityPOCOs;
using System.Collections.Generic;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Syncfusion.XlsIO;
using System.Data;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Stimulsoft.Base.Excel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class TariffDomainController : ApiController
    {
        private PortRepository portRepository;

        public HttpResponseMessage GetTariffsCounts()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);

                string loggedContactId = null;
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (loggedContact != null)
                {
                    loggedContactId = loggedContact.Id;
                }


                TariffQueryService tariffQueryService = new TariffQueryService(tenant);

                TariffsSummary myResult = tariffQueryService.GetCount(tenant);


                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTenantTariffSetting()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ITariffModuleContext iContext = TariffModuleContext.GetContext(authToken.Tenant);
                TariffSettingPM entityPM = (from d in iContext.TariffSettings
                                            where d.Tenant == authToken.Tenant
                                            select new TariffSettingPM()
                                            {
                                                Id = d.Id,
                                                Tenant = d.Tenant,
                                                DefaultPriceSteps = d.DefaultPriceSteps,
                                            }).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetDownloadTariff(string tariffId, int version, string type)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ITariffModuleContext context = TariffModuleContext.GetContext(tenant);
                TariffQueryService tariffQuery = new TariffQueryService(context);
                TariffPM tariff = tariffQuery.GetSingle(tariffId, false, false);

                string fileName = "";
                if (tariff != null)
                {
                    TariffVersionQueryService tariffVersionQuery = new TariffVersionQueryService(context);
                    TariffVersionPM tariffVersion = tariffVersionQuery.GetSingle(tariffId, version, true, false);

                    if (tariffVersion != null)
                    {
                        byte[] data = null;

                        if (tariff.TypeCode == "AFC")
                        {
                            data = this.ExportAirFreightCostLinesToExcel(tariff, tariffVersion.TariffLines, tenant, type);
                        }

                        else if (tariff.TypeCode == "ASC")
                        {
                            data = this.ExportAirSurchargesCostLinesToExcel(tariff, tariffVersion.TariffLines, tenant, type);
                        }
                        
                        fileName = "Tariffs" + DateTime.Now.ToShortDateString();

                        if (data != null)
                        {
                            BlobFileInfo fileInfo = new BlobFileInfo()
                            {
                                FileName = fileName,
                                FolderName = "others",
                                Extension = "xls",
                                Tenant = tenant,
                                FileSize = data.Length,
                            };

                            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                            storageservice.Write(data, fileInfo);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, fileName);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private byte[] ExportAirFreightCostLinesToExcel(TariffPM tariff, List<TariffLinePM> tariffLines, int tenant, string type)
        {
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet1 = workbook.Worksheets[0];

            // Build excel headers 
            DataTable table = new DataTable();
            string[] steps = tariff.PriceSteps.Split(',');
            if (steps.Length > 1)
            {
                table.Columns.Add("From");
                table.Columns.Add("To");
                table.Columns.Add("Min Price");
                sheet1.InsertColumn(3, steps.Length, ExcelInsertOptions.FormatAsAfter);
                for (int i = 0; i < steps.Length; i++)
                {
                    table.Columns.Add(steps[i] + " KG");
                }
            }
            else
            {
                table.Columns.Add("From");
                table.Columns.Add("To");
                table.Columns.Add("Min Price");
                table.Columns.Add(tariff.PriceSteps + " KG");
            }

            string range = "A1:D1";
            if (steps.Length == 2)
            {
                range = "A1:E1";
            }

            else if (steps.Length == 3)
            {
                range = "A1:F1";
            }

            else if (steps.Length == 4)
            {
                range = "A1:G1";
            }

            else if (steps.Length == 5)
            {
                range = "A1:H1";
            }

            else if (steps.Length == 6)
            {
                range = "A1:I1";
            }

            else if (steps.Length == 7)
            {
                range = "A1:J1";
            }

            else if (steps.Length == 8)
            {
                range = "A1:K1";
            }

            if (type == "Data")
            {
                if (tariffLines != null && tariffLines.Count > 0)
                {
                    foreach (var item in tariffLines)
                    {
                        DataRow row = table.NewRow();
                        row[0] = item.OriginPortCode ?? null;
                        row[1] = item.DestinationPortCode ?? null;
                        row[2] = item.MinPrice ?? null;
                        row[3] = item.Step1Price ?? null;

                        if (item.Step2Price.HasValue)
                        {
                            row[4] = item.Step2Price ?? null;
                        }

                        if (item.Step3Price.HasValue)
                        {
                            row[5] = item.Step3Price ?? null;
                        }

                        if (item.Step4Price.HasValue)
                        {
                            row[6] = item.Step4Price ?? null;
                        }

                        if (item.Step5Price.HasValue)
                        {
                            row[7] = item.Step5Price ?? null;
                        }

                        if (item.Step6Price.HasValue)
                        {
                            row[8] = item.Step6Price ?? null;
                        }

                        if (item.Step7Price.HasValue)
                        {
                            row[9] = item.Step7Price ?? null;
                        }

                        if (item.Step8Price.HasValue)
                        {
                            row[10] = item.Step8Price ?? null;
                        }

                        table.Rows.Add(row);
                    }
                }
            }

            sheet1.Range[range].CellStyle.Font.Color = ExcelKnownColors.White;
            sheet1.Range[range].CellStyle.Color = System.Drawing.Color.Gray;
            sheet1.Range[range].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet1.ImportDataTable(table, true, 1, 1);
            workbook.Version = ExcelVersion.Excel2007;
            workbook.SaveAs(memory);
            return memory.ToArray(); 
        }
        private byte[] ExportAirSurchargesCostLinesToExcel(TariffPM tariff, List<TariffLinePM> tariffLines, int tenant, string type)
        {
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet1 = workbook.Worksheets[0];

            DataTable table = new DataTable();
            table.Columns.Add("From");
            table.Columns.Add("To");

            #region header
            int count = 0;
            ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tenant);
            ChargesType chargesType = null;
            if (!string.IsNullOrEmpty(tariff.Surcharge1Id))
            {
                count = 1;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge1Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge2Id))
            {
                count = 2;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge2Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge3Id))
            {
                count = 3;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge3Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge4Id))
            {
                count = 4;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge4Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge5Id))
            {
                count = 5;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge5Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge6Id))
            {
                count = 6;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge6Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge7Id))
            {
                count = 7;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge7Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge8Id))
            {
                count = 8;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge8Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge9Id))
            {
                count = 9;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge9Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge10Id))
            {
                count = 10;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge10Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }
            #endregion

            #region range
            string range = "A1:B1";
            if (count == 1)
            {
                range = "A1:C1";
            }

            else if (count == 2)
            {
                range = "A1:D1";
            }

            else if (count == 3)
            {
                range = "A1:E1";
            }

            else if (count == 4)
            {
                range = "A1:F1";
            }

            else if (count == 5)
            {
                range = "A1:G1";
            }

            else if (count == 6)
            {
                range = "A1:H1";
            }

            else if (count == 7)
            {
                range = "A1:I1";
            }

            else if (count == 8)
            {
                range = "A1:J1";
            }

            else if (count == 9)
            {
                range = "A1:K1";
            }

            else if (count == 10)
            {
                range = "A1:L1";
            }
            #endregion
            
            if (type == "Data")
            {
                if (tariffLines != null && tariffLines.Count > 0)
                {
                    foreach (var item in tariffLines)
                    {
                        DataRow row = table.NewRow();
                        row[0] = item.OriginPortCode ?? null;
                        row[1] = item.DestinationPortCode ?? null;

                        if (count == 1)
                        {
                            if (item.Surcharge1Price.HasValue)
                            {
                                row[2] = item.Surcharge1Price ?? null;
                            }
                        }

                        else if (count == 2)
                        {
                            if (item.Surcharge2Price.HasValue)
                            {
                                row[3] = item.Surcharge2Price ?? null;
                            }
                        }

                        else if (count == 3)
                        {
                            if (item.Surcharge3Price.HasValue)
                            {
                                row[4] = item.Surcharge3Price ?? null;
                            }
                        }

                        else if (count == 4)
                        {
                            if (item.Surcharge4Price.HasValue)
                            {
                                row[5] = item.Surcharge4Price ?? null;
                            }
                        }

                        else if (count == 5)
                        {
                            if (item.Surcharge5Price.HasValue)
                            {
                                row[6] = item.Surcharge5Price ?? null;
                            }
                        }

                        else if (count == 6)
                        {
                            if (item.Surcharge6Price.HasValue)
                            {
                                row[7] = item.Surcharge6Price ?? null;
                            }
                        }

                        else if (count == 7)
                        {
                            if (item.Surcharge7Price.HasValue)
                            {
                                row[8] = item.Surcharge7Price ?? null;
                            }
                        }

                        else if (count == 8)
                        {
                            if (item.Surcharge8Price.HasValue)
                            {
                                row[9] = item.Surcharge8Price ?? null;
                            }
                        }

                        else if (count == 9)
                        {
                            if (item.Surcharge9Price.HasValue)
                            {
                                row[10] = item.Surcharge9Price ?? null;
                            }
                        }

                        else if (count == 10)
                        {
                            if (item.Surcharge10Price.HasValue)
                            {
                                row[11] = item.Surcharge10Price ?? null;
                            }
                        } 

                        table.Rows.Add(row);
                    }
                }
            }

            sheet1.Range[range].CellStyle.Font.Color = ExcelKnownColors.White;
            sheet1.Range[range].CellStyle.Color = System.Drawing.Color.Gray;
            sheet1.Range[range].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet1.ImportDataTable(table, true, 1, 1);
            workbook.Version = ExcelVersion.Excel2007;
            workbook.SaveAs(memory);
            return memory.ToArray();
        }
        
        public HttpResponseMessage GetApproveVersion(string tariffId, int version)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);

                string loggedContactId = null;
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (loggedContact != null)
                {
                    loggedContactId = loggedContact.Id;
                }

                ITariffModuleContext context = TariffModuleContext.GetContext(tenant);
                TariffVersionRepository tariffVersionRepository = new TariffVersionRepository(context);

                TariffVersion tariffVersion = tariffVersionRepository.GetSingle(tariffId, version, tenant);
                if (tariffVersion != null)
                {
                    tariffVersion.IsDraft = false;
                    tariffVersion.ApprovedByUserId = loggedContactId;
                    tariffVersion.ApproveDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                    tariffVersionRepository.Update(tariffVersion);
                    tariffVersionRepository.SubmitChanges();

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "VNAP",
                        UserId = loggedContactId,
                        EntityId = tariffId,
                        ObjectTableName = "Tariff",
                        Notes = "Version " + tariffVersion.Version + " approved",
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        
        [ActionName("PostUploadExcelFile")]
        public HttpResponseMessage PostUploadExcelFile(TariffFilterParameter filter)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                this.portRepository = new PortRepository(authToken.Tenant);

                filter.Tenant = authToken.Tenant;
                byte[] fileData = Convert.FromBase64String(filter.FileData);

                System.IO.MemoryStream stream = new System.IO.MemoryStream(fileData);
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
                IWorksheet sheet = workbook.Worksheets[0];

                List<ExcelTariffLines> tariffLinesResult = new List<ExcelTariffLines>();
                if (filter.TariffType == "AFC")
                {
                    tariffLinesResult = this.BuildAirFreightCostExcelLines(sheet, authToken.Tenant);
                }

                else if (filter.TariffType == "ASC")
                {
                    tariffLinesResult = this.BuildAirSurchargesCostExcelLines(sheet, authToken.Tenant);
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, tariffLinesResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private List<ExcelTariffLines> BuildAirFreightCostExcelLines(IWorksheet sheet, int tenant)
        {
            List<ExcelTariffLines> myResult = new List<ExcelTariffLines>();

            foreach (IRange row in sheet.UsedRange.Rows.Skip(1))
            {
                String[] rowData = new String[sheet.Columns.Count()];
                ExcelTariffLines tariffLine = new ExcelTariffLines();

                for (int i = 0; i < sheet.Columns.Count(); i++)
                {
                    rowData[i] = row.Cells[i].Value2.ToString();
                }

                Port fromPort = this.GetPortDetails(rowData[0], tenant);
                if (fromPort != null)
                {
                    tariffLine.FromPortId = fromPort.Id;
                    tariffLine.FromPortCode = fromPort.Code;
                    tariffLine.FromPortName = fromPort.EnglishName;
                }
                else
                {
                    tariffLine.FromPortText = this.TrimTo_20(rowData[0]);
                }

                Port toPort = this.GetPortDetails(rowData[1], tenant);
                if (toPort != null)
                {
                    tariffLine.ToPortId = toPort.Id;
                    tariffLine.ToPortCode = toPort.Code;
                    tariffLine.ToPortName = toPort.EnglishName;
                }
                else
                {
                    tariffLine.ToPortText = this.TrimTo_20(rowData[1]);
                }

                if (rowData.Length > 2)
                {
                    if (this.IsNumber(rowData[2]))
                    {
                        tariffLine.MinPrice = Convert.ToDecimal(rowData[2]);
                    }
                    else
                    {
                        tariffLine.MinPriceText = this.TrimTo_20(rowData[2]);
                    }
                }

                if (rowData.Length > 3)
                {
                    if (this.IsNumber(rowData[3]))
                    {
                        tariffLine.Step1Price = Convert.ToDecimal(rowData[3]);
                    }
                    else
                    {
                        tariffLine.Step1PriceText = this.TrimTo_20(rowData[3]);
                    }
                }

                if (rowData.Length > 4)
                {
                    if (this.IsNumber(rowData[4]))
                    {
                        tariffLine.Step2Price = Convert.ToDecimal(rowData[4]);
                    }
                    else
                    {
                        tariffLine.Step2PriceText = this.TrimTo_20(rowData[4]);
                    }
                }

                if (rowData.Length > 5)
                {
                    if (this.IsNumber(rowData[5]))
                    {
                        tariffLine.Step3Price = Convert.ToDecimal(rowData[5]);
                    }
                    else
                    {
                        tariffLine.Step3PriceText = this.TrimTo_20(rowData[5]);
                    }
                }

                if (rowData.Length > 6)
                {
                    if (this.IsNumber(rowData[6]))
                    {
                        tariffLine.Step4Price = Convert.ToDecimal(rowData[6]);
                    }
                    else
                    {
                        tariffLine.Step4PriceText = this.TrimTo_20(rowData[6]);
                    }
                }

                if (rowData.Length > 7)
                {
                    if (this.IsNumber(rowData[7]))
                    {
                        tariffLine.Step5Price = Convert.ToDecimal(rowData[7]);
                    }
                    else
                    {
                        tariffLine.Step5PriceText = this.TrimTo_20(rowData[7]);
                    }
                }

                if (rowData.Length > 8)
                {
                    if (this.IsNumber(rowData[8]))
                    {
                        tariffLine.Step6Price = Convert.ToDecimal(rowData[8]);
                    }
                    else
                    {
                        tariffLine.Step6PriceText = this.TrimTo_20(rowData[8]);
                    }
                }

                if (rowData.Length > 9)
                {
                    if (this.IsNumber(rowData[9]))
                    {
                        tariffLine.Step7Price = Convert.ToDecimal(rowData[9]);
                    }
                    else
                    {
                        tariffLine.Step7PriceText = this.TrimTo_20(rowData[9]);
                    }
                }

                if (rowData.Length >= 10)
                {
                    if (this.IsNumber(rowData[10]))
                    {
                        tariffLine.Step8Price = Convert.ToDecimal(rowData[10]);
                    }
                    else
                    {
                        tariffLine.Step8PriceText = this.TrimTo_20(rowData[10]);
                    }
                }

                myResult.Add(tariffLine);
            }

            foreach (ExcelTariffLines item in myResult)
            {
                this.SetErrors_AirFreightCost(item);
            }

            return myResult;
        }
        private List<ExcelTariffLines> BuildAirSurchargesCostExcelLines(IWorksheet sheet, int tenant)
        {
            List<ExcelTariffLines> myResult = new List<ExcelTariffLines>();

            foreach (IRange row in sheet.UsedRange.Rows.Skip(1))
            {
                String[] rowData = new String[sheet.Columns.Count()];
                ExcelTariffLines tariffLine = new ExcelTariffLines();

                for (int i = 0; i < sheet.Columns.Count(); i++)
                {
                    rowData[i] = row.Cells[i].Value2.ToString();
                }

                Port fromPort = this.GetPortDetails(rowData[0], tenant);
                if (fromPort != null)
                {
                    tariffLine.FromPortId = fromPort.Id;
                    tariffLine.FromPortCode = fromPort.Code;
                    tariffLine.FromPortName = fromPort.EnglishName;
                }
                else
                {
                    tariffLine.FromPortText = this.TrimTo_20(rowData[0]);
                }

                Port toPort = this.GetPortDetails(rowData[1], tenant);
                if (toPort != null)
                {
                    tariffLine.ToPortId = toPort.Id;
                    tariffLine.ToPortCode = toPort.Code;
                    tariffLine.ToPortName = toPort.EnglishName;
                }
                else
                {
                    tariffLine.ToPortText = this.TrimTo_20(rowData[1]);
                }

                if (rowData.Length > 2)
                {
                    if (this.IsNumber(rowData[2]))
                    {
                        tariffLine.Surcharge1Price = Convert.ToDecimal(rowData[2]);
                    }
                    else
                    {
                        tariffLine.Surcharge1PriceText = this.TrimTo_20(rowData[2]);
                    }
                }

                if (rowData.Length > 3)
                {
                    if (this.IsNumber(rowData[3]))
                    {
                        tariffLine.Surcharge2Price = Convert.ToDecimal(rowData[3]);
                    }
                    else
                    {
                        tariffLine.Surcharge2PriceText = this.TrimTo_20(rowData[3]);
                    }
                }

                if (rowData.Length > 4)
                {
                    if (this.IsNumber(rowData[4]))
                    {
                        tariffLine.Surcharge3Price = Convert.ToDecimal(rowData[4]);
                    }
                    else
                    {
                        tariffLine.Surcharge3PriceText = this.TrimTo_20(rowData[4]);
                    }
                }

                if (rowData.Length > 5)
                {
                    if (this.IsNumber(rowData[5]))
                    {
                        tariffLine.Surcharge4Price = Convert.ToDecimal(rowData[5]);
                    }
                    else
                    {
                        tariffLine.Surcharge4PriceText = this.TrimTo_20(rowData[5]);
                    }
                }

                if (rowData.Length > 6)
                {
                    if (this.IsNumber(rowData[6]))
                    {
                        tariffLine.Surcharge5Price = Convert.ToDecimal(rowData[6]);
                    }
                    else
                    {
                        tariffLine.Surcharge5PriceText = this.TrimTo_20(rowData[6]);
                    }
                }

                if (rowData.Length > 7)
                {
                    if (this.IsNumber(rowData[7]))
                    {
                        tariffLine.Surcharge6Price = Convert.ToDecimal(rowData[7]);
                    }
                    else
                    {
                        tariffLine.Surcharge6PriceText = this.TrimTo_20(rowData[7]);
                    }
                }

                if (rowData.Length > 8)
                {
                    if (this.IsNumber(rowData[8]))
                    {
                        tariffLine.Surcharge7Price = Convert.ToDecimal(rowData[8]);
                    }
                    else
                    {
                        tariffLine.Surcharge7PriceText = this.TrimTo_20(rowData[8]);
                    }
                }

                if (rowData.Length > 9)
                {
                    if (this.IsNumber(rowData[9]))
                    {
                        tariffLine.Surcharge8Price = Convert.ToDecimal(rowData[9]);
                    }
                    else
                    {
                        tariffLine.Surcharge8PriceText = this.TrimTo_20(rowData[9]);
                    }
                }

                if (rowData.Length >= 10)
                {
                    if (this.IsNumber(rowData[10]))
                    {
                        tariffLine.Surcharge9Price = Convert.ToDecimal(rowData[10]);
                    }
                    else
                    {
                        tariffLine.Surcharge9PriceText = this.TrimTo_20(rowData[10]);
                    }
                }

                if (rowData.Length >= 11)
                {
                    if (this.IsNumber(rowData[11]))
                    {
                        tariffLine.Surcharge10Price = Convert.ToDecimal(rowData[11]);
                    }
                    else
                    {
                        tariffLine.Surcharge10PriceText = this.TrimTo_20(rowData[11]);
                    }
                }

                myResult.Add(tariffLine);
            }

            foreach (ExcelTariffLines item in myResult)
            {
                this.SetErrors_AirSurchargesCost(item);
            }

            return myResult;
        }        

        private void SetErrors_AirFreightCost(ExcelTariffLines item)
        {
            bool error = false;
            string errorText = "";

            if (!string.IsNullOrEmpty(item.FromPortText) && string.IsNullOrEmpty(item.FromPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Port with code " + item.FromPortText + " not found";
                }

                else
                {
                    errorText = errorText + ", Port with code " + item.FromPortText + " not found";
                }
            }

            if (!string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Port with code " + item.ToPortText + " not found";
                }

                else
                {
                    errorText = errorText + ", Port with code " + item.ToPortText + " not found";
                }
            }

            if (!string.IsNullOrEmpty(item.MinPriceText) && item.MinPrice == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Min price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Min price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step1PriceText) && item.Step1Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 1 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 1 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step2PriceText) && item.Step2Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 2 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 2 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step3PriceText) && item.Step3Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 3 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 3 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step4PriceText) && item.Step4Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 4 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 4 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step5PriceText) && item.Step5Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 5 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 5 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step6PriceText) && item.Step6Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 6 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 6 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step7PriceText) && item.Step7Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 7 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 7 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step8PriceText) && item.Step8Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 8 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 8 price format is invalid";
                }
            }

            item.HasErrors = error;
            item.ErrorText = errorText;
        }
        private void SetErrors_AirSurchargesCost(ExcelTariffLines item)
        {
            bool error = false;
            string errorText = "";

            if (!string.IsNullOrEmpty(item.FromPortText) && string.IsNullOrEmpty(item.FromPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Port with code " + item.FromPortText + " not found";
                }

                else
                {
                    errorText = errorText + ", Port with code " + item.FromPortText + " not found";
                }
            }

            if (!string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Port with code " + item.ToPortText + " not found";
                }

                else
                {
                    errorText = errorText + ", Port with code " + item.ToPortText + " not found";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge1PriceText) && item.Surcharge1Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 1 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 1 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge2PriceText) && item.Surcharge2Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 2 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 2 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge3PriceText) && item.Surcharge3Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 3 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 3 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge4PriceText) && item.Surcharge4Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 4 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 4 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge5PriceText) && item.Surcharge5Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 5 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 5 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge6PriceText) && item.Surcharge6Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 6 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 6 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge7PriceText) && item.Surcharge7Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 7 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 7 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge8PriceText) && item.Surcharge8Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 8 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 8 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge9PriceText) && item.Surcharge9Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 9 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 9 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge10PriceText) && item.Surcharge10Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 10 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 10 price format is invalid";
                }
            }

            item.HasErrors = error;
            item.ErrorText = errorText;
        }

        private Port GetPortDetails(string code, int tenant)
        {
            Port myPort = null;

            myPort = this.portRepository.GetSinglePortByCode(tenant, code, true);
            if (myPort == null)
            {
                Port portZero = this.portRepository.GetSinglePortByCode(0, code, true);
                if (portZero != null)
                {
                    myPort = this.GetPortCopyToCurrentTenant(portZero, tenant);
                }
            }

            return myPort;
        }
        private Port GetPortCopyToCurrentTenant(Port ZeroPort, int tenant)
        {
            ICommonDataContext objectContext = this.portRepository.context;

            CountryRepository countryRepository = new CountryRepository(objectContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(objectContext);

            Port newPort;
            newPort = portRepository.GetSinglePortByCodeCountryCode(tenant, ZeroPort.Code, ZeroPort.Country.Code, false);
            Country country = null;

            if (newPort == null)
            {
                country = countryRepository.GetSingleCountryByCode(ZeroPort.Country.Code, tenant, false);

                if (country == null)
                {
                    GlobalZone globalzone = globalZoneRepository.GetSingleGlobalZoneByCode(ZeroPort.Country.GlobalZone.Code, tenant);

                    if (globalzone == null)
                    {
                        GlobalZone oldZone = globalZoneRepository.GetSingleGlobalZone(ZeroPort.Country.GlobalZoneId, 0);
                        globalzone = new GlobalZone()
                        {
                            Id = IdCounter.GetNumber("GlobalZone", tenant).ToString(),
                            Code = oldZone.Code,
                            EnglishName = oldZone.EnglishName,
                            LocalName = oldZone.LocalName,
                            Notes = oldZone.Notes,
                            SearchFields = oldZone.SearchFields,
                            Tenant = tenant,
                        };

                        globalZoneRepository.Add(globalzone);
                        globalZoneRepository.SubmitChanges();
                    }

                    Country oldCountry = CountryRepository.GetSingleCountry(ZeroPort.CountryId, 0, false);
                    country = new Country()
                    {
                        Id = IdCounter.GetNumber("Country", tenant).ToString(),
                        Tenant = tenant,
                        GlobalZoneId = oldCountry.GlobalZoneId,
                        EC = oldCountry.EC,
                        EnglishName = oldCountry.EnglishName,
                        Code = oldCountry.Code,
                        InActive = oldCountry.InActive,
                        Notes = oldCountry.Notes,
                        LocalName = oldCountry.LocalName,
                        SearchFields = oldCountry.SearchFields,
                    };

                    countryRepository.Add(country);
                    countryRepository.SubmitChanges();
                }

                newPort = new Port()
                {
                    Id = IdCounter.GetNumber("Port", tenant).ToString(),
                    Code = ZeroPort.Code,
                    EnglishName = ZeroPort.EnglishName,
                    LocalName = ZeroPort.LocalName,
                    Tenant = tenant,
                    AddedManually = false,
                    InActive = false,
                    CountryId = country.Id,
                    IsAir = ZeroPort.IsAir,
                    IsInland = ZeroPort.IsInland,
                    IsOcean = ZeroPort.IsOcean,
                    Latitude = ZeroPort.Latitude,
                    Longtitude = ZeroPort.Longtitude,
                    SearchFields = ZeroPort.SearchFields,
                    Notes = ZeroPort.Notes,
                };

                portRepository.Add(newPort);
                portRepository.SubmitChanges();
            }

            if (country == null)
            {
                country = countryRepository.GetSingleCountryByCode(ZeroPort.Country.Code, tenant, false);
            }

            return newPort;
        }
        private string TrimTo_20(string text)
        {
            string trimmedText = text;

            if (!string.IsNullOrEmpty(text))
            {
                if (text.Length > 20)
                {
                    trimmedText = text.Substring(0, 20);
                }
            }

            return trimmedText;
        }
        private bool IsNumber(string text)
        {
            bool isNumber = false;

            if (!string.IsNullOrEmpty(text))
            {
                decimal value;
                if (Decimal.TryParse(text, out value))
                {
                    isNumber = true;
                }
            }

            return isNumber;
        }
    }

    public class TariffFilterParameter
    {
        public int Tenant { get; set; }
        public string FileData { get; set; }
        public string PriceSteps { get; set; }
        public string TariffId { get; set; }
        public int Version { get; set; }
        public string TariffType { get; set; }
    }
    public class ExcelTariffLines
    {
        public string FromPortId { get; set; }
        public string FromPortCode { get; set; }
        public string FromPortName { get; set; }
        public string ToPortId { get; set; }
        public string ToPortCode { get; set; }
        public string ToPortName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? Step1Price { get; set; }
        public decimal? Step2Price { get; set; }
        public decimal? Step3Price { get; set; }
        public decimal? Step4Price { get; set; }
        public decimal? Step5Price { get; set; }
        public decimal? Step6Price { get; set; }
        public decimal? Step7Price { get; set; }
        public decimal? Step8Price { get; set; }

        public string FromPortText { get; set; }
        public string ToPortText { get; set; }
        public string MinPriceText { get; set; }
        public string Step1PriceText { get; set; }
        public string Step2PriceText { get; set; }
        public string Step3PriceText { get; set; }
        public string Step4PriceText { get; set; }
        public string Step5PriceText { get; set; }
        public string Step6PriceText { get; set; }
        public string Step7PriceText { get; set; }
        public string Step8PriceText { get; set; }

        public bool HasErrors { get; set; }
        public string ErrorText { get; set; }

        public decimal? Surcharge1Price { get; set; }
        public decimal? Surcharge2Price { get; set; }
        public decimal? Surcharge3Price { get; set; }
        public decimal? Surcharge4Price { get; set; }
        public decimal? Surcharge5Price { get; set; }
        public decimal? Surcharge6Price { get; set; }
        public decimal? Surcharge7Price { get; set; }
        public decimal? Surcharge8Price { get; set; }
        public decimal? Surcharge9Price { get; set; }
        public decimal? Surcharge10Price { get; set; }

        public string Surcharge1PriceText { get; set; }
        public string Surcharge2PriceText { get; set; }
        public string Surcharge3PriceText { get; set; }
        public string Surcharge4PriceText { get; set; }
        public string Surcharge5PriceText { get; set; }
        public string Surcharge6PriceText { get; set; }
        public string Surcharge7PriceText { get; set; }
        public string Surcharge8PriceText { get; set; }
        public string Surcharge9PriceText { get; set; }
        public string Surcharge10PriceText { get; set; }
    }
}


