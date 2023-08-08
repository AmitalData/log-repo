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

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UpsertSupplierInvioceByOcr_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader, GenericRequestParams>
    {
        const string label = "TABLE";

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader customResponse, GenericRequestParams requestParams)
        {


            this.MyResponseData = new INF_MSG_GenericResponseData();

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

                        foreach (var prediction in convertJson.pages[0].prediction)
                        {
                            if (prediction.label.ToUpper() != label && !dic.ContainsKey(prediction.label))
                            {
                                dic.Add(prediction.label, prediction.ocr_text);
                            }
                        }

                        List<Dictionary<string, string>> supplierInvoiceItemsList = new List<Dictionary<string, string>>();
                        Dictionary<string, string> dicItems = new Dictionary<string, string>();

                        var cells = convertJson.pages[0].prediction.Where(x => x.label.ToUpper() == label)?.First().cells;

                        int row = 1;
                        foreach (var cell in cells)
                        {
                            if (cell != null && cell.row != row && dicItems.Count > 0)
                            {
                                supplierInvoiceItemsList.Add(dicItems);
                                dicItems.Clear();
                            }
                            if (!dicItems.ContainsKey(cell.label))
                                dicItems.Add(cell.label, cell.text);
                            row = cell.row;
                        }

                        if (dicItems.Count > 0)
                        {
                            supplierInvoiceItemsList.Add(dicItems);
                        }

                        //insert or update supplierInvoice
                        try
                        {
                            UpsertSupplierInvoicebyOcr(customResponse, myOcrDocument.Reference, dic, supplierInvoiceItemsList);

                            this.MyResponseData.Succeeded = true;
                            this.MyResponseData.HasException = false;
                            this.MyResponseData.UserMessage = "חשבון יצואן עודכן בהצלחה";
                        }
                        catch (System.Exception ex)
                        {
                            this.MyResponseData.Succeeded = false;
                            this.MyResponseData.HasException = true;
                            this.MyResponseData.UserMessage = ex.Message + " : " + "  שגיאה ביצירת החשבון  ";
                            return;
                        }

                    }

                }
                catch (System.Exception ex)
                {
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = ex.Message + " : " + " לא ניתן לפתוח חשבון ממסמך זה, שגיאה בקבלת הנתונים  ";
                }


            }
            else
            {

                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;

                if (myOcrDocument == null)
                    this.MyResponseData.UserMessage = " OCR מסמך לא הוגדר כ";

                else
                {
                    string message = "לא ניתן לפתוח חשבון ממסמך זה";
                    this.MyResponseData.UserMessage =
                        string.IsNullOrEmpty(myOcrDocument.Reference) ? message + ", מספר חשבון יצואן חסר" : message + ", לא התקבל קובץ JSON";
                }

            }


        }





        public void UpsertSupplierInvoicebyOcr(DCAInUCBUpsertSupplierInvioceByOcrResponseContentHeader customResponse, string invoiceNumber, Dictionary<string, string> dic, List<Dictionary<string, string>> supplierInvoiceItemsList)
        {

            ICustomContext context = CustomContext.GetContext(customResponse.tenant);
            SupplierInvoiceQueryService supplierInvoiceQueryService = new SupplierInvoiceQueryService(customResponse.tenant);
            SupplierInvoicePM mySupplierInvoice = supplierInvoiceQueryService.GetInvoicesForDeclarationByInvoiceNum(customResponse.Declarationid, invoiceNumber, customResponse.tenant, true);

            if (mySupplierInvoice == null)
            {
                mySupplierInvoice = new SupplierInvoicePM()
                {
                    DeclarationId = customResponse.Declarationid,
                    Tenant = customResponse.tenant,
                    InvoiceNumber = invoiceNumber,
                    ChangeSetOp = ChangeSetOperation.Insert,
                };
            }
            else
            {
                mySupplierInvoice.ChangeSetOp = ChangeSetOperation.Update;
            }

            //mapping supplierInvoice from json

            if (dic.TryGetValue("buyer_name", out string buyerName))
            {
                mySupplierInvoice.BuyerName = buyerName;
            }
            else if (dic.TryGetValue("shipto_name", out string shiptoName))
            {
                mySupplierInvoice.BuyerName = shiptoName;

            }

            if (dic.TryGetValue("buyer_address", out string buyerAddress))
            {
                mySupplierInvoice.BuyerAddress = buyerAddress;
            }
            else if (dic.TryGetValue("shipto_address", out string shiptoAddress))
            {
                mySupplierInvoice.BuyerAddress = shiptoAddress;

            }
            if (dic.TryGetValue("currency", out string currency))
            {
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
            if (dic.TryGetValue("incoterrns", out string incoterrns))
            {
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
                        supplierInvoiceItemPM.ItemCode = productCode;
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
                        supplierInvoiceItemPM.OriginCountryCode = itemCountryOfOrigin;
                    }
                    if (supplierInvoiceItem.TryGetValue("Item_unit", out string ItemUnit))
                    {
                        supplierInvoiceItemPM.InvoiceQuantityType = ItemUnit;
                    }
                    //mapping supplierInvoiceItem from SupplierInvioceExportDefaults
                    supplierInvoiceItemPM.TransactionNatureCode = myInvoiceDefaults.TransactionNatureCode;
                    supplierInvoiceItemPM.ClaimReasonCode = myInvoiceDefaults.ClaimReasonCode;

                    //mapping supplierInvoiceItemProcesType from SupplierInvioceExportDefaults
                    SupplierInvoiceItemProcesTypePM supplierInvoiceItemProcesType = new SupplierInvoiceItemProcesTypePM();
                    supplierInvoiceItemProcesType.ProcessTypeCode = myInvoiceDefaults.ProcessTypeCode;
                    supplierInvoiceItemProcesType.DeclarationId = customResponse.Declarationid;
                    supplierInvoiceItemProcesType.ChangeSetOp = ChangeSetOperation.Insert;

                    supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes.Add(supplierInvoiceItemProcesType);

                    //add supplierInvoiceItem
                    mySupplierInvoice.SupplierInvoiceItems.Add(supplierInvoiceItemPM);



                }
            }


            SupplierInvoiceUpdateService supplierInvoiceUpdateService = new SupplierInvoiceUpdateService(context, new Dictionary<string, IContext>(), customResponse.tenant);

            supplierInvoiceUpdateService.Update(mySupplierInvoice, true);





        }


    }




    

}
