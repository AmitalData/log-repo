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

namespace WebFreight.Web.Helpers
{
    public class UploadTariffHelper
    {
        private int tenant;
        private string priceSteps;
        private string tariffType;
        private string fileData;
        private string tariffId;
        private PortRepository portRepository;
        private ICommonDataContext commonContext;
        public UploadTariffHelper(TariffFilterParameter filterParameter)
        {
            tenant = filterParameter.Tenant;
            fileData = filterParameter.FileData;
            priceSteps = filterParameter.PriceSteps;
            tariffType = filterParameter.TariffType;
            tariffId = filterParameter.TariffId;

            this.commonContext = CommonDataContext.GetContext(tenant);
            this.portRepository = new PortRepository(commonContext);
        }

        public List<ExcelTariffLines> Upload()
        {
            byte[] fileDataArray = Convert.FromBase64String(fileData);

            System.IO.MemoryStream stream = new System.IO.MemoryStream(fileDataArray);
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
            IWorksheet sheet = workbook.Worksheets[0];

            List<ExcelTariffLines> tariffLinesResult = new List<ExcelTariffLines>();
            if (tariffType == "AFC" || tariffType == "OLC")
            {
                tariffLinesResult = this.BuildOceanAirFreightCostExcelLines(sheet);
            }

            else if (tariffType == "OFC")
            {
                tariffLinesResult = this.BuildOceanFCLFreightCostExcelLines(sheet);
            }

            return tariffLinesResult;
        }
        
        public List<ExcelTariffLines> BuildOceanAirFreightCostExcelLines(IWorksheet sheet)
        {
            List<ExcelTariffLines> myResult = new List<ExcelTariffLines>();
            int rowIndex = 0;
            string notescolumn = sheet.Columns[sheet.Columns.Count() - 1].DisplayText;

            foreach (IRange row in sheet.UsedRange.Rows.Skip(1))
            {
                String[] rowData = new String[sheet.Columns.Count() - 1];
                ExcelTariffLines tariffLine = new ExcelTariffLines();
                tariffLine.Index = rowIndex;
                var rowDataLength = rowData.Length;
                var StepLength = rowData.Length;
                if (!string.IsNullOrEmpty(priceSteps))
                {
                    StepLength = priceSteps.Split(',').Length + 3;
                }

                String notesRowData = row.Cells[sheet.Columns.Count() - 1].Value2.ToString();

                for (int i = 0; i < sheet.Columns.Count() - 1; i++)
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
                if (!Regex.IsMatch(fromPortCode, @"^[a-zA-Z0-9]+$"))
                {
                    fromPortCode = Regex.Replace(fromPortCode, @"[^a-zA-Z0-9]+", "");
                }

                Port fromPort = this.GetPortDetails(fromPortCode, tenant);
                if (fromPort != null)
                {
                    if ((fromPort.IsAir && tariffType == "AFC") || (fromPort.IsOcean && tariffType == "OLC"))
                    {
                        tariffLine.FromPortId = fromPort.Id;
                        tariffLine.FromPortCode = fromPort.Code;
                        tariffLine.FromPortCombinedCode = fromPort.CombinedCode;
                        tariffLine.FromPortName = fromPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.FromPortIsNotAir = true;
                        tariffLine.FromPortText = fromPortCode;
                    }
                }
                else
                {
                    tariffLine.FromPortText = this.TrimTo_20(fromPortCode);
                }

                /*To Port*/
                string toPortCode = rowData[1];
                if (!Regex.IsMatch(toPortCode, @"^[a-zA-Z0-9]+$"))
                {
                    toPortCode = Regex.Replace(toPortCode, @"[^a-zA-Z0-9]+", "");
                }

                Port toPort = this.GetPortDetails(toPortCode, tenant);
                if (toPort != null)
                {
                    if ((toPort.IsAir && tariffType == "AFC") || (toPort.IsOcean && tariffType == "OLC"))
                    {
                        tariffLine.ToPortId = toPort.Id;
                        tariffLine.ToPortCode = toPort.Code;
                        tariffLine.ToPortCombinedCode = toPort.CombinedCode;
                        tariffLine.ToPortName = toPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.ToPortIsNotAir = true;
                        tariffLine.ToPortText = toPortCode;
                    }
                }
                else
                {
                    tariffLine.ToPortText = this.TrimTo_20(toPortCode);
                }

                if (StepLength > 2 && rowDataLength > 2)
                {
                    if (this.IsNumber(rowData[2]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[2]);
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
                        tariffLine.MinPriceText = this.TrimTo_20(rowData[2]);
                    }
                }

                if (StepLength > 3 && rowDataLength > 3)
                {
                    if (this.IsNumber(rowData[3]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[3]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step1Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep1PriceMinus = true;
                            tariffLine.Step1PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step1PriceText = this.TrimTo_20(rowData[3]);
                    }
                }

                if (StepLength > 4 && rowDataLength > 4)
                {
                    if (this.IsNumber(rowData[4]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[4]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step2Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep2PriceMinus = true;
                            tariffLine.Step2PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step2PriceText = this.TrimTo_20(rowData[4]);
                    }
                }

                if (StepLength > 5 && rowDataLength > 5)
                {
                    if (this.IsNumber(rowData[5]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[5]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step3Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep3PriceMinus = true;
                            tariffLine.Step3PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step3PriceText = this.TrimTo_20(rowData[5]);
                    }
                }

                if (StepLength > 6 && rowDataLength > 6)
                {
                    if (this.IsNumber(rowData[6]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[6]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step4Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep4PriceMinus = true;
                            tariffLine.Step4PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step4PriceText = this.TrimTo_20(rowData[6]);
                    }
                }

                if (StepLength > 7 && rowDataLength > 7)
                {
                    if (this.IsNumber(rowData[7]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[7]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step5Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep5PriceMinus = true;
                            tariffLine.Step5PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step5PriceText = this.TrimTo_20(rowData[7]);
                    }
                }

                if (StepLength > 8 && rowDataLength > 8)
                {
                    if (this.IsNumber(rowData[8]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[8]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step6Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep6PriceMinus = true;
                            tariffLine.Step6PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step6PriceText = this.TrimTo_20(rowData[8]);
                    }
                }

                if (StepLength > 9 && rowDataLength > 9)
                {
                    if (this.IsNumber(rowData[9]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[9]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step7Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep7PriceMinus = true;
                            tariffLine.Step7PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step7PriceText = this.TrimTo_20(rowData[9]);
                    }
                }

                if (StepLength > 10 && rowDataLength > 10)
                {
                    if (this.IsNumber(rowData[10]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[10]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step8Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep8PriceMinus = true;
                            tariffLine.Step8PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step8PriceText = this.TrimTo_20(rowData[10]);
                    }
                }

                if (!string.IsNullOrEmpty(notescolumn))
                {
                    tariffLine.Notes = notesRowData;

                    if (notesRowData.Length > 500)
                    {
                        tariffLine.Notes = notesRowData.Substring(0, 500);
                    }
                }

                myResult.Add(tariffLine);
                rowIndex++;
            }

            foreach (ExcelTariffLines item in myResult)
            {
                this.SetErrors_AirFreightCost(item);
            }

            return myResult;
        }
        public List<ExcelTariffLines> BuildOceanFCLFreightCostExcelLines(IWorksheet sheet)
        {
            List<ExcelTariffLines> myResult = new List<ExcelTariffLines>();
            int rowIndex = 0;

            string notescolumn = sheet.Columns[sheet.Columns.Count() - 1].DisplayText;

            foreach (IRange row in sheet.UsedRange.Rows.Skip(1))
            {
                String[] rowData = new String[sheet.Columns.Count() - 1];
                ExcelTariffLines tariffLine = new ExcelTariffLines();
                tariffLine.Index = rowIndex;

                String notesRowData = row.Cells[sheet.Columns.Count() - 1].Value2.ToString();

                for (int i = 0; i < sheet.Columns.Count() - 1; i++)
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
                if (!Regex.IsMatch(fromPortCode, @"^[a-zA-Z0-9]+$"))
                {
                    fromPortCode = Regex.Replace(fromPortCode, @"[^a-zA-Z0-9]+", "");
                }

                Port fromPort = this.GetPortDetails(fromPortCode, tenant);
                if (fromPort != null)
                {
                    if (fromPort.IsOcean)
                    {
                        tariffLine.FromPortId = fromPort.Id;
                        tariffLine.FromPortCode = fromPort.Code;
                        tariffLine.FromPortCombinedCode = fromPort.CombinedCode;
                        tariffLine.FromPortName = fromPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.FromPortIsNotAir = true;
                        tariffLine.FromPortText = fromPortCode;
                    }
                }
                else
                {
                    tariffLine.FromPortText = this.TrimTo_20(fromPortCode);
                }

                /*To Port*/
                string toPortCode = rowData[1];
                if (!Regex.IsMatch(toPortCode, @"^[a-zA-Z0-9]+$"))
                {
                    toPortCode = Regex.Replace(toPortCode, @"[^a-zA-Z0-9]+", "");
                }

                Port toPort = this.GetPortDetails(toPortCode, tenant);
                if (toPort != null)
                {
                    if (toPort.IsOcean)
                    {
                        tariffLine.ToPortId = toPort.Id;
                        tariffLine.ToPortCode = toPort.Code;
                        tariffLine.ToPortCombinedCode = toPort.CombinedCode;
                        tariffLine.ToPortName = toPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.ToPortIsNotAir = true;
                        tariffLine.ToPortText = toPortCode;
                    }
                }
                else
                {
                    tariffLine.ToPortText = this.TrimTo_20(toPortCode);
                }

                if (rowData.Length > 2)
                {
                    if (this.IsNumber(rowData[2]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[2]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Surcharge1Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsSurcharge1PriceMinus = true;
                            tariffLine.Surcharge1PriceText = String.Format("{0:0.000}", myNumber);
                        }
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
                        decimal myNumber = Convert.ToDecimal(rowData[3]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Surcharge2Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsSurcharge2PriceMinus = true;
                            tariffLine.Surcharge2PriceText = String.Format("{0:0.000}", myNumber);
                        }
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
                        decimal myNumber = Convert.ToDecimal(rowData[4]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Surcharge3Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsSurcharge3PriceMinus = true;
                            tariffLine.Surcharge3PriceText = String.Format("{0:0.000}", myNumber);
                        }
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
                        decimal myNumber = Convert.ToDecimal(rowData[5]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Surcharge4Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsSurcharge4PriceMinus = true;
                            tariffLine.Surcharge4PriceText = String.Format("{0:0.000}", myNumber);
                        }
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
                        decimal myNumber = Convert.ToDecimal(rowData[6]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Surcharge5Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsSurcharge5PriceMinus = true;
                            tariffLine.Surcharge5PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Surcharge5PriceText = this.TrimTo_20(rowData[6]);
                    }
                }

                if (!string.IsNullOrEmpty(notescolumn))
                {
                    tariffLine.Notes = notesRowData;

                    if (notesRowData.Length > 500)
                    {
                        tariffLine.Notes = notesRowData.Substring(0, 500);
                    }
                }

                myResult.Add(tariffLine);
                rowIndex++;
            }

            foreach (ExcelTariffLines item in myResult)
            {
                this.SetErrors_OceanFCLFreightCost(item);
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

                if (item.FromPortIsNotAir)
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
                        errorText = "Port with code " + item.FromPortText + " not found";
                    }

                    else
                    {
                        errorText = errorText + ", Port with code " + item.FromPortText + " not found";
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.FromPortText) && string.IsNullOrEmpty(item.FromPortId))
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

            if (!string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
            {
                error = true;

                if (item.ToPortIsNotAir)
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
                        errorText = "Port with code " + item.ToPortText + " not found";
                    }

                    else
                    {
                        errorText = errorText + ", Port with code " + item.ToPortText + " not found";
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
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

                if (item.IsStep1PriceMinus)
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

                if (item.IsStep2PriceMinus)
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

                if (item.IsStep3PriceMinus)
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

                if (item.IsStep4PriceMinus)
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

                if (item.IsStep5PriceMinus)
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

                if (item.IsStep6PriceMinus)
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

                if (item.IsStep7PriceMinus)
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

                if (item.IsStep8PriceMinus)
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
        private void SetErrors_OceanFCLFreightCost(ExcelTariffLines item)
        {
            bool error = false;
            string errorText = "";

            if (!string.IsNullOrEmpty(item.FromPortText) && string.IsNullOrEmpty(item.FromPortId))
            {
                error = true;

                if (item.FromPortIsNotAir)
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
                        errorText = "Port with code " + item.FromPortText + " not found";
                    }

                    else
                    {
                        errorText = errorText + ", Port with code " + item.FromPortText + " not found";
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.FromPortText) && string.IsNullOrEmpty(item.FromPortId))
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

            if (!string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
            {
                error = true;

                if (item.ToPortIsNotAir)
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
                        errorText = "Port with code " + item.ToPortText + " not found";
                    }

                    else
                    {
                        errorText = errorText + ", Port with code " + item.ToPortText + " not found";
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
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

                if (item.IsSurcharge1PriceMinus)
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

                if (item.IsSurcharge2PriceMinus)
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

                if (item.IsSurcharge3PriceMinus)
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

                if (item.IsSurcharge4PriceMinus)
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

                if (item.IsSurcharge5PriceMinus)
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

                if (myPort == null)
                {
                    Port portZero = null;
                    if (tariffType == "AFC")
                    {
                        portZero = this.portRepository.GetAirlinePortByCode(0, code, true);
                    }

                    else if (tariffType == "OLC" || tariffType == "OFC")
                    {
                        portZero = this.portRepository.GetOceanPortByCombinedCode(code, 0);
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
    }

    public class ExcelTariffLines
    {
        public string FromPortId { get; set; }
        public string FromPortCode { get; set; }
        public string FromPortCombinedCode { get; set; }
        public string FromPortName { get; set; }
        public string ToPortId { get; set; }
        public string ToPortCode { get; set; }
        public string ToPortCombinedCode { get; set; }
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

        public bool FromPortIsNotAir { get; set; }
        public bool ToPortIsNotAir { get; set; }

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

        public bool IsMinPriceMinus { get; set; }
        public bool IsStep1PriceMinus { get; set; }
        public bool IsStep2PriceMinus { get; set; }
        public bool IsStep3PriceMinus { get; set; }
        public bool IsStep4PriceMinus { get; set; }
        public bool IsStep5PriceMinus { get; set; }
        public bool IsStep6PriceMinus { get; set; }
        public bool IsStep7PriceMinus { get; set; }
        public bool IsStep8PriceMinus { get; set; }

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

        public bool IsSurcharge1PriceMinus { get; set; }
        public bool IsSurcharge2PriceMinus { get; set; }
        public bool IsSurcharge3PriceMinus { get; set; }
        public bool IsSurcharge4PriceMinus { get; set; }
        public bool IsSurcharge5PriceMinus { get; set; }
        public bool IsSurcharge6PriceMinus { get; set; }
        public bool IsSurcharge7PriceMinus { get; set; }
        public bool IsSurcharge8PriceMinus { get; set; }
        public bool IsSurcharge9PriceMinus { get; set; }
        public bool IsSurcharge10PriceMinus { get; set; }

        public int Index { get; set; }

        public string Notes { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateText { get; set; }
    }
}