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
    public class CreateExportSupplierInvoicesFromFileBatchUCB_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBCreateExportSupplierInvoicesWithResponseContentHeader, GenericRequestParams>
    {
        private List<InvoiceFromFile> fromFile = new List<InvoiceFromFile>();

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBCreateExportSupplierInvoicesWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBCreateExportSupplierInvoicesWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            if (customResponse.Declarationid != null)
            {
                LogMessagingUtil.Instance.AppendLine("customResponse.Declarationid: " + customResponse.Declarationid);

                ReadDataFromCsvFile(customResponse.decodedString);
                UpdateDB(customResponse.Declarationid, requestParams.Tenant);

                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyResponseData.Succeeded = true;
            }
        }
        private void UpdateDB(string declarationid, int tenant)
        {
            var context = CustomContext.GetContext(tenant);
            var declarationQueryService = new DeclarationQueryService(context);
            DeclarationPM declarationPM = declarationQueryService.GetSingle(declarationid, true, false);
            MyRequestSheetParam.RequestDescription = $"{declarationPM.DeclarationNumber} קליטת חשבונות יצואן מקובץ, הצהרה";
            string error = "";
            string errorItems = "";
            var invoiceFromDB = declarationPM.SupplierInvoices.FirstOrDefault();
            if (invoiceFromDB == null) return;
            SupplierInvoiceRepository supplierInvoiceRepository = new SupplierInvoiceRepository(context);
            int maxSequence = supplierInvoiceRepository.GetMaxSequenceNumeric(declarationid, tenant) ?? 0;
            foreach (var invoiceFromFile in fromFile)
            {
                // create new invoice
                var invoice = new SupplierInvoicePM
                {
                    AccountTypeCode = invoiceFromDB.AccountTypeCode,
                    IncotermCode = invoiceFromDB.IncotermCode,
                    InvoiceNumber = invoiceFromFile.InvoiceNumber,
                    IsPreference = invoiceFromFile.IsPreference,
                    InvoiceAmount = invoiceFromFile.SupplierInvoiceItems.Sum(x=>x.ItemPrice.GetValueOrDefault()),
                    BuyerName = invoiceFromDB.BuyerName,
                    BuyerAddress = invoiceFromDB.BuyerAddress,
                    BuyerRoleCode = invoiceFromDB.BuyerRoleCode,
                    BuyerCountryCode = invoiceFromDB.BuyerCountryCode,
                    PartyRelationshipCode = invoiceFromDB.PartyRelationshipCode,
                    DeclarationId = declarationid,
                    Tenant = tenant,
                    IssueDate = invoiceFromFile.IssueDate,
                    InvoiceCounterKey = ++maxSequence,
                    SequenceNumeric = maxSequence,

                    //ExportFreightAmount = invoiceFromDB.ExportFreightAmount,
                    //ExportInsuranceAmount = invoiceFromDB.ExportInsuranceAmount
                };
                error += invoiceFromFile.Errors;
                if (!string.IsNullOrWhiteSpace(invoiceFromFile.OriginCountryCode))
                {
                    CustomsCountryQueryService countryQueryService = new CustomsCountryQueryService(invoice.Tenant);
                    CustomsCountryPM country = countryQueryService.GetSingle(invoiceFromFile.OriginCountryCode, false, true);
                   if(country == null)
                    {
                        error += invoice.InvoiceNumber + ":OriginCountryCode = " + invoiceFromFile.OriginCountryCode + " could not translate to Logitude Id \n";

                    }
                    else
                    {
                        invoice.IssueCountryCode = invoiceFromFile.OriginCountryCode;
                    }
                }
                if (!string.IsNullOrWhiteSpace(invoiceFromFile.PreferenceDocumentTypeCode))
                {
                    TradeAgreementQueryService queryService = new TradeAgreementQueryService(invoice.Tenant);
                    var tradeAgreementCode = queryService.GetSingle(invoiceFromFile.PreferenceDocumentTypeCode, false, true);
                    if (tradeAgreementCode == null)
                    {
                        error += invoice.InvoiceNumber + ":TradeAgreementCode = " + invoiceFromFile.PreferenceDocumentTypeCode + " could not translate to Logitude Id \n";
                    }
                    else
                    {
                        invoice.PreferenceDocumentTypeCode = invoiceFromFile.PreferenceDocumentTypeCode;
                    }
                }
                else
                {
                    invoice.PreferenceDocumentTypeCode = invoiceFromDB.PreferenceDocumentTypeCode;
                }
                if (!string.IsNullOrWhiteSpace(invoiceFromFile.TradeAgreementProtocol))
                {
                    TradeAgreementProtocolQueryService queryService = new TradeAgreementProtocolQueryService(invoice.Tenant);
                    var dutyRegimeProtocolCode = queryService.GetSingle(invoiceFromFile.TradeAgreementProtocol, false, true);
                    if (dutyRegimeProtocolCode == null)
                    {
                        errorItems += invoice.InvoiceNumber + ":TradeAgreementProtocol = " + invoiceFromFile.TradeAgreementProtocol + " could not translate to Logitude Id \n";
                    }
                    else
                    {
                        invoice.DutyRegimeProtocolCode = invoiceFromFile.TradeAgreementProtocol;
                    }
                }
                else
                {
                    invoice.DutyRegimeProtocolCode = invoiceFromDB.DutyRegimeProtocolCode;
                }

                if (!string.IsNullOrWhiteSpace(invoiceFromFile.InvoiceCurrency))
                {
                    var isSuccess = SetCurrencyTypeCode(tenant, invoiceFromFile.InvoiceCurrency, invoice);
                    if (!isSuccess)
                    {
                        error += invoice.InvoiceNumber + ":InvoiceCurrencyTypeCode = " + invoiceFromFile.InvoiceCurrency + " could not translate to Logitude Id \n";
                        LogMessagingUtil.Instance.AppendLine(error);
                    }
                }

                if (string.IsNullOrEmpty(invoice.InvoiceNumber))
                    error += "InvoiceNumber is required. \n";

                if ( string.IsNullOrEmpty(invoice.InvoiceCurrencyTypeCode) || invoice.InvoiceAmount == null || string.IsNullOrEmpty (invoice.BuyerName) || string.IsNullOrEmpty(invoice.BuyerAddress) || string.IsNullOrEmpty(invoice.BuyerRoleCode)
                    || string.IsNullOrEmpty(invoice.BuyerRoleCode) || string.IsNullOrEmpty(invoice.PartyRelationshipCode) || !invoice.IssueDate.HasValue)
                {
                    error += invoice.InvoiceNumber + ":some fields is required. \n";

                }
                var sum = invoiceFromFile.SupplierInvoiceItems.Where(a => a.TransactionNatureCode == "2").Sum(a => a.ItemPrice.GetValueOrDefault());
                if (sum > 0)
                {
                    invoice.SupplierInvoicePayments = new List<SupplierInvoicePaymentPM>
                    {
                        new SupplierInvoicePaymentPM
                        {
                            DeclarationId = declarationid,
                            Tenant = tenant,
                            ChangeSetOp = ChangeSetOperation.Insert,
                            PaymentAmount = sum,
                            PaymentTypeCode = "2",
                            SequenceNumeric=1,
                        }
                    };
                }

                foreach (var item in invoiceFromDB.SupplierInvoiceModifications)
                {
                    if (item.TypeCode == "67" || item.TypeCode == "104")
                    {
                        invoice.SupplierInvoiceModifications.Add(

                            new SupplierInvoiceModificationPM
                            {
                                DeclarationId = declarationid,
                                Tenant = tenant,
                                ChangeSetOp = ChangeSetOperation.Insert,
                                TypeCode = item.TypeCode,
                                Amount = item.Amount,
                                CurrencyTypeCode = item.CurrencyTypeCode
                            });
                    }
                }

                invoice.ChangeSetOp = ChangeSetOperation.Insert;
                invoice.SupplierInvoiceItems = new List<SupplierInvoiceItemPM>();
                CreateSupplierInvoiceItems(invoice, tenant, declarationid, invoiceFromFile, invoiceFromDB, out errorItems);
                error += errorItems;
                declarationPM.SupplierInvoices.Add(invoice);
                this.MyResponseData.UserMessage += error;

                error = "";

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
                LogMessagingUtil.Instance.AppendLine("error in Saving declaration to DB: " + e.ToString());
                throw;
            }
        }

        private int? CalculateLuhnAlgorithm(string value)
        {
            var sum = 0;
            int d;
            for (var i = 0; i < value.Length; i++)
            {
                d = 0;
                if (!int.TryParse(value.Substring(i, 1), out d)) return null;
                if (i % 2 != 0)
                    d = d * 2;
                if (d > 9)
                    d -= 9;
                sum += d;
            }

            if (sum % 10 == 0)
            {
                return 0;
            }
            else
            {
                var x = sum % 10;
                return 10 - x;
            }
        }

        private string setClassificationCode(string value, out bool valid)
        {
            string pattern= "^[0-9]+$";
            bool ValueIsValidNumber = Regex.IsMatch(value, pattern);
            string res = "";
            valid = true;
            if (ValueIsValidNumber)
            {
                int? checkDigit = null;
                switch (value.Length)
                {
                    case 8:
                        res = value + "00";
                        checkDigit = CalculateLuhnAlgorithm(res);
                        if (checkDigit.HasValue)
                            res += checkDigit.Value.ToString();
                        break;
                    case 9:
                        var digit = value.Substring(8);
                        res = value.Substring(0, 8) + "00" + value.Substring(8);
                        checkDigit = CalculateLuhnAlgorithm(res.Substring(0, 10));
                        if (digit != checkDigit.GetValueOrDefault().ToString())
                        {
                            valid = false;
                            res = "";
                        }
                        break;
                    case 10:
                        checkDigit = CalculateLuhnAlgorithm(value);
                        if (checkDigit.HasValue)
                            res = value + "" + checkDigit.Value.ToString();
                        break;
                    case 11:
                        var digit_ = value.Substring(10);
                        checkDigit = CalculateLuhnAlgorithm(value.Substring(0, 10));
                        res = value;
                        if (digit_ != checkDigit.GetValueOrDefault().ToString())
                        {
                            valid = false;
                            res = "";
                        }
                        break;
                    default:
                        valid = false;
                        //res = value;
                        break;
                }
            }
            else
            {
                valid = false;
            }
            return res;
        }

        private void CreateSupplierInvoiceItems(SupplierInvoicePM invoice, int tenant, string declarationid, InvoiceFromFile invoiceFromFile, SupplierInvoicePM invoiceFromDB, out string errorItems)
        {
              errorItems = "";
            foreach (var invoiceItemFromFile in invoiceFromFile.SupplierInvoiceItems)
            {
                var invoiceItem = new SupplierInvoiceItemPM
                {
                    DeclarationId = declarationid,
                    Tenant = tenant,
                    InvoiceQuantity = invoiceItemFromFile.InvoiceQuentity,
                    //StatisticQuantity = invoiceItemFromFile.InvoiceQuentity,
                    //ItemDescription = invoiceItemFromFile.ItemDescription,
                    //ClassificationCode = invoiceItemFromFile.ClassificationCode,//"84253990000"//todo
                    ItemPrice = invoiceItemFromFile.ItemPrice,
                    InvoiceNumber = invoiceFromFile.InvoiceNumber,
                    // ItemCode = invoiceItemFromFile.ItemDescription,
                };
                invoiceItem.ClassificationCode = setClassificationCode(invoiceItemFromFile.ClassificationCode,out bool valid);
                if(!valid)
                {
                    errorItems += invoice.InvoiceNumber + "line[" + invoiceItemFromFile.rownum + "]:ClassificationCode = "+ invoiceItem.ClassificationCode + " not valid";
                }
                if (!string.IsNullOrWhiteSpace(invoiceItemFromFile.DutyRegimeProtocolCode))
                {
                    TradeAgreementProtocolQueryService queryService = new TradeAgreementProtocolQueryService(invoice.Tenant);
                    var dutyRegimeProtocolCode = queryService.GetSingle(invoiceItemFromFile.DutyRegimeProtocolCode, false, true);
                    if (dutyRegimeProtocolCode == null)
                    {
                        errorItems += invoice.InvoiceNumber + "line[" + invoiceItemFromFile.rownum + "]:DutyRegimeProtocolCode = " + invoiceItemFromFile.DutyRegimeProtocolCode + " could not translate to Logitude Id \n";
                    }
                    else
                    {
                        invoiceItem.DutyRegimeProtocolCode = invoiceItemFromFile.DutyRegimeProtocolCode;
                    }
                }
                
                if (!string.IsNullOrWhiteSpace(invoiceItemFromFile.TradeAgreement))
                {
                    TradeAgreementQueryService queryService = new TradeAgreementQueryService(invoice.Tenant);
                    var tradeAgreementCode = queryService.GetSingle(invoiceItemFromFile.TradeAgreement, false, true);
                    if (tradeAgreementCode == null)
                    {
                        errorItems += invoice.InvoiceNumber +"line[" + invoiceItemFromFile.rownum + "]:TradeAgreementCode = " + invoiceItemFromFile.TradeAgreement + " could not translate to Logitude Id \n";
                    }
                    else
                    {
                        invoiceItem.TradeAgreementCode = invoiceItemFromFile.TradeAgreement;
                    }
                }
                /*else
                {
                    if (invoiceFromDB.SupplierInvoiceItems != null)
                    {
                        invoiceItem.TradeAgreementCode = invoiceFromDB.SupplierInvoiceItems.FirstOrDefault()?.TradeAgreementCode;
                    }
                }*/
                if (!string.IsNullOrWhiteSpace(invoiceItemFromFile.OriginCountryCode))
                {
                    CustomsCountryQueryService countryQueryService = new CustomsCountryQueryService(invoice.Tenant);
                    CustomsCountryPM country = countryQueryService.GetSingle(invoiceItemFromFile.OriginCountryCode, false, true);
                    if (country == null)
                    {
                        errorItems += invoice.InvoiceNumber + "line[" + invoiceItemFromFile.rownum + "]:OriginCountryCode = " + invoiceItemFromFile.OriginCountryCode + " could not translate to Logitude Id \n";

                    }
                    else
                    {
                        invoiceItem.OriginCountryCode = invoiceItemFromFile.OriginCountryCode;
                    }
                }

                if (invoiceFromDB.SupplierInvoiceItems != null)
                {
                    invoiceItem.ClaimReasonCode = invoiceFromDB.SupplierInvoiceItems.FirstOrDefault()?.ClaimReasonCode;
                }
                if (!string.IsNullOrWhiteSpace(invoiceItemFromFile.InvoiceCurrency))
                {
                    CurrencyTypeQueryService queryService = new CurrencyTypeQueryService(tenant);
                    var currencyType = queryService.GetSingle(invoiceItemFromFile.InvoiceCurrency, false, true);
                    if (currencyType != null)
                    {
                        invoiceItem.ItemPriceCurrencyCode = invoiceItemFromFile.InvoiceCurrency;
                    }
                    else
                    {
                        errorItems += invoice.InvoiceNumber + "line[" + invoiceItemFromFile.rownum + "]:InvoiceCurrencyTypeCode = " + invoiceItemFromFile.InvoiceCurrency + " could not translate to Logitude Id \n";
                    }
                }
                if (!string.IsNullOrWhiteSpace(invoiceItemFromFile.ProcessType))
                {
                    ItemGovernmentProcedureTypeQueryService queryService = new ItemGovernmentProcedureTypeQueryService(tenant);
                    var processType = queryService.GetSingle(invoiceItemFromFile.ProcessType, false, true);
                    if (processType != null)
                    {
                        invoiceItem.SupplierInvoiceItemProcesTypes = new List<SupplierInvoiceItemProcesTypePM>
                        {
                            new SupplierInvoiceItemProcesTypePM
                            {
                               ProcessTypeCode = invoiceItemFromFile.ProcessType,
                               DeclarationId = declarationid,
                               Tenant = tenant,
                               ChangeSetOp = ChangeSetOperation.Insert
                            }
                        };
                        invoiceItem.ItemAdditionalStatus = true;
                    }
                    else
                    {
                        errorItems += invoice.InvoiceNumber + "line[" + invoiceItemFromFile.rownum + "]:ProcessType = " + invoiceItemFromFile.ProcessType + " could not translate to Logitude Id \n";
                    }
                }
                    
                if (!string.IsNullOrWhiteSpace(invoiceItemFromFile.TransactionNatureCode))
                {
                    TransactionNatureTypeQueryService queryService = new TransactionNatureTypeQueryService(invoice.Tenant);
                    var transactionNatureType = queryService.GetSingle(invoiceItemFromFile.TransactionNatureCode, false, true);
                    if (transactionNatureType == null)
                    {
                        errorItems += invoice.InvoiceNumber + "line[" + invoiceItemFromFile.rownum + "]:TransactionNatureCode = " + invoiceItemFromFile.TransactionNatureCode + " could not translate to Logitude Id \n";
                    }
                    else
                    {
                        invoiceItem.TransactionNatureCode = invoiceItemFromFile.TransactionNatureCode;
                    }
                }
                
                CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(tenant);
                invoiceItem.InvoiceQuantityType = customsItemQueryService.GetQuantityTypeByClassificationWithMultiCustomItems(invoiceItem.ClassificationCode, tenant, true);
                //invoiceItem.StatisticQuantityType = invoiceItem.InvoiceQuantityType;

                if(invoiceItem.InvoiceQuantity== null || string.IsNullOrEmpty(invoiceItem.ClassificationCode) || invoiceItem.ItemPrice==null || string.IsNullOrEmpty(invoiceItemFromFile.OriginCountryCode))
                {
                    errorItems += invoice.InvoiceNumber + "line[" + invoiceItemFromFile.rownum + "]:some fields is required. \n";
                    //break;
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
               
                InvoiceFromFile currentInvoice = null;
                for (int i = 1; i < lines.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;
                    string[] data = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    currentInvoice = fromFile.FirstOrDefault(x => x.InvoiceNumber == data[0]);
                    if (currentInvoice == null)
                    {
                        //new invoice
                        InvoiceFromFile row = new InvoiceFromFile();
                        row.InvoiceNumber = data[0];
                        row.InvoiceCurrency = data[11];
                        DateTime? issueDate = null;
                        if (!string.IsNullOrWhiteSpace(data[10]))
                        {
                            DateTime date;
                            if (!DateTime.TryParse(data[10], out date))
                                LogMessagingUtil.Instance.AppendLine("IssueDate is not valid ");
                            else
                                issueDate = date;
                        }
                        row.IssueDate = issueDate;
                        row.PreferenceDocumentTypeCode = data[7];
                        row.TradeAgreementProtocol = data[8];
                        //row.IsPreference = string.IsNullOrWhiteSpace(data[9]) ? false : Boolean.TryParse(data[9],out row.IsPreference);
                        if (Boolean.TryParse(data[9], out Boolean parsedValue))
                        {
                            row.IsPreference = parsedValue;
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(data[9]))
                            {
                                row.IsPreference = false; ;
                            }
                            else
                            {
                                row.Errors += " InvoiceNumber:" + row.InvoiceNumber + " Line: " + i + " IsPreference is not valid boolean value:" + data[9] +" ";
                            }
                        }


                        row.OriginCountryCode = data[2];
                        var supplierInvoiceItem = new InvoiceItemFromFile
                        {
                            rownum = (i + 1).ToString(),
                            ClassificationCode = data[1],
                            OriginCountryCode = data[2],
                            //InvoiceQuentity = string.IsNullOrWhiteSpace(data[3]) ? (int?)null : (Convert.ToInt32(data[3]) > 0 ? Convert.ToInt32(data[3]) : (int?)null),
                            //ItemPrice = string.IsNullOrWhiteSpace(data[4]) ? (decimal?)null : Convert.ToDecimal(data[4]),
                            TransactionNatureCode = data[5],
                            ProcessType = data[6],
                            TradeAgreement = data[7],
                            DutyRegimeProtocolCode = data[8],
                            InvoiceCurrency = data[11],
                        };
                        if (decimal.TryParse(data[3], out decimal parsedQuentity))
                        {
                            supplierInvoiceItem.InvoiceQuentity = parsedQuentity;
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(data[3]))
                            {
                                supplierInvoiceItem.InvoiceQuentity = null;
                            }
                            else
                            {
                                row.Errors += " InvoiceNumber:" + row.InvoiceNumber + " Line: " + i + " InvoiceQuentity is not valid decimal value:" + data[3] + " ";
                            }
                        }
                        if (decimal.TryParse(data[4], out decimal parsedPrice))
                        {
                            supplierInvoiceItem.ItemPrice = parsedPrice;
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(data[4]))
                            {
                                supplierInvoiceItem.ItemPrice = null;
                            }
                            else
                            {
                                row.Errors += " InvoiceNumber:" + row.InvoiceNumber + " Line: " + i + " ItemPrice is not valid decimal value:" + data[4] + " ";
                            }
                        }
                        row.SupplierInvoiceItems = new List<InvoiceItemFromFile> {
                            supplierInvoiceItem
                        };
                        fromFile.Add(row);
                        currentInvoice = row;
                        //i++;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(string.Join(" ", data))) continue;
                        //new item

                        var supplierInvoiceItem = new InvoiceItemFromFile
                        {
                            rownum = (i + 1).ToString(),
                            ClassificationCode = data[1],
                            OriginCountryCode = data[2],
                            //InvoiceQuentity = string.IsNullOrWhiteSpace(data[3]) ? (int?)null : Convert.ToInt32(data[3]),
                            //ItemPrice = string.IsNullOrWhiteSpace(data[4]) ? (decimal?)null : Convert.ToDecimal(data[4]),
                            TransactionNatureCode = data[5],
                            ProcessType = data[6],
                            TradeAgreement = data[7],
                            DutyRegimeProtocolCode = data[8],
                            InvoiceCurrency = data[11],
                        };
                        if (decimal.TryParse(data[3], out decimal parsedQuentity))
                        {
                            supplierInvoiceItem.InvoiceQuentity = parsedQuentity;
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(data[3]))
                            {
                                supplierInvoiceItem.InvoiceQuentity = null;
                            }
                            else
                            {
                                currentInvoice.Errors += " InvoiceNumber:" + currentInvoice.InvoiceNumber + " Line: " + i + " InvoiceQuentity is not valid decimal value:" + data[3] + " ";
                            }
                        }
                        if (decimal.TryParse(data[4], out decimal parsedPrice))
                        {
                            supplierInvoiceItem.ItemPrice = parsedPrice;
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(data[4]))
                            {
                                supplierInvoiceItem.ItemPrice = null;
                            }
                            else
                            {
                                currentInvoice.Errors += " InvoiceNumber:" + currentInvoice.InvoiceNumber + " Line: " + i + " ItemPrice is not valid decimal value:" + data[4] + " ";
                            }
                        }
                        if (string.IsNullOrWhiteSpace(currentInvoice.PreferenceDocumentTypeCode))
                            currentInvoice.PreferenceDocumentTypeCode = data[7];
                        if (string.IsNullOrWhiteSpace(currentInvoice.TradeAgreementProtocol))
                            currentInvoice.TradeAgreementProtocol = data[8];
                        currentInvoice.SupplierInvoiceItems.Add(supplierInvoiceItem);
                    }
                }
            }
            catch (Exception e)
            {
                LogMessagingUtil.Instance.AppendLine("fail Read Data From File: " + e.ToString());
                throw;
            }
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


        private class InvoiceFromFile
        {
            public DateTime? IssueDate;
            public string InvoiceNumber;
            public bool IsPreference;//חשבון העדפה
            public string TradeAgreementProtocol;//קוד פרוטוקול
            public string PreferenceDocumentTypeCode;//קוד הסכם
            public string InvoiceCurrency;
            public string OriginCountryCode;
            //public decimal? InvoiceAmount;
            //public string BuyerName;
            //public string BuyerCountryCode;
            //public string BuyerAddress;
            //public string BuyerRoleCode;
            //public string PartyRelationCode;
            public List<InvoiceItemFromFile> SupplierInvoiceItems;
            public string Errors;
        }

        private class InvoiceItemFromFile
        {
            public string rownum;
            public string TradeAgreement;// - קוד הסכם
            public decimal? InvoiceQuentity;//כמות
            public string TransactionNatureCode;//אופי עסקה
            public string ClassificationCode;
            public decimal? ItemPrice;//ערך במטח
            public string OriginCountryCode;
            public string ProcessType;//סוג תהליך
            public string DutyRegimeProtocolCode;//קוד פרוטוקול
            public string InvoiceCurrency;
            //public string ItemDescription;
            //public string InvoiceQuantityType;
            //public string StatisticQuantityType;
        }
    }
    
}
