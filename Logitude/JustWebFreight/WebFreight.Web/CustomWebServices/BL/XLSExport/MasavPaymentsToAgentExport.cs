
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
    public class MasavPaymentsToAgentExport
       : XLSExportBase<MasavPaymentsToAgentRequestParams,
        MasavPaymentsToAgentResponseData>, IExcelExport
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
                return "8368";
            }
        }

        protected override IRange AdjustAndFormatRequestRange(MasavPaymentsToAgentRequestParams requestParams, IWorksheet requestWorksheet)
        {

            throw new NotImplementedException();
            //877*71  === 880*70 
            //row ==32 
            //100|150|10|100|150|10|100|150|10|100|150|10 == 851



        }

        protected override IRange AdjustAndFormatResponseRange(MasavPaymentsToAgentResponseData responseData, IWorksheet worksheet)
        {
            throw new NotImplementedException();
        }

        protected override IRange AdjustRequest(MasavPaymentsToAgentRequestParams requestParams, MasavPaymentsToAgentResponseData responseData)
        {

            //877*71  === 880*70 
            //ReportWidth = 880;
            //row ==32 


            //_MainWorksheet.Range.RowHeight = GetExcelSizeFromPixel(32);

            IRange mainHeaderRange = SetSubHeader(_MainWorksheet[1, 1], "                                                    שאילתא לבקשת דו,,ח קופה");
            mainHeaderRange.HorizontalAlignment = ExcelHAlign.HAlignRight;
            mainHeaderRange.CellStyle.Font.Bold = true;

            var requestSection = GetRequestSection(requestParams, mainHeaderRange);

            var resultHeader = SetSubHeader(requestSection,"תוצאות שאילתא לבקשת דוח קופה");
            var responseSection = GetResponseSection(requestParams, responseData, resultHeader);




            //_MainWorksheet[1, 10, 1, 25].Merge();//requestParams.InterfaceTypeCode//150
            return _MainWorksheet.Range;
        }

        private IRange GetResponseSection(MasavPaymentsToAgentRequestParams requestParams, MasavPaymentsToAgentResponseData responseData, IRange resultHeader)
        {

            IRange firstLine =
         //GetResponse1Line(requestParams, responseData, resultHeader);
         // CreateLabelEditboxLine(responseData, resultHeader, Get1lineMetaData());
         resultHeader;

            IRange GuaranteeLettersList =
                //BuildGuaranteeLettersList(responseData , GuaranteeLettersListHeader);
                BuildGenList("PaymentList",
                responseData.AgentMasavPaymentResultList, 
                  new List<XLSExport.GridColumnMetaData>()
                  {
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="תהליך יוצר",
                             length=105,
                              PropName ="PaymentProcessName",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="סוג הוראה",
                             length=80,
                              PropName ="PaymentTypeName",
                               GridColumnType  = GridColumnTypeEnum.Object
                      },

                  }, 
                resultHeader);

            

            
            var last = GuaranteeLettersList;
            var responseSection = _MainWorksheet[firstLine.Row - 1, firstLine.Column, last.LastRow + 1, this.ReportWidth / 10];

            var destRange = _MainWorksheet[firstLine.Row - 1, firstLine.Column + 1, last.LastRow + 1, this.ReportWidth / 10];
            responseSection.MoveTo(destRange);


            MakeBorderSection(destRange);
            return null;
        }




        private IRange GetRequestSection(MasavPaymentsToAgentRequestParams requestParams, IRange mainHeaderRange)
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
                         Header="תאריך תשלום:",LabelSize=12,length=13,PropName="PaymentDate" , GridColumnType = GridColumnTypeEnum.Object
                        
                    },
                   
                }
                );

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



     
    }
}