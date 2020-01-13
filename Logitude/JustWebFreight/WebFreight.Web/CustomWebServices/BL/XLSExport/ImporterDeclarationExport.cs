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
    public class ImporterDeclarationExport
        : XLSExportBase<ImporterDeclarationRequestParams,
        ImporterDeclarationResponseData>, IExcelExport
    {
        public override int ReportWidth
        {
            get
            {
                return 830;//877*71  === 880*70 
            }
        }

        public string MainInterfaceCode
        {
            get
            {
                return "8326";
            }
        }

        protected override IRange AdjustAndFormatRequestRange(ImporterDeclarationRequestParams requestParams, IWorksheet requestWorksheet)
        {

            throw new NotImplementedException();
            //877*71  === 880*70 
            //row ==32 
            //100|150|10|100|150|10|100|150|10|100|150|10 == 851



        }

        

        protected override IRange AdjustRequest(ImporterDeclarationRequestParams requestParams, ImporterDeclarationResponseData responseData)
        {

            //877*71  === 880*70 
            //ReportWidth = 880;
            //row ==32 


            //_MainWorksheet.Range.RowHeight = GetExcelSizeFromPixel(32);

            IRange mainHeaderRange = SetSubHeader(_MainWorksheet[1, 1], "  שאילתא לתצהיר יבואן");
            mainHeaderRange.HorizontalAlignment = ExcelHAlign.HAlignRight;
            mainHeaderRange.CellStyle.Font.Bold = true;

            var requestSection = GetRequestSection(requestParams, mainHeaderRange);

            var resultHeader = SetSubHeader(requestSection, "תוצאות שאילתא לתצהיר יבואן",1);
            
            var responseSection = GetResponseSection(requestParams, responseData, resultHeader);




            //_MainWorksheet[1, 10, 1, 25].Merge();//requestParams.InterfaceTypeCode//150
            return _MainWorksheet.Range;
        }

        private IRange GetResponseSection(ImporterDeclarationRequestParams requestParams, ImporterDeclarationResponseData responseData, IRange resultHeader)
        {

            //רשימת תצהירים תקופתיים

            
            IRange firstLine =
         //GetResponse1Line(requestParams, responseData, resultHeader);
         // CreateLabelEditboxLine(responseData, resultHeader, Get1lineMetaData());
         resultHeader;

            var resultTitle1 = SetSubHeader(resultHeader, "רשימת תצהירים תקופתיים",1);

            IRange myPeriodDeclarationList =
                //BuildGuaranteeLettersList(responseData , GuaranteeLettersListHeader);
                BuildGenList("PeriodDeclarationList",
                responseData.PeriodDeclarationList,
                  new List<XLSExport.GridColumnMetaData>()
                  {
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="מספר תצהיר",
                             length=90,
                              PropName ="PeriodDeclarationID",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="מספר ספק",
                             length=90,
                              PropName ="VendorID",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },


                      new XLSExport.GridColumnMetaData()
                      {
                            Header=" ספק",
                             length=130,
                              PropName ="VendorName",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },
                      new XLSExport.GridColumnMetaData()
                      {
                            Header=" תאריך יצירה",
                             length=90,
                              PropName ="CreateDate",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },


                      new XLSExport.GridColumnMetaData()
                      {
                            Header=" תקף מתאריך",
                             length=90,
                              PropName ="ValidityFrom",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },

                       new XLSExport.GridColumnMetaData()
                      {
                            Header=" תאריך תוקף",
                             length=90,
                              PropName ="ExpirationDate",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },

                       new XLSExport.GridColumnMetaData()
                      {
                            Header="סטטוס",
                             length=90,
                              PropName ="StatusName",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },


                      new XLSExport.GridColumnMetaData()
                      {
                            Header="מזהה מסמך",
                             length=90,
                              PropName ="DocumentID",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },

                  },
                resultTitle1);



            var resultTitle2 = SetSubHeader(myPeriodDeclarationList, "רשימת תצהירים להצהרת יבוא",1);

            IRange myLoiDeclarationList =
                //BuildGuaranteeLettersList(responseData , GuaranteeLettersListHeader);
                BuildGenList("LoiDeclarationList",
                responseData.LoiDeclarationList,
                  new List<XLSExport.GridColumnMetaData>()
                  {
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="מספר הצהרת יבוא",
                             length=110,
                              PropName ="DeclarationID",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="מספר תצהיר",
                             length=90,
                              PropName ="LoiDeclarationID",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },


                      new XLSExport.GridColumnMetaData()
                      {
                            Header=" ספק",
                             length=100,
                              PropName ="VendorName",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },
                      new XLSExport.GridColumnMetaData()
                      {
                            Header=" תאריך יצירה",
                             length=100,
                              PropName ="CreateDate",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },



                      new XLSExport.GridColumnMetaData()
                      {
                            Header="מזהה מסמך",
                             length=90,
                              PropName ="DocumentID",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },

                  },
                resultTitle2);



            //רשימת תצהירים בטחוניים
            var resultTitle3 = SetSubHeader(myLoiDeclarationList, "רשימת תצהירים בטחוניים",1);

            IRange mySecurityDeclarationList =
                //BuildGuaranteeLettersList(responseData , GuaranteeLettersListHeader);
                BuildGenList("SecurityDeclarationList",
                responseData.LoiDeclarationList,
                  new List<XLSExport.GridColumnMetaData>()
                  {
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="סוג תצהיר",
                             length=100,
                              PropName ="SecurityDeclarationName",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="מספר תצהיר",
                             length=90,
                              PropName ="SecurityImporterDeclarationId",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },


                      new XLSExport.GridColumnMetaData()
                      {
                            Header=" תאריך תצהיר",
                             length=100,
                              PropName ="DeclarationDate",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },
                      new XLSExport.GridColumnMetaData()
                      {
                            Header=" תאריך תוקף",
                             length=100,
                              PropName ="ExpirationDate",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },

                      new XLSExport.GridColumnMetaData()
                      {
                            Header="סטטוס",
                             length=100,
                              PropName ="StatusName",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },


                      new XLSExport.GridColumnMetaData()
                      {
                            Header="מזהה מסמך",
                             length=90,
                              PropName ="DocumentID",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },

                  },
                resultTitle3);


            int identColumn = 2;
            var last = mySecurityDeclarationList;
            var responseSection = _MainWorksheet[firstLine.Row, last.Column, last.LastRow + 1, this.ReportWidth / 10];

            var destRange = _MainWorksheet[firstLine.Row, last.Column + identColumn, last.LastRow + 1, this.ReportWidth / 10];
            responseSection.MoveTo(destRange);


            MakeBorderSection(destRange);
            return null;
        }


        string Trans(string DeclarationConectCode)
        {
            switch (DeclarationConectCode)
            {
                case "3":
                    {
                        return "הצהרה";
                    }
                    break;

                case "2":
                    {
                        return "ספק";
                    }
                    break;

                case "1":
                    {
                        return "הצהרת יבוא";
                    }
                    break;
                case "0":
                default:
                    {
                        return "הכל";
                    }
                    break;
            }
        }

        private IRange GetRequestSection(ImporterDeclarationRequestParams requestParams, IRange mainHeaderRange)
        {
            var stratRequestSection = mainHeaderRange.LastRow + 1;




            //100|150|10|100|150|10|100|150|10|100|150|10 == 851
            var myline1 = base.CreateLabelEditboxLine(
                requestParams,
                _MainWorksheet[stratRequestSection, 1, stratRequestSection, ReportWidth / 10],
                new List<LabelEditBox>()
                {
                    //
                    new LabelEditBox()
                    {
                         Header="מספר יבואן:",LabelSize=12,length=13,PropName="ImporterNumber" , GridColumnType = GridColumnTypeEnum.Object

                    },
                });
            string textByHeader = "תצהירי יבואן לפי תוקף";
            if (requestParams.IsByType)
            {
                textByHeader = "תצהירים לפי סוג";
            }
            var myline2 = this.SetSubHeaderTextByheader(textByHeader, myline1);
            //SetAsLabelStyle(myline2);
            //var myline2 = base.CreateLabelEditboxLine(
            //    requestParams,
            //    myline1,
            //    new List<LabelEditBox>()
            //    {

            //        new LabelEditBox()
            //        {
            //             Header="תצהירי יבואן לפי תוקף",LabelSize=12,length=13,PropName="IsByExpireDate" , GridColumnType = GridColumnTypeEnum.TrueFalse
            //        },
            //        new LabelEditBox()
            //        {
            //             Header="תצהירים לפי סוג",LabelSize=12,length=13,PropName="IsByType" , GridColumnType = GridColumnTypeEnum.TrueFalse
            //        },
            //    });
            IRange mylineLast=null;
            if (requestParams.IsByExpireDate)
            {
                IRange myline3 = base.CreateLabelEditboxLine(
               requestParams,
               myline2,
               new List<LabelEditBox>()
               {
                    new LabelEditBox()
                    {
                         Header="תצהירים בתוקף עד:",LabelSize=12,length=13,PropName="DeclarationExpire" , GridColumnType = GridColumnTypeEnum.TrueFalse
                    },
               });
                mylineLast = myline3;
            }
            else//<!--ByTypeRadio-->
            {
                IRange  myline3 = base.CreateLabelEditboxLine(
               requestParams,
               myline2,
               new List<LabelEditBox>()
               {
                    new LabelEditBox()
                    {
                         Header="תצהירים שקשורים ל:",LabelSize=12,length=13,PropName="DeclarationConect" , GridColumnType = GridColumnTypeEnum.Text,TheValue=
                         Trans(requestParams.DeclarationConect)
                    },
                    new LabelEditBox()
                    {
                         Header="מספר:",LabelSize=12,length=13,PropName="Code" , GridColumnType = GridColumnTypeEnum.Text
                         
                    },
               });
                IRange myline4 = base.CreateLabelEditboxLine(
               requestParams,
               myline3,
               new List<LabelEditBox>()
               {
                    new LabelEditBox()
                    {
                         Header="מתאריך:",LabelSize=12,length=13,PropName="FromDate" , GridColumnType = GridColumnTypeEnum.Object,
                    },
                    new LabelEditBox()
                    {
                         Header="עד תאריך:",LabelSize=12,length=13,PropName="ToDate" , GridColumnType = GridColumnTypeEnum.Object,

                    },
               });

                mylineLast = myline4;
            }

            int identStartFromC = 4;//
            Ident3Columns(myline1.LastRow /*+ 2*/, mylineLast.Row, identStartFromC);

            IRange requestSection = SetRequestSection1LineB4And1LineAfter(myline1.LastRow, mylineLast.Row);
            MakeBorderSection(requestSection);




            return requestSection;
        }

        private IRange SetSubHeaderTextByheader(string textByHeader, IRange lastRange)
        {
            var currHeader = _MainWorksheet[lastRange.LastRow + 2, 1, lastRange.LastRow + 2, ReportWidth / 10 - 10];
            currHeader.Merge();
            currHeader[currHeader.Row, currHeader.Column].Text = textByHeader;// "תנועות אשראי";
            currHeader[currHeader.Row, currHeader.Column].VerticalAlignment = ExcelVAlign.VAlignCenter;
            currHeader[currHeader.Row, currHeader.Column].HorizontalAlignment = ExcelHAlign.HAlignRight;

            currHeader[currHeader.Row, currHeader.Column].CellStyle.Font.RGBColor = //this.ResultHeaderColor;
                this.SectionBorder;
            currHeader[currHeader.Row, currHeader.Column].IndentLevel = 0;
            return currHeader;
        }

        private IRange SetRequestSection1LineB4And1LineAfter(int saveStartLine, int lastLine)
        {
            var requestSection =
                            _MainWorksheet[saveStartLine - 1, 3, lastLine + 1, this.ReportWidth / 10];

            _IWorkbook.Names.Add("requestSection", requestSection);
            return requestSection;
        }
        private void Ident3Columns(int sectionStartLine, int currentLine, int identStartFromC)
        {
            var dest = _MainWorksheet[sectionStartLine, 1 + identStartFromC, currentLine, this.ReportWidth / 10 + identStartFromC];
            var source =
                _MainWorksheet[sectionStartLine, 1, currentLine, this.ReportWidth / 10];
            ;
            source.MoveTo(dest);
        }
        protected override IRange AdjustAndFormatResponseRange(ImporterDeclarationResponseData responseData, IWorksheet worksheet)
        {
            throw new NotImplementedException();
        }
    }
}