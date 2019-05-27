
using Logitude.Customs.BL.EntityQueryServices;
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
    public class BlockListInWarehouseDetailExport : XLSExportBase<BlockListInWarehouseRequestParams, BlockListInWarehouseResponseData>, IExcelExport
    {


        public string MainInterfaceCode
        {
            get
            {
                return "8330";
            }
        }

        public override int ReportWidth
        {
            get
            {
                return 900;
            }
        }

        protected override IRange AdjustAndFormatResponseRange(BlockListInWarehouseResponseData responseData, IWorksheet worksheet)
        {
            throw new NotImplementedException();
        }
        protected override IRange AdjustAndFormatRequestRange(BlockListInWarehouseRequestParams requestParams, IWorksheet requestWorksheet)
        {
            requestWorksheet.IsGridLinesVisible = false;
            //throw new NotImplementedException();
            var myRange = requestWorksheet[1, 1, 7, 5];


            myRange.CellStyle.Color = Color.FromArgb(198, 215, 239);
            //myRange.AutofitRows();
            //myRange.AutofitColumns();


            requestWorksheet.SetRowHeight(2, 10/*px*/);

            return myRange;
        }



      

        protected override IRange AdjustRequest(BlockListInWarehouseRequestParams requestParams, BlockListInWarehouseResponseData responseData)
        {

            //ReportWidth = 650;
            //row ==32 


            //_MainWorksheet.Range.RowHeight = GetExcelSizeFromPixel(32);

            IRange mainHeaderRange = SetSubHeader(_MainWorksheet[1, 1],
                TranslateText("Customs.General.O.BlockListInWarehouseQuery")
                );
            mainHeaderRange.HorizontalAlignment = ExcelHAlign.HAlignRight;
            mainHeaderRange.CellStyle.Font.Bold = true;

            replaceRequestParamsStorageId2Name(requestParams, responseData);

            var requestSection = GetRequestSection(requestParams, mainHeaderRange);

            //var resposeHeaderRange = _MainWorksheet[requestSection.Row, 1];//, requestSection.Row, this.ReportWidth / 10];
            var resultHeader = SetSubHeader(requestSection, "תוצאות שאילתא גושים במחסן");
            var responseSection = GetResponseSection(requestParams, responseData, resultHeader);




            //_MainWorksheet[1, 10, 1, 25].Merge();//requestParams.InterfaceTypeCode//150
            return _MainWorksheet.Range;
        }

        private static void replaceRequestParamsStorageId2Name(BlockListInWarehouseRequestParams requestParams, BlockListInWarehouseResponseData responseData)
        {
            if (!string.IsNullOrWhiteSpace(requestParams.StorageSiteNumber))
            {
                var qs = new SiteLookupQueryService(requestParams.Tenant);
                var pm=qs.GetSingle(requestParams.StorageSiteNumber, false, false);
                requestParams.StorageSiteNumber = pm.LocalName;
                
            }
        }

        private IRange GetResponseSection(BlockListInWarehouseRequestParams requestParams, BlockListInWarehouseResponseData responseData, IRange resultHeader)
        {
            IRange firstLine =
                _MainWorksheet[resultHeader.Row + 1, 1, resultHeader.Row + 1, this.ReportWidth / 10];
            ;


            IRange GuaranteeLettersList =
                BuildGenList("BlockListInWarehouseResultList",
                responseData.BlockListInWarehouseResultList, GetBlockListInWarehouseColumnList(), firstLine);

            int identColumn = 2;
            var last = GuaranteeLettersList;
            var responseSection = _MainWorksheet[firstLine.Row, last.Column, last.LastRow + 1, this.ReportWidth / 10];

            var destRange = _MainWorksheet[firstLine.Row, last.Column + identColumn, last.LastRow + 1, this.ReportWidth / 10];
            responseSection.MoveTo(destRange);


            MakeBorderSection(destRange);
            return null;

        }



        private List<GridColumnMetaData> GetBlockListInWarehouseColumnList()
        {
            var l = new List<GridColumnMetaData>() {

            new GridColumnMetaData()
            {
                PropName = "DeclarationNumber",
                length = 100,
                Header = TranslateText("Customs.BlockListInWarehouse.O.DeclarationNumber") ,
                  ExcelHAlign = ExcelHAlignEnum.HAlignRight,
                   GridColumnType= GridColumnTypeEnum.Text

            }
            ,
            new GridColumnMetaData()
            {
                PropName = "WarehouseBlockNumber", length = 100,
                Header = TranslateText("Customs.BlockListInWarehouse.O.WarehouseBlockNumber") ,
            }
            ,
                new GridColumnMetaData()
            {
                PropName = "ImporterTitle",
                length = 110,
                Header = TranslateText("Customs.BlockListInWarehouse.O.ImporterTitle") ,
            }

            ,
                new GridColumnMetaData()
            {
                PropName = "OpeningDate",
                length = 90,
                Header = TranslateText("Customs.BlockListInWarehouse.O.OpeningDate") ,
            }
                ,
                new GridColumnMetaData()
            {
                PropName = "OriginalOpeningDate",
                length = 90,
                Header = TranslateText("Customs.BlockListInWarehouse.O.OriginalOpeningDate") ,
            }

                ,new GridColumnMetaData()
            {
                PropName = "PhysicalPackagesQuantityBalance",
                length = 90,
                Header = TranslateText("Customs.BlockListInWarehouse.O.PhysicalPackagesQuantityBalance") ,GridColumnType= GridColumnTypeEnum.Number
                }
                ,new GridColumnMetaData()
            {
                PropName = "LogicalPackagesQuantityBalance",
                length = 90,
                Header = TranslateText("Customs.BlockListInWarehouse.O.LogicalPackagesQuantityBalance") 
                ,GridColumnType= GridColumnTypeEnum.Number
                }
                 ,new GridColumnMetaData()
            {
                PropName = "Value",
                length = 90,
                Header = TranslateText("Customs.BlockListInWarehouse.O.Value")
                ,GridColumnType= GridColumnTypeEnum.Number
                }

                  ,new GridColumnMetaData()
            {
                PropName = "SpecialActivityTypeName",
                length = 90,
                Header = TranslateText("Customs.BlockListInWarehouse.O.SpecialActivityTypeName") ,
                }
            };
            return l;
        }

        private IRange GetRequestSection(BlockListInWarehouseRequestParams requestParams, IRange mainHeaderRange)
        {




            //100|150|10|100|150|10|100|150|10|100|150|10 == 851
            var myline1 = base.CreateLabelEditboxLine(
                requestParams,
                _MainWorksheet[mainHeaderRange.LastRow + 1, 1, mainHeaderRange.LastRow + 1, ReportWidth / 10],
                new List<LabelEditBox>()
                {
                    new LabelEditBox()
                    {
                         Header="מתאריך פתיחת גוש:",LabelSize=10,length=11,PropName="FromDate" ,
                        //TheValue =GetGuaranteeName(requestParams.GuranteeType)
                    },
                                        new LabelEditBox()
                    {
                         Header="עד תאריך פתיחת גוש:",LabelSize=10,length=11,PropName="ToDate"
                         ,GridColumnType= GridColumnTypeEnum.Object

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
                         Header="אתר אחסון:",LabelSize=10,length=11,PropName="StorageSiteNumber"
                         ,GridColumnType= GridColumnTypeEnum.Text

                    },

                    new LabelEditBox()
                    {
                         Header="כולל גושים מאופסים:",LabelSize=10,length=11,PropName="ShowResetBlocks"
                         ,GridColumnType= GridColumnTypeEnum.Text

                    },

                }
                );


            int identStartFromC = 3;//
            Ident3Columns(myline1.LastRow /*+ 2*/, myline2.Row, identStartFromC);

            IRange requestSection = SetRequestSection1LineB4And1LineAfter(myline1.LastRow, myline2.Row);
            MakeBorderSection(requestSection);



            return requestSection;
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
    }
}