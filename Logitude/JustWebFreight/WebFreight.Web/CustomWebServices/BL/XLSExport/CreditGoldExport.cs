using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using WebFreight.Web.CustomWebServices.BL.XLSExport;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    //http://localhost:9996/api/CommunicationLogStep/GetExportExcelByLogId/?mainInterfaceCode=8289Z&logId=1-775051&tenant=1
    public class CreditGoldExport : XLSExportBase<CreditQueryRequestParams, RTGSInfoQueryResponseData>, IExcelExport
    {
        

        public string MainInterfaceCode
        {
            get
            {
                return "8289Z";
            }
        }

        public override int ReportWidth
        {
            get
            {
                return 1200;
            }
        }


        protected override IRange AdjustAndFormatRequestRange(CreditQueryRequestParams requestParams, IWorksheet requestWorksheet)
        {
            requestWorksheet.IsGridLinesVisible = false;
            var myRange = requestWorksheet[1, 1, 7, 5];




            myRange.CellStyle.Color = Color.FromArgb(198, 215, 239);


            requestWorksheet.SetRowHeight(2, 10/*px*/);

            return myRange;
        }

        private static List<LabelEditBox> GetlineMetaData()
        {
            return new List<LabelEditBox>()
            {
                new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Text,
                      Header="תאריך הקמה:",
                        LabelSize =7,
                         PropName ="CreationDate",
                          length=7
                },
                new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Text,
                      Header="הפקדות:",
                        LabelSize =7,
                         PropName ="RTGSDeposits",
                          length=7
                },
                new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Text,
                      Header= "ניצולים:",
                        LabelSize =7,
                         PropName ="RTGSUsed",
                          length=7
                },
                new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Text,
                      Header= "יתרה נוכחית:",
                        LabelSize =7,
                         PropName ="RTGSCurrentBalance",
                          length=7
                },
                new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Text,
                      Header= "האם פעיל:",
                        LabelSize =7,
                         PropName ="ActiveInd",
                          length=7
                }


            };
        }

        protected override IRange AdjustAndFormatResponseRange(RTGSInfoQueryResponseData responseData, IWorksheet worksheet)
        {
            
            //var gridRows = responseData.CurrencyRateList.Count();
            var headerRange = worksheet[1, 1, 1, 11];
            headerRange[1, 1].Text = TranslateText("Customs.CreditGoldQuery.O.TransactionType");
            worksheet.SetColumnWidthInPixels(1, 80);

            headerRange[1, 2].Text = TranslateText("Customs.CreditGoldQuery.O.PaymentID");
            worksheet.SetColumnWidthInPixels(2, 80);

            headerRange[1, 3].Text = TranslateText("Customs.CreditGoldQuery.O.PaymentStatus");
            worksheet.SetColumnWidthInPixels(3, 80);


            headerRange[1, 4].Text = TranslateText("Customs.CreditGoldQuery.O.PaymentDate");
            worksheet.SetColumnWidthInPixels(4, 80);
            
            headerRange[1, 5].Text = TranslateText("Customs.CreditGoldQuery.O.CustomFileNo");
            worksheet.SetColumnWidthInPixels(5, 80);

            headerRange[1, 6].Text = TranslateText("Customs.CreditGoldQuery.O.EntityType");
            worksheet.SetColumnWidthInPixels(6, 80);

            headerRange[1, 7].Text = TranslateText("Customs.CreditGoldQuery.O.EntityID");
            worksheet.SetColumnWidthInPixels(7, 90);

            headerRange[1, 8].Text = TranslateText("Customs.CreditGoldQuery.O.TransactionAmount");
            worksheet.SetColumnWidthInPixels(8, 80);

            headerRange[1, 9].Text = TranslateText("Customs.CreditGoldQuery.O.RTGSBalance");
            worksheet.SetColumnWidthInPixels(9, 80);

            headerRange[1, 10].Text = TranslateText("Customs.CreditGoldQuery.O.RTGSRefund");
            worksheet.SetColumnWidthInPixels(10, 80);

            headerRange[1, 11].Text = TranslateText("Customs.CreditGoldQuery.O.UpdateUser");
            worksheet.SetColumnWidthInPixels(11, 90);


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
        
            font.Italic = true;



            IRange rowsRange = null;
            if (responseData != null)
            {
                rowsRange = worksheet.Range[1, 1, 2 + responseData.TransactionsList.Count, 11];
                bool oddEven = false;
                int line = 2;
                foreach (var Transaction in responseData.TransactionsList)
                {

                    rowsRange[line, 1].Text = Transaction.TransactionType;
                    rowsRange[line, 2].Text = Transaction.PaymentID;
                    rowsRange[line, 3].Text = Transaction.PaymentStatus;
                    rowsRange[line, 4].Text = Transaction.PaymentDate;
                    rowsRange[line, 5].Text = Transaction.CustomFileNo;
                    rowsRange[line, 6].Text = Transaction.EntityType;
                    rowsRange[line, 7].Text = Transaction.EntityID;
                    rowsRange[line, 8].Text = Transaction.TransactionAmount;
                    rowsRange[line, 9].Text = Transaction.RTGSBalance;
                    rowsRange[line, 10].Text = responseData.RTGSRefund;
                    rowsRange[line, 11].Text = Transaction.UpdateUser;


                    oddEven = !oddEven;
                    line++;
                }


            }
            else
            {
                rowsRange = worksheet.Range[2, 1, 2, 11];//Empty Row
            }
            
            var requestListObj = _MainWorksheet.ListObjects.Create("Request ", rowsRange);
            requestListObj.BuiltInTableStyle = TableBuiltInStyles.TableStyleLight9;//  "TableStyleLight9"


            return rowsRange;


        }

        protected override IRange AdjustRequest(CreditQueryRequestParams requestParams, RTGSInfoQueryResponseData responseData)
        {

       


            IRange mainHeaderRange = SetSubHeader(_MainWorksheet[1, 1], "שאילתא לתקרת זהב");
            mainHeaderRange.HorizontalAlignment = ExcelHAlign.HAlignRight;
            mainHeaderRange.CellStyle.Font.Bold = true;


            var requestSection = GetRequestSection(requestParams, mainHeaderRange);

            var resultHeader = SetSubHeader(requestSection, "תוצאות שאילתא לתקרת זהב");
            
            var responseSection = GetResponseSection(requestParams, responseData, resultHeader);




            return _MainWorksheet.Range;
        }

    

        private IRange GetResponseSection(CreditQueryRequestParams requestParams, RTGSInfoQueryResponseData responseData, IRange resultHeader)
        {
            IRange firstLine =
                _MainWorksheet[resultHeader.Row +1, 1 , resultHeader.Row + 1, 95];

            
            IRange thirdLine =
               CreateLabelEditboxLine(responseData, firstLine, GetlineMetaData());

            IRange GuaranteeLettersList =
                BuildGenList("TransactionsList",
                responseData.TransactionsList, GetCreditGoldColumnList(), _MainWorksheet[thirdLine.Row + 1, 1, thirdLine.Row + 2, 95]);


          
            int identColumn = 2;
            var last = GuaranteeLettersList;
            var responseSection = _MainWorksheet[firstLine.Row , last.Column, last.LastRow + 1, 95];

            var destRange = _MainWorksheet[firstLine.Row, last.Column + identColumn, last.LastRow + 1, 95];
            responseSection.MoveTo(destRange);


            MakeBorderSection(destRange);
            return null;

        }



        private List<GridColumnMetaData> GetCreditGoldColumnList()
        {
            var l = new List<GridColumnMetaData>() {

            new GridColumnMetaData()
            {
                PropName = "TransactionType",
                length = 80,
                Header = "תנועה " ,
                  ExcelHAlign = ExcelHAlignEnum.HAlignRight,

            }
            ,
            new GridColumnMetaData()
            { PropName = "PaymentID", length = 80,
                Header = "הוראת תשלום "
            }
            ,
                new GridColumnMetaData()
            {
                PropName = "PaymentStatus",
                length = 80,
                Header = "סטטוס הוראה "
            },
                 new GridColumnMetaData()
            {
                PropName = "PaymentDate",
                length = 80,
                Header = "תאריך תשלום "
            }

            , 
                new GridColumnMetaData()
            {
                PropName = "CustomFileNo",
                length = 80,
                Header = "תיק עמילות "
            },
              
                new GridColumnMetaData()
            {
                PropName = "EntityType",
                length = 80,
                Header = "סוג ישות "
            },
               
                new GridColumnMetaData()
            {
                PropName = "EntityID",
                length = 90,
                Header = "מספר ישות ",
                GridColumnType = GridColumnTypeEnum.Text,
                ExcelHAlign = ExcelHAlignEnum.HAlignRight

            },
                 
                new GridColumnMetaData()
            {
                PropName = "TransactionAmount",
                length = 80,
                Header = "סכום "
            },
                 
                new GridColumnMetaData()
            {
                PropName = "RTGSBalance",
                length = 80,
                Header = "יתרה  "
            },
                
                new GridColumnMetaData()
            {
                PropName = "RTGSRefund",
                length = 80,
                Header = "החזרים "
            },
                
                new GridColumnMetaData()
            {
                PropName = "UpdateUser",
                length = 90,
                Header = "משתמש מעדכן "
            },
                
            


            };
            return l;
        }

        private IRange GetRequestSection(CreditQueryRequestParams requestParams, IRange mainHeaderRange)
        {
            
            


            var myline1 = base.CreateLabelEditboxLine(
                requestParams,
                _MainWorksheet[mainHeaderRange.LastRow + 1, 1, mainHeaderRange.LastRow + 1, 95],
                new List<LabelEditBox>()
                {
                    new LabelEditBox()
                    {
                         Header="ח.פ סוכן :",LabelSize=10,length=8,PropName="AgentExternalID"
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
                         Header="מספר סוכן :",LabelSize=10,length=8,PropName="AgentID"
                         ,GridColumnType= GridColumnTypeEnum.Text

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
                         Header="ח.פ יבואן :",LabelSize=10,length=8,PropName="ExternalID"
                         ,GridColumnType= GridColumnTypeEnum.Text

                    }
                }
                );
            var myline4 = base.CreateLabelEditboxLine(
                requestParams,
                myline3,
                new List<LabelEditBox>()
                {
                     new LabelEditBox()
                    {
                         Header="מתאריך :",LabelSize=10,length=8,PropName=  "DateFrom",GridColumnType= GridColumnTypeEnum.Object

                    }
                }
                );
            var myline5 = base.CreateLabelEditboxLine(
                requestParams,
                myline4,
                new List<LabelEditBox>()
                {
                     new LabelEditBox()
                    {
                         Header="עד תאריך :",LabelSize=10,length=8,PropName="DateTo",GridColumnType = GridColumnTypeEnum.Object

                    }
                }
                );
            
            int identStartFromC = 5;
            Ident3Columns(myline1.LastRow /*+ 2*/, myline5.Row,  identStartFromC);

            IRange requestSection = SetRequestSection1LineB4And1LineAfter(myline1.LastRow, myline5.Row);
            MakeBorderSection(requestSection);

            

            return requestSection;
        }

        private IRange SetRequestSection1LineB4And1LineAfter(int saveStartLine ,int lastLine)
        {
            var requestSection =
                            _MainWorksheet[saveStartLine - 1, 3, lastLine + 1, 95];

            _IWorkbook.Names.Add("requestSection", requestSection);
            return requestSection;
        }

        private void Ident3Columns(int sectionStartLine, int currentLine,  int identStartFromC)
        {
            var dest = _MainWorksheet[sectionStartLine, 1 + identStartFromC, currentLine, 80 + identStartFromC];
            var source =
                _MainWorksheet[sectionStartLine, 1, currentLine, 80];
            ;
            source.MoveTo(dest);
        }
    }
}