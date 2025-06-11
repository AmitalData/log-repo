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
using System.Text.RegularExpressions;
using Logitude.CustomsMessaging.Common.Gen;
using System.ComponentModel.DataAnnotations;

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

        public override void Update(DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Update method started.");

            ICustomContext context = CustomContext.GetContext(customResponse.tenant);
            this.MyResponseData = new UpsertSupplierInvioceByOcrResponseData();

            if (IsDeclarationSubmitted(customResponse.Declarationid, context))
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Declaration already submitted. Handling as submitted.");
                HandleSubmittedDeclaration(customResponse, requestParams, context);
                return;
            }

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Getting OCR document...");
            OcrDocument ocrDocument = GetOcrDocument(customResponse);
            if (ocrDocument == null)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("OCR document is null. Exiting.");
                return;
            }

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Cleaning OCR JSON data...");
            string cleanedJson = CleanJsonData(ocrDocument.JsonData);

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Deserializing JSON to SupplierInvoiceOcr...");
            SupplierInvoiceOcr supplierInvoiceOcr = JsonConvert.DeserializeObject<SupplierInvoiceOcr>(cleanedJson);
            if (supplierInvoiceOcr == null)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("SupplierInvoiceOcr deserialization failed. Exiting.");
                return;
            }

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Extracting fields from OCR pages...");
            Dictionary<string, string> fields = ExtractFieldsFromPages(supplierInvoiceOcr);

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Extracting items and positions from OCR pages...");
            (List<Dictionary<string, string>> itemsList, List<Dictionary<string, int>> positionsList) = ExtractItemsFromPages(supplierInvoiceOcr);

            try
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Calling UpsertSupplierInvoiceByOcr...");
                UpsertSupplierInvoiceResult result = UpsertSupplierInvoiceByOcr(customResponse, ocrDocument.Reference, fields, itemsList, positionsList);

                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Updating related entities...");
                UpdateRelatedEntities(customResponse, context, ocrDocument, result);

                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Handling success response...");
                HandleSuccessResponse(customResponse, result, context, requestParams);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Exception occurred in Update: " + ex.ToString());
                HandleFailureResponse(ex, customResponse, context, requestParams);
            }

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Update method completed.");
        }

        private bool IsDeclarationSubmitted(string declarationId, ICustomContext context)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Checking if declaration is submitted...");
            DeclarationQueryService service = new DeclarationQueryService(context);
            bool isSubmitted = service.GetSingle(declarationId, false, false)?.IsSubmitDeclaration ?? false;
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"Declaration submitted: {isSubmitted}");
            return isSubmitted;
        }

        private void HandleSubmittedDeclaration(DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader response, GenericRequestParams requestParams, ICustomContext context)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Handling submitted declaration.");
            this.MyResponseData.Succeeded = false;
            this.MyResponseData.HasException = true;
            this.MyResponseData.UserMessage = "Invoice cannot be updated, the declaration has been submitted";

            CustomsRequestsSheetQueryService sheetService = new CustomsRequestsSheetQueryService(context);
            CustomsRequestsSheetPM sheet = sheetService.GetRequestInProgress(response.tenant, "DCAOCR", ObjectTableRepository.GetObjectTableByName("Customs.Declaration"), response.Declarationid, null, null, null, false, requestParams.CustomsRequestsSheetId).FirstOrDefault();
            if (sheet != null)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Requeuing process for submitted declaration.");
                MessagingServiceFactoryHelper.ResolveAndReQueue("DCAOCR", requestParams.Tenant, sheet.Id, null, futureSendDateTime: DateTime.Now.AddMinutes(0.5));
            }
        }

        private OcrDocument GetOcrDocument(DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader response)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Getting OCR document from service.");
            OcrDocumentQueryService service = new OcrDocumentQueryService(response.tenant);
            OcrDocument document = service.GetOcrDocumentByDocumentFilingId(response.DocumentsFilingId, response.tenant);

            if (document != null && !string.IsNullOrEmpty(document.JsonData) && !string.IsNullOrEmpty(document.Reference))
            {
                CustomsDocumentQueryService customsService = new CustomsDocumentQueryService(response.tenant);
                CustomsDocumentPM customsDoc = customsService.GetSingle(document.DocId, false, false);

                if (customsDoc?.DocumentStatusCode == "7")
                {
                    throw new Exception("Customs Document Send In Progress !!!");
                }
                return document;
            }

            return null;
        }

        private string CleanJsonData(string jsonData)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Cleaning JSON data of control characters.");
            string pattern = "[\x00-\x08\x0B\x0C\x0E-\x1F]";
            return Regex.Replace(jsonData, pattern, "");
        }

        private Dictionary<string, string> ExtractFieldsFromPages(SupplierInvoiceOcr ocr)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Extracting fields from OCR pages.");
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var page in ocr.pages)
            {
                foreach (Prediction prediction in page.prediction)
                {
                    if (prediction.label.ToUpper() != label && !result.ContainsKey(prediction.label) && !string.IsNullOrEmpty(prediction.ocr_text))
                    {
                        result[prediction.label] = prediction.ocr_text;
                    }
                }
            }
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Finished extracting fields from OCR pages.");
            return result;
        }

        private (List<Dictionary<string, string>>, List<Dictionary<string, int>>) ExtractItemsFromPages(SupplierInvoiceOcr ocr)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Extracting items from OCR pages.");
            List<Dictionary<string, string>> items = new List<Dictionary<string, string>>();
            List<Dictionary<string, int>> positions = new List<Dictionary<string, int>>();

            for (int i = 0; i < ocr.pages.Count(); i++)
            {
                IEnumerable<Prediction> tables = ocr.pages[i].prediction.Where(x => x.label.ToUpper() == label);
                foreach (Prediction table in tables)
                {
                    int row = 0;
                    Dictionary<string, string> currentItem = new Dictionary<string, string>();
                    Dictionary<string, int> position = new Dictionary<string, int>();

                    foreach (var cell in table?.cells)
                    {
                        if (cell != null && cell.row != row && currentItem.Count > 0)
                        {
                            items.Add(currentItem);
                            positions.Add(position);
                            currentItem = new Dictionary<string, string>();
                            position = new Dictionary<string, int>();
                        }

                        if (!currentItem.ContainsKey(cell.label) && !string.IsNullOrEmpty(cell.text) && cell.label != ExpensesAmount && cell.label != ExpensesName)
                        {
                            currentItem[cell.label] = cell.text;
                            if (position.Count == 0)
                            {
                                position["ymin"] = cell.ymin;
                                position["ymax"] = cell.ymax;
                                position["page_no"] = table.page_no;
                            }
                        }
                        row = cell.row;
                    }

                    if (currentItem.Count > 0)
                    {
                        items.Add(currentItem);
                        positions.Add(position);
                    }
                }
            }
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Finished extracting items from OCR pages.");
            return (items, positions);
        }

        private void UpdateRelatedEntities(DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader response, ICustomContext context, OcrDocument ocrDoc, UpsertSupplierInvoiceResult result)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Updating related entities for OCR document.");
            CustomsDocumentsTicketQueryService ticketQuery = new CustomsDocumentsTicketQueryService(response.tenant);
            CustomsDocumentsTicketPM ticket = ticketQuery.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(response.Declarationid, "", "", "", response.tenant, "Declaration")
                ?.FirstOrDefault(x => x.DocumentsFilingId == response.DocumentsFilingId);

            SupplierInvoiceQueryService invoiceService = new SupplierInvoiceQueryService(response.tenant);
            var invoiceKey = invoiceService.GetInvoicesForDeclarationByInvoiceNum(response.Declarationid, ocrDoc.Reference, response.tenant, false)?[0]?.InvoiceCounterKey;

            if (ticket != null && invoiceKey != null)
            {
                ticket.ChangeSetOp = ChangeSetOperation.Update;
                foreach (var pointer in ticket.CustomsDocumentPointers)
                {
                    pointer.ChangeSetOp = ChangeSetOperation.Update;
                    pointer.Child1EntityCode = "SupplierInvoice";
                    pointer.Child1EntityId = invoiceKey.ToString();
                }

                CustomsDocumentsTicketUpdateService ticketUpdateService = new CustomsDocumentsTicketUpdateService(context, new Dictionary<string, IContext>(), response.tenant);
                ticketUpdateService.Update(ticket, true);
            }

            ocrDoc.NotConnect = true;
            OcrDocumentUpdateService ocrUpdateService = new OcrDocumentUpdateService(context, new Dictionary<string, IContext>(), response.tenant);
            OcrDocumentPM ocrPM = new OcrDocumentQueryService(response.tenant).GetEntityPM(ocrDoc, false);
            ocrPM.ChangeSetOp = ChangeSetOperation.Update;
            ocrUpdateService.Update(ocrPM, true);

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Finished updating related entities for OCR document.");
        }

        private void HandleSuccessResponse(DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader response, UpsertSupplierInvoiceResult result, ICustomContext context, GenericRequestParams requestParams)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Handling success response.");
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            string messageKey = result.isNewInvoice ? "Customs.OcrDocument.O.InvoiceSuccessfullyOpened" : "Customs.OcrDocument.O.InvoiceUpdatedSuccessfully";
            this.MyResponseData.UserMessage = TranslateTextsClass.Translate(messageKey, response.tenant, true);

            if (!string.IsNullOrWhiteSpace(result.invalidValuesRemarks))
            {
                this.MyResponseData.Remarks = "Invalid value, not exist in table - " + result.invalidValuesRemarks;
            }

            CustomsRequestsSheetQueryService sheetService = new CustomsRequestsSheetQueryService(context);
            CustomsRequestsSheetPM sheet = sheetService.GetRequestInProgress(response.tenant, "DCAOCR", ObjectTableRepository.GetObjectTableByName("Customs.Declaration"), response.Declarationid, null, null, null, false, requestParams.CustomsRequestsSheetId).FirstOrDefault();
            if (sheet != null)
            {
                MessagingServiceFactoryHelper.ResolveAndReQueue("DCAOCR", requestParams.Tenant, sheet.Id, null, futureSendDateTime: DateTime.Now.AddMinutes(0.5));
            }
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Finished handling success response.");
        }

        private void HandleFailureResponse(Exception ex, DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader response, ICustomContext context, GenericRequestParams requestParams)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Handling failure response.");
            this.MyResponseData.Succeeded = false;
            this.MyResponseData.HasException = true;
            this.MyResponseData.UserMessage = ex.Message + " : " + TranslateTextsClass.Translate("Customs.OcrDocument.O.ErrorCreatingInvoice", response.tenant, true);

            CustomsRequestsSheetQueryService sheetService = new CustomsRequestsSheetQueryService(context);
            CustomsRequestsSheetPM sheet = sheetService.GetRequestInProgress(response.tenant, "DCAOCR", ObjectTableRepository.GetObjectTableByName("Customs.Declaration"), response.Declarationid, null, null, null, false, requestParams.CustomsRequestsSheetId).FirstOrDefault();
            if (sheet != null)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Requeuing failed process.");
                MessagingServiceFactoryHelper.ResolveAndReQueue("DCAOCR", requestParams.Tenant, sheet.Id, null, futureSendDateTime: DateTime.Now.AddMinutes(0.5));
            }

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Failure response handled.");
        }

        public UpsertSupplierInvoiceResult UpsertSupplierInvoiceByOcr(DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader customResponse, string invoiceNumber, Dictionary<string, string> dic, List<Dictionary<string, string>> supplierInvoiceItemsList, List<Dictionary<string, int>> ocrPosition)
        {
            int tenant = customResponse.tenant;
            string originCountryField = "";
            ICustomContext context = CustomContext.GetContext(customResponse.tenant);
            SupplierInvoiceQueryService supplierInvoiceQueryService = new SupplierInvoiceQueryService(customResponse.tenant);
			SupplierInvioceItemCertificatUpdateService supplierInvioceItemCertificatUpdateService = new SupplierInvioceItemCertificatUpdateService(context, new Dictionary<string, IContext>(), tenant);

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
                if (buyerName.Length > 35)
                    buyerName = buyerName.Substring(0, 35);
                mySupplierInvoice.BuyerName = buyerName;
            }
            else if (dic.TryGetValue("shipto_name", out string shiptoName))
            {
                if (shiptoName.Length > 35)
                    shiptoName = shiptoName.Substring(0, 35);
                mySupplierInvoice.BuyerName = shiptoName;

            }

            if (dic.TryGetValue("buyer_address", out string buyerAddress))
            {
                if (buyerAddress.Length > 35)
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

                else
                    originCountryField = customsCountry.Code;
            }

            if (dic.TryGetValue("buyer_country", out string buyerCountry))
            {
                CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(tenant);
                CustomsCountryPM customsCountry = customsCountryQueryService.GetSingle(buyerCountry, false, true);
                if (customsCountry == null)
                    invalidValuesRemarks += $" FieldJson: buyer_country, FieldName: BuyerCountryCode, InvalidValueReceived: {buyerCountry};";
                else
                    mySupplierInvoice.BuyerCountryCode = customsCountry.Code;
            }
            else if (dic.TryGetValue("shipto_country", out string shiptoCountry))
            {
                CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(tenant);
                CustomsCountryPM customsCountry = customsCountryQueryService.GetSingle(shiptoCountry, false, true);
                if (customsCountry == null)
                    invalidValuesRemarks += $" FieldJson: shipto_country, FieldName: BuyerCountryCode, InvalidValueReceived: {shiptoCountry};";
                else
                    mySupplierInvoice.BuyerCountryCode = customsCountry.Code;

            }
            if (dic.TryGetValue("currency", out string currency))
            {
                CurrencyTypeQueryService currencyTypeQueryService = new CurrencyTypeQueryService(tenant);
                CurrencyTypePM CurrencyType = currencyTypeQueryService.GetSingle(new string(currency.Where(char.IsLetter).ToArray()), false, true);
                if (CurrencyType == null)
                    invalidValuesRemarks += $" FieldJson: currency, FieldName: InvoiceCurrencyTypeCode, InvalidValueReceived: {currency};";
                else
                    mySupplierInvoice.InvoiceCurrencyTypeCode = CurrencyType.Code;
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
                    mySupplierInvoice.IncotermCode = termsOfSaleType.Code;
            }

            //mapping more from SupplierInvioceExportDefaults

            SupplierInvioceExportDefaultQueryService supplierInvioceExportDefaultQueryService = new SupplierInvioceExportDefaultQueryService(context);
            SupplierInvioceExportDefaultPM myInvoiceDefaults = supplierInvioceExportDefaultQueryService.GetSupplierInvoiceExportDefaultByTenant(customResponse.tenant);

            if (myInvoiceDefaults != null)
            {
                mySupplierInvoice.AccountTypeCode = myInvoiceDefaults.AccountTypeCode;
                mySupplierInvoice.PartyRelationshipCode = myInvoiceDefaults.PartyRelationshipCode;
                mySupplierInvoice.BuyerRoleCode = myInvoiceDefaults.BuyerRoleCode;

                if (isNewInvoice && myInvoiceDefaults.TransactionNatureCode == "2")
                {
                    var mySupplierInvoicePayment = new SupplierInvoicePaymentPM()
                    {
                        Tenant = tenant,
                        PaymentTypeCode = "2",
                        PaymentAmount = mySupplierInvoice.InvoiceAmount ?? 0,
                        SequenceNumeric = 1,
                        ChangeSetOp = ChangeSetOperation.Insert
                    };
                    mySupplierInvoice.SupplierInvoicePayments.Add(mySupplierInvoicePayment);
                }
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
                int counter = 0;
                foreach (var supplierInvoiceItem in supplierInvoiceItemsList)
                {
                    SupplierInvoiceItemPM supplierInvoiceItemPM = new SupplierInvoiceItemPM();
                    supplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemPM.DeclarationId = customResponse.Declarationid;
                    supplierInvoiceItemPM.Tenant = customResponse.tenant;

                    if (supplierInvoiceItem.TryGetValue("Product_Code", out string productCode))
                    {
                        if (productCode.Length > 30)
                            supplierInvoiceItemPM.ItemCode = productCode.Substring(0, 30);
                        else
                            supplierInvoiceItemPM.ItemCode = productCode;
                    }
                    if (supplierInvoiceItem.TryGetValue("Quantity", out string quantity) && decimal.TryParse(quantity, out decimal invoiceQuantity))
                    {
                        supplierInvoiceItemPM.InvoiceQuantity = invoiceQuantity;
                    }
                    if (supplierInvoiceItem.TryGetValue("Description", out string description))
                    {
                        if (description.Length > 256)
                            supplierInvoiceItemPM.ItemDescription = description.Substring(0, 256);
                        else
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

                            supplierInvoiceItemPM.OriginCountryCode = CustomsCountry.Code;
                    }
                    SupplierInvoiceItemQueryService supplierInvoiceItemQueryService = new SupplierInvoiceItemQueryService(context);
                    if (supplierInvoiceItem.TryGetValue("ITEM_HS_CODE", out string itemCode))
                    {
                        itemCode = new string(itemCode.Where(char.IsDigit).ToArray());
                        if (itemCode.Length > 7)
                        {
                            var validate = supplierInvoiceItemQueryService.ValidateClassificationCode(itemCode);
                            if (validate != null)
                            {
                                supplierInvoiceItemPM.ClassificationCode = validate;
                            }
                            else
                            {
                                invalidValuesRemarks += $" FieldJson: ITEM_HS_CODE, FieldName: ClassificationCode, InvalidValueReceived: {itemCode};";
                            }
                        }
                        else
                        {
                            invalidValuesRemarks += $" FieldJson: ITEM_HS_CODE, FieldName: ClassificationCode, InvalidValueReceived: {itemCode};";
                        }
                    }
                    else if (dic.TryGetValue("HS_CODE", out string classificationCode))
                    {
                        classificationCode = new string(classificationCode.Where(char.IsDigit).ToArray());
                        if (classificationCode.Length > 7)
                        {
                            var validate = supplierInvoiceItemQueryService.ValidateClassificationCode(classificationCode);
                            if (validate != null)
                            {
                                supplierInvoiceItemPM.ClassificationCode = validate;
                            }
                            else
                            {
                                invalidValuesRemarks += $" FieldJson: HS_CODE, FieldName: ClassificationCode, InvalidValueReceived: {classificationCode};";
                            }
                        }
                        else
                        {
                            invalidValuesRemarks += $" FieldJson: HS_CODE, FieldName: ClassificationCode, InvalidValueReceived: {classificationCode};";
                        }
                    }

                    if (string.IsNullOrEmpty(supplierInvoiceItemPM.OriginCountryCode) && !string.IsNullOrEmpty(originCountryField))
                    {
                        supplierInvoiceItemPM.OriginCountryCode = originCountryField;
                    }

                    if (myInvoiceDefaults != null)
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
                        if (myInvoiceDefaults.SupplierInvItemCertificatDefs != null && myInvoiceDefaults.SupplierInvItemCertificatDefs.Count > 0) 
                        { 
                             foreach(var CerDef in myInvoiceDefaults.SupplierInvItemCertificatDefs)
                             {
                                 var supplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM()
                                 {
						     		 ChangeSetOp = ChangeSetOperation.Insert,
			                         CertificateNumber = CerDef.CertificateNumber,
			                         ReqConfirmationTypeCode = CerDef.ReqConfirmationTypeCode,
						     		 CertificateExemptionTypeCode = CerDef.CertificateExemptionTypeCode,
						     		 AttachmentTypeCode = CerDef.AttachmentTypeCode,
			                         ResConfirmationTypeCode = CerDef.ResConfirmationTypeCode,
			                         CustomsAttachmentID = CerDef.CustomsAttachmentID,
			                         SequenceNumeric = CerDef.SequenceNumeric,
                             
						     	};
								supplierInvoiceItemPM.CertificatesStatusCode = supplierInvioceItemCertificatUpdateService.UpdateCertificateStatus(supplierInvioceItemCertificatPM, tenant, true);
								supplierInvoiceItemPM.SupplierInvioceItemCertificats.Add(supplierInvioceItemCertificatPM);
						     }
						}
					}
                    // update ocr column position: page_no, ymin, ymax - #94509
                    if (ocrPosition.Count > counter)
                    {
                        if (ocrPosition[counter].TryGetValue("page_no", out int ocrPageNumber))
                        {
                            supplierInvoiceItemPM.OcrPageNumber = ocrPageNumber + 1;
                        }
                        if (ocrPosition[counter].TryGetValue("ymin", out int ymin))
                        {
                            supplierInvoiceItemPM.OcrTop = ymin; ;
                            if (ocrPosition[counter].TryGetValue("ymax", out int ymax))
                            {
                                supplierInvoiceItemPM.OcrHeight = ymax - ymin;
                            }
                        }
                    }

                    //add supplierInvoiceItem
                    mySupplierInvoice.SupplierInvoiceItems.Add(supplierInvoiceItemPM);
                    counter++;
                }
            }


            SupplierInvoiceUpdateService supplierInvoiceUpdateService = new SupplierInvoiceUpdateService(context, new Dictionary<string, IContext>(), customResponse.tenant);

            supplierInvoiceUpdateService.Update(mySupplierInvoice, true);

            UpsertSupplierInvoiceResult upsertSupplierInvoiceResult = new UpsertSupplierInvoiceResult()
            {
                isNewInvoice = isNewInvoice,
                invalidValuesRemarks = invalidValuesRemarks
            };
            return upsertSupplierInvoiceResult;



        }

        private string ValidateClassificationCode(string classificationCode)
        {
            string newValue = classificationCode;

            if (newValue.Length == 8)
            {
                newValue += "00";
                newValue += LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            }
            else if (newValue.Length == 9)
            {
                newValue = newValue.Substring(0, 8) + "00" + newValue.Substring(8);
                newValue += LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.Substring(0, 10));
            }
            else if (newValue.Length == 10)
            {
                newValue += LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            }
            else if (newValue.Length == 11)
            {
                newValue += LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.Substring(0, 10));
            }

            return newValue;
        }



        public class UpsertSupplierInvoiceResult
        {
            public bool isNewInvoice { get; set; }
            public string invalidValuesRemarks { get; set; }
        }


    }






}
