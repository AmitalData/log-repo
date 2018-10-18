using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Syncfusion.XlsIO;
using System.Globalization;
using System.Collections;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public class GuaranteeExport
       : XLSExportBase<GuaranteeRequestParams,
        GuaranteeResponseData>, IExcelExport
    {
        public override int ReportWidth
        {
            get
            {
                return 880;//877*71  === 880*70 
            }
        }

        public string MainInterfaceCode
        {
            get
            {
                return "8305";
            }
        }

        protected override IRange AdjustAndFormatRequestRange(GuaranteeRequestParams requestParams, IWorksheet requestWorksheet)
        {

            throw new NotImplementedException();
            //877*71  === 880*70 
            //row ==32 
            //100|150|10|100|150|10|100|150|10|100|150|10 == 851



        }

        protected override IRange AdjustAndFormatResponseRange(GuaranteeResponseData responseData, IWorksheet worksheet)
        {
            throw new NotImplementedException();
        }

        protected override IRange AdjustRequest(GuaranteeRequestParams requestParams, GuaranteeResponseData responseData)
        {

            //877*71  === 880*70 
            //ReportWidth = 880;
            //row ==32 


            //_MainWorksheet.Range.RowHeight = GetExcelSizeFromPixel(32);

            IRange mainHeaderRange = SetSubHeader(_MainWorksheet[1, 1], "שאילתא לערבויות");
            mainHeaderRange.HorizontalAlignment = ExcelHAlign.HAlignRight;
            mainHeaderRange.CellStyle.Font.Bold = true;

            var requestSection = GetRequestSection(requestParams, mainHeaderRange);

            var resultHeader = SetSubHeader(requestSection, "תוצאות שאילתא לערבויות");
            var responseSection = GetResponseSection(requestParams, responseData, resultHeader);




            //_MainWorksheet[1, 10, 1, 25].Merge();//requestParams.InterfaceTypeCode//150
            return _MainWorksheet.Range;
        }

        private IRange GetResponseSection(GuaranteeRequestParams requestParams, GuaranteeResponseData responseData, IRange resultHeader)
        {
            IRange firstLine =
            //GetResponse1Line(requestParams, responseData, resultHeader);
            CreateLabelEditboxLine(responseData, resultHeader, Get1lineMetaData());
            IRange secondLine = CreateLabelEditboxLine(responseData, firstLine, Get2lineMetaData());
            IRange thirdLine =
                //GetResponse3Line(requestParams, responseData, secondLine);
                CreateLabelEditboxLine(responseData, secondLine, Get3lineMetaData());
            IRange Line4 = //GetResponse4Line(requestParams, responseData, thirdLine);
                CreateLabelEditboxLine(responseData, thirdLine, Get4lineMetaData());

            IRange GuaranteeLettersListHeader = SetSubHeader(Line4, "רשימת כתבי ערבות");

            IRange GuaranteeLettersList =
                //BuildGuaranteeLettersList(responseData , GuaranteeLettersListHeader);
                BuildGenList("GuaranteeLetters",
                responseData.GuaranteeLettersList, GetGuaranteeLettersColumnList(), GuaranteeLettersListHeader);

            IRange CreditTransactionsListHeader = SetSubHeader(GuaranteeLettersList, "תנועות אשראי");
            IRange CreditTransactionsList =
                //BuildCreditTransactionsList(responseData, CreditTransactionsListHeader);
                BuildGenList("CreditTransactions",
                responseData.CreditTransactionsList, GetCreditTransactionsColumnList(), CreditTransactionsListHeader);




            IRange RequireDocumentsListHeader = SetSubHeader(CreditTransactionsList, "מסמכים נדרשים");
            IRange RequireDocumentsList = BuildGenList("RequireDocuments",
                responseData.RequireDocumentsList, GetRequireDocumentsGridColumnMeteData(), RequireDocumentsListHeader);



            var last = RequireDocumentsList;
            var responseSection = _MainWorksheet[firstLine.Row - 1, firstLine.Column, last.LastRow + 1, this.ReportWidth / 10];

            var destRange = _MainWorksheet[firstLine.Row - 1, firstLine.Column + 1, last.LastRow + 1, this.ReportWidth / 10];
            responseSection.MoveTo(destRange);


            MakeBorderSection(destRange);
            return null;
        }

        private List<LabelEditBox> Get1lineMetaData()
        {
            return new List<XLSExport.LabelEditBox>()
            {
                new XLSExport.LabelEditBox()
                {
                    PropName="DisplayFileNumber",
                    length=15,
                     Header= "מספר תיק:",
                      LabelSize=10,


                },
                new XLSExport.LabelEditBox()
                {
                    PropName="CustomOfficeName",
                    length=15,
                     Header= "בית המכס:",
                      LabelSize=10,


                },
                new XLSExport.LabelEditBox()
                {
                    PropName="CreditLimit",
                    length=15,
                     Header=  "תקרת אשראי:",
                      LabelSize=10,
                       GridColumnType = GridColumnTypeEnum.Number


                }
            };
        }

        private List<GridColumnMetaData> GetRequireDocumentsGridColumnMeteData()
        {
            return new List<XLSExport.GridColumnMetaData>()
            {
                new XLSExport.GridColumnMetaData()
                {
                    PropName="TypeName",
                    length=100,
                     Header="סוג מסמך"
                },
                new XLSExport.GridColumnMetaData()
                {
                    PropName="DisplayFileNumber",
                    length=90,
                     Header="מספר תיק"
                },
                new XLSExport.GridColumnMetaData()
                {
                    PropName="DocumentID",
                    length=100,
                     Header="סימוכין מכס"
                }
            };
        }



        private List<GridColumnMetaData> GetCreditTransactionsColumnList()
        {
            var l = new List<GridColumnMetaData>() {

            new GridColumnMetaData()
            {
                PropName = "CreditTransactionDate",
                length = 100,
                Header = "מועד תנועה"
            }
            ,
            new GridColumnMetaData()
            { PropName = "CreditTransactionName",
                length = 90,
                Header = "סוג תנועה"
            }
            ,
                new GridColumnMetaData()
            {
                PropName = "EntityTypeName",
                length = 100,
                Header = "ישות"
            }

            ,
                new GridColumnMetaData()
            {
                PropName = "EntityNumber",
                length = 100,
                Header ="מספר ישות"
            }
            ,//var GuaranteeValidityDate = 
                new GridColumnMetaData()
            {
                PropName = "CreditTransactionAmount",
                length = 80,
                Header = "סכום",
                GridColumnType =  GridColumnTypeEnum.Number
            }

            };
            return l;
        }







        private static List<GridColumnMetaData> GetGuaranteeLettersColumnList()
        {
            var l = new List<GridColumnMetaData>() { 
            //var GuaranteeTypeName = 
            new GridColumnMetaData()
            {
                PropName = "GuaranteeTypeName",
                length = 100,
                Header = "כתב ערבות"
            }
            ,//var CertificateID = 
            new GridColumnMetaData()
            { PropName = "CertificateID", length = 100,
                Header = "מספר ערבות פנימי"
            }
            ,//var GuaranteeExternalCertificateNumebr = 
                new GridColumnMetaData()
            {
                PropName = "GuaranteeExternalCertificateNumebr",
                length = 110,
                Header = "מספר ערבות חיצוני"
            }

            ,//var GuaranatorName = 
                new GridColumnMetaData()
            {
                PropName = "GuaranatorName",
                length = 90,
                Header = "שם הערב"
            }
            ,//var GuaranteeValidityDate = 
                new GridColumnMetaData()
            {
                PropName = "GuaranteeValidityDate",
                length = 80,
                Header = "תוקף הערבות"
            }
            ,//var CertificateAmount = 
                new GridColumnMetaData()
            {
                PropName = "CertificateAmount",
                length = 95,
                Header = "סכום ערבות כולל",
                GridColumnType =  GridColumnTypeEnum.Number

            }
            ,//var CertificateAllocation = 
                new GridColumnMetaData()
            {
                PropName = "CertificateAllocation",
                length = 90,
                Header = "סכום מוקצה",
                GridColumnType =  GridColumnTypeEnum.Number
            }
            ,//var AvaliableCertificateAmount = 
                new GridColumnMetaData()
            {
                PropName = "AvaliableCertificateAmount",
                length = 80,
                Header = "סכום פנוי",
                GridColumnType =  GridColumnTypeEnum.Number
            }

            ,//var GuaranteeStatusName = 
                new GridColumnMetaData()
            {
                PropName = "GuaranteeStatusName",
                length = 90,
                Header = "סטטוס"
            }
            };
            return l;
        }

        private static List<LabelEditBox> Get4lineMetaData()
        {
            return new List<LabelEditBox>()
            {
                new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Object,
                      Header="הסוכן:",
                        LabelSize =10,
                         PropName ="AgentName",
                          length=15
                },
                new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Object,
                      Header="תוקף:",
                    LabelSize =10,
                         PropName ="Validity",
                          length=15
                },
                new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Number,
                      Header=  "סכום ערבות:",
            LabelSize =10,
                         PropName ="GuaranteeAmount",
                          length=15
                }


            };
        }








        private static List<LabelEditBox> Get2lineMetaData()
        {
            return new List<LabelEditBox>()
            {
                new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Object,
                      Header="סטטוס התיק:",
                        LabelSize =10,
                         PropName ="StatusName",
                          length=15
                },

new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Object,
                      Header="ישות:",
            LabelSize =10,
                         PropName ="EntityTypeName",
                          length=15
                },

new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Number,
                      Header="יתרת אשראי:",
            LabelSize =10,
                         PropName ="CreditBalance",
                          length=15
                },



            };
        }

        private static List<LabelEditBox> Get3lineMetaData()
        {
            return new List<LabelEditBox>()
            {
                new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Object,
                      Header="שם הנערב:",
                        LabelSize =10,
                         PropName ="GuaranteedName",
                          length=15
                },
                new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Object,
                      Header="מספר ישות:",
            LabelSize =10,
                         PropName ="EntityNumber",
                          length=15
                },
                new LabelEditBox()
                {
                     GridColumnType = GridColumnTypeEnum.Number,
                      Header= "סכום שממומש:",
            LabelSize =10,
                         PropName ="GuaranteeExecutedAmountAdjusted",
                          length=15
                }


            };
        }
#if false

           private IRange GetResponse3Line(GuaranteeRequestParams requestParams, GuaranteeResponseData responseData, IRange secondLine)
        {
            //90,150,10//90,150,10//90,150,10//90,150,10 == 850
            var startRow = secondLine.LastRow + 2;
            var GuaranteedNameLabel = _MainWorksheet[startRow, 1, startRow, 1 + 9];
            GuaranteedNameLabel.Merge();
            _IWorkbook.Names.Add("GuaranteedNameLabel", GuaranteedNameLabel);
            GuaranteedNameLabel.Text = "שם הנערב:";
            SetAsLabelStyle(GuaranteedNameLabel);

            var GuaranteedNameEdit = _MainWorksheet[startRow, GuaranteedNameLabel.LastColumn + 1, startRow, GuaranteedNameLabel.LastColumn + 15 + 1];
            GuaranteedNameEdit.Merge();
            _IWorkbook.Names.Add("GuaranteedNameEdit", GuaranteedNameEdit);
            GuaranteedNameEdit.Value = responseData.GuaranteedName;

            SetAsEditBoxStyle(GuaranteedNameEdit);



            var EntityNumberLabel = _MainWorksheet[startRow, GuaranteedNameEdit.LastColumn + 1, startRow, GuaranteedNameEdit.LastColumn + 1 + 10];
            EntityNumberLabel.Merge();
            _IWorkbook.Names.Add("EntityNumberLabel", EntityNumberLabel);
            EntityNumberLabel.Text = "מספר ישות:";
            SetAsLabelStyle(EntityNumberLabel);

            var EntityNumberEdit = _MainWorksheet[startRow, EntityNumberLabel.LastColumn + 1, startRow, EntityNumberLabel.LastColumn + 1 + 15];
            EntityNumberEdit.Merge();
            _IWorkbook.Names.Add("EntityNumberEdit", EntityNumberEdit);
            EntityNumberEdit.Value = responseData.EntityNumber;
            SetAsEditBoxStyle(EntityNumberEdit);




            var GuaranteeExecutedAmountAdjustedLabel = _MainWorksheet[startRow, EntityNumberEdit.LastColumn + 1, startRow, EntityNumberEdit.LastColumn + 1 + 9];
            GuaranteeExecutedAmountAdjustedLabel.Merge();
            _IWorkbook.Names.Add("GuaranteeExecutedAmountAdjustedLabel", GuaranteeExecutedAmountAdjustedLabel);
            GuaranteeExecutedAmountAdjustedLabel.Text = "סכום שממומש:";
            SetAsLabelStyle(GuaranteeExecutedAmountAdjustedLabel);

            var GuaranteeExecutedAmountAdjustedEdit = _MainWorksheet[startRow, GuaranteeExecutedAmountAdjustedLabel.LastColumn + 1, startRow, GuaranteeExecutedAmountAdjustedLabel.LastColumn + 1 + 15];
            GuaranteeExecutedAmountAdjustedEdit.Merge();
            _IWorkbook.Names.Add("GuaranteeExecutedAmountAdjustedEdit", GuaranteeExecutedAmountAdjustedEdit);
            GuaranteeExecutedAmountAdjustedEdit.Value = responseData.GuaranteeExecutedAmountAdjusted;
            SetAsEditBoxStyle(GuaranteeExecutedAmountAdjustedEdit);



            return _MainWorksheet[GuaranteedNameLabel.Row, GuaranteedNameLabel.Column, GuaranteeExecutedAmountAdjustedEdit.Row, GuaranteeExecutedAmountAdjustedEdit.LastColumn];
        }

        private IRange OldLine2(GuaranteeResponseData responseData, int startRow)
        {
            var StatusNameLabel = _MainWorksheet[startRow, 1, startRow, 1 + 9];
            StatusNameLabel.Merge();
            _IWorkbook.Names.Add("StatusNameLabel", StatusNameLabel);
            StatusNameLabel.Text = "סטטוס התיק:";
            SetAsLabelStyle(StatusNameLabel);

            var StatusNameEdit = _MainWorksheet[startRow, StatusNameLabel.LastColumn + 1, startRow, StatusNameLabel.LastColumn + 15 + 1];
            StatusNameEdit.Merge();
            _IWorkbook.Names.Add("StatusName", StatusNameEdit);
            StatusNameEdit.Value = responseData.StatusName;

            SetAsEditBoxStyle(StatusNameEdit);




            var EntityTypeNameLabel = _MainWorksheet[startRow, StatusNameEdit.LastColumn + 1, startRow, StatusNameEdit.LastColumn + 1 + 10];
            EntityTypeNameLabel.Merge();
            _IWorkbook.Names.Add("EntityTypeNameLabel", EntityTypeNameLabel);
            EntityTypeNameLabel.Text = "ישות:";
            SetAsLabelStyle(EntityTypeNameLabel);

            var EntityTypeNameEdit = _MainWorksheet[startRow, EntityTypeNameLabel.LastColumn + 1, startRow, EntityTypeNameLabel.LastColumn + 1 + 15];
            EntityTypeNameEdit.Merge();
            _IWorkbook.Names.Add("EntityTypeNameEdit", EntityTypeNameEdit);
            EntityTypeNameEdit.Value = responseData.EntityTypeName ?? string.Empty;
            SetAsEditBoxStyle(EntityTypeNameEdit);




            var CreditBalanceLabel = _MainWorksheet[startRow, EntityTypeNameEdit.LastColumn + 1, startRow, EntityTypeNameEdit.LastColumn + 1 + 9];
            CreditBalanceLabel.Merge();
            _IWorkbook.Names.Add("CreditBalanceLabel", CreditBalanceLabel);
            CreditBalanceLabel.Text = "יתרת אשראי:";
            SetAsLabelStyle(CreditBalanceLabel);

            var CreditBalanceEdit = _MainWorksheet[startRow, CreditBalanceLabel.LastColumn + 1, startRow, CreditBalanceLabel.LastColumn + 1 + 15];
            CreditBalanceEdit.Merge();
            _IWorkbook.Names.Add("CreditBalanceEdit", CreditBalanceEdit);
            CreditBalanceEdit.Value = responseData.CreditBalance;
            SetAsEditBoxStyle(CreditBalanceEdit);


            return _MainWorksheet[StatusNameLabel.Row, StatusNameLabel.Column, CreditBalanceEdit.Row, CreditBalanceEdit.LastColumn];
        }

        private IRange GetResponse1Line(GuaranteeRequestParams requestParams, GuaranteeResponseData responseData, IRange resultHeader)
        {
            //90,150,10//90,150,10//90,150,10//90,150,10 == 850
            var startRow = resultHeader.LastRow + 2;


            IRange DisplayFileNumberLabel = CreateLabel(startRow, 1, 10, "DisplayFileNumber", "מספר תיק:");

            IRange DisplayFileNumberEdit = CreateEditBox(responseData, startRow, "DisplayFileNumber", DisplayFileNumberLabel, 15);
            //var DisplayFileNumberEdit = _MainWorksheet[startRow, DisplayFileNumberLabel.LastColumn + 1, startRow, DisplayFileNumberLabel.LastColumn + 15 + 1];
            //DisplayFileNumberEdit.Merge();
            //_IWorkbook.Names.Add("DisplayFileNumberEdit", DisplayFileNumberEdit);
            //DisplayFileNumberEdit.Value = responseData.DisplayFileNumber;

            //var CustomOfficeNameLabel = _MainWorksheet[startRow, DisplayFileNumberEdit.LastColumn + 1, startRow, DisplayFileNumberEdit.LastColumn + 1 + 10];
            //CustomOfficeNameLabel.Merge();
            //_IWorkbook.Names.Add("CustomOfficeNameLabel", CustomOfficeNameLabel);
            //CustomOfficeNameLabel.Text = "בית המכס:";
            //SetAsLabelStyle(CustomOfficeNameLabel);
            var CustomOfficeNameLabel = CreateLabel(startRow, DisplayFileNumberEdit.LastColumn, 10, "CustomOfficeName", "בית המכס:");

            //var CustomOfficeNameEdit = _MainWorksheet[startRow, CustomOfficeNameLabel.LastColumn + 1, startRow, CustomOfficeNameLabel.LastColumn + 1 + 15];
            //CustomOfficeNameEdit.Merge();
            //_IWorkbook.Names.Add("CustomOfficeNameEdit", CustomOfficeNameEdit);
            //CustomOfficeNameEdit.Value = responseData.CustomOfficeName;
            //SetAsEditBoxStyle(CustomOfficeNameEdit);
            var CustomOfficeNameEdit = CreateEditBox(responseData, startRow, "CustomOfficeName", CustomOfficeNameLabel, 15);



            //var CreditLimitLabel = _MainWorksheet[startRow, CustomOfficeNameEdit.LastColumn + 1, startRow, CustomOfficeNameEdit.LastColumn + 1 + 9];
            //CreditLimitLabel.Merge();
            //_IWorkbook.Names.Add("CreditLimitLabel", CreditLimitLabel);
            //CreditLimitLabel.Text = "תקרת אשראי:";
            //SetAsLabelStyle(CreditLimitLabel);
            var CreditLimitLabel = CreateLabel(startRow, CustomOfficeNameEdit.LastColumn, 10, "CreditLimit", "תקרת אשראי:");
            //var CreditLimitEdit = _MainWorksheet[startRow, CreditLimitLabel.LastColumn + 1, startRow, CreditLimitLabel.LastColumn + 1 + 15];
            //CreditLimitEdit.Merge();
            //_IWorkbook.Names.Add("CreditLimitEdit", CreditLimitEdit);
            //CreditLimitEdit.Value = responseData.CreditLimit;
            //SetAsEditBoxStyle(CreditLimitEdit);
            var CreditLimitEdit =
                CreateEditBox(responseData, startRow, "CreditLimit", CreditLimitLabel, 15);

            return _MainWorksheet[DisplayFileNumberLabel.Row, DisplayFileNumberLabel.Column, CreditLimitEdit.Row, CreditLimitEdit.LastColumn];
        }


#endif




        private IRange GetRequestSection(GuaranteeRequestParams requestParams, IRange mainHeaderRange)
        {
            var stratRequestSection = mainHeaderRange.LastRow + 4;



            //100|150|10|100|150|10|100|150|10|100|150|10 == 851
            var myline = base.CreateLabelEditboxLine(
                requestParams,
                _MainWorksheet[stratRequestSection, 1, stratRequestSection, ReportWidth / 10],
                new List<LabelEditBox>()
                {
                    new LabelEditBox()
                    {
                         Header="סוג ערבות:",LabelSize=10,length=15,PropName="GuranteeType" ,
                        TheValue =GetGuaranteeName(requestParams.GuranteeType)
                    },
                    new LabelEditBox()
                    {
                         Header="מספר תיק תפג:",LabelSize=10,length=15,PropName="FileNumber"
                         ,GridColumnType= GridColumnTypeEnum.Text

                    },
                     new LabelEditBox()
                    {
                         Header="מספר רץ:",LabelSize=10,length=15,PropName="Numeral"
                         ,GridColumnType= GridColumnTypeEnum.Text

                    }
                }
                );

#if false


            var guranteeTypeLabel = _MainWorksheet[stratRequestSection, 1, stratRequestSection, 9];
            guranteeTypeLabel.Merge();///"סוג ערבות:";//100
            _IWorkbook.Names.Add("guranteeTypeLabel", guranteeTypeLabel);
            guranteeTypeLabel.Text = "סוג ערבות:";
            SetAsLabelStyle(guranteeTypeLabel);



            var guranteeTypeEdit = _MainWorksheet[stratRequestSection, 10, stratRequestSection, 24];
            guranteeTypeEdit.Merge();
            _IWorkbook.Names.Add("guranteeTypeEdit", guranteeTypeEdit);
            guranteeTypeEdit.Value = GetGuaranteeName(requestParams.GuranteeType);//
            SetAsEditBoxStyle(guranteeTypeEdit);

            //_MainWorksheet[1, 26].Text = _MainWorksheet[1, 16].Text;//blank





            var FileNumberLabel = _MainWorksheet[stratRequestSection, 25, stratRequestSection, 34];
            FileNumberLabel.Merge();///"סוג ערבות:";//100
            _IWorkbook.Names.Add("FileNumberLabel", FileNumberLabel);
            FileNumberLabel.Text = "מספר תיק תפג:";
            SetAsLabelStyle(FileNumberLabel);



            var FileNumberEdit = _MainWorksheet[stratRequestSection, 35, stratRequestSection, 49];
            FileNumberEdit.Merge();
            _IWorkbook.Names.Add("FileNumberEdit", FileNumberEdit);
            FileNumberEdit.Value = requestParams.FileNumber;//
            SetAsEditBoxStyle(FileNumberEdit);


            var NumeralLabel = _MainWorksheet[stratRequestSection, 50, stratRequestSection, 59];
            NumeralLabel.Merge();///"סוג ערבות:";//100
            _IWorkbook.Names.Add("NumeralLabel", NumeralLabel);
            NumeralLabel.Text = "מספר רץ:";
            SetAsLabelStyle(NumeralLabel);



            var NumeralEdit = _MainWorksheet[stratRequestSection, 60, stratRequestSection, 75];
            NumeralEdit.Merge();
            _IWorkbook.Names.Add("NumeralEdit", NumeralEdit);
            NumeralEdit.Value = requestParams.Numeral;//
            SetAsEditBoxStyle(NumeralEdit);
            //_MainWorksheet[1, 26].Text = _MainWorksheet[1, 16].Text;//blank


#endif
            var dest = _MainWorksheet[stratRequestSection, 1 + 3, stratRequestSection, 75 + 3];
            var source =
                //_MainWorksheet[stratRequestSection, 1, stratRequestSection, 75];
                myline;
            source.MoveTo(dest);



            var requestSection =
                _MainWorksheet[stratRequestSection - 1, 2, stratRequestSection + 1, this.ReportWidth / 10];

            _IWorkbook.Names.Add("requestSection", requestSection);
            MakeBorderSection(requestSection);



            return requestSection;
        }



        private string GetGuaranteeName(string guranteeType)
        {
            switch (guranteeType)
            {
                case "4":
                    return "תיק ערבות";// base.TranslateText("Customs.GuaranteeFileFilterQuery.F.GuranteeType.File");
                case "6":
                    return "בקשה לערבות"; ;//base.TranslateText("Customs.GuaranteeFileFilterQuery.F.GuranteeType.Req");
                default:
                    return guranteeType;
                    break;
            }
            //myGuranteeType.Code = "4"; // "Gurantee Type"
            //myGuranteeType.Name = TextCodeTranslator.Translate("Customs.GuaranteeFileFilterQuery.F.GuranteeType.File"); ;
            //this.GuranteeTypeFilterList.push(myGuranteeType);

            //var myGuranteeRequest: CodeNameClass = new CodeNameClass();
            //myGuranteeRequest.Code = "6"; // "Gurantee Request"
            //myGuranteeRequest.Name = TextCodeTranslator.Translate("Customs.GuaranteeFileFilterQuery.F.GuranteeType.Req"); ;
            //this.GuranteeTypeFilterList.push(myGuranteeRequest);
        }
    }
}