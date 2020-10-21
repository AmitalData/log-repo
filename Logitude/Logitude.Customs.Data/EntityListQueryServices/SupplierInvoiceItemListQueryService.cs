	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class SupplierInvoiceItemListQueryService
    {
        private IQueryable<SupplierInvoiceItemList> GetIqueryableList(IQueryable<SupplierInvoiceItem> iQueryable)
        {
            IQueryable<SupplierInvoiceItemList> query = (from a in iQueryable.Include("TradeAgreement").Include("OriginCountry").Include("InvoiceMeasurmentUnit").Include("AdditionalMeasurmentUnit")
                                                         select new SupplierInvoiceItemList()
                                                     {
                                                         ClassificationCode = a.ClassificationCode,
                                                         CustomsBookTypeCode = a.CustomsBookTypeCode,
                                                         DangerousClassificationCode = a.DangerousClassificationCode,
                                                         DangerousPackingGroupTypeCode = a.DangerousPackingGroupTypeCode,
                                                         DeclarationId = a.DeclarationId,
                                                         SequenceNumeric = a.SequenceNumeric,
                                                         CounterKey = a.CounterKey,
                                                         ItemCode = a.ItemCode,
                                                         ItemPrice = a.ItemPrice,
                                                         ManufactureIdentifier = a.ManufactureIdentifier,
                                                         NonCustomsItemPrice = a.NonCustomsItemPrice,
                                                         OptionalTamaPercentage = a.OptionalTamaPercentage,
                                                         OriginCountryCode = a.OriginCountryCode,
                                                        // PROCESS_TYPE = a.PROCESS_TYPE,
                                                         SalesTaxExemptionTypeCode = a.SalesTaxExemptionTypeCode,
                                                         TradeAgreementCode = a.TradeAgreementCode,
                                                         TaxExemptCode = a.TaxExemptCode,
                                                         WholeSaleItemPrice = a.WholeSaleItemPrice,
                                                         ItemPriceCurrencyCode = a.ItemPriceCurrencyCode,
                                                         LineNumber = a.LineNumber,
                                                         NonCustomsItemPriceCurCode = a.NonCustomsItemPriceCurCode,
                                                         ActualInvoiceLines = a.ActualInvoiceLines,
                                                         TradeAgreementName = a.TradeAgreement.LocalName,
                                                         OriginCountryName = a.OriginCountry.LocalName,
                                                         InvoiceQuantity = a.InvoiceQuantity,
                                                         InvoiceQuantityType=a.InvoiceQuantityType,
                                                         InvoiceQuantityTypeName = a.InvoiceMeasurmentUnit != null ? (a.InvoiceMeasurmentUnit.LocalName != null ? a.InvoiceMeasurmentUnit.LocalName : a.InvoiceMeasurmentUnit.EnglishName) : null,
                                                         AdditionalQuantity = a.AdditionalQuantity,
                                                         AdditionalQuantityType = a.AdditionalQuantityType,
                                                         AdditionalQuantityTypeName = a.AdditionalMeasurmentUnit != null ? (a.AdditionalMeasurmentUnit.LocalName != null ? a.AdditionalMeasurmentUnit.LocalName : a.AdditionalMeasurmentUnit.EnglishName) : null,
                                                             Tenant = a.Tenant,


                                                         });
            return query;
        }

        private IQueryable<SupplierInvoiceItem> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvoiceItem> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<SupplierInvoiceItemList> GetSupplierInvoiceItemsClasifiedRemarks(string entityParentId, int tenant)
        {



            List<SupplierInvoiceItemList> invoiceItemsLists = (from a in context.SupplierInvoiceItems.Include("TradeAgreement").Include("OriginCountry")
                                                               where a.DeclarationId == entityParentId && a.ClasifiedRemarks != null && a.Tenant == tenant
                                                               select new SupplierInvoiceItemList()
                                                               {
                                                                   ClassificationCode = a.ClassificationCode,
                                                                   CustomsBookTypeCode = a.CustomsBookTypeCode,
                                                                   DangerousClassificationCode = a.DangerousClassificationCode,
                                                                   DangerousPackingGroupTypeCode = a.DangerousPackingGroupTypeCode,
                                                                   DeclarationId = a.DeclarationId,
                                                                   Tenant = a.Tenant,
                                                                   CounterKey = a.CounterKey,
                                                                   ItemCode = a.ItemCode,
                                                                   ItemPrice = a.ItemPrice,
                                                                   ManufactureIdentifier = a.ManufactureIdentifier,
                                                                   NonCustomsItemPrice = a.NonCustomsItemPrice,
                                                                   OptionalTamaPercentage = a.OptionalTamaPercentage,
                                                                   OriginCountryCode = a.OriginCountryCode,
                                                                   //   PROCESS_TYPE = a.PROCESS_TYPE,
                                                                   SalesTaxExemptionTypeCode = a.SalesTaxExemptionTypeCode,
                                                                   TradeAgreementCode = a.TradeAgreementCode,
                                                                   TaxExemptCode = a.TaxExemptCode,
                                                                   WholeSaleItemPrice = a.WholeSaleItemPrice,
                                                                   ItemPriceCurrencyCode = a.ItemPriceCurrencyCode,
                                                                   LineNumber = a.LineNumber,
                                                                   NonCustomsItemPriceCurCode = a.NonCustomsItemPriceCurCode,
                                                                   ActualInvoiceLines = a.ActualInvoiceLines,
                                                                   TradeAgreementName = a.TradeAgreement.LocalName,
                                                                   OriginCountryName = a.OriginCountry.LocalName,
                                                                   InvoiceQuantity = a.InvoiceQuantity,
                                                                   InvoiceQuantityType = a.InvoiceQuantityType,
                                                                   StatisticQuantity = a.StatisticQuantity,
                                                                   StatisticQuantityType = a.StatisticQuantityType,
                                                                   SequenceNumeric = a.SequenceNumeric,
                                                                   IsParent = a.IsParent,
                                                                   ClasifiedRemarks = a.ClasifiedRemarks,
                                                                   InvoiceNumber=a.SupplierInvoice.InvoiceNumber,


                                                               }).ToList();
            return invoiceItemsLists.OrderBy(d => d.SequenceNumeric).ToList();


        }

        public List<SupplierInvoiceItemList> GetSupplierInvoiceItemsForInvoices(string entityParentId, string keys, int tenant)
        {
            List<int> intKeys = new List<int>();
            string[] keysArray = keys.Split(',');
            foreach (string key in keysArray)
            {
                int keyInt;
                int.TryParse(key, out keyInt);
                intKeys.Add(keyInt);
            }

            List<SupplierInvoiceItemList> invoiceItemsLists = (from a in context.SupplierInvoiceItems.Include("TradeAgreement").Include("OriginCountry")
                                                               where a.DeclarationId == entityParentId && intKeys.Contains(a.CounterKey) && a.Tenant == tenant
                                                               select new SupplierInvoiceItemList()
                                                               {
                                                                   ClassificationCode = a.ClassificationCode,
                                                                   CustomsBookTypeCode = a.CustomsBookTypeCode,
                                                                   DangerousClassificationCode = a.DangerousClassificationCode,
                                                                   DangerousPackingGroupTypeCode = a.DangerousPackingGroupTypeCode,
                                                                   DeclarationId = a.DeclarationId,
                                                                   Tenant = a.Tenant,
                                                                   CounterKey = a.CounterKey,
                                                                   ItemCode = a.ItemCode,
                                                                   ItemPrice = a.ItemPrice,
                                                                   ManufactureIdentifier = a.ManufactureIdentifier,
                                                                   NonCustomsItemPrice = a.NonCustomsItemPrice,
                                                                   OptionalTamaPercentage = a.OptionalTamaPercentage,
                                                                   OriginCountryCode = a.OriginCountryCode,
                                                                   //   PROCESS_TYPE = a.PROCESS_TYPE,
                                                                   SalesTaxExemptionTypeCode = a.SalesTaxExemptionTypeCode,
                                                                   TradeAgreementCode = a.TradeAgreementCode,
                                                                   TaxExemptCode = a.TaxExemptCode,
                                                                   WholeSaleItemPrice = a.WholeSaleItemPrice,
                                                                   ItemPriceCurrencyCode = a.ItemPriceCurrencyCode,
                                                                   LineNumber = a.LineNumber,
                                                                   NonCustomsItemPriceCurCode = a.NonCustomsItemPriceCurCode,
                                                                   ActualInvoiceLines = a.ActualInvoiceLines,
                                                                   TradeAgreementName = a.TradeAgreement.LocalName,
                                                                   OriginCountryName = a.OriginCountry.LocalName,
                                                                   InvoiceQuantity = a.InvoiceQuantity,
                                                                   InvoiceQuantityType = a.InvoiceQuantityType,
                                                                   StatisticQuantity = a.StatisticQuantity,
                                                                   StatisticQuantityType = a.StatisticQuantityType,
                                                                   SequenceNumeric = a.SequenceNumeric

                                                               }).ToList();
            return invoiceItemsLists;


        }

        public List<SupplierInvoiceItemList> GetSupplierInvoiceItemsForInvoice(string entityParentId, int counterKey, int tenant)
        {



            List<SupplierInvoiceItemList> invoiceItemsLists = (from a in context.SupplierInvoiceItems.Include("TradeAgreement").Include("OriginCountry")
                                                               where a.DeclarationId == entityParentId && a.CounterKey == counterKey && a.Tenant == tenant
                                                               select new SupplierInvoiceItemList()
                                                               {
                                                                   ClassificationCode = a.ClassificationCode,
                                                                   CustomsBookTypeCode = a.CustomsBookTypeCode,
                                                                   DangerousClassificationCode = a.DangerousClassificationCode,
                                                                   DangerousPackingGroupTypeCode = a.DangerousPackingGroupTypeCode,
                                                                   DeclarationId = a.DeclarationId,
                                                                   Tenant = a.Tenant,
                                                                   CounterKey = a.CounterKey,
                                                                   ItemCode = a.ItemCode,
                                                                   ItemPrice = a.ItemPrice,
                                                                   ManufactureIdentifier = a.ManufactureIdentifier,
                                                                   NonCustomsItemPrice = a.NonCustomsItemPrice,
                                                                   OptionalTamaPercentage = a.OptionalTamaPercentage,
                                                                   OriginCountryCode = a.OriginCountryCode,
                                                                   //   PROCESS_TYPE = a.PROCESS_TYPE,
                                                                   SalesTaxExemptionTypeCode = a.SalesTaxExemptionTypeCode,
                                                                   TradeAgreementCode = a.TradeAgreementCode,
                                                                   TaxExemptCode = a.TaxExemptCode,
                                                                   WholeSaleItemPrice = a.WholeSaleItemPrice,
                                                                   ItemPriceCurrencyCode = a.ItemPriceCurrencyCode,
                                                                   LineNumber = a.LineNumber,
                                                                   NonCustomsItemPriceCurCode = a.NonCustomsItemPriceCurCode,
                                                                   ActualInvoiceLines = a.ActualInvoiceLines,
                                                                   TradeAgreementName = a.TradeAgreement.LocalName,
                                                                   OriginCountryName = a.OriginCountry.LocalName,
                                                                   InvoiceQuantity = a.InvoiceQuantity,
                                                                   InvoiceQuantityType = a.InvoiceQuantityType,
                                                                   StatisticQuantity = a.StatisticQuantity,
                                                                   StatisticQuantityType = a.StatisticQuantityType,
                                                                   SequenceNumeric = a.SequenceNumeric,
                                                                   IsParent = a.IsParent


                                                               }).ToList();
            return invoiceItemsLists.OrderBy(d => d.SequenceNumeric).ToList();


        }

        public IQueryable<SupplierInvoiceItemList> GetSupplierInvoiceItemsForInvoices(string entityParentId, string keys, int tenant, int skip, int take)
        {
            IQueryable<SupplierInvoiceItemList> invoiceItemsLists = null;
            List<int> intKeys = new List<int>();
            if (keys != null)
            {
                string[] keysArray = keys.Split(',');
                foreach (string key in keysArray)
                {
                    int keyInt;
                    int.TryParse(key, out keyInt);
                    intKeys.Add(keyInt);
                }


                invoiceItemsLists = (from a in context.SupplierInvoiceItems.Include("TradeAgreement").Include("OriginCountry")
                                     where a.DeclarationId == entityParentId && intKeys.Contains(a.CounterKey) && a.Tenant == tenant && !a.IsParent
                                     select new SupplierInvoiceItemList()
                                     {
                                         ClassificationCode = a.ClassificationCode,
                                         CustomsBookTypeCode = a.CustomsBookTypeCode,
                                         DangerousClassificationCode = a.DangerousClassificationCode,
                                         DangerousPackingGroupTypeCode = a.DangerousPackingGroupTypeCode,
                                         DeclarationId = a.DeclarationId,
                                         Tenant = a.Tenant,
                                         CounterKey = a.CounterKey,
                                         ItemCode = a.ItemCode,
                                         ItemPrice = a.ItemPrice,
                                         ManufactureIdentifier = a.ManufactureIdentifier,
                                         NonCustomsItemPrice = a.NonCustomsItemPrice,
                                         OptionalTamaPercentage = a.OptionalTamaPercentage,
                                         OriginCountryCode = a.OriginCountryCode,
                                         //   PROCESS_TYPE = a.PROCESS_TYPE,
                                         SalesTaxExemptionTypeCode = a.SalesTaxExemptionTypeCode,
                                         TradeAgreementCode = a.TradeAgreementCode,
                                         TaxExemptCode = a.TaxExemptCode,
                                         WholeSaleItemPrice = a.WholeSaleItemPrice,
                                         ItemPriceCurrencyCode = a.ItemPriceCurrencyCode,
                                         LineNumber = a.LineNumber,
                                         NonCustomsItemPriceCurCode = a.NonCustomsItemPriceCurCode,
                                         ActualInvoiceLines = a.ActualInvoiceLines,
                                         TradeAgreementName = a.TradeAgreement.LocalName,
                                         OriginCountryName = a.OriginCountry.LocalName,
                                         InvoiceQuantity = a.InvoiceQuantity,
                                         InvoiceQuantityType = a.InvoiceQuantityType,
                                         StatisticQuantity = a.StatisticQuantity,
                                         StatisticQuantityType = a.StatisticQuantityType,
                                         SequenceNumeric = a.SequenceNumeric,
                                         IsParent = a.IsParent

                                     });

            }

            return invoiceItemsLists;




        }

        public IQueryable<SupplierInvoiceItemList> GetSelectedSupplierInvoiceItems(string declarationId, string counterKeys, string lineNumbers, int tenant)
        {
            IQueryable<SupplierInvoiceItemList> invoiceItemsLists = null;
            List<int> intKeys = new List<int>();
            List<int> intLines = new List<int>();
            if (counterKeys != null && lineNumbers != null)
            {
                string[] keysArray = counterKeys.Split(',');
                foreach (string key in keysArray)
                {
                    int keyInt;
                    int.TryParse(key, out keyInt);
                    intKeys.Add(keyInt);
                }


                string[] linesArray = lineNumbers.Split(',');
                foreach (string line in linesArray)
                {
                    int keyInt;
                    int.TryParse(line, out keyInt);
                    intLines.Add(keyInt);
                }

                invoiceItemsLists = (from a in context.SupplierInvoiceItems.Include("TradeAgreement").Include("OriginCountry")
                                     where a.DeclarationId == declarationId && intKeys.Contains(a.CounterKey) && intLines.Contains(a.LineNumber) && a.Tenant == tenant && !a.IsParent
                                     select new SupplierInvoiceItemList()
                                     {
                                         ClassificationCode = a.ClassificationCode,
                                         CustomsBookTypeCode = a.CustomsBookTypeCode,
                                         DangerousClassificationCode = a.DangerousClassificationCode,
                                         DangerousPackingGroupTypeCode = a.DangerousPackingGroupTypeCode,
                                         DeclarationId = a.DeclarationId,
                                         Tenant = a.Tenant,
                                         CounterKey = a.CounterKey,
                                         ItemCode = a.ItemCode,
                                         ItemPrice = a.ItemPrice,
                                         ManufactureIdentifier = a.ManufactureIdentifier,
                                         NonCustomsItemPrice = a.NonCustomsItemPrice,
                                         OptionalTamaPercentage = a.OptionalTamaPercentage,
                                         OriginCountryCode = a.OriginCountryCode,
                                         //   PROCESS_TYPE = a.PROCESS_TYPE,
                                         SalesTaxExemptionTypeCode = a.SalesTaxExemptionTypeCode,
                                         TradeAgreementCode = a.TradeAgreementCode,
                                         TaxExemptCode = a.TaxExemptCode,
                                         WholeSaleItemPrice = a.WholeSaleItemPrice,
                                         ItemPriceCurrencyCode = a.ItemPriceCurrencyCode,
                                         LineNumber = a.LineNumber,
                                         NonCustomsItemPriceCurCode = a.NonCustomsItemPriceCurCode,
                                         ActualInvoiceLines = a.ActualInvoiceLines,
                                         TradeAgreementName = a.TradeAgreement.LocalName,
                                         OriginCountryName = a.OriginCountry.LocalName,
                                         InvoiceQuantity = a.InvoiceQuantity,
                                         InvoiceQuantityType = a.InvoiceQuantityType,
                                         StatisticQuantity = a.StatisticQuantity,
                                         StatisticQuantityType = a.StatisticQuantityType,
                                         SequenceNumeric = a.SequenceNumeric,
                                         IsParent = a.IsParent

                                     });



               
            }
            return invoiceItemsLists;
        }

    }


}
	