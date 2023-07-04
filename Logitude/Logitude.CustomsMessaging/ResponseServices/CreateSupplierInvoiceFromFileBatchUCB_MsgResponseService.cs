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
using Logitude.Customs.Data.EntityListQueryServices;
using System.Text.RegularExpressions;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CreateSupplierInvoiceFromFileBatchUCB_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBCreateSupplierInvoiceWithResponseContentHeader, GenericRequestParams>
    {
        private List<InvoiceFromFile> fromFile = new List<InvoiceFromFile>();
        private AmitalContext amitalContext;

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBCreateSupplierInvoiceWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBCreateSupplierInvoiceWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            amitalContext = AmitalContext.GetContext(requestParams.Tenant);

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            if (customResponse.Declarationid != null)
            {
                LogMessagingUtil.Instance.AppendLine("customResponse.Declarationid: " + customResponse.Declarationid);

                ReadDataFromCsvFile(customResponse.decodedString);
                UpdateDB(customResponse.Declarationid, customResponse.PartnerId, requestParams.Tenant);

                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyResponseData.Succeeded = true;
            }
        }
        private void UpdateDB(string declarationid, string partnerId, int tenant)
        {
            var context = CustomContext.GetContext(tenant);
            var declarationQueryService = new DeclarationQueryService(context);
            DeclarationPM declarationPM = declarationQueryService.GetSingle(declarationid, true, false);
            MyRequestSheetParam.RequestDescription = $"{declarationPM.DeclarationNumber} קליטת חשבון ספק מקובץ, הצהרה";

            foreach (var invoiceFromFile in fromFile)
            {
                var invoiceFromDB = declarationPM.SupplierInvoices.FirstOrDefault(x => x.InvoiceNumber == invoiceFromFile.InvoiceNumber);
                if (invoiceFromDB == null)
                {
                    // create new invoice
                    var invoice = new SupplierInvoicePM 
                    {
                        InvoiceNumber = invoiceFromFile.InvoiceNumber,
                        DeclarationId = declarationid,
                        Tenant = tenant,
                        IssueDate = invoiceFromFile.IssueDate,
                        
                    };
                    if (invoiceFromFile.FreightAmount != null && !string.IsNullOrWhiteSpace(invoiceFromFile.FreightAmountCurrencyType))
                    {
                        invoice.SupplierInvoiceFreightAmounts = new List<SupplierInvoiceFreightAmountPM>();
                        var freightAmount =
                                new SupplierInvoiceFreightAmountPM
                                {
                                    DeclarationId = declarationid,
                                    Tenant = tenant,
                                    ChangeSetOp = ChangeSetOperation.Insert,
                                    Amount = invoiceFromFile.FreightAmount,
                                };
                        if (!string.IsNullOrWhiteSpace(invoiceFromFile.FreightAmountCurrencyType))
                        {
                            var _currencyType = GetTranslationL2P(partnerId, "CTBCURRENCY", invoiceFromFile.FreightAmountCurrencyType);
                            if (!string.IsNullOrWhiteSpace(_currencyType))
                            {
                                var isSuccess = SetFreightAmountCurrencyTypeCode(tenant, _currencyType, freightAmount);
                                if (!isSuccess)
                                {
                                    isSuccess = SetFreightAmountCurrencyTypeCode(tenant, invoiceFromFile.FreightAmountCurrencyType, freightAmount);
                                    if (!isSuccess)
                                    {
                                        LogMessagingUtil.Instance.AppendLine("FreightAmountCurrencyType = " + invoiceFromFile.FreightAmountCurrencyType + " could not translate to Logitude Id");
                                    }
                                }
                            }
                            else
                            {
                                var isSuccess = SetFreightAmountCurrencyTypeCode(tenant, invoiceFromFile.FreightAmountCurrencyType, freightAmount);
                                if (!isSuccess)
                                {
                                    LogMessagingUtil.Instance.AppendLine("FreightAmountCurrencyType = " + invoiceFromFile.FreightAmountCurrencyType + " could not translate to Logitude Id");
                                }
                            }
                        }
                        invoice.SupplierInvoiceFreightAmounts.Add(freightAmount);
                    }
                    if (!string.IsNullOrWhiteSpace(invoiceFromFile.InvoiceCurrencyTypeCode))
                    {
                        var _currencyType = GetTranslationL2P(partnerId, "CTBCURRENCY", invoiceFromFile.InvoiceCurrencyTypeCode);
                        if (!string.IsNullOrWhiteSpace(_currencyType))
                        {
                            var isSuccess = SetCurrencyTypeCode(tenant, _currencyType, invoice);
                            if (!isSuccess)
                            {
                                isSuccess = SetCurrencyTypeCode(tenant, invoiceFromFile.InvoiceCurrencyTypeCode, invoice);
                                if (!isSuccess)
                                {
                                    LogMessagingUtil.Instance.AppendLine("InvoiceCurrencyTypeCode = " + invoiceFromFile.InvoiceCurrencyTypeCode + " could not translate to Logitude Id");
                                }
                            }
                        }
                        else
                        {
                            var isSuccess = SetCurrencyTypeCode(tenant, invoiceFromFile.InvoiceCurrencyTypeCode, invoice);
                            if (!isSuccess)
                            {
                                LogMessagingUtil.Instance.AppendLine("InvoiceCurrencyTypeCode = " + invoiceFromFile.InvoiceCurrencyTypeCode + " could not translate to Logitude Id");
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(invoiceFromFile.Incoterm))
                    {
                        var _incoterm = GetTranslationL2P(partnerId, "CTBUINCOTERMS", invoiceFromFile.Incoterm);
                        if (!string.IsNullOrWhiteSpace(_incoterm))
                        {
                            var isSuccess = SetIncoterm(tenant, _incoterm, invoice);
                            if (!isSuccess)
                            {
                                isSuccess = SetIncoterm(tenant, invoiceFromFile.Incoterm, invoice);
                                if (!isSuccess)
                                {
                                    LogMessagingUtil.Instance.AppendLine("Incoterm = " + invoiceFromFile.Incoterm + " could not translate to Logitude Id");
                                }
                            }
                        }
                        else
                        {
                            var isSuccess = SetIncoterm(tenant, invoiceFromFile.Incoterm, invoice);
                            if (!isSuccess)
                            {
                                LogMessagingUtil.Instance.AppendLine("Incoterm = " + invoiceFromFile.Incoterm + " could not translate to Logitude Id");
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(invoiceFromFile.VendorId))
                    {
                        var _vendor = GetTranslationL2P(partnerId, "CTBCUSTSUP", invoiceFromFile.VendorId);
                        if (!string.IsNullOrWhiteSpace(_vendor))
                        {
                            var isSuccess = SetVendor(tenant, _vendor, invoice);
                            if(!isSuccess)
                            {
                                isSuccess = SetVendor(tenant, invoiceFromFile.VendorId, invoice);
                                if (!isSuccess)
                                {
                                    LogMessagingUtil.Instance.AppendLine("VendorId = " + invoiceFromFile.VendorId + " could not translate to Logitude Id");
                                }
                            }
                        }
                        else
                        {
                            var isSuccess = SetVendor(tenant, invoiceFromFile.VendorId, invoice);
                            if (!isSuccess)
                            {
                                LogMessagingUtil.Instance.AppendLine("VendorId = " + invoiceFromFile.VendorId + " could not translate to Logitude Id");
                            }
                        }
                    }

                    invoice.ChangeSetOp = ChangeSetOperation.Insert;
                    invoice.SupplierInvoiceItems = new List<SupplierInvoiceItemPM>();
                    CreateSupplierInvoiceItems(invoice, tenant, declarationid, invoiceFromFile, partnerId, declarationPM.CustomerCode);

                    declarationPM.SupplierInvoices.Add(invoice);
                }
                else
                {
                    //create invoiceitems
                    /*CreateSupplierInvoiceItems(invoiceFromDB, tenant, declarationid, invoiceFromFile, partnerId);
                    invoiceFromDB.ChangeSetOp = ChangeSetOperation.Update;*/
                    LogMessagingUtil.Instance.AppendLine("***INVOICE "+ invoiceFromFile.InvoiceNumber + " ALREADY EXISTS, NOT CREATE");
                }
            }
            declarationPM.ChangeSetOp = ChangeSetOperation.Update;
            try
            {
                LogMessagingUtil.Instance.AppendLine("===Start Saving declaration to DB===");
                var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);
                myDeclarationUpdateService.Update(declarationPM, true);
                LogMessagingUtil.Instance.AppendLine("===End Saving declaration to DB===");
            }
            catch (Exception e)
            {
                LogMessagingUtil.Instance.AppendLine("error in Saving declaration to DB: "+ e.ToString());
                throw;
            }
        }

        private void CreateSupplierInvoiceItems(SupplierInvoicePM invoice, int tenant, string declarationid, InvoiceFromFile invoiceFromFile, string partnerId, string customerId)
        {
            foreach (var invoiceItemFromFile in invoiceFromFile.SupplierInvoiceItems)
            {
                var invoiceItem = new SupplierInvoiceItemPM
                {
                    DeclarationId = declarationid,
                    Tenant = tenant,
                    ItemPriceCurrencyCode = invoice.InvoiceCurrencyTypeCode,
                    ItemPrice = invoiceItemFromFile.ItemPrice,
                    TradeAgreementCode = invoiceItemFromFile.TradeAgreemenCode == "" ? null : invoiceItemFromFile.TradeAgreemenCode,
                    InvoiceQuantity = 1,
                    InvoiceNumber = invoiceFromFile.InvoiceNumber,
                    ItemDescription = invoiceItemFromFile.ItemDescription,
                    ItemCode = invoiceItemFromFile.ItemDescription,
                    //ClassificationCode = "84253990000"//todo
                };
                CTBCARMODRepository re = new CTBCARMODRepository(amitalContext);
                var classificationCode = re.GetSingle(customerId, invoiceItemFromFile.ClassificationCode)?.PRAT;
                //var classificationCode = GetTranslationL2P(partnerId, "CTBCARMOD", invoiceItemFromFile.ClassificationCode);
                if (!string.IsNullOrWhiteSpace(classificationCode))
                {
                    invoiceItem.ClassificationCode = classificationCode;
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("classificationCode = " + invoiceItemFromFile.ClassificationCode + " not exists for customer: "+ customerId);
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.UserMessage = "לא נמצא סיווג עבור הדגם: " + invoiceItemFromFile.ClassificationCode + " והלקוח בתיק";
                    //return;
                    throw new Exception("לא נמצא סיווג עבור הדגם: " + invoiceItemFromFile.ClassificationCode + " והלקוח בתיק");
                }
                invoiceItem.SupplierInvoiceItemVehicles = new List<SupplierInvoiceItemVehiclePM>
                        {
                            new SupplierInvoiceItemVehiclePM
                            {
                                RichbitFileNumber = invoiceItemFromFile.RichbitFileNumber,
                                VehicleTypeCode = "ZZZ",
                                VehicleChassisNumber = invoiceItemFromFile.VehicleChassisNumber,
                                DeclarationId = declarationid,
                                Tenant = tenant,
                                ChangeSetOp = ChangeSetOperation.Insert,
                            }
                        };
                CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(tenant);
                invoiceItem.InvoiceQuantityType = customsItemQueryService.GetQuantityTypeByClassificationCode(invoiceItem.ClassificationCode, tenant);

                if (!string.IsNullOrWhiteSpace(invoiceItemFromFile.TradeAgreemenCode))
                {
                    var _TradeAgreemenCode = GetTranslationL2P(partnerId, "CTBTARIFF", invoiceItemFromFile.TradeAgreemenCode);
                    if (!string.IsNullOrWhiteSpace(_TradeAgreemenCode))
                    {
                        var isSuccess = SetTradeAgreemenCode(tenant, _TradeAgreemenCode, invoiceItem);
                        if (!isSuccess)
                        {
                            isSuccess = SetTradeAgreemenCode(tenant, invoiceItemFromFile.TradeAgreemenCode, invoiceItem);
                            if (!isSuccess)
                            {
                                LogMessagingUtil.Instance.AppendLine("TradeAgreemenCode = " + invoiceItemFromFile.TradeAgreemenCode + " could not translate to Logitude Id");
                            }
                        }
                    }
                    else
                    {
                        var isSuccess = SetTradeAgreemenCode(tenant, invoiceItemFromFile.TradeAgreemenCode, invoiceItem);
                        if (!isSuccess)
                        {
                            LogMessagingUtil.Instance.AppendLine("TradeAgreemenCode = " + invoiceItemFromFile.TradeAgreemenCode + " could not translate to Logitude Id");
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(invoiceItemFromFile.OriginCountryCode))
                {
                    var _Country = GetTranslationL2P(partnerId, "CTBCOUNTRY", invoiceItemFromFile.OriginCountryCode);
                    if (!string.IsNullOrWhiteSpace(_Country))
                    {
                        var isSuccess = SetCountry(tenant, _Country, invoiceItem);
                        if (!isSuccess)
                        {
                            isSuccess = SetCountry(tenant, invoiceItemFromFile.OriginCountryCode, invoiceItem);
                            if (!isSuccess)
                            {
                                LogMessagingUtil.Instance.AppendLine("OriginCountryCode = " + invoiceItemFromFile.OriginCountryCode + " could not translate to Logitude Id");
                            }
                        }
                    }
                    else
                    {
                        var isSuccess = SetCountry(tenant, invoiceItemFromFile.OriginCountryCode, invoiceItem);
                        if (!isSuccess)
                        {
                            LogMessagingUtil.Instance.AppendLine("OriginCountryCode = " + invoiceItemFromFile.OriginCountryCode + " could not translate to Logitude Id");
                        }
                    }
                }


                invoiceItem.ChangeSetOp = ChangeSetOperation.Insert;
                invoice.SupplierInvoiceItems.Add(invoiceItem);
            }
        }

        private void ReadDataFromCsvFile(string decodedString)
        {
            try
            {
                var lines = decodedString.Split(new string[] { "\n" }, StringSplitOptions.None).ToList();

                Boolean CodeExist = false;
                for (int i = 1; i < lines.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;
                    string[] data = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                    CodeExist = false;
                    string invoiceNumber = data[0];
                    if (string.IsNullOrWhiteSpace(invoiceNumber))
                    {
                        //LogMessagingUtil.Instance.AppendLine("invoiceNumber cannot be null");
                        throw new Exception("invoiceNumber cannot be null");
                    }
                    //if (string.IsNullOrWhiteSpace(data[5]))
                    //{
                    //    LogMessagingUtil.Instance.AppendLine("RichbitFileNumber cannot be null");
                    //    throw new Exception("RichbitFileNumber cannot be null");
                    //}
                    if (string.IsNullOrWhiteSpace(data[7]))
                    {
                        //LogMessagingUtil.Instance.AppendLine("ClassificationCode cannot be null");
                        throw new Exception("ClassificationCode cannot be null");
                    }
                    if (string.IsNullOrWhiteSpace(data[9]))
                    {
                        //LogMessagingUtil.Instance.AppendLine("ItemPrice cannot be null");
                        throw new Exception("ItemPrice cannot be null");
                    }

                    foreach (var item in fromFile) // check if code exist in list already
                    {
                        if (item.InvoiceNumber == invoiceNumber)
                        {
                            CodeExist = true;
                            item.SupplierInvoiceItems.Add(new InvoiceItemFromFile
                            {
                                ItemPriceCurrencyCode = data[4],
                                RichbitFileNumber = data[5],
                                VehicleChassisNumber = data[6],
                                ClassificationCode = data[7],//todo
                                ItemDescription = data[8],
                                ItemPrice = string.IsNullOrWhiteSpace(data[9]) ? (decimal?)null : Convert.ToDecimal(data[9]),
                                TradeAgreemenCode = data[10],
                                OriginCountryCode = data[11],
                                AdditionalQuantity = data[14],
                                AdditionalQuantityCurrencyType = data[15],
                                ModificationAndDiscountTypeAmount = data[16],
                            });
                            break;
                        }
                    }
                    if (!CodeExist)
                    {
                        InvoiceFromFile row = new InvoiceFromFile();
                        row.InvoiceNumber = invoiceNumber;
                        row.InvoiceCurrencyTypeCode = data[4];
                        row.VendorId = data[2];
                        row.Incoterm = data[3];
                        row.FreightAmount = string.IsNullOrWhiteSpace(data[12]) ? (decimal?)null : Convert.ToDecimal(data[12]);
                        row.FreightAmountCurrencyType = data[13];
                        if (!string.IsNullOrWhiteSpace(data[1]))
                        {
                            DateTime date;
                            if (!DateTime.TryParse(data[1], out date))
                            {
                                LogMessagingUtil.Instance.AppendLine("IssueDate is not valid ");
                            }
                            else
                            {
                                row.IssueDate = date;
                            }
                        }

                        row.SupplierInvoiceItems = new List<InvoiceItemFromFile>
                        {
                            new InvoiceItemFromFile
                            {
                            ItemPriceCurrencyCode = data[4],
                            RichbitFileNumber = data[5],
                            VehicleChassisNumber = data[6],
                            ClassificationCode = data[7],//todo
                            ItemDescription = data[8],
                            ItemPrice = string.IsNullOrWhiteSpace(data[9]) ? (decimal?)null : Convert.ToDecimal(data[9]),
                            TradeAgreemenCode = data[10],
                            OriginCountryCode = data[11],
                            AdditionalQuantity = data[14],
                            AdditionalQuantityCurrencyType = data[15],
                            ModificationAndDiscountTypeAmount = data[16],
                            }
                        };
                        fromFile.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
                LogMessagingUtil.Instance.AppendLine("fail Read Data From File: " + e.ToString());
                throw;
            }
        }

        private bool SetTradeAgreemenCode(int tenant, string TradeAgreemenCodeId, SupplierInvoiceItemPM invoiceItem)
        {
            TradeAgreementRepository tradeAgreemenQueryService = new TradeAgreementRepository(tenant);
            var tradeAgreemen = tradeAgreemenQueryService.GetSingle(TradeAgreemenCodeId);
            if (tradeAgreemen != null)
            {
                invoiceItem.TradeAgreementCode = tradeAgreemen.Code;
                return true;
            }
            return false;
        }
        private bool SetCountry(int tenant, string countryId, SupplierInvoiceItemPM invoiceItem)
        {
            CustomsCountryQueryService vendorQueryService = new CustomsCountryQueryService(tenant);
            var country = vendorQueryService.GetSingle(countryId, false, true);
            if (country != null)
            {
                invoiceItem.OriginCountryCode = country.Code;
                return true;
            }
            return false;
        }

        private bool SetVendor(int tenant, string vendorId, SupplierInvoicePM invoice)
        {
            CustomsVendorQueryService vendorQueryService = new CustomsVendorQueryService(tenant);
            CustomsVendorPM vendor = vendorQueryService.GetVendorByNumber(vendorId, tenant);
            if (vendor != null)
            {
                invoice.VendorName = vendor.VendorName;
                invoice.VendorNumber = vendor.VendorNumber;
                invoice.VendorId = vendor.Id;
                return true;
            }
            return false;
        }

        private bool SetFreightAmountCurrencyTypeCode(int tenant, string currencyTypeCode, SupplierInvoiceFreightAmountPM FreightAmount)
        {
            CurrencyTypeQueryService incotermQueryService = new CurrencyTypeQueryService(tenant);
            var currencyType = incotermQueryService.GetSingle(currencyTypeCode, false, true);
            if (currencyType != null)
            {
                FreightAmount.CurrencyTypeCode = currencyType.Code;
                return true;
            }
            return false;
        }

        private bool SetCurrencyTypeCode(int tenant, string currencyTypeCode, SupplierInvoicePM invoice)
        {
            CurrencyTypeQueryService incotermQueryService = new CurrencyTypeQueryService(tenant);
            var currencyType = incotermQueryService.GetSingle(currencyTypeCode, false, true);
            if (currencyType != null)
            {
                invoice.InvoiceCurrencyTypeCode = currencyType.Code;
                return true;
            }
            return false;
        }

        private bool SetIncoterm(int tenant, string incotermId, SupplierInvoicePM invoice)
        {
            TermsOfSaleTypeRepository incotermQueryService = new TermsOfSaleTypeRepository(tenant);
            var incoterm = incotermQueryService.GetSingle(incotermId);
            if (incoterm != null)
            {
                invoice.IncotermCode = incoterm.Code;
                invoice.IncotermName = incoterm.LocalName;
                return true;
            }
            return false;
        }

        private string GetTranslationL2P(string partnerID, string tableID, string localCode)
        {
            var rec = (from a in amitalContext.GTRTRANs
                       where a.PARTNERID == partnerID && a.TABLEID == tableID && a.PARTNERCODE == localCode
                       select a).FirstOrDefault();
            if (rec == null)
            {
                return null;
            }
            return rec.LOCALCODE;
        }

        private class InvoiceFromFile
        {
            public string InvoiceNumber;
            public DateTime? IssueDate;
            public string InvoiceCurrencyTypeCode;
            public string VendorId;
            public string Incoterm;
            public decimal? FreightAmount;
            public string FreightAmountCurrencyType;
            public List<InvoiceItemFromFile> SupplierInvoiceItems;
        }

        private class InvoiceItemFromFile
        {
            public string ItemPriceCurrencyCode;
            public decimal? ItemPrice;
            public string TradeAgreemenCode;
            public string OriginCountryCode;
            public string RichbitFileNumber;
            public string VehicleChassisNumber;
            public string ClassificationCode;
            public string ItemDescription;
            
            public string AdditionalQuantity;
            public string AdditionalQuantityCurrencyType;
            public string ModificationAndDiscountTypeAmount;
        }
    }
    
}
