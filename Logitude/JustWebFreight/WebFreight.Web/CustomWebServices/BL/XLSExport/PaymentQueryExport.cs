using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public class PaymentQueryExport : XLSExportBase<TSH_NG_8285_Web01_PaymentRequestParams, TSH_NG_8285_Web01_PaymentResponseData>, IExcelExport
    {
        public string MainInterfaceCode => "8285";
        public override int ReportWidth => 900;

        protected override IRange AdjustAndFormatResponseRange(TSH_NG_8285_Web01_PaymentResponseData responseData, IWorksheet worksheet)
        {
            throw new NotImplementedException();
        }

        protected override IRange AdjustAndFormatRequestRange(TSH_NG_8285_Web01_PaymentRequestParams requestParams, IWorksheet requestWorksheet)
        {
            requestWorksheet.IsGridLinesVisible = false;
            var myRange = requestWorksheet[1, 1, 7, 5];

            myRange.CellStyle.Color = Color.FromArgb(198, 215, 239);
            //myRange.AutofitRows();
            //myRange.AutofitColumns();

            requestWorksheet.SetRowHeight(2, 10/*px*/);

            return myRange;
        }

        protected override IRange AdjustRequest(TSH_NG_8285_Web01_PaymentRequestParams requestParams, TSH_NG_8285_Web01_PaymentResponseData responseData)
        {
            IRange mainHeaderRange = SetSubHeader(_MainWorksheet[1, 1], "                                                    שאילתא להוראות תשלום");
            mainHeaderRange.HorizontalAlignment = ExcelHAlign.HAlignRight;
            mainHeaderRange.CellStyle.Font.Bold = true;

            var requestSection = GetRequestSection(requestParams, mainHeaderRange);

            var resultHeader = SetSubHeader(requestSection, "תוצאות שאילתא להוראות תשלום");
            var responseSection = GetResponseSection(requestParams, responseData, resultHeader);

            //_MainWorksheet[1, 10, 1, 25].Merge();//requestParams.InterfaceTypeCode//150
            return _MainWorksheet.Range;
        }

        private IRange GetResponseSection(TSH_NG_8285_Web01_PaymentRequestParams requestParams, TSH_NG_8285_Web01_PaymentResponseData responseData, IRange resultHeader)
        {
            IRange firstLine = resultHeader;
            var paymentsDetailsResult = new PaymentsDetailsResult();
            IRange GuaranteeLettersList =
                //BuildGuaranteeLettersList(responseData , GuaranteeLettersListHeader);
                BuildGenList("PaymentList",
                responseData.PaymentsDetailsList,
                  new List<XLSExport.GridColumnMetaData>()
                  {
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="מספר הוראת תשלום",
                            length=105,
                            PropName = nameof(paymentsDetailsResult.PaymentID),
                            GridColumnType  = GridColumnTypeEnum.Object
                      },
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="סוג הוראה",
                            length=80,
                            PropName =nameof(paymentsDetailsResult.PaymentType),
                            GridColumnType  = GridColumnTypeEnum.Object
                      },
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="סכום הוראה",
                            length=80,
                            PropName =nameof(paymentsDetailsResult.PaymentAmount),
                            GridColumnType  = GridColumnTypeEnum.Object
                      },
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="יבואן",
                            length=280,
                            PropName =nameof(paymentsDetailsResult.Importer),
                            GridColumnType  = GridColumnTypeEnum.Object
                      },
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="מספר ישות",
                            length=160,
                            PropName =nameof(paymentsDetailsResult.EntityExternalID),
                            GridColumnType  = GridColumnTypeEnum.Text
                      },
                      new XLSExport.GridColumnMetaData()
                      {
                            Header="אמצעי תשלום",
                            length=80,
                            PropName =nameof(paymentsDetailsResult.PaymentMethodType),
                            GridColumnType  = GridColumnTypeEnum.Object
                      },
                     new XLSExport.GridColumnMetaData()
                      {
                            Header="סטטוס הוראה",
                            length=80,
                            PropName =nameof(paymentsDetailsResult.PaymentStatus),
                            GridColumnType  = GridColumnTypeEnum.Object
                      },
                  },
                resultHeader);

            var last = GuaranteeLettersList;
            var responseSection = _MainWorksheet[firstLine.Row - 1, firstLine.Column - 1, last.LastRow + 1, this.ReportWidth / 10];

            var destRange = _MainWorksheet[firstLine.Row - 1, firstLine.Column, last.LastRow + 1, this.ReportWidth / 10];
            responseSection.MoveTo(destRange);

            MakeBorderSection(destRange);
            return null;
        }


        private IRange GetRequestSection(TSH_NG_8285_Web01_PaymentRequestParams requestParams, IRange mainHeaderRange)
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
                         Header="תאריך תשלום מ:",LabelSize=12,length=13,PropName="paymentDateFrom" , GridColumnType = GridColumnTypeEnum.Object
                    },
                    new LabelEditBox()
                    {
                         Header="עד:",LabelSize=12,length=13,PropName="paymentDateTo" , GridColumnType = GridColumnTypeEnum.Object
                    },
                });

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