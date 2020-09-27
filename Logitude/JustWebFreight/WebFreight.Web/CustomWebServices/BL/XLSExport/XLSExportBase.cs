using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using WebFreight.Web.Controllers.CommonDataModel.Extended;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public abstract class XLSExportBase<TRequestParams, TResponseData>
        where TRequestParams : RequestParamsBase, new()
        where TResponseData : ResponseDataBase, new()
    {
        protected IWorksheet _MainWorksheet;
        protected IWorkbook _IWorkbook;
        IStyle _EditBoxStyle;
        IStyle _LabelStyle;
        protected TRequestParams _RequestParams;
        protected TResponseData _ResponseData;
        protected int Tenant { get; private set; }
        public IRequestProvider RequestProvider { get; set; }


        protected Color SectionBorder = ColorTranslator.FromHtml("#3BB3E2");//border: 1px solid #3BB3E2;
        protected Color WindowHeaderColor = Color.FromArgb(27, 144, 203);
        protected Color ResultHeaderColor = Color.FromArgb(27, 144, 203);
        protected int WindowHeaderFontSize = 18;//px
        protected int ResultHeaderFontSize = 12;//px


        //EditBox Border ==border: 1px solid #AAAAAA;
        Color EditBoxBorderColor = ColorTranslator.FromHtml("#AAAAAA");
        private IStyle _GridColHeaderStyle;
        private IStyle _GridRowBorderDownStyle;

        public abstract int ReportWidth
        {
            get;
        }
        protected abstract IRange AdjustAndFormatRequestRange(TRequestParams requestParams, IWorksheet requestWorksheet);
        protected abstract IRange AdjustAndFormatResponseRange(TResponseData responseData, IWorksheet worksheet);


        protected abstract IRange AdjustRequest(TRequestParams requestParams, TResponseData responseData);

        protected double GetExcelSizeFromPixel(double sizeInPixels)
        {
            const double Size10px = 1.07;
            var ExcelSize = sizeInPixels / 10 * Size10px;
            return ExcelSize;

        }
        public byte[] ExportCustomRequestToExcel(string mainInterfaceCode/*=8347*/, string communicationLogId, int tenant)
        {
            ///http://localhost:9996/api/CommunicationLogStep/GetCommunicationLogStepsRequestParamResponseData/?mainInterfaceCode=8347&logId=1-1370569&tenant=1
            List<CommunicationLogStepList> stepLIstOut = null;

            this.Tenant = tenant;
            if (string.IsNullOrWhiteSpace(communicationLogId) && RequestProvider != null)
            {
                stepLIstOut = GetFromProvider();
            }
            else
            {
                stepLIstOut = GetFromDB(communicationLogId, tenant);
            }
            var RequestParams = stepLIstOut.First(r => r.Name == CustomsStepEnum.StartRequestParams.ToString());
            var AnalyzeResponseData = stepLIstOut.First(r => r.Name == CustomsStepEnum.AnalyzeResponseData.ToString());

            _RequestParams = XmlGenericUtil<TRequestParams>.DeSerializeObject(RequestParams.DocumentData);
            if (!String.IsNullOrWhiteSpace(AnalyzeResponseData.DocumentData))
            {
                _ResponseData = XmlGenericUtil<TResponseData>.DeSerializeObject(AnalyzeResponseData.DocumentData);
            }

            System.IO.MemoryStream memory = new System.IO.MemoryStream();

            //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
            //The instantiation process consists of two steps.

            //Step 1 : Instantiate the spreadsheet creation engine.
            var excelEngine = new ExcelEngine();
            try
            {
                //Step 2 : Instantiate the excel application object.
                IApplication application = excelEngine.Excel;
                application.DefaultVersion =
                    ExcelVersion.Excel2007;// Excel97to2003 version does not support gradient fill type
                //A new workbook is created.[Equivalent to creating a new workbook in MS Excel]
                //The new workbook will have 5 worksheets
                _IWorkbook = excelEngine.Excel.Workbooks.Create(1);
                _IWorkbook.Version = ExcelVersion.Excel97to2003;// Excel97to2003 version does not
                //The first worksheet object in the worksheets collection is accessed.
                _MainWorksheet = _IWorkbook.Worksheets[0];
                _MainWorksheet.IsRightToLeft = true;
                _IWorkbook.StandardFontSize = 12/*font-weight:400*/- 2;
                _IWorkbook.StandardFont = "Arial"; ;
                for (int i = 1; i < (ReportWidth / 10) + 2; i++)
                {
                    _MainWorksheet.SetColumnWidthInPixels(i, 10);// = GetExcelSizeFromPixel(10);
                }
                //_IWorksheet.Name = "Customs.General.O.CurrencyExchangeRateQuery";
                CreateLabelStyle();
                CreateEditBoxStyle();
                CreateGridColHeaderStyle();
                CreateGridRowBorderDownStyle();


                if (true)
                {
                    AdjustRequest(_RequestParams, _ResponseData);

                }
                else
                {
                    //var mainHeaderRange = CreateHeader();
                    //OldVersionOfFormart(mainHeaderRange);
                }


                // UsedRange excludes the blank cell which has formatting
                _MainWorksheet.UsedRangeIncludesFormatting = false;

                // Modifying the column width and row height of the used range

                _MainWorksheet.UsedRange.RowHeight = 20;
                _MainWorksheet.IsGridLinesVisible = false;
                _IWorkbook.SaveAs(memory, ExcelSaveType.SaveAsXLS);
            }
            finally
            {


                //No exception will be thrown if there are unsaved workbooks.
                excelEngine.ThrowNotSavedOnDestroy = false;
                excelEngine.Dispose();
            }
            return memory.ToArray();
        }

        private static List<CommunicationLogStepList> GetFromDB(string communicationLogId, int tenant)
        {
            List<CommunicationLogStepList> stepLIstOut;
            var myFilter = new int[] { 0, 30 };
            //var controller = new CommunicationLogStepController();
            //List<CommunicationLogStepList> myResult = controller.GetCommunicationLogStepsDocumentData(mainInterfaceCode, communicationLogId, tenant, myFilter, false);
            var communicationLogStepQuery = new CommunicationLogStepQuery();
            stepLIstOut = communicationLogStepQuery.GetCommunicationLogStepsDocumentData(communicationLogId, tenant, myFilter);
            return stepLIstOut;
        }

        private List<CommunicationLogStepList> GetFromProvider()
        {
            return new List<CommunicationLogStepList>()
                {
                    new CommunicationLogStepList()
                    {
                        Name = CustomsStepEnum.StartRequestParams.ToString(),
                        DocumentData = this.RequestProvider.GetRequest()
                    },
                    new CommunicationLogStepList()
                    {
                        Name = CustomsStepEnum.AnalyzeResponseData.ToString(),
                        DocumentData = this.RequestProvider.GetResponse()
                    }
                };
        }

        private void OldVersionOfFormart(IRange mainHeaderRange)
        {
            var requestRange = AdjustAndFormatRequestRange(_RequestParams, _IWorkbook.Worksheets.Create("Request"));




            var requestRangeInMain = _MainWorksheet.Range[mainHeaderRange.Rows.Length + 2, 2, mainHeaderRange.Rows.Length + 2 + requestRange.Rows.Length, requestRange.Columns.Length];
            requestRange.CopyTo(requestRangeInMain);

            _IWorkbook.Names.Add("RequestRange", requestRangeInMain);

            var responseRange = AdjustAndFormatResponseRange(_ResponseData, _IWorkbook.Worksheets.Create("Response"));



            var responseRangeInMain = _MainWorksheet.Range[
                mainHeaderRange.Rows.Length + 2 + requestRangeInMain.Rows.Length,
                2,
                mainHeaderRange.Rows.Length + 2 + requestRangeInMain.Rows.Length + responseRange.Rows.Length, responseRange.Columns.Length];
            responseRange.CopyTo(responseRangeInMain);
            _IWorkbook.Names.Add("ResponseRange", responseRangeInMain);
        }


        protected IRange BuildGenList(string gridName, IList RecordList, List<GridColumnMetaData> columnList, IRange lastRange)
        {

            int start = lastRange.LastRow + 1;
            var LastCol = columnList.Sum(r => r.length) / 10;
            var myHeader = _MainWorksheet[start, 1, start, LastCol];
            int currCol = 1;
            foreach (var item in columnList)
            {
                SetGridHeader(gridName, myHeader, item, currCol);
                currCol = currCol + item.length / 10;
            }
            int row = myHeader.LastRow + 1;
            foreach (var rec in RecordList)
            {

                var currRow =
                    //_MainWorksheet[row, 1 , row, LastCol ];
                    _MainWorksheet[row, 1 + 1, row, LastCol + 1];
                currRow.CellStyle = _GridRowBorderDownStyle;

                currCol = 1;
                foreach (var colMetadata in columnList)
                {
                    SetGenericCell(row, currCol, colMetadata, rec);
                    currCol = currCol + colMetadata.length / 10;
                }

                row++;
            }
            var myRange = _MainWorksheet[start, 1, row, LastCol];
            _IWorkbook.Names.Add(gridName, myRange);
            return myRange;
        }



        private void SetGenericCell(int row, int currCol, GridColumnMetaData colMetadata, object rec)
        {
            int LastCol = currCol + colMetadata.length / 10;
            var cell = _MainWorksheet[row, currCol + 1, row, LastCol];
            cell.Merge();
            var theValCell = cell[cell.Row, cell.Column];
            switch (colMetadata.GridColumnType)
            {

                case GridColumnTypeEnum.Text:
                    {
                        theValCell.Text = "'" + GetValue(rec, colMetadata.PropName);
                    }
                    break;
                case GridColumnTypeEnum.Number:
                    double mydouble;
                    if (double.TryParse(GetValue(rec, colMetadata.PropName), out mydouble))
                    {
                        theValCell.Number = mydouble;
                        theValCell.NumberFormat = "###,##0.00";

                    }
                    else
                    {
                        theValCell.Value = GetValue(rec, colMetadata.PropName);
                    }
                    break;
                default:
                    theValCell.Value = GetValue(rec, colMetadata.PropName);
                    break;
            }
            //theValCell.IndentLevel = 1;
            ExcelHAlign myHorizontalAlignment = ExcelHAlign.HAlignGeneral;
            Enum.TryParse<ExcelHAlign>(colMetadata.ExcelHAlign.ToString(), out myHorizontalAlignment);
            theValCell.HorizontalAlignment = myHorizontalAlignment;

        }

        private string GetValue(object src, string propName)
        {
            if (src.GetType().GetProperty(propName)==null)
            {
                return string.Empty;
            }
            var val = src.GetType().GetProperty(propName).GetValue(src, null);
            val = val ?? string.Empty;
            return val.ToString();
        }
        void SetGridHeader(string gridName, IRange myHeader, GridColumnMetaData colMetadata, int CurrCol)
        {
            /*
border-left-color:
rgb(27, 144, 203)
             */
            //int CurrCol = 1;
            int LastCol = CurrCol + colMetadata.length / 10;
            var colHeader = myHeader[myHeader.LastRow, CurrCol + 1, myHeader.LastRow, LastCol];
            colHeader.Merge();
            _IWorkbook.Names.Add(gridName + ":" + colMetadata.PropName, colHeader);
            colHeader[colHeader.Row, colHeader.Column].Text = colMetadata.Header;
            SetAsGridColHeaderStyle(colHeader);








        }
        protected string TranslateText(string code)
        {
            return TextCodesTranslator.TranslateText(code, this.Tenant,true);
        }

        protected void AddEditBoxBorder(IRange myRange)
        {
            myRange.Borders[ExcelBordersIndex.DiagonalUp].LineStyle = ExcelLineStyle.None;
            myRange.Borders[ExcelBordersIndex.DiagonalDown].LineStyle = ExcelLineStyle.None;


            var my1stRow = myRange
                //[stratRequestSection - 1, 2, stratRequestSection - 1, 80];
                [myRange.Row, myRange.Column, myRange.Row, myRange.LastColumn];
            my1stRow.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            my1stRow.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = this.EditBoxBorderColor;


            var myLastRow = _MainWorksheet
                //[stratRequestSection + 1, 2, stratRequestSection + 1, 80];
                [myRange.LastRow, myRange.Column, myRange.LastRow, myRange.LastColumn];
            myLastRow.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            myLastRow.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = this.EditBoxBorderColor;


            var my1stCol = _MainWorksheet
                //[stratRequestSection - 1, 2, stratRequestSection + 1, 2];
                [myRange.Row, myRange.Column, myRange.LastRow, myRange.Column];
            my1stCol.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
            my1stCol.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = this.EditBoxBorderColor;

            var myLastCol = _MainWorksheet
                //[stratRequestSection - 1, 2, stratRequestSection + 1, 2];
                [myRange.Row, myRange.LastColumn, myRange.LastRow, myRange.LastColumn];
            myLastCol.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
            myLastCol.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = this.EditBoxBorderColor;

        }
        protected void SetAsEditBoxStyle(IRange range)
        {
            range.CellStyle = _EditBoxStyle;
            AddEditBoxBorder(range);
            range.VerticalAlignment = ExcelVAlign.VAlignCenter;
        }
        protected void SetAsLabelStyle(IRange range)
        {
            range.CellStyle = _LabelStyle;

            range.VerticalAlignment = ExcelVAlign.VAlignCenter;
        }

        protected void SetAsGridColHeaderStyle(IRange range)
        {
            range.CellStyle = _GridColHeaderStyle;

            range.VerticalAlignment = ExcelVAlign.VAlignCenter;
            range.HorizontalAlignment = ExcelHAlign.HAlignCenter;
        }
        private void CreateGridRowBorderDownStyle()
        {
            _GridRowBorderDownStyle = _IWorkbook.Styles.Add("GridRowBorderDownStyle");
            _GridRowBorderDownStyle.Borders.LineStyle = ExcelLineStyle.Thin; // LineStyle.Continuous;
            _GridRowBorderDownStyle.Borders.ColorRGB = Color.FromArgb(186, 206, 227);
            _GridRowBorderDownStyle.Borders[ExcelBordersIndex.DiagonalUp].LineStyle = ExcelLineStyle.None;
            _GridRowBorderDownStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.None;
            _GridRowBorderDownStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.None;
            _GridRowBorderDownStyle.Borders[ExcelBordersIndex.DiagonalDown].LineStyle = ExcelLineStyle.Thin;
        }
        private void CreateGridColHeaderStyle()
        {

            try
            {
                _GridColHeaderStyle = _IWorkbook.Styles.Add("GridColHeader");
                _GridColHeaderStyle.BeginUpdate();

                _GridColHeaderStyle.Color = //Color.FromArgb(27, 144, 203);
                    ColorTranslator.FromHtml("#f5f5f5");
                _GridColHeaderStyle.Font.RGBColor =
                ColorTranslator.FromHtml("#1B90CB");
                //Color.FromArgb(163, 245, 245); ;
                _GridColHeaderStyle.Borders.LineStyle = ExcelLineStyle.Thin; // LineStyle.Continuous;
                //_GridColHeaderStyle.Borders.ColorRGB = Color.FromArgb(69, 73, 74);
                if (_IWorkbook.Version == ExcelVersion.Excel97to2003)
                {
                    _GridColHeaderStyle.Borders.ColorRGB = Color.FromArgb(186, 206, 227);//linear - gradient(rgb(255, 255, 255) 0 %, rgb(186, 206, 227) 100 %)
                }
                else
                {

                    if (true)
                    {
                        var style = _GridColHeaderStyle;
                        style.Interior.FillPattern = ExcelPattern.Gradient;// Excel97to2003 version does not support gradient fill type
                        style.Interior.Gradient.TwoColorGradient(ExcelGradientStyle.Diagonl_Up, ExcelGradientVariants.ShadingVariants_3);
                        style.Interior.Gradient.ForeColor = Color.FromArgb(245, 245, 245);
                        style.Interior.Gradient.BackColor = Color.FromArgb(247, 247, 247);
                    }
                    else
                    {
                        _GridColHeaderStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        _GridColHeaderStyle.Interior.FillPattern = ExcelPattern.Gradient;
                        _GridColHeaderStyle.Interior.Gradient.TwoColorGradient(ExcelGradientStyle.Horizontal, ExcelGradientVariants.ShadingVariants_3);
                        _GridColHeaderStyle.Interior.Gradient.ForeColor = Color.FromArgb(255, 255, 255);

                        _GridColHeaderStyle.Interior.Gradient.BackColor =
                            Color.FromArgb(186, 206, 227);

                    }

                }


                _GridColHeaderStyle.Borders[ExcelBordersIndex.DiagonalDown].ShowDiagonalLine = false;
                _GridColHeaderStyle.Borders[ExcelBordersIndex.DiagonalUp].ShowDiagonalLine = false;

                _GridColHeaderStyle.Font.FontName = "Arial";
                _GridColHeaderStyle.Font.Size = 12/*font-weight:400*/- 2;


                //_GridColHeaderStyle.Font.Bold = true;
                _GridColHeaderStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                _GridColHeaderStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                _GridColHeaderStyle.EndUpdate();

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void CreateEditBoxStyle()
        {

            try
            {
                _EditBoxStyle = _IWorkbook.Styles.Add("EditBoxStyle");
                _EditBoxStyle.BeginUpdate();

                //_EditBoxStyle.Color = Color.Black; //Color.FromArgb(69, 73, 74);
                //Color.FromArgb(163, 245, 245); ;
                _EditBoxStyle.Borders.LineStyle = ExcelLineStyle.Thin; // LineStyle.Continuous;
                _EditBoxStyle.Borders.ColorRGB = Color.FromArgb(69, 73, 74);
                _EditBoxStyle.Borders[ExcelBordersIndex.DiagonalDown].ShowDiagonalLine = false;
                _EditBoxStyle.Borders[ExcelBordersIndex.DiagonalUp].ShowDiagonalLine = false;


                _EditBoxStyle.Font.Bold = true;
                _EditBoxStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                _EditBoxStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                _EditBoxStyle.IndentLevel = 1;
                _EditBoxStyle.EndUpdate();

            }
            catch (Exception)
            {

                throw;
            }

        }

        protected IRange CreateLabelEditboxLine(object data, IRange lastLine, List<LabelEditBox> myMetaDataLine)
        {
            var startRow = lastLine.LastRow + 2;
            int lastColumn = 1;
            foreach (var myMetaData in myMetaDataLine)
            {

                IRange Label = CreateLabel(startRow, lastColumn, myMetaData.LabelSize, myMetaData.PropName, myMetaData.Header);
                lastColumn = lastColumn + myMetaData.LabelSize + 1;
                IRange DisplayFileNumberEdit = CreateEditBox(data, Label, myMetaData);
                lastColumn = lastColumn + myMetaData.length + 1;
            }

            return _MainWorksheet[startRow, 1, startRow, lastColumn];
        }
        protected IRange CreateEditBox(object data,

            IRange lastControl,
            LabelEditBox myMetaData)
        {
            var editCell = _MainWorksheet[lastControl.LastRow, lastControl.LastColumn + 1, lastControl.LastRow, lastControl.LastColumn + myMetaData.length + 1];
            editCell.Merge();
            _IWorkbook.Names.Add(myMetaData.PropName + ":Edit", editCell);
            SetAsEditBoxStyle(editCell);
            //edit.Value = GetValue(data, PropName);
            var theEditCellVal = editCell[editCell.Row, editCell.Column];
            if (!String.IsNullOrWhiteSpace(myMetaData.TheValue))
            {
                theEditCellVal.Value = myMetaData.TheValue;

            }
            else
            {
                switch (myMetaData.GridColumnType)
                {

                    case GridColumnTypeEnum.Text:
                        {

                            theEditCellVal.Text = "'" + GetValue(data, myMetaData.PropName);
                        }
                        break;

                    case GridColumnTypeEnum.Number:
                        double mydouble;
                        if (double.TryParse(GetValue(data, myMetaData.PropName), out mydouble))
                        {
                            theEditCellVal.Number = mydouble;
                            theEditCellVal.NumberFormat = "###,##0.00";

                        }
                        else
                        {
                            theEditCellVal.Value = GetValue(data, myMetaData.PropName);
                        }
                        break;
                    default:
                        theEditCellVal.Value = GetValue(data, myMetaData.PropName);
                        break;
                }
            }

            return editCell;
        }
        protected IRange CreateLabel(int startRow, int startCol, int size, string Name, string Text)
        {
            var label = _MainWorksheet[startRow, startCol + 1, startRow, startCol + size + 1];
            label.Merge();
            _IWorkbook.Names.Add(Name + ":Label", label);
            label[label.Row, label.Column].Text = Text;
            SetAsLabelStyle(label);
            return label;
        }

        private void CreateLabelStyle()
        {
            //        font - size: 11px;
            //        color: #6E7172;
            //text - align: left;
            //        cursor: text !important;
            //        -moz - user - select: text !important;
            //        -webkit - user - select: text !important;
            //        -ms - user - select: text !important;
            //        user - select: text !important;
            try
            {
                _LabelStyle = _IWorkbook.Styles.Add("LabelStyle");
                _LabelStyle.BeginUpdate();
                //_LabelStyle.Font.Size = 11;
                _LabelStyle.Font.RGBColor = ColorTranslator.FromHtml("#6E7172");
                _LabelStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;

                _LabelStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                _LabelStyle.IndentLevel = 2;


                _LabelStyle.EndUpdate();

            }
            catch (Exception)
            {

                throw;
            }

        }

        protected void MakeBorderSection(IRange requestSection)
        {


            requestSection.Borders[ExcelBordersIndex.DiagonalUp].LineStyle = ExcelLineStyle.None;
            requestSection.Borders[ExcelBordersIndex.DiagonalDown].LineStyle = ExcelLineStyle.None;


            var my1stRow = requestSection
                //[stratRequestSection - 1, 2, stratRequestSection - 1, 80];
                [requestSection.Row, requestSection.Column, requestSection.Row, requestSection.LastColumn];
            my1stRow.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            my1stRow.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = this.SectionBorder;


            var myLastRow = _MainWorksheet
                //[stratRequestSection + 1, 2, stratRequestSection + 1, 80];
                [requestSection.LastRow, requestSection.Column, requestSection.LastRow, requestSection.LastColumn];
            myLastRow.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            myLastRow.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = this.SectionBorder;


            var my1stCol = _MainWorksheet
                //[stratRequestSection - 1, 2, stratRequestSection + 1, 2];
                [requestSection.Row, requestSection.Column, requestSection.LastRow, requestSection.Column];
            my1stCol.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
            my1stCol.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = this.SectionBorder;


            var myLastCol = _MainWorksheet
                //[stratRequestSection - 1, 80, stratRequestSection + 1, 80];
                [requestSection.Row, requestSection.LastColumn, requestSection.LastRow, requestSection.LastColumn];
            myLastCol.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
            myLastCol.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = this.SectionBorder;
        }

        protected IRange SetSubHeader(IRange lastRange, string Text,int? the1stColumn=null)
        {
            int col = lastRange.Column;
            if (the1stColumn.GetValueOrDefault()> 0)
            {
                col = the1stColumn.GetValueOrDefault();
            }
             
            var currHeader = _MainWorksheet[lastRange.LastRow + 2, col, lastRange.LastRow + 2, ReportWidth / 10 - 2];
            currHeader.Merge();
            currHeader[currHeader.Row, currHeader.Column].Text = Text;// "תנועות אשראי";
            currHeader[currHeader.Row, currHeader.Column].VerticalAlignment = ExcelVAlign.VAlignCenter;
            currHeader[currHeader.Row, currHeader.Column].HorizontalAlignment = ExcelHAlign.HAlignRight;

            currHeader[currHeader.Row, currHeader.Column].CellStyle.Font.RGBColor = //this.ResultHeaderColor;
                this.SectionBorder;
            currHeader[currHeader.Row, currHeader.Column].IndentLevel = 0;
            return currHeader;
        }
    }

    public class GridColumnMetaData
    {
        public GridColumnTypeEnum GridColumnType { get; internal set; }
        public string Header { get; internal set; }
        public int length { get; internal set; }
        public string PropName { get; internal set; }
        public ExcelHAlignEnum ExcelHAlign { get; internal set; }
    }
    public enum GridColumnTypeEnum
    {
        Object = 0,
        Number,
        Text,
        TrueFalse
    }

    public enum ExcelHAlignEnum
    {
        HAlignGeneral = 0,
        HAlignLeft = 1,
        HAlignCenter = 2,
        HAlignRight = 3,
    }

    public class LabelEditBox : GridColumnMetaData
    {
        //public string Label { get; set; }
        public int LabelSize { get; set; }
        public string TheValue { get; internal set; }
    }

}