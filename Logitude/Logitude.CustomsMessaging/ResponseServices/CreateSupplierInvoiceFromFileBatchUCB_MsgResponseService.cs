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

                //todo:
                ReadDataFromCsvFile(customResponse.decodedString);
                UpdateDB();

                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyResponseData.Succeeded = true;
            }
        }
        private void UpdateDB()
        {

        }
        private void ReadDataFromCsvFile(string decodedString)
        {
            var lines = decodedString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> results = new List<string>();
            foreach (string line in lines)
            {
                results.AddRange(Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))"));
            }
            Boolean CodeExist = false;
            for (int i = 17; i < results.Count;) // the excel has 17 cols  , Excel Analayze
            {
                CodeExist = false;
                string invoiceNumber = results[i];
                foreach (var item in fromFile) // check if code exist in list already
                {
                    if (item.InvoiceNumber == invoiceNumber)
                    {
                        CodeExist = true;
                        item.SupplierInvoiceItems.Add(new InvoiceItemFromFile
                        {
                            ItemPriceCurrencyCode = results[i + 4],
                            RichbitFileNumber = results[i + 5],
                            VehicleChassisNumber = results[i + 6],
                            ClassificationCode = results[i + 7],//todo
                            ItemDescription = results[i + 8],
                            ItemPrice = results[i + 9],
                            TradeAgreemenCode = results[i + 10],
                            OriginCountryCode = results[i + 11],
                            FreightAmount = results[i + 12],
                            FreightAmountCurrencyType = results[i + 13],
                            AdditionalQuantity = results[i + 14],
                            AdditionalQuantityCurrencyType = results[i + 15],
                            ModificationAndDiscountTypeAmount = results[i + 16],
                        });
                        break;
                    }
                }
                if (!CodeExist)
                {
                    InvoiceFromFile row = new InvoiceFromFile();
                    row.InvoiceNumber = invoiceNumber;
                    row.InvoiceCurrencyTypeCode = results[i + 4];
                    row.VendorId = results[i + 2];//todo
                    row.Incoterm = results[i + 3];//todo
                    if(!DateTime.TryParse(results[i + 1], out row.IssueDate))
                    {
                        LogMessagingUtil.Instance.AppendLine("IssueDate is not valid ");
                    }
                    row.SupplierInvoiceItems = new List<InvoiceItemFromFile>
                    {
                        new InvoiceItemFromFile
                        {
                            ItemPriceCurrencyCode = results[i + 4],
                            RichbitFileNumber = results[i + 5],
                            VehicleChassisNumber = results[i + 6],
                            ClassificationCode = results[i + 7],//todo
                            ItemDescription = results[i + 8],
                            ItemPrice = results[i + 9],
                            TradeAgreemenCode = results[i + 10],
                            OriginCountryCode = results[i + 11],
                            FreightAmount = results[i + 12],
                            FreightAmountCurrencyType = results[i + 13],
                            AdditionalQuantity = results[i + 14],
                            AdditionalQuantityCurrencyType = results[i + 15],
                            ModificationAndDiscountTypeAmount = results[i + 16],
                        }
                    };
                    fromFile.Add(row);
                }
                i += 17;
            }
        }

        private class InvoiceFromFile
        {
            public string InvoiceNumber;
            public DateTime IssueDate;
            public string InvoiceCurrencyTypeCode;
            public string VendorId;
            public string Incoterm;
            public List<InvoiceItemFromFile> SupplierInvoiceItems;
        }

        private class InvoiceItemFromFile
        {
            public string ItemPriceCurrencyCode;
            public string ItemPrice;
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
