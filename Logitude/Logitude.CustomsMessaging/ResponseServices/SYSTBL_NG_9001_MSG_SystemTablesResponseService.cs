using Logitude.AmitalMessaging.Utils;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Helpers.ClosedTable;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{

    public class SYSTBL_NG_9001_MSG_SystemTablesResponseService
        : ResponseServiceBase<SystemTableResponseData, SYSTBL_NG_9001_MSG_SystemTablesResponse, SystemTableRequestParams>
    {
        public override void Update(SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse, SystemTableRequestParams requestParams)
        {
            bool anatWantAsDataSetExample = false;
            if (anatWantAsDataSetExample || requestParams.ForAnatSaveAsDATASET)
            {
                var filePath = @"c:\temp\DataSet" + requestParams.TableId + ".xml";
                var myEncoder = new UTF8Encoding();
                string xml = XmlGenericUtil<SYSTBL_NG_9001_MSG_SystemTablesResponse>.SerializeObject(customResponse);
                File.WriteAllText(@"c:\temp\" + requestParams.TableId + ".xml", xml);
                if (!string.IsNullOrWhiteSpace(customResponse.TableAsDataSetTableData))
                {
                    Byte[] bytes = myEncoder.GetBytes(customResponse.TableAsDataSetTableData);
                    var DataSet1 = new DataSet();
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        //this.AutoGenerateColumns = true;

                        DataSet1.ReadXml(ms as Stream);
                        DataSet1.WriteXml(filePath);

                    }
                }
                return;
            }

            if (requestParams.Pseudo)
            {
                return;
            }
            //if (requestParams.TableId == "1892")
            //{
            //    Update1892(customResponse, requestParams);
            //    return;
            //}
            var listOf9001TDExt = ManipulateCustomResponse(requestParams.TableId, customResponse);
            if ((requestParams.TableId == "1344" || requestParams.TableId == "2653") && listOf9001TDExt.Count == 0)
            {
                LogMessagingUtil.Instance.AppendLine("1344 dataset is null>> no update");
                return;

            }
            var done = false;

            if (
                //customResponse.TableData != null
                listOf9001TDExt != null
                )
            {

                //var customContext = CustomContext.GetContext(0);
                //closedTableService can dispose the customContext
                var closedTableService = Logitude.CustomsMessaging.Helpers.ClosedTable.ClosedTableServiceFactory.CreateNew(
                    CustomContext.GetContext(0),//closedTableService can dispose the customContext //customContext,
                                                //requestParams.TableId,
                    customResponse.tableName,
                    //customResponse.TableData.ToList(),
                    listOf9001TDExt,
                    requestParams.Tenant
                    );
                if (closedTableService != null)
                {
                    try
                    {
                        closedTableService.UpdateSingleClosedTable();
                    }
                    catch (System.Exception)
                    {

                        throw;
                    }

                    CustomsClosedTableRepository closedTableRep = new CustomsClosedTableRepository(CustomContext.GetContext(0));
                    CustomsClosedTable table = closedTableRep.GetSingle(new CustomsClosedTableKeys() { Id = requestParams.TableId });
                    ObjectTableRepository objectTableRepository = new ObjectTableRepository(0);

                    if (closedTableService.ClosedTableHasChanged())
                    {
                        //
                        TableLastUpdateM myTableLastUpdateM = new TableLastUpdateM() { ObjectTableId = table.ObjectTableId };
                        if (requestParams != null)
                        {
                            myTableLastUpdateM.AlternativeUserId = requestParams.LoggingUserId;
                            myTableLastUpdateM.AlternativeUserTenant = requestParams.Tenant;

                        }
                        TableLastUpdateClass.UpdateTableHistory(0, table.DbName, myTableLastUpdateM);


                    }

                    table.LastUpdateDate = DateTime.Now;
                    table.StatusCode = "3";
                    closedTableRep.Update(table);
                    closedTableRep.SubmitChanges();
                    done = true;
                }
            }
            if (!done)
            {
                LoadCustomClosedTables.UpdateSingleClosedTable(requestParams.TableId, requestParams, customResponse);
            }
            bool allways_try_To_Build_Custom_Zip_File = true;
            if (allways_try_To_Build_Custom_Zip_File)
            {
                UpdateCustomZipFile(requestParams.Tenant, requestParams.LoggingUserId);//allways try To Build Custom Zip File !!! 
            }

            //others ...
            var syncUnifreight = SystemTables.SyncUnifreight(requestParams.TableId);
            if (syncUnifreight)
            {
                var list = customResponse.TableData.OrderBy(rec => rec.id).ToList();
                SystemTables.Send2Amital(requestParams.TableId, list, requestParams.Tenant);
            }
        }

        private void UpdateCustomZipFile(int tenant, string LoggingUserId)
        {
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsClosedTable");
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
            var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, "UCTZIP", objectTableId, tenant.ToString(), null, null, null, true);
            if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            {

                LogMessagingUtil.Instance.AppendLine("Requestsheet  with Interface Type  = UCTZIP  already in progress  !!!");
                return;
            }
            LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCTZIP  !!!");





            var genericRequestParams = new GenericRequestParams()
            {
                Tenant = tenant,
                AppicationId = tenant.ToString(),
                RequestVIA = SendRequestVIA.WebServiceBatch,
                LoggingEnabled = true,
                InterfaceTypeCode = "UCTZIP",
                MainInterfaceCode = "UCTZIP",
                LoggingEntityId = tenant.ToString(),
                //LoggingEntityReference = this._DeclarationPM.DeclarationNumber;
                LoggingObjectTableId = objectTableId,
                LoggingUserId = LoggingUserId,
            };
            //var messService = new Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService();
            //var responseData = messService.SendSheet(genericRequestParams);


            using (var trans = TransactionFactory.GetNewTransaction())
            {
                try
                {

                    var testIt = false;
                    if (testIt)
                    {
                        var testCustomTableZip = new Logitude.CustomsMessaging.MessagingServices.DCAInUniCustomTableZip_MsgMessagingService();
                        genericRequestParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
                        var res = testCustomTableZip.Send(genericRequestParams);
                    }
                    else
                    {
                        SBQMessageService.CreateSheetSBQMessage<Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams>(genericRequestParams
                            , false, DateTime.Now.AddMinutes(5)
                            );
                    }
                    trans.Complete();
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCTZIP SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);

                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCTZIP SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    //throw;
                }
            }



        }






        private List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> ManipulateCustomResponse(string tableId, SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse)
        {

            var writeHighlight = false;
            switch (tableId)
            {
                case "1121":
                case "1385": //1385 FAILED , לדלג מעל רשומות שגודלם מעל 3 תווים
                case "1946"://טיפול בטבלת מכס 1946 - סוגי רכבים =T16287
                    {
                        customResponse.TableData = RemoveMoreThen(customResponse.TableData, 3);
                    }
                    break;

                case "1339": //כמו כן , יש נפילה בשל אורך שדות , יש לשים טיפול שיתעלם משדות באורך גדול מ 17 (ייתכן שזה גם הגורם לכך שלא מתעדכן שדה SiteTypeCode )
                    {
                        customResponse.TableData = RemoveMoreThen(customResponse.TableData, 17);
                    }
                    break;
                case "2653":
                case "1344": //EnglishName must be a string or array type with a maximum length of '40'.
                    {
                        //customResponse.TableData = TruncateNameTo(customResponse.TableData, 40);
                        var extList = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
                        if (!String.IsNullOrWhiteSpace(customResponse.TableAsDataSetTableData))
                        {

                            Logitude.CustomsMessaging.Helpers.ClosedTable.
                                                        ManipulateCustomResponse.
                                                    DataSetToTableData(customResponse,
                                                    (newResponseTableData, dr) =>
                                                    {
                                                        var newExt =
                                                            SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(newResponseTableData);
                                                        newExt.MyInternationalSite = new Helpers.ClosedTable.InternationalSiteP();
                                                        if (!writeHighlight)
                                                        {
                                                            LogMessagingUtil.Instance.Append(
                            @"1344:InternationalSite:Calc=
 if (!string.IsNullOrWhiteSpace(dr[""extraNumericData""].ToString()))
     CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(CustomContext.GetContext(0));
        var myCountry = customsCountryQueryService.GetSingleByMalamID(newResponseTableData.extraNumericData.ToString());
        if (myCountry != null && !string.IsNullOrWhiteSpace(myCountry.Code))
        {
            newExt.MyInternationalSite.CountryTypeCode = myCountry.Code;

ID List :
");
                                                            writeHighlight = true;
                                                        }
                                                        /*
                                                        if (!string.IsNullOrWhiteSpace(dr["ExtraNumericData"].ToString()))
                                                        {
                                                            LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                            CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(CustomContext.GetContext(0));
                                                            if (!string.IsNullOrWhiteSpace(newResponseTableData.extraNumericData.ToString()))
                                                            {
                                                                var myCountry = customsCountryQueryService.GetSingleByMalamID(newResponseTableData.extraNumericData.ToString());

                                                                if (myCountry != null && !string.IsNullOrWhiteSpace(myCountry.Code))
                                                                {
                                                                    LogMessagingUtil.Instance.Append("amitalCountryMalamID = " + newResponseTableData.extraNumericData.ToString() + " Translated to " + myCountry.Code);
                                                                    newExt.MyInternationalSite.CountryTypeCode = myCountry.Code;
                                                                }
                                                            }
                                                        }*/

                                                        if (!string.IsNullOrWhiteSpace(dr["ID"].ToString()))
                                                        {
                                                            LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                            CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(CustomContext.GetContext(0));
                                                            if (!string.IsNullOrWhiteSpace(newResponseTableData.id))
                                                            {
                                                                var myCountry = customsCountryQueryService.GetSingle(newResponseTableData.id.Substring(0, 2), false, true);

                                                                if (myCountry != null && !string.IsNullOrWhiteSpace(myCountry.Code))
                                                                {
                                                                    LogMessagingUtil.Instance.Append("Country ID = " + myCountry.Code);
                                                                    newExt.MyInternationalSite.CountryTypeCode = myCountry.Code;
                                                                }
                                                            }
                                                        }

                                                        if (newResponseTableData.name.Length > 40)
                                                        {
                                                            newExt.name = newResponseTableData.name.Substring(0, 40);
                                                        }
                                                        extList.Add(newExt);
                                                    });
                        }
                        return extList;
                    }
                    break;
                case "1091":
                    {
                        Logitude.CustomsMessaging.Helpers.ClosedTable.
                            ManipulateCustomResponse.
                        DataSetToTableData(customResponse,
                        (newResponseTableData, dr) =>
                        {
                            if (!writeHighlight)
                            {
                                LogMessagingUtil.Instance.Append(
@"1091:Calc=
if (newResponseTableData.state != 0)
    if (!dr[""IsTypeInCustoms""].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
        newResponseTableData.state = 0;

ID List :
");
                                writeHighlight = true;
                            }
                            if (newResponseTableData.state != 0)
                            {
                                if (!dr["IsTypeInCustoms"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                {

                                    LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                    newResponseTableData.state = 0;
                                }
                            }
                        });
                    }
                    break;
                case "1144":
                    {

                        Logitude.CustomsMessaging.Helpers.ClosedTable.
                                                    ManipulateCustomResponse.
                                                DataSetToTableData(customResponse,
                                                (newResponseTableData, dr) =>
                                                {
                                                    if (!writeHighlight)
                                                    {
                                                        LogMessagingUtil.Instance.Append(
                        @"1144:Calc=
 if (newResponseTableData.state != 0)
    if (!dr[""IsEditable""].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
        newResponseTableData.state = 0;

ID List :
");
                                                        writeHighlight = true;
                                                    }
                                                    if (newResponseTableData.state != 0)
                                                    {
                                                        if (!dr["IsEditable"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                        {

                                                            LogMessagingUtil.Instance.Append(newResponseTableData.id + ","); newResponseTableData.state = 0;
                                                        }
                                                    }
                                                    if (newResponseTableData.id == "ILS")
                                                    {
                                                        newResponseTableData.state = 1;
                                                    }
                                                });

                    }
                    break;
                case "1354":
                case "GovernmentProcedureType":
                    {
                        var extList = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
                        Logitude.CustomsMessaging.Helpers.ClosedTable.
                                                    ManipulateCustomResponse.
                                                DataSetToTableData(customResponse,
                                                (newResponseTableData, dr) =>
                                                {
                                                    var newExt =
                                                        SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(newResponseTableData);
                                                    newExt.MyGovernmentProcedureType = new Helpers.ClosedTable.GovernmentProcedureType();
                                                    if (!writeHighlight)
                                                    {
                                                        LogMessagingUtil.Instance.Append(
                        @"1354:GovernmentProcedureType:Calc=
 if (dr[""InUseByImportDeclaration""].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
        newExt.MyGovernmentProcedureType.IsImport = true;

ID List :
");
                                                        writeHighlight = true;
                                                    }
                                                    if (dr["InUseByImportDeclaration"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyGovernmentProcedureType.IsImport = true;
                                                    }
                                                    if (dr["InUseByExportDeclaration"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyGovernmentProcedureType.IsExport = true;
                                                    }
                                                    extList.Add(newExt);
                                                });
                        return extList;
                        break;
                    }

                case "1416":
                case "ModificationAndDiscountType":
                    {
                        var extList = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
                        Logitude.CustomsMessaging.Helpers.ClosedTable.
                                                    ManipulateCustomResponse.
                                                DataSetToTableData(customResponse,
                                                (newResponseTableData, dr) =>
                                                {
                                                    var newExt =
                                                        SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(newResponseTableData);
                                                    newExt.MyModificationAndDiscountType = new Helpers.ClosedTable.ModificationAndDiscountType();
                                                    if (!writeHighlight)
                                                    {
                                                        LogMessagingUtil.Instance.Append(
                        @"1416:ModificationAndDiscountType:Calc=
 if (dr[""IsRelevantInvoice""].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
        newExt.MyModificationAndDiscountType.IsRelevantInvoice = true;
if (dr[""IsRelevantGoodsItem""].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
        newExt.MyModificationAndDiscountType.IsRelevantGoodsItem = true;
ID List :
");
                                                        writeHighlight = true;
                                                    }
                                                    if (dr["IsRelevantInvoice"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyModificationAndDiscountType.IsRelevantInvoice = true;
                                                    }
                                                    if (dr["IsRelevantGoodsItem"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyModificationAndDiscountType.IsRelevantGoodsItem = true;
                                                    }
                                                    if (dr["IsRelevantGoodsItemExport"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyModificationAndDiscountType.IsRelevantGoodsItemExport = true;
                                                    }
                                                    if (dr["IsRelevantInvoiceExport"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyModificationAndDiscountType.IsRelevantInvoiceExport = true;
                                                    }
                                                    if (dr["ExtraNumericData"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyModificationAndDiscountType.ExtraNumericData = dr["ExtraNumericData"].ToString();
                                                    }
                                                    if (dr["IsCustomsValueComponent"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyModificationAndDiscountType.IsCustomsValueComponent = true;
                                                    }
                                                    if (dr["IsCustomsValueComponentExport"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyModificationAndDiscountType.IsCustomsValueComponentExport = true;
                                                    }

                                                    if (dr["CurrencyMustBeSameAsInvoice"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyModificationAndDiscountType.CurrencyMustBeSameAsInvoice = true;
                                                    }
                                                    if (dr["CurrencyMustBeSameAsInvoiceExport"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyModificationAndDiscountType.CurrencyMustSameInvoiceExport = true;
                                                    }
                                                    if (dr["IsCustomUseExport"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyModificationAndDiscountType.IsCustomUseExport = true;
                                                    }
                                                    if (dr["ExportNetoValuesModificationAffectTypeID"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyModificationAndDiscountType.ExportNetoValuesModificationAffectTypeID = dr["ExportNetoValuesModificationAffectTypeID"].ToString();
                                                    }
                                                    if (dr["ExportFOBValuesModificationAffectTypeID"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyModificationAndDiscountType.ExportFOBValuesModificationAffectTypeID = dr["ExportFOBValuesModificationAffectTypeID"].ToString();
                                                    }

                                                    extList.Add(newExt);
                                                });
                        return extList;
                        break;

                    }
                case "1259":
                case "CargoIdentifireType":
                    {
                        var extList = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
                        Logitude.CustomsMessaging.Helpers.ClosedTable.
                                                    ManipulateCustomResponse.
                                                DataSetToTableData(customResponse,
                                                (newResponseTableData, dr) =>
                                                {
                                                    var newExt =
                                                        SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(newResponseTableData);
                                                    newExt.MyCargoIdentifireType = new Helpers.ClosedTable.CargoIdentifireType();
                                                    if (!writeHighlight)
                                                    {
                                                        writeHighlight = true;
                                                    }
                                                    if (dr["IsForDeclarationExport"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyCargoIdentifireType.IsForDeclarationExport = true;
                                                    }
                                                    if (dr["IsForDeclarationImport"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyCargoIdentifireType.IsForDeclarationImport = true;
                                                    }
                                                    if (dr["IsForManifest"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyCargoIdentifireType.IsForManifest = true;
                                                    }
                                                    if (dr["IsKey2Mandatory"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyCargoIdentifireType.IsKey2Mandatory = true;
                                                    }
                                                    if (dr["IsKey3Mandatory"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyCargoIdentifireType.IsKey3Mandatory = true;
                                                    }
                                                    if (dr["CargoIdentifierKey1Name"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyCargoIdentifireType.CargoIdentifierKey1Name = dr["CargoIdentifierKey1Name"].ToString();
                                                    }
                                                    if (dr["CargoIdentifierKey2Name"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyCargoIdentifireType.CargoIdentifierKey2Name = dr["CargoIdentifierKey2Name"].ToString();
                                                    }
                                                    if (dr["CargoIdentifierKey3Name"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyCargoIdentifireType.CargoIdentifierKey3Name = dr["CargoIdentifierKey3Name"].ToString();
                                                    }
                                                    extList.Add(newExt);
                                                });
                        return extList;

                    }
                case "2009":
                case "TradeAgreementTypeView":
                    {
                        var extList = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
                        Logitude.CustomsMessaging.Helpers.ClosedTable.
                                                    ManipulateCustomResponse.
                                                DataSetToTableData(customResponse,
                                                (newResponseTableData, dr) =>
                                                {
                                                    var newExt = SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(newResponseTableData);
                                                    newExt.MyTradeAgreement = new Helpers.ClosedTable.TradeAgreement();

                                                    if (!writeHighlight)
                                                    {
                                                        writeHighlight = true;
                                                    }
                                                    if (dr["CustomsBookTypeID"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        int.TryParse(dr["CustomsBookTypeID"]?.ToString(), out int val);
                                                        newExt.MyTradeAgreement.CustomsBookTypeID = val;
                                                    }
                                                    if (dr["CountryGroupID"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        int.TryParse(dr["CountryGroupID"]?.ToString(), out int val);
                                                        newExt.MyTradeAgreement.CountryGroupID = val;
                                                    }

                                                    extList.Add(newExt);
                                                });
                        return extList;

                    }
                case "1423":
                case "CertificateExemptionType":
                    {
                        var extList = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
                        Logitude.CustomsMessaging.Helpers.ClosedTable.
                                                    ManipulateCustomResponse.
                                                DataSetToTableData(customResponse,
                                                (newResponseTableData, dr) =>
                                                {
                                                    var newExt =
                                                        SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(newResponseTableData);
                                                    newExt.MyCertificateExemptionType = new Helpers.ClosedTable.CertificateExemptionType();
                                                    if (!writeHighlight)
                                                    {
                                                        writeHighlight = true;
                                                    }
                                                    if (dr["IsImportDeclaration"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyCertificateExemptionType.IsImportDeclaration = true;
                                                    }
                                                    if (dr["IsExportDeclaration"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyCertificateExemptionType.IsExportDeclaration = true;
                                                    }
                                                    extList.Add(newExt);
                                                });
                        return extList;
                        break;

                    }
                case "1604":
                case "ConfirmationType":
                    {
                        var extList = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
                        Logitude.CustomsMessaging.Helpers.ClosedTable.
                                                    ManipulateCustomResponse.
                                                DataSetToTableData(customResponse,
                                                (newResponseTableData, dr) =>
                                                {
                                                    var newExt =
                                                        SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(newResponseTableData);
                                                    newExt.MyConfirmationType = new Helpers.ClosedTable.ConfirmationType();
                                                    if (!writeHighlight)
                                                    {
                                                        writeHighlight = true;
                                                    }
                                                    if (CheckDRString(dr["MalamID"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        int val = IntFromDR(dr["MalamID"]);
                                                        newExt.MyConfirmationType.MalamID = val;
                                                    }
                                                    if (CheckDRString(dr["State"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        int val = IntFromDR(dr["State"]);
                                                        newExt.MyConfirmationType.State = val;
                                                    }
                                                    if (CheckDRString(dr["Exempt_CertificateDocumentCategoryTypeID"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        int val = IntFromDR(dr["Exempt_CertificateDocumentCategoryTypeID"]);
                                                        newExt.MyConfirmationType.Exempt_CertificateDocumentCategoryTypeID = val;
                                                    }
                                                    if (CheckDRBool(dr["IsImport"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        newExt.MyConfirmationType.IsImport = true;
                                                    }
                                                    if (CheckDRBool(dr["IsExemptOtherAuthority"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        newExt.MyConfirmationType.IsExemptOtherAuthority = true;
                                                    }
                                                    if (CheckDRString(dr["ConfirmationComputerizationLevelTypeID"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        int val = IntFromDR(dr["ConfirmationComputerizationLevelTypeID"]);
                                                        newExt.MyConfirmationType.ConfirmationComputerizationLevelTypeID = val;
                                                    }
                                                    if (CheckDRBool(dr["IsCEO"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        newExt.MyConfirmationType.IsCEO = true;
                                                    }
                                                    if (CheckDRBool(dr["IsNeedDeclaration"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        newExt.MyConfirmationType.IsNeedDeclaration = true;
                                                    }
                                                    if (CheckDRString(dr["CertificateDocumentCategoryTypeID"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        int val = IntFromDR(dr["CertificateDocumentCategoryTypeID"]);
                                                        newExt.MyConfirmationType.CertificateDocumentCategoryTypeID = val;
                                                    }
                                                    if (CheckDRString(dr["AuthorityID"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        int val = IntFromDR(dr["AuthorityID"]);
                                                        newExt.MyConfirmationType.AuthorityID = val;
                                                    }
                                                    if (CheckDRBool(dr["IsQuotaCheckNeeded"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        newExt.MyConfirmationType.IsQuotaCheckNeeded = true;
                                                    }
                                                    if (CheckDRString(dr["ExternalIDNumPerAuthority"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        int val = IntFromDR(dr["ExternalIDNumPerAuthority"]);
                                                        newExt.MyConfirmationType.ExternalIDNumPerAuthority = val;
                                                    }
                                                    if (CheckDRBool(dr["IsForCustomsItem"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        newExt.MyConfirmationType.IsForCustomsItem = true;
                                                    }
                                                    if (CheckDRBool(dr["IsPharmacy"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        newExt.MyConfirmationType.IsPharmacy = true;
                                                    }
                                                    if (CheckDRBool(dr["IsVeterinarian"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        newExt.MyConfirmationType.IsVeterinarian = true;
                                                    }
                                                    if (CheckDRBool(dr["IsVehicleStandardization"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        newExt.MyConfirmationType.IsVehicleStandardization = true;
                                                    }
                                                    if (CheckDRBool(dr["IsQuantityMandatory"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        newExt.MyConfirmationType.IsQuantityMandatory = true;
                                                    }
                                                    if (CheckDRBool(dr["IsForCE"]))
                                                    {
                                                        LogAddRow(newResponseTableData);
                                                        newExt.MyConfirmationType.IsForCE = true;
                                                    }

                                                    extList.Add(newExt);
                                                });
                        return extList;
                        break;

                    }
                case "23928":
                case "IncotemrsFileValidation":
                    {
                        var extList = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
                        Logitude.CustomsMessaging.Helpers.ClosedTable.
                                                    ManipulateCustomResponse.
                                                DataSetToTableData(customResponse,
                                                (newResponseTableData, dr) =>
                                                {
                                                    var newExt =
                                                        SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(newResponseTableData);
                                                    newExt.MyIncotemrsFileValidation = new Helpers.ClosedTable.IncotemrsFileValidation();
                                                    if (!writeHighlight)
                                                    {
                                                        writeHighlight = true;
                                                    }
                                                    if (dr["TermsOfSaleTypeID"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyIncotemrsFileValidation.TermsOfSaleTypeID = dr["TermsOfSaleTypeID"].ToString();
                                                    }
                                                    if (dr["IsFreightCharge"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyIncotemrsFileValidation.IsFreightCharge = true;
                                                    }
                                                    if (dr["IsPortIsraelCharge"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyIncotemrsFileValidation.IsPortIsraelCharge = true;
                                                    }
                                                    if (dr["IsInsurance"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyIncotemrsFileValidation.IsInsurance = true;
                                                    }
                                                    if (dr["CargoIdentifierTypeID"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyIncotemrsFileValidation.CargoIdentifierTypeID = dr["CargoIdentifierTypeID"].ToString(); ;
                                                    }
                                                    if (dr["CargoIdentifierTypeName"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyIncotemrsFileValidation.CargoIdentifierTypeName = dr["CargoIdentifierTypeName"].ToString(); ;
                                                    }
                                                    if (dr["LeadDocumentTypeID"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyIncotemrsFileValidation.LeadDocumentTypeID = dr["LeadDocumentTypeID"].ToString(); ;
                                                    }
                                                    if (dr["LeadDocumentTypeName"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyIncotemrsFileValidation.LeadDocumentTypeName = dr["LeadDocumentTypeName"].ToString(); ;
                                                    }
                                                    extList.Add(newExt);
                                                });
                        return extList;
                        break;

                    }
                case "1998":
                case "NDMessageActionCode":
                    {

                        var extList = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
                        Logitude.CustomsMessaging.Helpers.ClosedTable.
                                                    ManipulateCustomResponse.
                                                DataSetToTableData(customResponse,
                                                (newResponseTableData, dr) =>
                                                {
                                                    var newExt =
                                                        SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(newResponseTableData);
                                                    newExt.MyNDMessageActionCode = new Helpers.ClosedTable.NDMessageActionCode();
                                                    if (!writeHighlight)
                                                    {
                                                        writeHighlight = true;
                                                    }
                                                    if (dr["StartDate"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyNDMessageActionCode.StartDate = DateTime.Parse(dr["StartDate"].ToString());

                                                    }
                                                    if (dr["StartDate"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyNDMessageActionCode.EndDate = DateTime.Parse(dr["EndDate"].ToString());
                                                    }

                                                    extList.Add(newExt);
                                                });
                        return extList;
                        break;

                    }
                case "1366":
                case "ContainerType":
                    {

                        var extList = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
                        Logitude.CustomsMessaging.Helpers.ClosedTable.
                                                    ManipulateCustomResponse.
                                                DataSetToTableData(customResponse,
                                                (newResponseTableData, dr) =>
                                                {
                                                    var newExt =
                                                        SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(newResponseTableData);
                                                    newExt.MyContainerType = new Helpers.ClosedTable.ContainerType();
                                                    if (!writeHighlight)
                                                    {
                                                        writeHighlight = true;
                                                    }
                                                    if (dr["IsIsoTankContainer"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        if (dr["IsIsoTankContainer"].ToString() == "true")
                                                        {
                                                            newExt.MyContainerType.IsIsoTankContainer = true;
                                                        }
                                                    }
                                                    if (dr["IsNeedSeal"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        if (dr["IsNeedSeal"].ToString() == "true")
                                                        {
                                                            newExt.MyContainerType.IsNeedSeal = true;
                                                        }
                                                    }
                                                    if (dr["IsAerial"].ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        if (dr["IsAerial"].ToString() == "true")
                                                        {
                                                            newExt.MyContainerType.IsAerial = true;
                                                        }
                                                    }

                                                    extList.Add(newExt);
                                                });
                        return extList;
                        break;

                    }
                case "1422":
                case "ItemGovernmentProcedureType":
                    {

                        var extList = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
                        Logitude.CustomsMessaging.Helpers.ClosedTable.
                                                    ManipulateCustomResponse.
                                                DataSetToTableData(customResponse,
                                                (newResponseTableData, dr) =>
                                                {
                                                    var newExt =
                                                        SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(newResponseTableData);
                                                    newExt.MyItemGovernmentProcedureType = new Helpers.ClosedTable.ItemGovernmentProcedureType();
                                                    if (!writeHighlight)
                                                    {
                                                        LogMessagingUtil.Instance.Append(
                       @"1422:ItemGovernmentProcedureType:Calc=
 if (dr[""LeadDocumentTypeID""].ToString() != null)
       newExt.MyItemGovernmentProcedureType.LeadDocumentTypeID = dr[""LeadDocumentTypeID""].ToString();");

                                                        writeHighlight = true;
                                                    }

                                                    if (dr["LeadDocumentTypeID"].ToString() != null)
                                                    {
                                                        LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");
                                                        newExt.MyItemGovernmentProcedureType.LeadDocumentTypeID = dr["LeadDocumentTypeID"].ToString(); ;
                                                    }

                                                    extList.Add(newExt);
                                                });
                        return extList;
                        break;

                    }
                default:
                    break;
            }
            if (customResponse.TableData == null)
            {
                return null;
            }
            var ext = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
            foreach (var item in customResponse.TableData)
            {
                ext.Add(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(item));
            }
            return ext;
        }

        private static int IntFromDR(object data)
        {
            int.TryParse(data?.ToString(), out int val);
            return val;
        }

        private static bool CheckDRString(object data) =>
            data.ToString() != null;


        private static bool CheckDRBool(object data) =>
            data.ToString().Equals(true.ToString(), StringComparison.OrdinalIgnoreCase);


        private static void LogAddRow(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData newResponseTableData) =>
            LogMessagingUtil.Instance.Append(newResponseTableData.id + ",");


        private SYSTBL_NG_9001_MSG_SystemTablesResponseTableData[] TruncateNameTo(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData[] sYSTBL_NG_9001_MSG_SystemTablesResponseTableData, int iTrancateNameTo)
        {
            var list = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData>();
            sYSTBL_NG_9001_MSG_SystemTablesResponseTableData.ToList().ForEach(
                rec =>
                {
                    if (rec.name.Length >= iTrancateNameTo)
                    {
                        rec.name = rec.name.Substring(0, iTrancateNameTo);
                    }
                }
                );

            return sYSTBL_NG_9001_MSG_SystemTablesResponseTableData;
        }


        private SYSTBL_NG_9001_MSG_SystemTablesResponseTableData[] RemoveMoreThen(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData[] sYSTBL_NG_9001_MSG_SystemTablesResponseTableData, int lengthMoreThen)
        {
            var list = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData>(sYSTBL_NG_9001_MSG_SystemTablesResponseTableData);
            var delAffect = list.RemoveAll(rec => rec.id.Length > lengthMoreThen);
            return list.ToArray();
        }

        private void Update1892(SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse, SystemTableRequestParams requestParams)
        {
            throw new NotImplementedException();
        }

        public override SystemTableResponseData GetResponse(SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse, SystemTableRequestParams requestParams)
        {
            var reqData = new SystemTableResponseData() { Succeeded = true };
            if (requestParams.Pseudo)
            {
                reqData.MySystemTableResponse = (new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData>(customResponse.TableData) as object);
            }
            return reqData;
        }


        public static string UNLOCODEinternationalSiteUpSert(List<string> lines)
        {
            var sw = Stopwatch.StartNew();
            var sb = new StringBuilder();
            try
            {
                int i = 0;
                var CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                string name = "";
                string code = "";
                string country = "";
                ICustomContext MyContext = CustomContext.GetContext(1);
                var updateService = new InternationalSiteUpdateService(MyContext, new System.Collections.Generic.Dictionary<string, IContext>(), 1);
                var queryService = new InternationalSiteQueryService(MyContext);

                foreach (var line in lines)
                {
                    //Separating columns to array
                    //string[] X = CSVParser.Split(line);

                    var parts = //line.Split(',');
                        CSVParser.Split(line);
                    if (parts.Length < 5)
                    {
                        throw new System.Exception(@"(parts.Length < 12) Line no " + i + @"" + line);
                    }

                    country = parts[1];
                    if (country.Length > 3)
                    {
                        country = country.Substring(1, country.Length - 2);
                    }
                    if (parts[2].Length > 3)
                    {
                        parts[2] = parts[2].Substring(1, parts[2].Length - 2);
                    }
                     if (country.Length == 2 && parts[2].Length == 3)
                    {

                        code = country + parts[2];
                        if (parts[4].Length > 3)
                        {
                            parts[4] = parts[4].Substring(1, parts[4].Length - 2);
                        }
                        name = code + " " + parts[4];
                        if (name.Length > 40)
                        {
                            name = name.Substring(0, 40);
                        }

                        var entity = queryService.GetSingle(code, false, false);
                        if (entity != null)
                        {
                            Boolean changes = false;
                            if (entity.CountryTypeCode != country)
                            {
                                entity.CountryTypeCode = country;
                                entity.ChangeSetOp = ChangeSetOperation.Update;
                            }
                            if (entity.EnglishName != name)
                            {
                                entity.CountryTypeCode = country;
                                entity.EnglishName = name;
                                entity.LocalName = name;
                                entity.SearchFields = name;
                                entity.ChangeSetOp = ChangeSetOperation.Update;
                            }
                            if (entity.ChangeSetOp == ChangeSetOperation.Update)
                            {
                                updateService.Update(entity, true);
                            }
                        }
                        else
                        {
                            var newEntity = new InternationalSitePM();
                            newEntity.CountryTypeCode = country;
                            newEntity.ChangeSetOp = ChangeSetOperation.Insert;
                            newEntity.LocalName = name;
                            newEntity.EnglishName = name;
                            newEntity.Code = code;
                            newEntity.SearchFields = name;
                            updateService.Update(newEntity, true);
                        }

                    }
                }

            }
            catch (System.Exception eee)
            {

                sb.AppendLine(eee.ToString());
            }
            sb
                .Append("end")
                .AppendLine(sw.Elapsed.ToString());

            return sb.ToString();


        }
        public static string internationalSiteUpSert(List<string> lines)
        {
            var sw = Stopwatch.StartNew();
            const string labHeader = "ID,Name,State,Description,EnglishName,Locode,PortType,CreateMonth,IataCode,CountryID,StartDate,EndDate";
            var sb = new StringBuilder();
            try
            {
                var header = lines[0].Trim();
                if (!header.Equals(labHeader, StringComparison.OrdinalIgnoreCase))
                {
                    throw new System.Exception(@"Header not equal to labHeader
" + labHeader);

                }
                var customResponse = new SYSTBL_NG_9001_MSG_SystemTablesResponse();
                var list = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData>();
                lines.RemoveAt(0);
                int i = 0;
                var CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                string name = "";
                string state = "";
                string id = "";
                bool? bstate = null;
                foreach (var line in lines)
                {


                    //Separating columns to array
                    //string[] X = CSVParser.Split(line);

                    var parts = //line.Split(',');
                        CSVParser.Split(line);
                    if (parts.Length != 12)
                    {
                        throw new System.Exception(@"(parts.Length != 12) Line no " + i + @"
" + line);
                    }

                    name = parts[1];
                    state = parts[2];
                    id = parts[5];
                    bstate = null;
                    if (
                        string.IsNullOrWhiteSpace(name) ||
                        string.IsNullOrWhiteSpace(state) ||
                        string.IsNullOrWhiteSpace(id))
                    {
                        throw new System.Exception(@"(name,state,id is must !!) Line no " + i + @"
" + line);
                    }
                    if (name.StartsWith(id, StringComparison.OrdinalIgnoreCase))
                    {
                        name = name.Substring(id.Length);
                        name = name.Trim();

                    }


                    if (state == "0")
                    {
                        bstate = false;
                    }
                    else if (state == "1")
                    {
                        bstate = true;
                    }
                    else
                    {

                        throw new System.Exception(@"bstate = null , Line no " + i + @"
" + line);

                    }
                    //if (name.Length >= 40)
                    //{
                    //    name = name.Substring(0, 40); 
                    //}

                    var newE = new SYSTBL_NG_9001_MSG_SystemTablesResponseTableData()
                    {
                        name = name,
                        state = bstate.Value ? 1 : 0,// int.Parse(parts[2]),
                        id = id,

                    };
                    list.Add(newE);
                    i++;
                    //if (i > 10)
                    //{
                    //    break;
                    //}
                }
                sb
                .Append("Prepare")
                .AppendLine(sw.Elapsed.ToString());
                var rs = new SYSTBL_NG_9001_MSG_SystemTablesResponseService();
                customResponse.TableData = list.ToArray();
                var requestParams = new SystemTableRequestParams()
                {
                    TableId = "1344",
                    Tenant = 1,

                };
                //using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromHours(1)))
                {
                    rs.Update(customResponse, requestParams);
                    //scope.Complete();
                }

            }
            catch (System.Exception eee)
            {

                sb.AppendLine(eee.ToString());
            }
            sb
                .Append("end")
                .AppendLine(sw.Elapsed.ToString());

            return sb.ToString();


        }
        public override void OnRequestFail(SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse, SystemTableRequestParams requestParams)
        {
            CustomsClosedTableRepository closedTableRep = new CustomsClosedTableRepository(CustomContext.GetContext(0));
            CustomsClosedTable table = closedTableRep.GetSingle(new CustomsClosedTableKeys() { Id = requestParams.TableId });
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(0);

            LogMessagingUtil.Instance.AppendLine("OnUpdateFail  -  table.StatusCode = 3; ");

            table.StatusCode = "3";
            closedTableRep.Update(table);
            closedTableRep.SubmitChanges();
            base.OnRequestFail(customResponse, requestParams);
        }
    }

}
