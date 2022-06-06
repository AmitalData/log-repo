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

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CreateSupplierInvoiceFromFileBatchUCB_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBCreateSupplierInvoiceWithResponseContentHeader, GenericRequestParams>
    {
        private List<InvoiceFromFile> fromFile = new List<InvoiceFromFile>();

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBCreateSupplierInvoiceWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBCreateSupplierInvoiceWithResponseContentHeader customResponse, GenericRequestParams requestParams)
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
                        IssueDate = invoiceFromFile.IssueDate,
                        VendorId = invoiceFromFile.VendorId,
                        IncotermCode = invoiceFromFile.Incoterm,
                        InvoiceCurrencyTypeCode = invoiceFromFile.InvoiceCurrencyTypeCode,
                    };
                    invoice.ChangeSetOp = ChangeSetOperation.Insert;
                    invoice.SupplierInvoiceItems = new List<SupplierInvoiceItemPM>();
                    foreach (var invoiceItemFromFile in invoiceFromFile.SupplierInvoiceItems)
                    {
                        var invoiceItem = new SupplierInvoiceItemPM
                        {
                            DeclarationId = declarationid,
                            ItemPriceCurrencyCode = invoiceFromFile.InvoiceCurrencyTypeCode,
                            ItemPrice = invoiceItemFromFile.ItemPrice,
                            TradeAgreementCode = invoiceItemFromFile.TradeAgreemenCode,
                            OriginCountryCode = invoiceItemFromFile.OriginCountryCode,
                            InvoiceQuantity = 1,
                            InvoiceNumber = invoiceFromFile.InvoiceNumber,
                            ClassificationCode = invoiceItemFromFile.ClassificationCode//todo
                            //InvoiceQuantityType = todo
                        };
                        invoiceItem.ChangeSetOp = ChangeSetOperation.Insert;
                        invoice.SupplierInvoiceItems.Add(invoiceItem);
                    }
                    declarationPM.SupplierInvoices.Add(invoice);
                }
                else
                {
                    //create invoiceitems
                    //invoiceFromDB.SupplierInvoiceItems
                }
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
                                FreightAmount = data[12],
                                FreightAmountCurrencyType = data[13],
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
                        row.VendorId = data[2];//todo
                        row.Incoterm = data[3];//todo
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
                            FreightAmount = data[12],
                            FreightAmountCurrencyType = data[13],
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
                LogMessagingUtil.Instance.AppendLine(e.ToString());
                throw;
            }
        }

        private class InvoiceFromFile
        {
            public string InvoiceNumber;
            public DateTime? IssueDate;
            public string InvoiceCurrencyTypeCode;
            public string VendorId;
            public string Incoterm;
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
            public string FreightAmount;
            public string FreightAmountCurrencyType;
            public string AdditionalQuantity;
            public string AdditionalQuantityCurrencyType;
            public string ModificationAndDiscountTypeAmount;
        }
    }
    
}
