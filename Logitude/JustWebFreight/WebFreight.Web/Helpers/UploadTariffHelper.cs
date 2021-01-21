using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.TariffModule.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityQueryServices;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools;
using System.IO;

namespace WebFreight.Web.Helpers
{
    public class UploadTariffHelper
    {
        private int tenant;
        private string priceSteps;
        private string tariffType;
        private string fileData;
        private string tariffId;
        private string carrierId;
        private int version;        
        private string fileName;
        private string fileExtension;
        private MemoryStream memoryStream;
        private TariffQueryService tariffQueryService;
        private TariffCarrierTranslationRepository tariffCarrierTranslationRepository;
        private PortRepository portRepository;
        private ICommonDataContext commonContext;
        private ITariffModuleContext tariffContext;
        public UploadTariffHelper(TariffFilterParameter filterParameter)
        {
            tenant = filterParameter.Tenant;
            fileData = filterParameter.FileData;
            priceSteps = filterParameter.PriceSteps;
            tariffType = filterParameter.TariffType;
            tariffId = filterParameter.TariffId;
            version = filterParameter.Version;
            fileName = filterParameter.FileName;
            fileExtension = filterParameter.FileExtension;

            this.tariffContext = TariffModuleContext.GetContext(tenant);
            this.commonContext = CommonDataContext.GetContext(tenant);

            this.tariffQueryService = new TariffQueryService(tariffContext);
            this.portRepository = new PortRepository(commonContext);
            this.tariffCarrierTranslationRepository = new TariffCarrierTranslationRepository(commonContext);

            this.SetCarrierId();
            this.GetTariffAndVersion();
            this.InitDates();
        }

        private void SetCarrierId()
        {
            TariffRepository tariffRepository = new TariffRepository(tenant);
            Tariff tariff = tariffRepository.GetSingle(tariffId, tenant);
            if (tariff != null)
            {
                carrierId = tariff.SellerId;
            }
        }

        private TariffPM tariffPM;
        private TariffVersionPM versionPM;
        private void GetTariffAndVersion()
        {
            tariffPM = tariffQueryService.GetSingle(tariffId, true, false);
            versionPM = tariffPM.TariffVersions.Where(d => d.Version == version).FirstOrDefault();
        }

        private DateTime? startDate;
        private DateTime? expirationDate;
        private void InitDates()
        {
            startDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            expirationDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            if(versionPM != null)
            {
                startDate = versionPM.StartDate;
                expirationDate = versionPM.InitialEnddate;
            }
        }

        public void Upload()
        {
            byte[] fileDataArray = Convert.FromBase64String(fileData);
            memoryStream = new MemoryStream(fileDataArray);
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(memoryStream);
            IWorksheet sheet = workbook.Worksheets[0];

            if (sheet.UsedRange.Rows.Count() - 1 > 1000)
            {
                throw new ApplicationException("Can't upload this excel as it exceeds tariff lines limitation of 1000");
            }

            else
            {
                List<TariffLinePM> tariffLinesResult = new List<TariffLinePM>();
                List<ExcelSheetLine> excelSheetLines = new List<ExcelSheetLine>();
                if (tariffType == "AFC" || tariffType == "OLC")
                {
                    excelSheetLines = this.ReadExcelSheetData_LCL(sheet);
                    tariffLinesResult = this.BuildTariffLines_LCL(excelSheetLines);
                }

                else if (tariffType == "OFC")
                {
                    excelSheetLines = this.ReadExcelSheetData_FCL(sheet);
                    tariffLinesResult = this.BuildTariffLines_FCL(excelSheetLines);
                }

                this.CreateTariffUploadExcel(sheet.UsedRange.Rows.Count() - 1);
                this.SaveTariff(tariffLinesResult);
            }
        }

        private List<ExcelSheetLine> ReadExcelSheetData_LCL(IWorksheet sheet)
        {
            List<ExcelSheetLine> excelSheetLines = new List<ExcelSheetLine>();

            string notescolumn = sheet.Columns[sheet.Columns.Count() - 1].DisplayText;
            string transitTimecolumn = sheet.Columns[sheet.Columns.Count() - 2].DisplayText;

            foreach (IRange row in sheet.UsedRange.Rows.Skip(1))
            {
                String[] rowData = new String[sheet.Columns.Count() - 1];
                ExcelSheetLine myLine = new ExcelSheetLine();
                var rowDataLength = rowData.Length;
                var StepLength = rowData.Length;
                if (!string.IsNullOrEmpty(priceSteps))
                {
                    StepLength = priceSteps.Split(',').Length + 3;
                }

                String notesRowData = row.Cells[sheet.Columns.Count() - 1].Value2.ToString();
                String transitTimeRowData = row.Cells[sheet.Columns.Count() - 2].Value2.ToString();

                for (int i = 0; i < sheet.Columns.Count() - 2; i++)
                {
                    if (row.Cells[i].HasFormula)
                    {
                        rowData[i] = row.Cells[i].FormulaNumberValue.ToString();
                    }
                    else
                    {
                        rowData[i] = row.Cells[i].Value2.ToString();
                    }
                }

                /*From Port*/
                string fromPortCode = rowData[0];
                fromPortCode = Regex.Replace(fromPortCode, @"\*+", "");
                myLine.FromPort = fromPortCode;

                /*To Port*/
                string toPortCode = rowData[1];
                toPortCode = Regex.Replace(toPortCode, @"\*+", "");
                myLine.ToPort = toPortCode;

                if (StepLength > 2 && rowDataLength > 2)
                {
                    myLine.MinPrice = rowData[2];
                }

                if (StepLength > 3 && rowDataLength > 3)
                {
                    myLine.Price1 = rowData[3];
                }

                if (StepLength > 4 && rowDataLength > 4)
                {
                    myLine.Price2 = rowData[4];
                }

                if (StepLength > 5 && rowDataLength > 5)
                {
                    myLine.Price3 = rowData[5];
                }

                if (StepLength > 6 && rowDataLength > 6)
                {
                    myLine.Price4 = rowData[6];
                }

                if (StepLength > 7 && rowDataLength > 7)
                {
                    myLine.Price5 = rowData[7];
                }

                if (StepLength > 8 && rowDataLength > 8)
                {
                    myLine.Price6 = rowData[8];
                }

                if (StepLength > 9 && rowDataLength > 9)
                {
                    myLine.Price7 = rowData[9];
                }

                if (StepLength > 10 && rowDataLength > 10)
                {
                    myLine.Price8 = rowData[10];
                }

                if (!string.IsNullOrEmpty(transitTimecolumn))
                {
                    myLine.TransitTime = transitTimeRowData;
                }

                if (!string.IsNullOrEmpty(notescolumn))
                {
                    myLine.Notes = notesRowData;
                }

                excelSheetLines.Add(myLine);
            }

            return excelSheetLines;
        }
        private List<ExcelSheetLine> ReadExcelSheetData_FCL(IWorksheet sheet)
        {
            List<ExcelSheetLine> excelSheetLines = new List<ExcelSheetLine>();

            string notescolumn = sheet.Columns[sheet.Columns.Count() - 1].DisplayText;
            string transitTimecolumn = sheet.Columns[sheet.Columns.Count() - 2].DisplayText;

            foreach (IRange row in sheet.UsedRange.Rows.Skip(1))
            {
                String[] rowData = new String[sheet.Columns.Count() - 1];
                ExcelSheetLine myLine = new ExcelSheetLine();

                String notesRowData = row.Cells[sheet.Columns.Count() - 1].Value2.ToString();
                String transitTimeRowData = row.Cells[sheet.Columns.Count() - 2].Value2.ToString();

                for (int i = 0; i < sheet.Columns.Count() - 2; i++)
                {
                    if (row.Cells[i].HasFormula)
                    {
                        rowData[i] = row.Cells[i].FormulaNumberValue.ToString();
                    }
                    else
                    {
                        rowData[i] = row.Cells[i].Value2.ToString();
                    }
                }

                /*From Port*/
                string fromPortCode = rowData[0];
                fromPortCode = Regex.Replace(fromPortCode, @"\*+", "");
                myLine.FromPort = fromPortCode;

                /*To Port*/
                string toPortCode = rowData[1];
                toPortCode = Regex.Replace(toPortCode, @"\*+", "");
                myLine.ToPort = toPortCode;

                if (rowData.Length > 2)
                {
                    myLine.Price1 = rowData[2];
                }

                if (rowData.Length > 3)
                {
                    myLine.Price2 = rowData[3];
                }

                if (rowData.Length > 4)
                {
                    myLine.Price3 = rowData[4];
                }

                if (rowData.Length > 5)
                {
                    myLine.Price4 = rowData[5];
                }

                if (rowData.Length > 6)
                {
                    myLine.Price5 = rowData[6];
                }

                if (!string.IsNullOrEmpty(transitTimecolumn))
                {
                    myLine.TransitTime = transitTimeRowData;
                }

                if (!string.IsNullOrEmpty(notescolumn))
                {
                    myLine.Notes = notesRowData;
                }

                excelSheetLines.Add(myLine);
            }

            return excelSheetLines;
        }

        private List<TariffLinePM> BuildTariffLines_LCL(List<ExcelSheetLine> excelSheetLines)
        {
            List<TariffLinePM> myResult = new List<TariffLinePM>();

            int rowIndex = 0;

            foreach (ExcelSheetLine row in excelSheetLines)
            {
                TariffLinePM tariffLine = new TariffLinePM();
                tariffLine.ChangeSetOp = ChangeSetOperation.Insert;
                tariffLine.TariffId = tariffId;
                tariffLine.Tenant = tenant;
                tariffLine.Version = version;
                tariffLine.StartDate = startDate;
                tariffLine.ExpirationDate = expirationDate;
                tariffLine.Index = rowIndex;

                //From Port
                Port fromPort = this.GetPortDetails(row.FromPort, tenant);
                if (fromPort != null)
                {
                    if ((fromPort.IsAir && tariffType == "AFC") || (fromPort.IsOcean && tariffType == "OLC"))
                    {
                        tariffLine.OriginPortId = fromPort.Id;
                        tariffLine.OriginPortCode = fromPort.Code;
                        tariffLine.OriginPortCombinedCode = fromPort.CombinedCode;
                        tariffLine.OriginPortName = fromPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.OriginPortHasWrongTransMode = true;
                        tariffLine.OriginPortText = row.FromPort;
                    }
                }
                else
                {
                    tariffLine.OriginPortText = this.TrimTo_20(row.FromPort);
                }

                //To Port
                Port toPort = this.GetPortDetails(row.ToPort, tenant);
                if (toPort != null)
                {
                    if ((toPort.IsAir && tariffType == "AFC") || (toPort.IsOcean && tariffType == "OLC"))
                    {
                        tariffLine.DestinationPortId = toPort.Id;
                        tariffLine.DestinationPortCode = toPort.Code;
                        tariffLine.DestinationPortCombinedCode = toPort.CombinedCode;
                        tariffLine.DestinationPortName = toPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.DestinationPortHasWrongTransMode = true;
                        tariffLine.DestinationPortText = row.ToPort;
                    }
                }
                else
                {
                    tariffLine.DestinationPortText = this.TrimTo_20(row.ToPort);
                }

                //Min Price
                if (this.IsNumber(row.MinPrice))
                {
                    decimal myNumber = Convert.ToDecimal(row.MinPrice);
                    if (myNumber >= 0)
                    {
                        tariffLine.MinPrice = myNumber;
                    }

                    else
                    {
                        tariffLine.IsMinPriceMinus = true;
                        tariffLine.MinPriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.MinPriceText = this.TrimTo_20(row.MinPrice);
                }

                //Price 1
                if (this.IsNumber(row.Price1))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price1);

                    if (myNumber >= 0)
                    {
                        tariffLine.Step1Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice1Minus = true;
                        tariffLine.Step1PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Step1PriceText = this.TrimTo_20(row.Price1);
                }

                //Price 2
                if (this.IsNumber(row.Price2))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price2);

                    if (myNumber >= 0)
                    {
                        tariffLine.Step2Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice2Minus = true;
                        tariffLine.Step2PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Step2PriceText = this.TrimTo_20(row.Price2);
                }

                //Price 3
                if (this.IsNumber(row.Price3))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price3);

                    if (myNumber >= 0)
                    {
                        tariffLine.Step3Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice3Minus = true;
                        tariffLine.Step3PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Step3PriceText = this.TrimTo_20(row.Price3);
                }

                //Price 4
                if (this.IsNumber(row.Price4))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price4);

                    if (myNumber >= 0)
                    {
                        tariffLine.Step4Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice4Minus = true;
                        tariffLine.Step4PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Step4PriceText = this.TrimTo_20(row.Price4);
                }

                //Price 5
                if (this.IsNumber(row.Price5))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price5);

                    if (myNumber >= 0)
                    {
                        tariffLine.Step5Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice5Minus = true;
                        tariffLine.Step5PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Step5PriceText = this.TrimTo_20(row.Price5);
                }

                //Price 6
                if (this.IsNumber(row.Price6))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price6);

                    if (myNumber >= 0)
                    {
                        tariffLine.Step6Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice6Minus = true;
                        tariffLine.Step6PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Step6PriceText = this.TrimTo_20(row.Price6);
                }

                //Price 7
                if (this.IsNumber(row.Price7))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price7);

                    if (myNumber >= 0)
                    {
                        tariffLine.Step7Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice7Minus = true;
                        tariffLine.Step7PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Step7PriceText = this.TrimTo_20(row.Price7);
                }

                //Price 8
                if (this.IsNumber(row.Price8))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price8);

                    if (myNumber >= 0)
                    {
                        tariffLine.Step8Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice8Minus = true;
                        tariffLine.Step8PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Step8PriceText = this.TrimTo_20(row.Price8);
                }

                //Transit Time
                if (!string.IsNullOrEmpty(row.TransitTime))
                {
                    tariffLine.TransitTime = row.TransitTime;

                    if (row.TransitTime.Length > 100)
                    {
                        tariffLine.TransitTime = row.TransitTime.Substring(0, 100);
                    }
                }

                //Notes
                if (!string.IsNullOrEmpty(row.Notes))
                {
                    tariffLine.Notes = row.Notes;

                    if (row.Notes.Length > 500)
                    {
                        tariffLine.Notes = row.Notes.Substring(0, 500);
                    }
                }

                myResult.Add(tariffLine);
                rowIndex++;
            }

            foreach (TariffLinePM item in myResult)
            {
                this.SetErrors_LCL(item);
            }

            return myResult;
        }
        private List<TariffLinePM> BuildTariffLines_FCL(List<ExcelSheetLine> excelSheetLines)
        {
            List<TariffLinePM> myResult = new List<TariffLinePM>();

            int rowIndex = 0;

            foreach (ExcelSheetLine row in excelSheetLines)
            {
                TariffLinePM tariffLine = new TariffLinePM();
                tariffLine.ChangeSetOp = ChangeSetOperation.Insert;
                tariffLine.TariffId = tariffId;
                tariffLine.Tenant = tenant;
                tariffLine.Version = version;
                tariffLine.StartDate = startDate;
                tariffLine.ExpirationDate = expirationDate;
                tariffLine.Index = rowIndex;

                //From Port
                Port fromPort = this.GetPortDetails(row.FromPort, tenant);
                if (fromPort != null)
                {
                    if (fromPort.IsOcean)
                    {
                        tariffLine.OriginPortId = fromPort.Id;
                        tariffLine.OriginPortCode = fromPort.Code;
                        tariffLine.OriginPortCombinedCode = fromPort.CombinedCode;
                        tariffLine.OriginPortName = fromPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.OriginPortHasWrongTransMode = true;
                        tariffLine.OriginPortText = row.FromPort;
                    }
                }
                else
                {
                    tariffLine.OriginPortText = this.TrimTo_20(row.FromPort);
                }

                //To Port
                Port toPort = this.GetPortDetails(row.ToPort, tenant);
                if (toPort != null)
                {
                    if (toPort.IsOcean)
                    {
                        tariffLine.DestinationPortId = toPort.Id;
                        tariffLine.DestinationPortCode = toPort.Code;
                        tariffLine.DestinationPortCombinedCode = toPort.CombinedCode;
                        tariffLine.DestinationPortName = toPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.DestinationPortHasWrongTransMode = true;
                        tariffLine.DestinationPortText = row.ToPort;
                    }
                }
                else
                {
                    tariffLine.DestinationPortText = this.TrimTo_20(row.ToPort);
                }

                //Price 1
                if (this.IsNumber(row.Price1))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price1);

                    if (myNumber >= 0)
                    {
                        tariffLine.Surcharge1Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice1Minus = true;
                        tariffLine.Surcharge1PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Surcharge1PriceText = this.TrimTo_20(row.Price1);
                }

                //Price 2
                if (this.IsNumber(row.Price2))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price2);

                    if (myNumber >= 0)
                    {
                        tariffLine.Surcharge2Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice2Minus = true;
                        tariffLine.Surcharge2PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Surcharge2PriceText = this.TrimTo_20(row.Price2);
                }

                //Price 3
                if (this.IsNumber(row.Price3))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price3);

                    if (myNumber >= 0)
                    {
                        tariffLine.Surcharge3Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice3Minus = true;
                        tariffLine.Surcharge3PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Surcharge3PriceText = this.TrimTo_20(row.Price3);
                }

                //Price 4
                if (this.IsNumber(row.Price4))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price4);

                    if (myNumber >= 0)
                    {
                        tariffLine.Surcharge4Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice4Minus = true;
                        tariffLine.Surcharge4PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Surcharge4PriceText = this.TrimTo_20(row.Price4);
                }

                //Price 5
                if (this.IsNumber(row.Price5))
                {
                    decimal myNumber = Convert.ToDecimal(row.Price5);

                    if (myNumber >= 0)
                    {
                        tariffLine.Surcharge5Price = myNumber;
                    }
                    else
                    {
                        tariffLine.IsPrice5Minus = true;
                        tariffLine.Surcharge5PriceText = String.Format("{0:0.000}", myNumber);
                    }
                }
                else
                {
                    tariffLine.Surcharge5PriceText = this.TrimTo_20(row.Price5);
                }

                //Transit Time
                if (!string.IsNullOrEmpty(row.TransitTime))
                {
                    tariffLine.TransitTime = row.TransitTime;

                    if (row.TransitTime.Length > 100)
                    {
                        tariffLine.TransitTime = row.TransitTime.Substring(0, 100);
                    }
                }

                //Notes
                if (!string.IsNullOrEmpty(row.Notes))
                {
                    tariffLine.Notes = row.Notes;

                    if (row.Notes.Length > 500)
                    {
                        tariffLine.Notes = row.Notes.Substring(0, 500);
                    }
                }

                myResult.Add(tariffLine);
                rowIndex++;
            }

            foreach (TariffLinePM item in myResult)
            {
                this.SetErrors_FCL(item);
            }

            return myResult;
        }
        
        private void SetErrors_LCL(TariffLinePM item)
        {
            bool error = false;
            string errorText = "";

            if (!string.IsNullOrEmpty(item.OriginPortText) && string.IsNullOrEmpty(item.OriginPortId))
            {
                error = true;

                if (item.OriginPortHasWrongTransMode)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port Not Found";
                    }

                    else
                    {
                        errorText = errorText + ", Port Not Found";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port with code " + item.OriginPortText + " not found";
                    }

                    else
                    {
                        errorText = errorText + ", Port with code " + item.OriginPortText + " not found";
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.OriginPortText) && string.IsNullOrEmpty(item.OriginPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Missing Origin Port";
                }

                else
                {
                    errorText = errorText + ", Missing Origin Port";
                }
            }

            if (!string.IsNullOrEmpty(item.DestinationPortText) && string.IsNullOrEmpty(item.DestinationPortId))
            {
                error = true;

                if (item.DestinationPortHasWrongTransMode)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port Not Found";
                    }

                    else
                    {
                        errorText = errorText + ", Port Not Found";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port with code " + item.DestinationPortText + " not found";
                    }

                    else
                    {
                        errorText = errorText + ", Port with code " + item.DestinationPortText + " not found";
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.DestinationPortText) && string.IsNullOrEmpty(item.DestinationPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Missing Destination Port";
                }

                else
                {
                    errorText = errorText + ", Missing Destination Port";
                }
            }

            if (!string.IsNullOrEmpty(item.MinPriceText) && item.MinPrice == null)
            {
                error = true;

                if (item.IsMinPriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Min price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Min price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Min price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Min price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step1PriceText) && item.Step1Price == null)
            {
                error = true;

                if (item.IsPrice1Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 1 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 1 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 1 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 1 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step2PriceText) && item.Step2Price == null)
            {
                error = true;

                if (item.IsPrice2Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 2 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 2 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 2 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 2 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step3PriceText) && item.Step3Price == null)
            {
                error = true;

                if (item.IsPrice3Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 3 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 3 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 3 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 3 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step4PriceText) && item.Step4Price == null)
            {
                error = true;

                if (item.IsPrice4Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 4 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 4 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 4 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 4 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step5PriceText) && item.Step5Price == null)
            {
                error = true;

                if (item.IsPrice5Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 5 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 5 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 5 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 5 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step6PriceText) && item.Step6Price == null)
            {
                error = true;

                if (item.IsPrice6Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 6 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 6 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 6 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 6 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step7PriceText) && item.Step7Price == null)
            {
                error = true;

                if (item.IsPrice7Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 7 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 7 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 7 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 7 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step8PriceText) && item.Step8Price == null)
            {
                error = true;

                if (item.IsPrice8Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 8 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 8 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 8 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 8 price format is invalid";
                    }
                }
            }

            item.HasErrors = error;
            item.ErrorText = errorText;
        }
        private void SetErrors_FCL(TariffLinePM item)
        {
            bool error = false;
            string errorText = "";

            if (!string.IsNullOrEmpty(item.OriginPortText) && string.IsNullOrEmpty(item.OriginPortId))
            {
                error = true;

                if (item.OriginPortHasWrongTransMode)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port Not Found";
                    }

                    else
                    {
                        errorText = errorText + ", Port Not Found";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port with code " + item.OriginPortText + " not found";
                    }

                    else
                    {
                        errorText = errorText + ", Port with code " + item.OriginPortText + " not found";
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.OriginPortText) && string.IsNullOrEmpty(item.OriginPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Missing Origin Port";
                }

                else
                {
                    errorText = errorText + ", Missing Origin Port";
                }
            }

            if (!string.IsNullOrEmpty(item.DestinationPortText) && string.IsNullOrEmpty(item.DestinationPortId))
            {
                error = true;

                if (item.DestinationPortHasWrongTransMode)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port Not Found";
                    }

                    else
                    {
                        errorText = errorText + ", Port Not Found";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port with code " + item.DestinationPortText + " not found";
                    }

                    else
                    {
                        errorText = errorText + ", Port with code " + item.DestinationPortText + " not found";
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.DestinationPortText) && string.IsNullOrEmpty(item.DestinationPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Missing Destination Port";
                }

                else
                {
                    errorText = errorText + ", Missing Destination Port";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge1PriceText) && item.Surcharge1Price == null)
            {
                error = true;

                if (item.IsPrice1Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 1 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Container 1 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 1 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Container 1 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge2PriceText) && item.Surcharge2Price == null)
            {
                error = true;

                if (item.IsPrice2Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 2 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Container 2 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 2 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Container 2 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge3PriceText) && item.Surcharge3Price == null)
            {
                error = true;

                if (item.IsPrice3Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 3 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Container 3 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 3 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Container 3 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge4PriceText) && item.Surcharge4Price == null)
            {
                error = true;

                if (item.IsPrice4Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 4 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Container 4 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 4 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Container 4 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge5PriceText) && item.Surcharge5Price == null)
            {
                error = true;

                if (item.IsPrice5Minus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 5 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Container 5 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 5 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Container 5 price format is invalid";
                    }
                }
            }

            item.HasErrors = error;
            item.ErrorText = errorText;
        }

        private Port GetPortDetails(string code, int tenant)
        {
            Port myPort = null;

            if (!string.IsNullOrEmpty(code))
            {
                code = code.Trim();
                if (tariffType == "AFC")
                {
                    myPort = this.portRepository.GetAirlinePortByCode(tenant, code, true);
                }

                else if (tariffType == "OLC" || tariffType == "OFC")
                {
                    myPort = this.portRepository.GetOceanPortByCombinedCode(code, tenant);
                }

                //Search in translations
                if (myPort == null)
                {
                    TariffCarrierTranslation carrierTranslation = tariffCarrierTranslationRepository.GetCarrierTranslationByPartnerCodeAndCarrier(code, carrierId, tenant);
                    if (carrierTranslation != null)
                    {
                        myPort = this.portRepository.GetSinglePort(tenant, carrierTranslation.PortId);
                    }
                }

                // Search for names
                if (myPort == null)
                {
                    IQueryable<Port> ports = null;

                    if (tariffType == "AFC")
                    {
                        ports = this.portRepository.GetAirlinePortsByName(code, tenant);
                    }

                    else if (tariffType == "OLC" || tariffType == "OFC")
                    {
                        ports = this.portRepository.GetOceanPortsByName(code, tenant);
                    }

                    if (ports != null && ports.Count() == 1)
                    {
                        myPort = ports.FirstOrDefault();
                    }
                }

                if (myPort == null)
                {
                    Port portZero = null;
                    if (tariffType == "AFC")
                    {
                        portZero = this.portRepository.GetAirlinePortByCode(0, code, true);

                        if(portZero == null)
                        {
                            IQueryable<Port> ports = this.portRepository.GetAirlinePortsByName(code, 0);
                            if (ports != null && ports.Count() == 1)
                            {
                                portZero = ports.FirstOrDefault();
                            }
                        }
                    }

                    else if (tariffType == "OLC" || tariffType == "OFC")
                    {
                        portZero = this.portRepository.GetOceanPortByCombinedCode(code, 0);

                        if (portZero == null)
                        {
                            IQueryable<Port> ports = this.portRepository.GetOceanPortsByName(code, 0);
                            if (ports != null && ports.Count() == 1)
                            {
                                portZero = ports.FirstOrDefault();
                            }
                        }
                    }

                    if (portZero != null)
                    {
                        myPort = this.GetPortCopyToCurrentTenant(portZero, tenant);
                    }
                }
            }

            return myPort;
        }
        private Port GetPortCopyToCurrentTenant(Port ZeroPort, int tenant)
        {
            CountryRepository countryRepository = new CountryRepository(commonContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(commonContext);

            Country country = countryRepository.GetSingleCountryByCode(ZeroPort.Country.Code, tenant, false);

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

            Port newPort = new Port()
            {
                Id = IdCounter.GetNumber("Port", tenant).ToString(),
                Code = ZeroPort.Code,
                CombinedCode = ZeroPort.CombinedCode,
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

            TableLastUpdateClass.UpdateTableHistory(tenant, "Port");

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
        private bool IsDateTime(string text)
        {
            bool isDateTime = false;

            if (!string.IsNullOrEmpty(text))
            {
                DateTime value;
                if (DateTime.TryParse(text, out value))
                {
                    isDateTime = true;
                }
            }

            return isDateTime;
        }

        private void CreateTariffUploadExcel(int count)
        {
            if (tariffPM != null && versionPM != null)
            {
                TariffVersionUploadedExcelRepository excelRepository = new TariffVersionUploadedExcelRepository(tariffContext);
                IQueryable<TariffVersionUploadedExcel> uploadedExcels = excelRepository.GetAllVersionUploadedExcels(tariffId, version, tenant);

                string documentId = this.UploadExcelFileToStorage(tariffPM.TariffNumber,  tenant);

                TariffVersionUploadedExcelPM tariffVersionUploadedExcel = new TariffVersionUploadedExcelPM();
                tariffVersionUploadedExcel.ChangeSetOp = ChangeSetOperation.Insert;
                tariffVersionUploadedExcel.TariffId = tariffId;
                tariffVersionUploadedExcel.Tenant = tenant;
                tariffVersionUploadedExcel.Version = version;
                tariffVersionUploadedExcel.NumberOfLines = count;
                tariffVersionUploadedExcel.DocumentId = documentId;
                tariffVersionUploadedExcel.Index = uploadedExcels.Count() + 1;

                TariffVersionUploadedExcelUpdateService updateService = new TariffVersionUploadedExcelUpdateService(tariffContext, new Dictionary<string, IContext>(), tenant);
                updateService.Update(tariffVersionUploadedExcel, true);
            }
        }

        private void SaveTariff(List<TariffLinePM> tariffLinesResult)
        {
            if(tariffPM != null && versionPM != null)
            {
                tariffPM.ChangeSetOp = ChangeSetOperation.Update;
                versionPM.ChangeSetOp = ChangeSetOperation.Update;
                tariffPM.TariffLinesAddedFromExcel = true;

                foreach(TariffLinePM item in versionPM.TariffLines)
                {
                    item.ChangeSetOp = ChangeSetOperation.Delete;
                }

                versionPM.TariffLines.AddRange(tariffLinesResult);

                TariffUpdateService tariffUpdateService = new TariffUpdateService(tariffContext, new Dictionary<string, IContext>(), tenant);
                TariffVersionUpdateService tariffVersionUpdateService = new TariffVersionUpdateService(tariffContext);
                tariffUpdateService.Update(tariffPM, true);
            }
        }

        private string UploadExcelFileToStorage(string tariffNumber, int tenant)
        {
            Document document = null;
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            
            if (memoryStream != null)
            {
                byte[] ByteData = memoryStream.ToArray();
                DocumentRepository documentRepository = new DocumentRepository(tenant);
                document = new Document()
                {
                    FileName = fileName,
                    CreateDate = DateTime.Now,
                    Extension = fileExtension,
                    FileSize = ByteData.Length,
                    Tenant = tenant,
                    Id = IdCounter.GetNumber("Document", tenant),
                    HasFile = true,
                    Folder = "tariff",
                };

                documentRepository.Add(document);
                documentRepository.SubmitChanges();

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = "tariff",
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = document.FileSize,
                };

                storageservice.Write(ByteData, fileInfo);
            }

            return document != null ? document.Id : null;
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
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
    
    public class ExcelSheetLine
    {
        public string FromPort { get; set; }
        public string ToPort { get; set; }
        public string MinPrice { get; set; }
        public string Price1 { get; set; }
        public string Price2 { get; set; }
        public string Price3 { get; set; }
        public string Price4 { get; set; }
        public string Price5 { get; set; }
        public string Price6 { get; set; }
        public string Price7 { get; set; }
        public string Price8 { get; set; }
        public string Notes { get; set; }
        public string TransitTime { get; set; }
    }
}