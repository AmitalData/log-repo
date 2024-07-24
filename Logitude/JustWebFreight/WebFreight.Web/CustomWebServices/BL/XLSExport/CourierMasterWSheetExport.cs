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
using Logitude.Customs.BL.EntityQueryServices;

using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public class CourierMasterWSheetExport
    {
        public byte[] ExportReport(string courierMasterId, int tenant,string userId,bool IsWorkSheetFromExcel)
        {
            ICustomContext MyContext = CustomContext.GetContext(tenant);
            DeclarationCourierStatusListQueryService declarationCourierStatusQuery = new DeclarationCourierStatusListQueryService(MyContext);
            declarationCourierStatusQuery.RequiredFieldErrorsForCourierDeclarationIsValid = true;
            var q = declarationCourierStatusQuery.GetByCourierMasterId(courierMasterId, tenant,userId, IsWorkSheetFromExcel)
                .Select(r => new
                {
                    r.AirlineId,
                    r.MAWB,
                    r.MasterHAWB,
                    r.DeclarationId,
                    MasterGrossMassMeasure = r.MasterGrossMassMeasure ?? 0,
                    MasterPackageQuantity = r.MasterPackageQuantity ?? 0,
                    r.MasterCreateDateTime,
                    r.MasterGatewayPortCode,
                    r.MasterEstimatedArrivalDate,
                    r.DeclarationStorageSiteCode,
                    r.CourierHawb,
                    r.ProcedureCurrentName,
                    r.FastIndividualProcessCode,
                    r.ImporterName,
                    r.ImporterCode,
                    TotalInvoiceAmountInUSD = r.TotalInvoiceAmountInUSD ?? 0,
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
                    r.ImporterAddress,
                    r.CasualImporterTel


                });
            ;

            Boolean isExtendedReport = false;
            FeatureQuery featureQuery = new FeatureQuery();
            var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(tenant), tenant);
            var feature = features.Features.FirstOrDefault(x => x.Code == "ExportMasterExtended");
            if (feature != null)
            {
                isExtendedReport = true;
            }
            var group2 = (from d in MyContext.DeclarationPendings
                          join c in q
                          on d.DeclarationID equals c.DeclarationId

                          group d by d.DeclarationID into PendingGroup
                          select new { declaration = PendingGroup.Key, pending = PendingGroup.Select(g => g.CourierPendingReason.LocalName) });

            var Group3Var = new Dictionary<string, Group3Variables>();
            if (isExtendedReport)
            {


                var qSupplierInvoice = (from a in MyContext.SupplierInvoices
                                        group a by a.DeclarationId into qSupplierInvoices
                                        select new
                                      {
                                          DeclarationId = qSupplierInvoices.Key,
                                          InvoiceCurrencyTypeCode = qSupplierInvoices.Min(r => r.InvoiceCurrencyTypeCode),
                                      });

                var qSupplierInvoiceItem = (from a in MyContext.SupplierInvoiceItems
                                            group a by a.DeclarationId into qSupplierInvoicesItems
                                            select
                                          new
                                          {
                                              DeclarationId = qSupplierInvoicesItems.Key,
                                              InvoiceQuantity = qSupplierInvoicesItems.Sum(x => x.InvoiceQuantity),
                                          });

                var qConsignmentCargoDescription = (from a in MyContext.Consignments
                                                    group a by a.DeclarationId into gConsignments
                                                    select
                                                    new
                                                    {
                                                        DeclarationId = gConsignments.Key,
                                                        CargoDescription = gConsignments.Min(r => r.CargoDescription),

                                                    });


                var group3 = (from //d in MyContext.Declarations

                               dec in q /*on d.Id equals dec.DeclarationId into qjoinDeclarations
                              from myJoinDeclaration in qjoinDeclarations.DefaultIfEmpty()*/

                              join recConsignment in qConsignmentCargoDescription
                                                     on dec.DeclarationId equals recConsignment.DeclarationId into qjoinConsignments
                              from myJoinConsignment in qjoinConsignments.DefaultIfEmpty()

                              join recSupplierInvoices in qSupplierInvoice
                                                     on dec.DeclarationId equals recSupplierInvoices.DeclarationId into qjoinSupplierInvoice
                              from myJoinqSupplierInvoice in qjoinSupplierInvoice.DefaultIfEmpty()

                              join recSupplierInvoicesItems in qSupplierInvoiceItem
                                                   on dec.DeclarationId equals recSupplierInvoicesItems.DeclarationId into qjoinSupplierInvoiceItem
                              from myJoinqSupplierInvoiceItem in qjoinSupplierInvoiceItem.DefaultIfEmpty()

                              select new
                              {
                                  declaration = dec.DeclarationId,
                                  ImporterAddress = dec.ImporterAddress,
                                  CasualImportelTel = dec.CasualImporterTel,
                                  CargoDescription = myJoinConsignment != null ? myJoinConsignment.CargoDescription : null,
                                  InvoiceCurrencyTypeCode = myJoinqSupplierInvoice != null ? myJoinqSupplierInvoice.InvoiceCurrencyTypeCode : null,
                                  InvoiceQuantity = myJoinqSupplierInvoiceItem != null ? myJoinqSupplierInvoiceItem.InvoiceQuantity : null,
                              }); ;

                foreach (var item in group3)
                {
                    Group3Var.Add(item.declaration, new Group3Variables
                    {
                        CargoDescription = item.CargoDescription,
                        CasualImportelTel = item.CasualImportelTel,
                        InvoiceCurrencyTypeCode = item.InvoiceCurrencyTypeCode,
                        InvoiceQuantity = item.InvoiceQuantity.ToString(),
                        ImporterAddress = item.ImporterAddress,
                    });
                }
            }
            Dictionary<string, string> pendings = new Dictionary<string, string>();


            foreach (var item in group2)
            {
                pendings.Add(item.declaration, string.Join(",", item.pending));
            }

            var dicConPackages = new Dictionary<string, conPackagesValues>();

            if (isExtendedReport)
            {
                var qConsignmentPackages = (from a in MyContext.ConsignmentPackages
                                            where (a.PackageMeasureQualifierCode == "2")
                                            join cp in q
                                            on a.DeclarationId equals cp.DeclarationId

                                            group a by a.DeclarationId into qConsPackages
                                            select
                                          new
                                          {
                                              DeclarationId = qConsPackages.Key,
                                              GrossMassMeasure = qConsPackages.Where(t => t.GrossMassMeasure.HasValue).Sum(x => x.GrossMassMeasure),
                                              PackageQuantity = qConsPackages.Sum(x => x.PackageQuantity)
                                          });


                foreach (var item in qConsignmentPackages)
                {
                    dicConPackages.Add(item.DeclarationId, new conPackagesValues()
                    {
                        GrossMassMeasure = item.GrossMassMeasure.ToString(),
                        PackageQuantity = item.PackageQuantity.ToString()
                    });
                }
            }
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


                settingCol.Columns.Add(new Column() { Index = 1, Code = "CourierMasterId", Name = "CourierMasterId", DataTypeCode = "String", Width = 150, });
                dt.Columns.Add(new DataColumn() { Caption = /*"Courier Master"*/"ש.מ.ר", ColumnName = "CourierMasterId", DataType = System.Type.GetType("System.String"), });


                dt.Columns.Add(new DataColumn() { Caption = /*"Master HAWB"*/"שטר מטען פנימ", ColumnName = "MasterHAWB", DataType = System.Type.GetType("System.String") });
                settingCol.Columns.Add(new Column() { Index = 2, Code = "MasterHAWB", Name = "MasterHAWB", DataTypeCode = "String", Width = 100, });

                settingCol.Columns.Add(new Column() { Index = 3, Code = "ConsignmentPackageGrossMassMeasure", Name = "ConsignmentPackageGrossMassMeasure", DataTypeCode = "Decimal", Width = 100, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.ConsignmentPackage.F.GrossMassMeasure", tenant, true), ColumnName = "ConsignmentPackageGrossMassMeasure", DataType = System.Type.GetType("System.Decimal") });


                settingCol.Columns.Add(new Column() { Index = 4, Code = "ConsignmentPackagePackageQuantity", Name = "ConsignmentPackagePackageQuantity", DataTypeCode = "Decimal", Width = 100, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.ConsignmentPackage.F.PackageQuantity", tenant, true), ColumnName = "ConsignmentPackagePackageQuantity", DataType = System.Type.GetType("System.Decimal") });


                settingCol.Columns.Add(new Column() { Index = 5, Code = "MasterCreateDateTime", Name = "MasterCreateDateTime", DataTypeCode = "DateTime", Width = 100, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.CourierMaster.F.CreateDateTime", tenant, true), ColumnName = "MasterCreateDateTime", DataType = DateTime.Now.GetType() });



                settingCol.Columns.Add(new Column() { Index = 6, Code = "MasterGatewayPortCode", Name = "MasterGatewayPortCode", DataTypeCode = "String", Width = 100, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.CourierMaster.F.GatewayPortCode", tenant, true), ColumnName = "MasterGatewayPortCode", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 7, Code = "MasterEstimatedArrivalDate", Name = "MasterEstimatedArrivalDate", DataTypeCode = "DateTime", Width = 100, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.CourierMaster.F.EstimatedArrivalDate", tenant, true), ColumnName = "MasterEstimatedArrivalDate", DataType = DateTime.Now.GetType() });

                settingCol.Columns.Add(new Column() { Index = 8, Code = "MasterStorageSiteCode", Name = "MasterStorageSiteCode", DataTypeCode = "String", Width = 80, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.CourierMaster.F.StorageSiteCode", tenant, true), ColumnName = "MasterStorageSiteCode", DataType = "".GetType() });

                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierHawb", tenant, true), ColumnName = "CourierHawb", DataType = System.Type.GetType("System.String") });
                settingCol.Columns.Add(new Column() { Index = 9, Code = "CourierHawb", Name = "CourierHawb", DataTypeCode = "String", Width = 100, });

                settingCol.Columns.Add(new Column() { Index = 10, Code = "ProcedureCurrentName", Name = "ProcedureCurrentName", DataTypeCode = "String", Width = 122, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.ProcedureCurrentName", tenant, true), ColumnName = "ProcedureCurrentName", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 11, Code = "HighLowValue", Name = "HighLowValue", DataTypeCode = "String", Width = 70, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.FastIndividualProcessCode", tenant, true), ColumnName = "HighLowValue", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 12, Code = "ImporterName", Name = "ImporterName", DataTypeCode = "String", Width = 200, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CustomerName", tenant, true), ColumnName = "ImporterName", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 13, Code = "ImporterCode", Name = "ImporterCode", DataTypeCode = "String", Width = 90, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.ImporterCode", tenant, true), ColumnName = "ImporterCode", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 14, Code = "TotalInvoiceAmountInUSD", Name = "TotalInvoiceAmountInUSD", DataTypeCode = "Decimal", Width = 100, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.TotalInvoiceAmountInUSD", tenant, true), ColumnName = "TotalInvoiceAmountInUSD", DataType = typeof(Decimal) });

                settingCol.Columns.Add(new Column() { Index = 15, Code = "DocumentStatusCode", Name = "DocumentStatusCode", DataTypeCode = "String", Width = 55, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.DocumentStatusCode", tenant, true), ColumnName = "DocumentStatusCode", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 16, Code = "IsCourierMissingClassification", Name = "IsCourierMissingClassification", DataTypeCode = "String", Width = 53, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.IsCourierMissingClassification", tenant, true), ColumnName = "IsCourierMissingClassification", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 17, Code = "CourierManifestStatusCode", Name = "CourierManifestStatusCode", DataTypeCode = "String", Width = 53, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierManifestStatusCode", tenant, true), ColumnName = "CourierManifestStatusCode", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 18, Code = "CourierDeclarationStatusCode", Name = "CourierDeclarationStatusCode", DataTypeCode = "String", Width = 53, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierDeclarationStatusCode", tenant, true), ColumnName = "CourierDeclarationStatusCode", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 19, Code = "CourierPaymentStatusCode", Name = "CourierPaymentStatusCode", DataTypeCode = "String", Width = 53, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierPaymentStatusCode", tenant, true), ColumnName = "CourierPaymentStatusCode", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 20, Code = "CourierCustomStatusName", Name = "CourierCustomStatusName", DataTypeCode = "String", Width = 90, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierCustomStatusName", tenant, true), ColumnName = "CourierCustomStatusName", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 21, Code = "StorageSiteStatusCode", Name = "StorageSiteStatusCode", DataTypeCode = "String", Width = 98, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.StorageSiteStatusCode", tenant, true), ColumnName = "StorageSiteStatusCode", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 22, Code = "CourierSuspentionName", Name = "CourierSuspentionName", DataTypeCode = "String", Width = 100, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierSuspentionName", tenant, true), ColumnName = "CourierSuspentionName", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 23, Code = "SpecialActionStatus", Name = "SpecialActionStatus", DataTypeCode = "String", Width = 98, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.SpecialActionStatus", tenant, true), ColumnName = "SpecialActionStatus", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 24, Code = "DeclarationStatusTypeName", Name = "DeclarationStatusTypeName", DataTypeCode = "String", Width = 200, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.DeclarationStatusTypeName", tenant, true), ColumnName = "DeclarationStatusTypeName", DataType = "".GetType() });

                settingCol.Columns.Add(new Column() { Index = 25, Code = "CourierPendingReasonName", Name = "CourierPendingReasonName", DataTypeCode = "String", Width = 105, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierPendingReasonName", tenant, true), ColumnName = "CourierPendingReasonName", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 26, Code = "LastMileStatusCode", Name = "LastMileStatusCode", DataTypeCode = "String", Width = 100, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.LastMileStatusCode", tenant, true), ColumnName = "LastMileStatusCode", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 27, Code = "IsClosedForFollowUp", Name = "IsClosedForFollowUp", DataTypeCode = "String", Width = 90, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.IsClosedForFollowUp", tenant, true), ColumnName = "IsClosedForFollowUp", DataType = "".GetType() });


                settingCol.Columns.Add(new Column() { Index = 28, Code = "CourierPendingReasonList", Name = "CourierPendingReasonList", DataTypeCode = "String", Width = 80, });
                dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierPendingReasonList", tenant, true), ColumnName = "CourierPendingReasonList", DataType = "".GetType() });

                if (isExtendedReport)
                {
                    settingCol.Columns.Add(new Column() { Index = 29, Code = "ImporterAddress", Name = "ImporterAddress", DataTypeCode = "String", Width = 80, });
                    dt.Columns.Add(new DataColumn() { Caption = "כתובת", ColumnName = "ImporterAddress", DataType = "".GetType() });

                    settingCol.Columns.Add(new Column() { Index = 30, Code = "CasualImportelTel", Name = "CasualImportelTel", DataTypeCode = "String", Width = 90, });
                    dt.Columns.Add(new DataColumn() { Caption = "טלפון", ColumnName = "CasualImportelTel", DataType = "".GetType() });

                    settingCol.Columns.Add(new Column() { Index = 31, Code = "CargoDescription", Name = "CargoDescription", DataTypeCode = "String", Width = 90, });
                    dt.Columns.Add(new DataColumn() { Caption = "תאור טובין", ColumnName = "CargoDescription", DataType = "".GetType() });

                    settingCol.Columns.Add(new Column() { Index = 32, Code = "InvoiceQuantity", Name = "InvoiceQuantity", DataTypeCode = "String", Width = 90, });
                    dt.Columns.Add(new DataColumn() { Caption = "כמות יחידות בחשבונית", ColumnName = "InvoiceQuantity", DataType = "".GetType() });

                    settingCol.Columns.Add(new Column() { Index = 33, Code = "InvoiceCurrencyTypeCode", Name = "InvoiceCurrencyTypeCode", DataTypeCode = "String", Width = 90, });
                    dt.Columns.Add(new DataColumn() { Caption = "מטבע חשבונית", ColumnName = "InvoiceCurrencyTypeCode", DataType = "".GetType() });
                }




                var l = q.ToList();
                l.ForEach(r =>
                {
                    var newrow = dt.NewRow();
                    newrow[0] = $"{r.AirlineId}-{r.MAWB}";
                    //newrow[1] = r.CourierHawb;
                    newrow[1] = r.MasterHAWB;
                    newrow[2] = isExtendedReport?dicConPackages.ContainsKey(r.DeclarationId)?  dicConPackages[r.DeclarationId].GrossMassMeasure: "0":r.MasterGrossMassMeasure.ToString();
                    newrow[3] = isExtendedReport?dicConPackages.ContainsKey(r.DeclarationId) ? dicConPackages[r.DeclarationId].PackageQuantity : "0":r.MasterPackageQuantity.ToString();
                    newrow[4] = ((object)r.MasterCreateDateTime) ?? DBNull.Value;
                    newrow[5] = r.MasterGatewayPortCode;
                    newrow[6] = ((object)r.MasterEstimatedArrivalDate) ?? DBNull.Value;
                    newrow[7] = r.DeclarationStorageSiteCode;
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
                    newrow[24] = pendings.FirstOrDefault(x => x.Key == r.DeclarationId).Value;  //r.CourierPendingReasonName;
                    newrow[25] = r.LastMileStatusCode;
                    newrow[26] = r.IsClosedForFollowUp;
                    newrow[27] = r.CourierPendingReasonList;
                    if (isExtendedReport)
                    {
                        newrow[28] = Group3Var.FirstOrDefault(x => x.Key == r.DeclarationId).Value.ImporterAddress;
                        newrow[29] = Group3Var.FirstOrDefault(x => x.Key == r.DeclarationId).Value.CasualImportelTel;
                        newrow[30] = Group3Var.FirstOrDefault(x => x.Key == r.DeclarationId).Value.CargoDescription;
                        newrow[31] = Group3Var.FirstOrDefault(x => x.Key == r.DeclarationId).Value.InvoiceQuantity;
                        newrow[32] = Group3Var.FirstOrDefault(x => x.Key == r.DeclarationId).Value.InvoiceCurrencyTypeCode;
                    }
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
                .Where(x => x.CourierCustomStatusCode == "2")
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
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierCustomStatusName", tenant, true), ColumnName = "CourierCustomStatusName", DataType = System.Type.GetType("System.String") });

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
                .Where(x => x.CourierPendingReasonList != null)
            .Select(r => new
            {
                r.DeclarationId,
                r.CourierHawb,
                r.ImporterName,
                r.ImporterCode,
                TotalInvoiceAmountInUSD = r.TotalInvoiceAmountInUSD ?? 0,
                //CourierPendingReasonNameList = r.CourierPendingReasonNameList
                //  CourierPendingReasonNameList = r.CourierPendingReasonName
            });



            var group2 = (from d in MyContext.DeclarationPendings
                          join c in q on d.DeclarationID equals c.DeclarationId
                          where d.Status != "S"
                          group d by d.DeclarationID into PendingGroup
                          select new { declaration = PendingGroup.Key, pending = PendingGroup.Select(g => g.CourierPendingReasonCode) });
        

       
            Dictionary<string, PendingReport> pendings = new Dictionary<string, PendingReport>();


            foreach (var item in group2)
            {
                var pendingCodes = item.pending.ToList();
                var CourierPendingReasonName = "";
                var PendingRemarks = "";
                foreach (var pendingCode in pendingCodes)
                {
                    var declarationPendingQueryService = new DeclarationPendingQueryService(MyContext);
                    var declarationPendingPM = declarationPendingQueryService.GetSingle(item.declaration, pendingCode, false, false);
                    if (declarationPendingPM != null)
                    {
                        CourierPendingReasonName+= ","+declarationPendingPM.CourierPendingReasonName;
                        PendingRemarks +=","+declarationPendingPM.PendingRemarks;
                    }
                }
                pendings.Add(item.declaration, new PendingReport(CourierPendingReasonName, PendingRemarks));
            }




            //var ug2 = (from PendingGroup in group2
            //           select new { PendingGroup.declaration, pendings = string.Join(",", PendingGroup.pending) });
            //ug2.ToList();

            //var q2 = q
            //   .Select(r => new {
            //       r.CourierHawb,
            //       r.ImporterName,
            //       r.ImporterCode,
            //       TotalInvoiceAmountInUSD = r.TotalInvoiceAmountInUSD ?? 0,
            //       CourierPendingReasonNameList =

            //       String.Join(",", MyContext.DeclarationPendings.AsEnumerable()
            //           .Where(c => c.DeclarationID == r.DeclarationId)
            //           .Select(c => c.CourierPendingReason.LocalName))
            //   });

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

            settingCol.Columns.Add(new Column() { Index = 6, Code = "CourierPendingRemark", Name = "CourierPendingRemark", DataTypeCode = "String", Width = 200, });
            dt.Columns.Add(new DataColumn() { Caption = "הערות", ColumnName = "CourierPendingRemark", DataType = "".GetType() });

            var l = q.ToList();
            l.ForEach(r =>
            {
                var newrow = dt.NewRow();
                newrow[0] = r.CourierHawb;
                newrow[1] = r.ImporterName;
                newrow[2] = r.ImporterCode;
                newrow[3] = r.TotalInvoiceAmountInUSD;
                var pending = pendings.FirstOrDefault(x => x.Key == r.DeclarationId);
                newrow[4] = pending.Value?.pendings;
                newrow[5] = pending.Value?.pendingRemark + ";" ;

                dt.Rows.Add(newrow);
            });
            var xls = new ExportToExcelHelper();
            var res = xls.ExportDataTableToExcel(dt, tenant, settingCol);
            return res;
        }

    }
    public class Group3Variables
    {
        public string ImporterAddress { get; set; }
        public string CasualImportelTel { get; set; }
        public string CargoDescription { get; set; }
        public string InvoiceQuantity { get; set; }
        public string InvoiceCurrencyTypeCode { get; set; }
       

    }
    public class PendingReport
    {
        public PendingReport(string pendings,string pendingRemark)
        {
            this.pendings = pendings;
            this.pendingRemark = pendingRemark;
        }
        public string pendings;
        public string pendingRemark;
    }

    public class conPackagesValues
    {
        public string GrossMassMeasure { get; set; }
        public string PackageQuantity { get; set; }

    }
}