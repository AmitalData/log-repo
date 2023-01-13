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
            foreach (var invoiceFromFile in fromFile)
            {
                // create new invoice
                var invoice = new SupplierInvoicePM
                {
                    AccountTypeCode="380",
                    InvoiceNumber = invoiceFromFile.InvoiceNumber,
                    InvoiceAmount = invoiceFromFile.InvoiceAmount,
                    BuyerName = invoiceFromFile.BuyerName,
                    BuyerAddress = invoiceFromFile.BuyerAddress,
                    BuyerRoleCode = invoiceFromFile.BuyerRoleCode,
                    BuyerCountryCode = invoiceFromFile.BuyerCountryCode,
                    PartyRelationshipCode = invoiceFromFile.PartyRelationCode,
                    DeclarationId = declarationid,
                    Tenant = tenant,
                    IssueDate = invoiceFromFile.IssueDate,
                };

                if (!string.IsNullOrWhiteSpace(invoice.BuyerCountryCode))
                {
                    CustomsCountryQueryService countryQueryService = new CustomsCountryQueryService(invoice.Tenant);
                    CustomsCountryPM country = countryQueryService.GetSingle(invoice.BuyerCountryCode, false, true);
                   if(country == null)
                    {
                        error += invoice.InvoiceNumber + ":BuyerCountryCode = " + invoiceFromFile.BuyerCountryCode + " could not translate to Logitude Id \n";

                    }
                }

                if (!string.IsNullOrWhiteSpace(invoice.PartyRelationshipCode))
                {
                    PartyRelationshipTypeQueryService partyRelationshipQueryService = new PartyRelationshipTypeQueryService(invoice.Tenant);
                    PartyRelationshipTypePM partyRelationship = partyRelationshipQueryService.GetSingle(invoice.PartyRelationshipCode, false, true);

                    if (partyRelationship == null)
                    {
                        error += invoice.InvoiceNumber + ":PartyRelationshipCode = " + invoiceFromFile.PartyRelationCode + " could not translate to Logitude Id \n";

                    }

                }

                if (!string.IsNullOrWhiteSpace(invoice.BuyerRoleCode))
                {
                    CustomerRoleTypeQueryService buyerRoleCodeQueryService = new CustomerRoleTypeQueryService(invoice.Tenant);
                    CustomerRoleTypePM buyerRoleCode = buyerRoleCodeQueryService.GetSingle(invoice.BuyerRoleCode, false, true);
                    if (buyerRoleCode == null)
                    {
                        error += invoice.InvoiceNumber + ":BuyerRoleCode = " + invoiceFromFile.BuyerRoleCode + " could not translate to Logitude Id \n";

                    }
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
                invoice.ChangeSetOp = ChangeSetOperation.Insert;
                invoice.SupplierInvoiceItems = new List<SupplierInvoiceItemPM>();
                CreateSupplierInvoiceItems(invoice, tenant, declarationid, invoiceFromFile, out errorItems);
                error += errorItems;

                if (string.IsNullOrEmpty(error))
                    declarationPM.SupplierInvoices.Add(invoice);
                else
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

        private void CreateSupplierInvoiceItems(SupplierInvoicePM invoice, int tenant, string declarationid, InvoiceFromFile invoiceFromFile,out string errorItems)
        {
              errorItems = "";
            foreach (var invoiceItemFromFile in invoiceFromFile.SupplierInvoiceItems)
            {
                var invoiceItem = new SupplierInvoiceItemPM
                {
                    DeclarationId = declarationid,
                    Tenant = tenant,
                    InvoiceQuantity = invoiceItemFromFile.InvoiceQuentity,
                    StatisticQuantity = invoiceItemFromFile.InvoiceQuentity,
                    ItemDescription = invoiceItemFromFile.ItemDescription,
                    ClassificationCode = invoiceItemFromFile.ClassificationCode,//"84253990000"//todo
                    ItemPrice = invoiceItemFromFile.ItemPrice,
                    //ItemPriceCurrencyCode = invoice.InvoiceCurrencyTypeCode,
                    InvoiceNumber = invoiceFromFile.InvoiceNumber,
                    // ItemCode = invoiceItemFromFile.ItemDescription,
                    OriginCountryCode="IL"
                };
               
                CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(tenant);
                invoiceItem.InvoiceQuantityType = customsItemQueryService.GetQuantityTypeByClassificationCode(invoiceItem.ClassificationCode, tenant);
                invoiceItem.StatisticQuantityType = invoiceItem.InvoiceQuantityType;

                if(invoiceItem.InvoiceQuantity== null || string.IsNullOrEmpty(invoiceItem.ClassificationCode) || invoiceItem.ItemPrice==null || string.IsNullOrEmpty(invoiceItemFromFile.OriginCountryCode))
                {
                    errorItems += invoice.InvoiceNumber + ":some fields is required. \n";
                    break;
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
                string[] firstRow = Regex.Split(lines[0], ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                DateTime? issueDate = null;
                if (!string.IsNullOrWhiteSpace(firstRow[0]))
                {
                    DateTime date;
                    if (!DateTime.TryParse(firstRow[0], out date))
                        LogMessagingUtil.Instance.AppendLine("IssueDate is not valid ");
                    else
                        issueDate = date;
                }
                InvoiceFromFile currentInvoice = null;
                for (int i = 1; i < lines.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;
                    string[] data = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    if (data[0].StartsWith("ORIGIN"))
                    {
                        //new invoice
                        InvoiceFromFile row = new InvoiceFromFile();
                        var invoiceNumber = string.IsNullOrWhiteSpace(data[1]) ? "" : data[1].Split(':')[1];
                        row.InvoiceNumber = invoiceNumber.Replace(" ","");
                        var amount = string.IsNullOrWhiteSpace(data[2]) ? "" : data[2].Split(':')[1];
                        row.InvoiceAmount = string.IsNullOrWhiteSpace(amount) ? (decimal?)null : Convert.ToDecimal(amount);
                        row.InvoiceCurrency = data[3];
                        row.IssueDate = issueDate;
                        row.SupplierInvoiceItems = new List<InvoiceItemFromFile> { };
                        fromFile.Add(row);
                        currentInvoice = row;
                        i++;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(string.Join(" ", data))) continue;
                        //new item
                        currentInvoice.BuyerName = data[5];
                        currentInvoice.BuyerCountryCode = data[6];
                        currentInvoice.BuyerAddress = data[7];
                        currentInvoice.BuyerRoleCode = data[8];
                        currentInvoice.PartyRelationCode = data[9];
                        string num = "";
                        if (!string.IsNullOrWhiteSpace(data[0]))
                            num = data[0].Substring(0, data[0].IndexOf('.') > 0 ? data[0].IndexOf('.') : data[0].Length);
                        currentInvoice.SupplierInvoiceItems.Add(new InvoiceItemFromFile
                        {
                            InvoiceQuentity = string.IsNullOrWhiteSpace(num) ? (int?)null : Convert.ToInt32(num),
                            ItemDescription = data[2],
                            ClassificationCode = data[3],
                            ItemPrice = string.IsNullOrWhiteSpace(data[4]) ? (decimal?)null : Convert.ToDecimal(data[4]),
                            OriginCountryCode = "IL",
                            /*/StatisticQuantityType = //
                            InvoiceQuantityType = //*/
                        });
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
            public decimal? InvoiceAmount;
            public string InvoiceCurrency;
            public string BuyerName;
            public string BuyerCountryCode;
            public string BuyerAddress;
            public string BuyerRoleCode;
            public string PartyRelationCode;
            public List<InvoiceItemFromFile> SupplierInvoiceItems;
        }

        private class InvoiceItemFromFile
        {
            public int? InvoiceQuentity;
            public string ItemDescription;
            public string ClassificationCode;
            public decimal? ItemPrice;
            public string OriginCountryCode;
            public string InvoiceQuantityType;
            public string StatisticQuantityType;
        }
    }
    
}
