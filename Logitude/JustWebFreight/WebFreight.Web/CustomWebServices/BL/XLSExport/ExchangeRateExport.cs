using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using WebFreight.Web.CustomWebServices.BL.XLSExport;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    //http://localhost:9996/api/CommunicationLogStep/GetExportExcelByLogId/?mainInterfaceCode=8347&logId=1-775051&tenant=1
    public class ExchangeRateExport: XLSExportBase<CD_NG_8347_Web01_CurrencyRateSearchRequestParams, CD_NG_8348_Web02_CurrencyRateDetailResponseData>, IExcelExport
    {
        

        public string MainInterfaceCode
        {
            get
            {
                return "8347";
            }
        }

        public override int ReportWidth
        {
            get
            {
                return 450;
            }
        }


        protected override IRange AdjustAndFormatRequestRange(CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams, IWorksheet requestWorksheet)
        {
            requestWorksheet.IsGridLinesVisible = false;
            //throw new NotImplementedException();
            var myRange = requestWorksheet[1, 1, 7, 5];


#if false


            myRange[1, 1].CellStyle = _LabelStyle;
            myRange[1, 1].Text = TranslateText("Customs.CustomsExchangeRate.F.DateFrom");


            myRange[1, 3].CellStyle = _EditBoxStyle;
            myRange[1, 3].DateTime = requestParams.FromDate.Value;


            

            myRange[3, 1].CellStyle = _LabelStyle;
            myRange[3, 1].Text = TextCodesTranslator.TranslateText("Customs.CustomsExchangeRate.F.DateTo", this.Tenant);



            myRange[3, 3].CellStyle = _EditBoxStyle;
            myRange[3, 3].DateTime = requestParams.ToDate.Value;




            myRange[5, 1].CellStyle = _LabelStyle;
            myRange[5, 1].Text = "קוד מטבע";



            myRange[5, 3].CellStyle = _EditBoxStyle;
            myRange[5, 3].Text = requestParams.CurrencyTypeId;



            AddEditBoxBorder(myRange[1, 3]);
            AddEditBoxBorder(myRange[3, 3]);
            AddEditBoxBorder(myRange[5, 3]);
#endif

            myRange.CellStyle.Color = Color.FromArgb(198, 215, 239);
            //myRange.AutofitRows();
            //myRange.AutofitColumns();


            requestWorksheet.SetRowHeight(2, 10/*px*/);

            return myRange;
        }

        

        protected override IRange AdjustAndFormatResponseRange(CD_NG_8348_Web02_CurrencyRateDetailResponseData responseData, IWorksheet worksheet)
        {
            
            //var gridRows = responseData.CurrencyRateList.Count();
            var headerRange = worksheet[1, 1, 1, 4];
            headerRange[1, 1].Text = "קוד מטבע";
            worksheet.SetColumnWidth(1, 100/10);

            headerRange[1, 2].Text = TranslateText("Customs.ExchangeRate.O.CurrencyTypeName");
            worksheet.SetColumnWidth(2, 100/10);


            headerRange[1, 3].Text = "שער";
            worksheet.SetColumnWidth(3, 100/10);


            headerRange[1, 4].Text = "תאריך שער";
            worksheet.SetColumnWidth(4, 150/10);

            worksheet.IsGridLinesVisible = false;
            //headerRange.Activate();

            var style = headerRange.CellStyle;
            style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            if (worksheet.Workbook.Version == ExcelVersion.Excel97to2003)
            {
                // Excel97to2003 version does not support gradient fill type
            }
            else
            {
                style.Interior.FillPattern = ExcelPattern.Gradient;// Excel97to2003 version does not support gradient fill type
                style.Interior.Gradient.TwoColorGradient(ExcelGradientStyle.Diagonl_Up, ExcelGradientVariants.ShadingVariants_3);
                style.Interior.Gradient.ForeColor = Color.FromArgb(245, 245, 245);
                style.Interior.Gradient.BackColor = Color.FromArgb(247, 247, 247);
            }
            IFont font = style.Font;
            //font.FontName = "Segoe UI";
            //font.Size = 22;
            font.Italic = true;
            //"linear-gradient(rgb(247, 247, 247) 0%, rgb(245, 245, 245) 52.38"



            IRange rowsRange = null;
            if (responseData != null)
            {
                rowsRange = worksheet.Range[2, 1, 2 + responseData.CurrencyRateList.Count, 4];
                bool oddEven = false;
                int line = 2;
                foreach (var currencyRate in responseData.CurrencyRateList)
                {

                    rowsRange[line, 1].Text = currencyRate.CurrencyTypeId;
                    rowsRange[line, 2].Text = currencyRate.CurrencyTypeName;
                    rowsRange[line, 3].Number = (double)currencyRate.CustomsCurrencyRate.Value;
                    rowsRange[line, 4].DateTime = currencyRate.StartDate.Value;


                    oddEven = !oddEven;
                    line++;
                }


            }
            else
            {
                rowsRange = worksheet.Range[2, 1, 2, 4];//Empty Row
            }
            //IRanges responseRange = worksheet.CreateRangesCollection();

            // range1 and range2 are considered as a single range
            //responseRange.Add(headerRange);
            //responseRange.Add(rowsRange);
            var requestListObj = _MainWorksheet.ListObjects.Create("Request ", rowsRange);
            requestListObj.BuiltInTableStyle = TableBuiltInStyles.TableStyleLight9;//  "TableStyleLight9"


            return rowsRange;


        }

        protected override IRange AdjustRequest(CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams, CD_NG_8348_Web02_CurrencyRateDetailResponseData responseData)
        {

            //ReportWidth = 650;
            //row ==32 


            //_MainWorksheet.Range.RowHeight = GetExcelSizeFromPixel(32);

            IRange mainHeaderRange = SetSubHeader(_MainWorksheet[1, 1], "שאילתא לשערים");
            mainHeaderRange.HorizontalAlignment = ExcelHAlign.HAlignRight;
            mainHeaderRange.CellStyle.Font.Bold = true;

            replaceRequestParamsCurrencyTypeIdFromResLineCurrencyTypeName(requestParams, responseData);

            var requestSection = GetRequestSection(requestParams, mainHeaderRange);

            //var resposeHeaderRange = _MainWorksheet[requestSection.Row, 1];//, requestSection.Row, this.ReportWidth / 10];
            var resultHeader = SetSubHeader(requestSection, "תוצאות שאילתא לשערים");
            var responseSection = GetResponseSection(requestParams, responseData, resultHeader);




            //_MainWorksheet[1, 10, 1, 25].Merge();//requestParams.InterfaceTypeCode//150
            return _MainWorksheet.Range;
        }

        private static void replaceRequestParamsCurrencyTypeIdFromResLineCurrencyTypeName(CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams, CD_NG_8348_Web02_CurrencyRateDetailResponseData responseData)
        {
            if (!string.IsNullOrWhiteSpace(requestParams.CurrencyTypeId))
            {
                var resLine = responseData.CurrencyRateList.FirstOrDefault(r => r.CurrencyTypeId == requestParams.CurrencyTypeId);
                if (resLine != null)
                {
                    requestParams.CurrencyTypeId = resLine.CurrencyTypeName;
                }
            }
        }

        private IRange GetResponseSection(CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams, CD_NG_8348_Web02_CurrencyRateDetailResponseData responseData, IRange resultHeader)
        {
            IRange firstLine =
                _MainWorksheet[resultHeader.Row +1, 1 , resultHeader.Row + 1, this.ReportWidth / 10];
             ;
            

            IRange GuaranteeLettersList =
                BuildGenList("CurrencyRateList",
                responseData.CurrencyRateList, GetCurrencyRateColumnList(), firstLine);

            int identColumn = 2;
            var last = GuaranteeLettersList;
            var responseSection = _MainWorksheet[firstLine.Row , last.Column, last.LastRow + 1, this.ReportWidth / 10];

            var destRange = _MainWorksheet[firstLine.Row , last.Column + identColumn, last.LastRow + 1, this.ReportWidth / 10];
            responseSection.MoveTo(destRange);


            MakeBorderSection(destRange);
            return null;

        }



        private List<GridColumnMetaData> GetCurrencyRateColumnList()
        {
            var l = new List<GridColumnMetaData>() {

            new GridColumnMetaData()
            {
                PropName = "CurrencyTypeId",
                length = 100,
                Header = "קוד מטבע " ,
                  ExcelHAlign = ExcelHAlignEnum.HAlignRight,

            }
            ,//var CertificateID = 
            new GridColumnMetaData()
            { PropName = "CurrencyTypeName", length = 100,
                Header = "שם מטבע "
            }
            ,//var GuaranteeExternalCertificateNumebr = 
                new GridColumnMetaData()
            {
                PropName = "CustomsCurrencyRate",
                length = 110,
                Header = "שער "
            }

            ,//var GuaranatorName = 
                new GridColumnMetaData()
            {
                PropName = "StartDate",
                length = 90,
                Header = "תאריך שער"
            }
            };
            return l;
        }

        private IRange GetRequestSection(CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams, IRange mainHeaderRange)
        {
            
            


            //100|150|10|100|150|10|100|150|10|100|150|10 == 851
            var myline1 = base.CreateLabelEditboxLine(
                requestParams,
                _MainWorksheet[mainHeaderRange.LastRow + 1, 1, mainHeaderRange.LastRow + 1, ReportWidth / 10],
                new List<LabelEditBox>()
                {
                    new LabelEditBox()
                    {
                         Header="מתאריך:",LabelSize=10,length=11,PropName="FromDate" ,
                        //TheValue =GetGuaranteeName(requestParams.GuranteeType)
                    }
                }
                );


            var myline2 = base.CreateLabelEditboxLine(
                requestParams,
                myline1,
                new List<LabelEditBox>()
                {
                    new LabelEditBox()
                    {
                         Header="עד תאריך:",LabelSize=10,length=11,PropName="ToDate"
                         ,GridColumnType= GridColumnTypeEnum.Object

                    }
                }
                );


            var myline3 = base.CreateLabelEditboxLine(
                requestParams,
                myline2,
                new List<LabelEditBox>()
                {
                     new LabelEditBox()
                    {
                         Header="קוד מטבע :",LabelSize=10,length=11,PropName="CurrencyTypeId"
                         ,GridColumnType= GridColumnTypeEnum.Text

                    }
                }
                );

            
            int identStartFromC = 3;//
            Ident3Columns(myline1.LastRow /*+ 2*/, myline3.Row,  identStartFromC);

            IRange requestSection = SetRequestSection1LineB4And1LineAfter(myline1.LastRow, myline3.Row);
            MakeBorderSection(requestSection);



            return requestSection;
        }

        private IRange SetRequestSection1LineB4And1LineAfter(int saveStartLine ,int lastLine)
        {
            var requestSection =
                            _MainWorksheet[saveStartLine - 1, 3, lastLine + 1, this.ReportWidth / 10];

            _IWorkbook.Names.Add("requestSection", requestSection);
            return requestSection;
        }

        private void Ident3Columns(int sectionStartLine, int currentLine,  int identStartFromC)
        {
            var dest = _MainWorksheet[sectionStartLine, 1 + identStartFromC, currentLine, this.ReportWidth / 10 + identStartFromC];
            var source =
                _MainWorksheet[sectionStartLine, 1, currentLine, this.ReportWidth / 10];
            ;
            source.MoveTo(dest);
        }
    }
}