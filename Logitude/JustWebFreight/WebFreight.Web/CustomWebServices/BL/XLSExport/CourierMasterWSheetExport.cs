using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure;
using WebFreight.Web.Helpers;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public class CourierMasterWSheetExport
    {
        public byte[] ExportReport(string courierMasterId, int tenant)
        {
            ICustomContext MyContext = CustomContext.GetContext(tenant);
            DeclarationCourierStatusListQueryService declarationCourierStatusQuery = new DeclarationCourierStatusListQueryService(MyContext);
            declarationCourierStatusQuery.RequiredFieldErrorsForCourierDeclarationIsValid = true;
            var q = declarationCourierStatusQuery.GetByCourierMasterId(courierMasterId, tenant)
                .Select(r => new
                {
                    r.AirlineId,
                    r.MAWB,
                    r.MasterHAWB,

                    MasterGrossMassMeasure=r.MasterGrossMassMeasure??0,
                    MasterPackageQuantity =r.MasterPackageQuantity ?? 0,
                    r.MasterCreateDateTime,
                    r.MasterGatewayPortCode,
                    r.MasterEstimatedArrivalDate,
                    r.MasterStorageSiteCode,
                    r.CourierHawb,
                    r.ProcedureCurrentName,
                    r.FastIndividualProcessCode,
                    r.ImporterName,
                    r.ImporterCode,
                    TotalInvoiceAmountInUSD=r.TotalInvoiceAmountInUSD ?? 0,
                    r.DocumentStatusCode,
                    r.IsCourierMissingClassification,
                    r.CourierManifestStatusCode,
                    r.CourierDeclarationStatusCode,
                    r.CourierPaymentStatusCode,
                    r.CourierCustomStatusName,
                    r.StorageSiteStatusCode,
                    r.CourierSuspentionName,
                    r.SpecialActionStatus,
                    r.DeclarationStatusTypeName,
                    r.CourierPendingReasonName,
                    r.LastMileStatusCode,
                    r.IsClosedForFollowUp,
                    r.CourierPendingReasonList,
                    
                    

                });
            ;
            /*
                 <div class="TextTrimming" *ngIf="fieldName == 'HighLowValue'" style="text-align:right;">
        <span *ngIf="_CourierWorksheet['FastIndividualProcessCode'] == 'F'">{{'Customs.CourierMaster.HighLowValue.High'  | TextCodeTranslationPipe }}</span>
        <span *ngIf="_CourierWorksheet['FastIndividualProcessCode'] == 'I'">{{'Customs.CourierMaster.HighLowValue.Low'  | TextCodeTranslationPipe }}</span>
    </div>

             */

            DataTable dt = null;
            var settingCol = new BITabularViewSettings()
            {
                Columns = new List<Column>()
            };
            if (false)
            {
                dt = q.AsEnumerable().ToDataTable();
            }
            else
            {
                dt = new DataTable("Courier Master");


                settingCol.Columns.Add(new Column() {Index = 1,Code = "CourierMasterId",Name = "CourierMasterId",DataTypeCode = "String",Width = 150,});
                dt.Columns.Add(new DataColumn(){Caption = /*"Courier Master"*/"ש.מ.ר", ColumnName = "CourierMasterId",DataType = System.Type.GetType("System.String"),});


                dt.Columns.Add(new DataColumn() { Caption = /*"Master HAWB"*/"שטר מטען פנימ", ColumnName = "MasterHAWB", DataType = System.Type.GetType("System.String") });
                settingCol.Columns.Add(new Column() { Index = 2, Code = "MasterHAWB", Name = "MasterHAWB", DataTypeCode = "String", Width = 100, });



                settingCol.Columns.Add(new Column(){Index = 3,Code = "MasterGrossMassMeasure",Name = "MasterGrossMassMeasure",DataTypeCode = "Decimal",Width = 100,});
                dt.Columns.Add(new DataColumn(){Caption = TextCodesTranslator.TranslateText("Customs.CourierMaster.F.GrossMassMeasure", tenant,true),ColumnName = "MasterGrossMassMeasure",DataType = System.Type.GetType("System.Decimal")});


                settingCol.Columns.Add(new Column(){Index = 4,Code = "MasterPackageQuantity",Name = "MasterPackageQuantity",DataTypeCode = "Decimal",Width = 100,});
                dt.Columns.Add(new DataColumn(){Caption = TextCodesTranslator.TranslateText("Customs.CourierMaster.F.PackageQuantity", tenant, true),ColumnName = "MasterPackageQuantity",DataType = System.Type.GetType("System.Decimal")});

                settingCol.Columns.Add(new Column(){Index = 5,Code = "MasterCreateDateTime",Name = "MasterCreateDateTime",DataTypeCode = "DateTime",Width = 100,});
                dt.Columns.Add(new DataColumn(){Caption = TextCodesTranslator.TranslateText("Customs.CourierMaster.F.CreateDateTime", tenant, true),ColumnName = "MasterCreateDateTime",DataType = DateTime.Now.GetType()});



                settingCol.Columns.Add(new Column(){Index = 6,Code = "MasterGatewayPortCode",Name = "MasterGatewayPortCode",DataTypeCode = "String",Width = 100,});
                dt.Columns.Add(new DataColumn(){Caption = TextCodesTranslator.TranslateText("Customs.CourierMaster.F.GatewayPortCode", tenant, true),ColumnName = "MasterGatewayPortCode",DataType = "".GetType()});


                settingCol.Columns.Add(new Column(){Index = 7,Code = "MasterEstimatedArrivalDate",Name = "MasterEstimatedArrivalDate",DataTypeCode = "DateTime",Width = 100,});
                dt.Columns.Add(new DataColumn(){Caption = TextCodesTranslator.TranslateText("Customs.CourierMaster.F.EstimatedArrivalDate", tenant, true),ColumnName = "MasterEstimatedArrivalDate",DataType = DateTime.Now.GetType()});

                settingCol.Columns.Add(new Column() { Index = 8, Code = "MasterStorageSiteCode", Name = "MasterStorageSiteCode", DataTypeCode = "String", Width = 80, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.CourierMaster.F.StorageSiteCode", tenant, true), ColumnName = "MasterStorageSiteCode", DataType = "".GetType() });


                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierHawb", tenant, true), ColumnName = "CourierHawb", DataType = System.Type.GetType("System.String") });
                settingCol.Columns.Add(new Column() { Index = 9, Code = "CourierHawb", Name = "CourierHawb", DataTypeCode = "String", Width = 100, });



                settingCol.Columns.Add(new Column() { Index = 9, Code = "ProcedureCurrentName", Name = "ProcedureCurrentName", DataTypeCode = "String", Width = 122, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.ProcedureCurrentName", tenant, true), ColumnName = "ProcedureCurrentName", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 10, Code = "HighLowValue", Name = "HighLowValue", DataTypeCode = "String", Width = 70, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.FastIndividualProcessCode", tenant, true), ColumnName = "HighLowValue", DataType = "".GetType() });






                settingCol.Columns.Add(new Column() { Index = 11, Code = "ImporterName", Name = "ImporterName", DataTypeCode = "String", Width = 200, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CustomerName", tenant, true), ColumnName = "ImporterName", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 12, Code = "ImporterCode", Name = "ImporterCode", DataTypeCode = "String", Width = 90, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.ImporterCode", tenant, true), ColumnName = "ImporterCode", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 13, Code = "TotalInvoiceAmountInUSD", Name = "TotalInvoiceAmountInUSD", DataTypeCode = "Decimal", Width = 100, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.TotalInvoiceAmountInUSD", tenant, true), ColumnName = "TotalInvoiceAmountInUSD", DataType = typeof(Decimal) });

                

                settingCol.Columns.Add(new Column() { Index = 14, Code = "DocumentStatusCode", Name = "DocumentStatusCode", DataTypeCode = "String", Width = 55, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.DocumentStatusCode", tenant, true), ColumnName = "DocumentStatusCode", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 15, Code = "IsCourierMissingClassification", Name = "IsCourierMissingClassification", DataTypeCode = "String", Width = 53, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.IsCourierMissingClassification", tenant, true), ColumnName = "IsCourierMissingClassification", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 16, Code = "CourierManifestStatusCode", Name = "CourierManifestStatusCode", DataTypeCode = "String", Width = 53, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierManifestStatusCode", tenant, true), ColumnName = "CourierManifestStatusCode", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 17, Code = "CourierDeclarationStatusCode", Name = "CourierDeclarationStatusCode", DataTypeCode = "String", Width = 53, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierDeclarationStatusCode", tenant, true), ColumnName = "CourierDeclarationStatusCode", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 18, Code = "CourierPaymentStatusCode", Name = "CourierPaymentStatusCode", DataTypeCode = "String", Width = 53, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierPaymentStatusCode", tenant, true), ColumnName = "CourierPaymentStatusCode", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 19, Code = "CourierCustomStatusName", Name = "CourierCustomStatusName", DataTypeCode = "String", Width = 90, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierCustomStatusName", tenant, true), ColumnName = "CourierCustomStatusName", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 21, Code = "StorageSiteStatusCode", Name = "StorageSiteStatusCode", DataTypeCode = "String", Width = 98, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.StorageSiteStatusCode", tenant, true), ColumnName = "StorageSiteStatusCode", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 20, Code = "CourierSuspentionName", Name = "CourierSuspentionName", DataTypeCode = "String", Width = 100, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierSuspentionName", tenant, true), ColumnName = "CourierSuspentionName", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 22, Code = "SpecialActionStatus", Name = "SpecialActionStatus", DataTypeCode = "String", Width = 98, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.SpecialActionStatus", tenant, true), ColumnName = "SpecialActionStatus", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 23, Code = "DeclarationStatusTypeName", Name = "DeclarationStatusTypeName", DataTypeCode = "String", Width = 200, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.DeclarationStatusTypeName", tenant, true), ColumnName = "DeclarationStatusTypeName", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 24, Code = "CourierPendingReasonName", Name = "CourierPendingReasonName", DataTypeCode = "String", Width = 105, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierPendingReasonName", tenant, true), ColumnName = "CourierPendingReasonName", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 25, Code = "LastMileStatusCode", Name = "LastMileStatusCode", DataTypeCode = "String", Width = 100, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.LastMileStatusCode", tenant, true), ColumnName = "LastMileStatusCode", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 26, Code = "IsClosedForFollowUp", Name = "IsClosedForFollowUp", DataTypeCode = "String", Width = 90, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.IsClosedForFollowUp", tenant, true), ColumnName = "IsClosedForFollowUp", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 27, Code = "CourierPendingReasonList", Name = "CourierPendingReasonList", DataTypeCode = "String", Width = 80, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierPendingReasonList", tenant, true), ColumnName = "CourierPendingReasonList", DataType = "".GetType() });



         

                var l = q.ToList();
                l.ForEach(r =>
                {
                    var newrow = dt.NewRow();
                    newrow[0] = $"{r.AirlineId}-{r.MAWB}";
                    //newrow[1] = r.CourierHawb;
                    newrow[1] = r.MasterHAWB;
                    newrow[2] = r.MasterGrossMassMeasure;
                    newrow[3] = r.MasterPackageQuantity;
                    newrow[4] = r.MasterCreateDateTime;
                    newrow[5] = r.MasterGatewayPortCode;
                    newrow[6] = r.MasterEstimatedArrivalDate;
                    newrow[7] = r.MasterStorageSiteCode;
                    newrow[8] = r.CourierHawb;
                    newrow[9] = r.ProcedureCurrentName;
                    newrow[10] = r.FastIndividualProcessCode;
                    newrow[11] = r.ImporterName;
                    newrow[12] = r.ImporterCode;
                    newrow[13] = r.TotalInvoiceAmountInUSD;
                    newrow[14] = r.DocumentStatusCode;
                    newrow[15] = r.IsCourierMissingClassification;
                    newrow[16] = r.CourierManifestStatusCode;
                    newrow[17] = r.CourierDeclarationStatusCode;
                    newrow[18] = r.CourierPaymentStatusCode;
                    newrow[19] = r.CourierCustomStatusName;
                    newrow[20] = r.StorageSiteStatusCode;
                    newrow[21] = r.SpecialActionStatus;
                    newrow[22] = r.CourierSuspentionName;
                    newrow[23] = r.DeclarationStatusTypeName;
                    newrow[24] = r.CourierPendingReasonName;
                    newrow[25] = r.LastMileStatusCode;
                    newrow[26] = r.IsClosedForFollowUp;
                    newrow[27] = r.CourierPendingReasonList;
                    

                    dt.Rows.Add(newrow);

                });
            }

            var xls = new ExportToExcelHelper();
            var res = xls.ExportDataTableToExcel(dt, tenant, settingCol);



            return res;


        }

        public byte[] ExportCourierSuspentionReport(string courierMasterId, int tenant)
        {
            ICustomContext MyContext = CustomContext.GetContext(tenant);
            DeclarationCourierStatusListQueryService declarationCourierStatusQuery = new DeclarationCourierStatusListQueryService(MyContext);
            declarationCourierStatusQuery.RequiredFieldErrorsForCourierDeclarationIsValid = true;
            var q = declarationCourierStatusQuery.GetByCourierMasterId(courierMasterId, tenant)
                .Where(x=>x.CourierCustomStatusCode == "2")
                .Select(r => new
                {
                    r.CourierHawb,
                    r.ImporterName,
                    r.ImporterCode,
                    TotalInvoiceAmountInUSD = r.TotalInvoiceAmountInUSD ?? 0,
                    CourierCustomStatus = "מעוכב",
                    r.CourierSuspentionReasonName,
                    r.DeclarationStatusTypeName,
                });
            DataTable dt = null;
            var settingCol = new BITabularViewSettings() { Columns = new List<Column>() };
            dt = new DataTable("Courier Master");
            
            settingCol.Columns.Add(new Column() { Index = 1, Code = "CourierHawb", Name = "CourierHawb", DataTypeCode = "String", Width = 100, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierHawb", tenant, true), ColumnName = "CourierHawb", DataType = System.Type.GetType("System.String") });

            settingCol.Columns.Add(new Column() { Index = 2, Code = "ImporterName", Name = "ImporterName", DataTypeCode = "String", Width = 200, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CustomerName", tenant, true), ColumnName = "ImporterName", DataType = "".GetType() });

            settingCol.Columns.Add(new Column() { Index = 3, Code = "ImporterCode", Name = "ImporterCode", DataTypeCode = "String", Width = 90, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.ImporterCode", tenant, true), ColumnName = "ImporterCode", DataType = "".GetType() });

            settingCol.Columns.Add(new Column() { Index = 4, Code = "TotalInvoiceAmountInUSD", Name = "TotalInvoiceAmountInUSD", DataTypeCode = "Decimal", Width = 100, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.TotalInvoiceAmountInUSD", tenant, true), ColumnName = "TotalInvoiceAmountInUSD", DataType = typeof(Decimal) });
            
            settingCol.Columns.Add(new Column() { Index = 5, Code = "CourierCustomStatusName", Name = "CourierCustomStatusName", DataTypeCode = "String", Width = 90, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierCustomStatusName", tenant, true), ColumnName = "CourierCustomStatusName", DataType = "".GetType() });

            settingCol.Columns.Add(new Column() { Index = 6, Code = "CourierSuspentionReasonName", Name = "CourierSuspentionReasonName", DataTypeCode = "String", Width = 100, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierSuspentionReasonName", tenant, true), ColumnName = "CourierSuspentionReasonName", DataType = "".GetType() });

            settingCol.Columns.Add(new Column() { Index = 7, Code = "DeclarationStatusTypeName", Name = "DeclarationStatusTypeName", DataTypeCode = "String", Width = 200, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.DeclarationStatusTypeName", tenant, true), ColumnName = "DeclarationStatusTypeName", DataType = "".GetType() });

            var l = q.ToList();
            l.ForEach(r =>
            {
                var newrow = dt.NewRow();
                newrow[0] = r.CourierHawb;
                newrow[1] = r.ImporterName;
                newrow[2] = r.ImporterCode;
                newrow[3] = r.TotalInvoiceAmountInUSD;
                newrow[4] = r.CourierCustomStatus;
                newrow[5] = r.CourierSuspentionReasonName;
                newrow[6] = r.DeclarationStatusTypeName;
                dt.Rows.Add(newrow);
            });
            var xls = new ExportToExcelHelper();
            var res = xls.ExportDataTableToExcel(dt, tenant, settingCol);
            return res;
        }

        public byte[] ExportCourierPendingReport(string courierMasterId, int tenant)
        {
            ICustomContext MyContext = CustomContext.GetContext(tenant);
            DeclarationCourierStatusListQueryService declarationCourierStatusQuery = new DeclarationCourierStatusListQueryService(MyContext);
            declarationCourierStatusQuery.RequiredFieldErrorsForCourierDeclarationIsValid = true;
            var q = declarationCourierStatusQuery.GetByCourierMasterId(courierMasterId, tenant)
                .Where(x => !string.IsNullOrWhiteSpace(x.CourierPendingReasonList))
                .Select(r => new
                {
                    r.CourierHawb,
                    r.ImporterName,
                    r.ImporterCode,
                    TotalInvoiceAmountInUSD = r.TotalInvoiceAmountInUSD ?? 0,
                    r.CourierPendingReasonNameList
                });
            DataTable dt = null;
            var settingCol = new BITabularViewSettings() { Columns = new List<Column>() };
            dt = new DataTable("Courier Master");

            settingCol.Columns.Add(new Column() { Index = 1, Code = "CourierHawb", Name = "CourierHawb", DataTypeCode = "String", Width = 100, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierHawb", tenant, true), ColumnName = "CourierHawb", DataType = System.Type.GetType("System.String") });

            settingCol.Columns.Add(new Column() { Index = 2, Code = "ImporterName", Name = "ImporterName", DataTypeCode = "String", Width = 200, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CustomerName", tenant, true), ColumnName = "ImporterName", DataType = "".GetType() });

            settingCol.Columns.Add(new Column() { Index = 3, Code = "ImporterCode", Name = "ImporterCode", DataTypeCode = "String", Width = 90, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.ImporterCode", tenant, true), ColumnName = "ImporterCode", DataType = "".GetType() });

            settingCol.Columns.Add(new Column() { Index = 4, Code = "TotalInvoiceAmountInUSD", Name = "TotalInvoiceAmountInUSD", DataTypeCode = "Decimal", Width = 100, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.TotalInvoiceAmountInUSD", tenant, true), ColumnName = "TotalInvoiceAmountInUSD", DataType = typeof(Decimal) });

            settingCol.Columns.Add(new Column() { Index = 5, Code = "CourierPendingReasonNameList", Name = "CourierPendingReasonNameList", DataTypeCode = "String", Width = 200, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierPendingReasonNameList", tenant, true), ColumnName = "CourierPendingReasonNameList", DataType = "".GetType() });

            var l = q.ToList();
            l.ForEach(r =>
            {
                var newrow = dt.NewRow();
                newrow[0] = r.CourierHawb;
                newrow[1] = r.ImporterName;
                newrow[2] = r.ImporterCode;
                newrow[3] = r.TotalInvoiceAmountInUSD;
                newrow[4] = r.CourierPendingReasonNameList;
                dt.Rows.Add(newrow);
            });
            var xls = new ExportToExcelHelper();
            var res = xls.ExportDataTableToExcel(dt, tenant, settingCol);
            return res;
        }

    }
}