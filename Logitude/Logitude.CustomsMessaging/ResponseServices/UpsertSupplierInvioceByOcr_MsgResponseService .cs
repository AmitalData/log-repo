using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.Repsitories;
using Microsoft.Practices.Unity;
using Logitude.CustomsMessaging.Utils;
using Logitude.Customs.Data.EntityPOCOs;
using System.IO;
using System.Net;
using Logitude.Server.Tools.BlobServiceReference;
using System.IO.Compression;
using System.Net.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Newtonsoft.Json;
using Logitude.Server.Tools.StorageService;
using DocumentFormat.OpenXml.Office2019.Excel.RichData2;
using System.Security.Policy;
using Logitude.Customs.Data.Repsitories;
using UnifreightIIG.Common.MessageLib.Unifreight.Customs;
using DocumentFormat.OpenXml.Wordprocessing;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityKeys;
using static Logitude.Customs.BL.Messaging.Customs.SupplierInvoiceByOcr;
using static Logitude.CustomsMessaging.ResponseServices.UpsertSupplierInvioceByOcr_MsgResponseService;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UpsertSupplierInvioceByOcr_MsgResponseService : ResponseServiceBase<UpsertSupplierInvioceByOcrResponseData, DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader, GenericRequestParams>
    {
        const string label = "TABLE";
        const string ExpensesAmount = "Expenses_amount";
        const string ExpensesName = "Expenses_name";

        public override UpsertSupplierInvioceByOcrResponseData GetResponse(DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader customResponse, GenericRequestParams requestParams){

            ICustomContext context = CustomContext.GetContext(customResponse.tenant);

            this.MyResponseData = new UpsertSupplierInvioceByOcrResponseData();

            OcrDocumentQueryService ocrDocumentService = new OcrDocumentQueryService(customResponse.tenant);

            var myOcrDocument = ocrDocumentService.GetOcrDocumentByDocumentFilingId(customResponse.DocumentsFilingId, customResponse.tenant);
            if (myOcrDocument != null && !string.IsNullOrEmpty(myOcrDocument.JsonData) && !string.IsNullOrEmpty(myOcrDocument.Reference))
            {
                try
                {
                    SupplierInvoiceOcr convertJson = JsonConvert.DeserializeObject<SupplierInvoiceOcr>(myOcrDocument.JsonData);//json מיפוי

                    if (convertJson != null)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();

                        foreach (var page in convertJson.pages)
                        {
                            foreach (var prediction in page.prediction)
                            {
                                if (prediction.label.ToUpper() != label && !dic.ContainsKey(prediction.label))
                                {
                                    dic.Add(prediction.label, prediction.ocr_text);
                                }
                            }
                        }

                        List<Dictionary<string, string>> supplierInvoiceItemsList = new List<Dictionary<string, string>>();
                        Dictionary<string, string> dicItems = new Dictionary<string, string>();
                        for(int i = 0; i < convertJson.pages.Count(); i++)
                        {
                            var tables = convertJson.pages[i].prediction.Where(x => x.label.ToUpper() == label);
                            if (tables.Any())
                            {
                                foreach(var table in tables)
                                {  
                                    int row = 0;
                                    dicItems = new Dictionary<string, string>();
                                    foreach (var cell in table?.cells)
                                    {
                                        if (cell != null && cell.row != row && dicItems.Count > 0)
                                        {
                                            supplierInvoiceItemsList.Add(dicItems);
                                            dicItems = new Dictionary<string, string>();
                                        }
                                        if (!dicItems.ContainsKey(cell.label) && cell.label != ExpensesAmount && cell.label != ExpensesName)
                                            dicItems.Add(cell.label, cell.text);
                                        row = cell.row;
                                    }

                                    if (dicItems.Count > 0)
                                    {
                                        supplierInvoiceItemsList.Add(dicItems);
                                    }

                                }


                            }
                            
                        }
                        

                        //insert or update supplierInvoice
                        try
                        {
                            UpsertSupplierInvoiceResult Result = UpsertSupplierInvoiceByOcr(customResponse, myOcrDocument.Reference, dic, supplierInvoiceItemsList);
                            
                            if(Result.isNewInvoice)// update CustomsDocumentPointer
                            {
                                CustomsDocumentsTicketQueryService customsDocumentsTicketQuery = new CustomsDocumentsTicketQueryService(customResponse.tenant);
                                CustomsDocumentsTicketPM customsDocumentsTicketPM = customsDocumentsTicketQuery.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(customResponse.Declarationid, "", "", "", customResponse.tenant, "Declaration")
                                    ?.Where(x => x.DocumentsFilingId == customResponse.DocumentsFilingId)?.FirstOrDefault();
                                CustomsDocumentsTicketUpdateService customsDocumentsTicketUpdateService = new CustomsDocumentsTicketUpdateService(context, new Dictionary<string, IContext>(), customResponse.tenant);
                                
                                SupplierInvoiceQueryService supplierInvoiceQueryService = new SupplierInvoiceQueryService(customResponse.tenant);
                                var invoiceCounterKey = supplierInvoiceQueryService.GetInvoicesForDeclarationByInvoiceNum(customResponse.Declarationid, myOcrDocument.Reference, customResponse.tenant, false)?[0]?.InvoiceCounterKey;
                                if (customsDocumentsTicketPM != null && invoiceCounterKey != null)
                                {
                                    customsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Update;
                                    foreach (var CustomsDocumentPointer in customsDocumentsTicketPM.CustomsDocumentPointers)
                                    {
                                        CustomsDocumentPointer.ChangeSetOp = ChangeSetOperation.Update;
                                        CustomsDocumentPointer.Child1EntityCode = "SupplierInvoice";
                                        CustomsDocumentPointer.Child1EntityId = invoiceCounterKey.ToString();
                                    }
                                customsDocumentsTicketUpdateService.Update(customsDocumentsTicketPM, true);
                                }
                                
                            }
                            myOcrDocument.NotConnect = true;
                            OcrDocumentUpdateService ocrDocumentUpdateService = new OcrDocumentUpdateService(context, new Dictionary<string, IContext>(), customResponse.tenant);      
                            OcrDocumentPM myOcrDocumentPM = ocrDocumentService.GetEntityPM(myOcrDocument, false);
                            myOcrDocumentPM.ChangeSetOp = ChangeSetOperation.Update;
                            ocrDocumentUpdateService.Update(myOcrDocumentPM, true);

                            this.MyResponseData.Succeeded = true;
                            this.MyResponseData.HasException = false;
                            string InvoiceSuccess = Result.isNewInvoice ? "Customs.OcrDocument.O.InvoiceSuccessfullyOpened" : "Customs.OcrDocument.O.InvoiceUpdatedSuccessfully";                            this.MyResponseData.UserMessage =
                            this.MyResponseData.UserMessage = TranslateTextsClass.Translate(InvoiceSuccess, customResponse.tenant, true);
                            if (Result.invalidValuesRemarks != null)
                                this.MyResponseData.Remarks = "Invalid value, not exist in table - " + Result.invalidValuesRemarks;
                        }
                        catch (System.Exception ex)
                        {
                            this.MyResponseData.Succeeded = false;
                            this.MyResponseData.HasException = true;
                            this.MyResponseData.UserMessage = ex.Message + " : " + " "+ TranslateTextsClass.Translate("Customs.OcrDocument.O.ErrorCreatingInvoice", customResponse.tenant, true) +" ";
                            return;
                        }

                    }

                }
                catch (System.Exception ex)
                {
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = ex.Message + " : " + " "+ TranslateTextsClass.Translate("Customs.OcrDocument.O.ErrorInReceivingData", customResponse.tenant, true) + " ";
                }


            }
            else
            {

                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;

                if (myOcrDocument == null)
                    this.MyResponseData.UserMessage = TranslateTextsClass.Translate("Customs.OcrDocument.O.IsNotOcrDocument", customResponse.tenant, true);

                else
                {
                    string message = TranslateTextsClass.Translate("Customs.OcrDocument.O.CannotOpenInvoice", customResponse.tenant, true);
                    this.MyResponseData.UserMessage =
                        string.IsNullOrEmpty(myOcrDocument.Reference) ? message + "," + TranslateTextsClass.Translate("Customs.OcrDocument.O.MissingInvoiceNumber", customResponse.tenant, true)
                        : message + "," + TranslateTextsClass.Translate("Customs.OcrDocument.O.JSONFileNotReceived", customResponse.tenant, true);
                }
            }
        }





        public UpsertSupplierInvoiceResult UpsertSupplierInvoiceByOcr(DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader customResponse, string invoiceNumber, Dictionary<string, string> dic, List<Dictionary<string, string>> supplierInvoiceItemsList)
        {
            int tenant = customResponse.tenant;
            ICustomContext context = CustomContext.GetContext(customResponse.tenant);
            SupplierInvoiceQueryService supplierInvoiceQueryService = new SupplierInvoiceQueryService(customResponse.tenant);
            List<SupplierInvoicePM> mySupplierInvoices = supplierInvoiceQueryService.GetInvoicesForDeclarationByInvoiceNum(customResponse.Declarationid, invoiceNumber, customResponse.tenant, true);
            SupplierInvoicePM mySupplierInvoice = 
                                    mySupplierInvoices.FirstOrDefault(x => x.InvoiceNumber == invoiceNumber)
                                    ?? mySupplierInvoices.FirstOrDefault(x => x.InvoiceNumber == null)
                                    ?? null;
            bool isNewInvoice = false;
            string invalidValuesRemarks = null;
             if (mySupplierInvoice == null)
            {
                isNewInvoice = true;
                mySupplierInvoice = new SupplierInvoicePM()
                {
                    DeclarationId = customResponse.Declarationid,
                    Tenant = tenant,
                    InvoiceNumber = invoiceNumber,
                    ChangeSetOp = ChangeSetOperation.Insert,
                };
            }
            else
            {
                mySupplierInvoice.ChangeSetOp = ChangeSetOperation.Update;
                if (string.IsNullOrEmpty(mySupplierInvoice.InvoiceNumber))
                {
                    mySupplierInvoice.InvoiceNumber = invoiceNumber;
                    isNewInvoice = true;
                }
               
            }

            //mapping supplierInvoice from json

            if (dic.TryGetValue("buyer_name", out string buyerName))
            {
                if(buyerName.Length > 35)
                    buyerName = buyerName.Substring(0, 35);
                mySupplierInvoice.BuyerName = buyerName;
            }
            else if (dic.TryGetValue("shipto_name", out string shiptoName))
            {
                if(shiptoName.Length > 35)
                    shiptoName = shiptoName.Substring(0, 35);
                mySupplierInvoice.BuyerName = shiptoName;

            }

            if (dic.TryGetValue("buyer_address", out string buyerAddress))
            {
                if(buyerAddress.Length > 35)
                    buyerAddress = buyerAddress.Substring(0, 35);
                mySupplierInvoice.BuyerAddress = buyerAddress;
            }
            else if (dic.TryGetValue("shipto_address", out string shiptoAddress))
            {
                if (shiptoAddress.Length > 35)
                    shiptoAddress = shiptoAddress.Substring(0, 35);
                mySupplierInvoice.BuyerAddress = shiptoAddress;

            }

            if (dic.TryGetValue("country_of_origin", out string originCountry))
            {
                CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(tenant);
                CustomsCountryPM customsCountry = customsCountryQueryService.GetSingle(originCountry, false, true);
                if (customsCountry == null)
                    invalidValuesRemarks += $" FieldJson: country_of_origin, FieldName: originCountry, InvalidValueReceived: {originCountry};";
              
            }

            if (dic.TryGetValue("buyer_country", out string buyerCountry))
            {
                CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(tenant);
                CustomsCountryPM customsCountry = customsCountryQueryService.GetSingle(buyerCountry, false, true);
                if (customsCountry == null)
                    invalidValuesRemarks += $" FieldJson: buyer_country, FieldName: BuyerCountryCode, InvalidValueReceived: {buyerCountry};";
                else
                    mySupplierInvoice.BuyerCountryCode = buyerCountry;
            }
            else if (dic.TryGetValue("shipto_country", out string shiptoCountry))
            {
                CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(tenant);
                CustomsCountryPM customsCountry = customsCountryQueryService.GetSingle(shiptoCountry, false, true);
                if (customsCountry == null)
                    invalidValuesRemarks += $" FieldJson: shipto_country, FieldName: BuyerCountryCode, InvalidValueReceived: {shiptoCountry};";
                else
                    mySupplierInvoice.BuyerCountryCode = shiptoCountry;

            }
            if (dic.TryGetValue("currency", out string currency))
            {
                CurrencyTypeQueryService currencyTypeQueryService = new CurrencyTypeQueryService(tenant);
                CurrencyTypePM CurrencyType = currencyTypeQueryService.GetSingle(new string(currency.Where(char.IsLetter).ToArray()), false, true);
                if (CurrencyType == null)
                    invalidValuesRemarks += $" FieldJson: currency, FieldName: InvoiceCurrencyTypeCode, InvalidValueReceived: {currency};";
                else
                    mySupplierInvoice.InvoiceCurrencyTypeCode = new string(currency.Where(char.IsLetter).ToArray());
            }
            if (dic.TryGetValue("invoice_amount", out string invoiceAmount) && decimal.TryParse(invoiceAmount, out decimal amount))
            {
                mySupplierInvoice.InvoiceAmount = amount;
            }
            if (dic.TryGetValue("invoice_date", out string invoiceDate) && DateTime.TryParse(invoiceDate, out DateTime date))
            {
                mySupplierInvoice.IssueDate = date;
            }
            if (dic.TryGetValue("incoterms", out string incoterrns))
            {
                TermsOfSaleTypeQueryService termsOfSaleTypeQueryService = new TermsOfSaleTypeQueryService(tenant);
                TermsOfSaleTypePM termsOfSaleType = termsOfSaleTypeQueryService.GetSingle(incoterrns, false, true);
                if (termsOfSaleType == null)
                    invalidValuesRemarks += $" FieldJson: incoterrns, FieldName: IncotermCode, InvalidValueReceived: {incoterrns};";
                else
                    mySupplierInvoice.IncotermCode = incoterrns;
            }

            //mapping more from SupplierInvioceExportDefaults

            SupplierInvioceExportDefaultQueryService supplierInvioceExportDefaultQueryService = new SupplierInvioceExportDefaultQueryService(context);
            SupplierInvioceExportDefault myInvoiceDefaults = supplierInvioceExportDefaultQueryService.GetSupplierInvoiceExportDefaultByTenant(customResponse.tenant);

            if (myInvoiceDefaults != null)
            {
                mySupplierInvoice.AccountTypeCode = myInvoiceDefaults.AccountTypeCode;
                mySupplierInvoice.PartyRelationshipCode = myInvoiceDefaults.PartyRelationshipCode;
                mySupplierInvoice.BuyerRoleCode = myInvoiceDefaults.BuyerRoleCode;
            }


            if (mySupplierInvoice.SupplierInvoiceItems.Count > 0)
            {
                var mySupplierInvoiceUpdateService = new SupplierInvoiceUpdateService(context, new Dictionary<string, IContext>(), customResponse.tenant);

                mySupplierInvoiceUpdateService.DeclarationSupplierInvoiceItemsParentsFastDelete(mySupplierInvoice, context, false);

                mySupplierInvoice.SupplierInvoiceItems = null;

                mySupplierInvoice.InvoiceItemLastLineNumber = 0;


            }


            //mapping supplierInvoiceItem from json

            if (supplierInvoiceItemsList.Count > 0)
            {
                foreach (var supplierInvoiceItem in supplierInvoiceItemsList)
                {
                    SupplierInvoiceItemPM supplierInvoiceItemPM = new SupplierInvoiceItemPM();
                    supplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemPM.DeclarationId = customResponse.Declarationid;
                    supplierInvoiceItemPM.Tenant = customResponse.tenant;

                    if (supplierInvoiceItem.TryGetValue("Product_Code", out string productCode))
                    {
                        if(productCode.Length>30)
                        supplierInvoiceItemPM.ItemCode = productCode.Substring(0,30);
                    }
                    if (supplierInvoiceItem.TryGetValue("Quantity", out string quantity) && decimal.TryParse(quantity, out decimal invoiceQuantity))
                    {
                        supplierInvoiceItemPM.InvoiceQuantity = invoiceQuantity;
                    }
                    if (supplierInvoiceItem.TryGetValue("Description", out string description))
                    {
                        supplierInvoiceItemPM.ItemDescription = description;
                    }
                    if (supplierInvoiceItem.TryGetValue("Line_Amount_after_discount", out string lineAmountAfterDiscount) && decimal.TryParse(lineAmountAfterDiscount, out decimal lineAmount))
                    {
                        supplierInvoiceItemPM.ItemPrice = lineAmount;
                    }
                    else if (supplierInvoiceItem.TryGetValue("Line_Amount", out string itemAmount) && decimal.TryParse(itemAmount, out decimal itemPrice))
                    {
                        supplierInvoiceItemPM.ItemPrice = itemPrice;
                    }
                    if (supplierInvoiceItem.TryGetValue("Item_country_of_origin", out string itemCountryOfOrigin))
                    {
                        CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(tenant);
                        CustomsCountryPM CustomsCountry = customsCountryQueryService.GetSingle(itemCountryOfOrigin, false, true);
                        if (CustomsCountry == null)
                            invalidValuesRemarks += $" FieldJson: Item_country_of_origin, FieldName: OriginCountryCode, InvalidValueReceived: {itemCountryOfOrigin};";
                        else
                            supplierInvoiceItemPM.OriginCountryCode = itemCountryOfOrigin;
                        if (string.IsNullOrEmpty(supplierInvoiceItemPM.OriginCountryCode))
                        {
                            supplierInvoiceItemPM.OriginCountryCode = originCountry;
                        }
                    }
                    if (supplierInvoiceItem.TryGetValue("Item_unit", out string ItemUnit))
                    {
                        MeasurmentUnitQueryService measurmentUnitQueryService = new MeasurmentUnitQueryService(tenant);
                        MeasurmentUnitPM MeasurmentUnit = measurmentUnitQueryService.GetSingle(ItemUnit, false, true);
                        if (MeasurmentUnit == null)
                            invalidValuesRemarks += $" FieldJson: Item_unit, FieldName: InvoiceQuantityType, InvalidValueReceived: {ItemUnit};";
                        else
                            supplierInvoiceItemPM.InvoiceQuantityType = ItemUnit;
                    }
                    if(myInvoiceDefaults != null)
                    {
                        //mapping supplierInvoiceItem from SupplierInvioceExportDefaults
                        supplierInvoiceItemPM.TransactionNatureCode = myInvoiceDefaults.TransactionNatureCode;
                        supplierInvoiceItemPM.ClaimReasonCode = myInvoiceDefaults.ClaimReasonCode;

                        //mapping supplierInvoiceItemProcesType from SupplierInvioceExportDefaults
                        SupplierInvoiceItemProcesTypePM supplierInvoiceItemProcesType = new SupplierInvoiceItemProcesTypePM();
                        supplierInvoiceItemProcesType.ProcessTypeCode = myInvoiceDefaults.ProcessTypeCode;
                        supplierInvoiceItemProcesType.DeclarationId = customResponse.Declarationid;
                        supplierInvoiceItemProcesType.ChangeSetOp = ChangeSetOperation.Insert;

                        supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes.Add(supplierInvoiceItemProcesType);

                        if (!string.IsNullOrEmpty(supplierInvoiceItemProcesType.ProcessTypeCode))
                        {
                            supplierInvoiceItemPM.ItemAdditionalStatus = true;
                        }
                    }
                    

                    //add supplierInvoiceItem
                    mySupplierInvoice.SupplierInvoiceItems.Add(supplierInvoiceItemPM);



                }
            }


            SupplierInvoiceUpdateService supplierInvoiceUpdateService = new SupplierInvoiceUpdateService(context, new Dictionary<string, IContext>(), customResponse.tenant);

            supplierInvoiceUpdateService.Update(mySupplierInvoice, true);

            UpsertSupplierInvoiceResult upsertSupplierInvoiceResult =  new UpsertSupplierInvoiceResult() 
            { 
                isNewInvoice = isNewInvoice, 
                invalidValuesRemarks = invalidValuesRemarks 
            };             
            return upsertSupplierInvoiceResult;



        }
        public class UpsertSupplierInvoiceResult
        {
            public bool isNewInvoice { get; set; }
            public string invalidValuesRemarks { get; set; }
        }


    }




    

}
