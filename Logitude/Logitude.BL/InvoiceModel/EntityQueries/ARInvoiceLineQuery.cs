using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARInvoiceLineQuery
    {
        private ARInvoiceLineRepository repository;

        public ARInvoiceLineQuery(int tenant)
        {
            repository = new ARInvoiceLineRepository(tenant);
        }

        public ARInvoiceLineQuery(ARInvoiceLineRepository arInvoiceLineRepository)
        {
            repository = arInvoiceLineRepository;
        }

        public List<ARInvoiceLinePM> GetInvoiceLinePMsByInvoiceId(string invoiceId, int tenant)
        {
            List<ARInvoiceLinePM> list = (from a in repository.context.ARInvoiceLines.Include("ARInvoiceLineAction")
                                          where a.Tenant == tenant && a.ARInvoiceId == invoiceId
                                          select new ARInvoiceLinePM()
                                          {
                                              ARInvoiceId = a.ARInvoiceId,
                                              ForiegnCurrencyAmount = a.ForiegnCurrencyAmount,
                                              InvoiceCurrencyAmount = a.InvoiceCurrencyAmount,
                                              LocalCurrencyAmount = a.LocalCurrencyAmount,
                                              ChargesTypeId = a.ChargesTypeId,                                              
                                              ForiegnCurrencyId = a.ForiegnCurrencyId,                                              
                                              Id = a.Id,
                                              EntityId = a.EntityId,
                                              ForiegnExchangeRate = a.ForiegnExchangeRate,
                                              Tenant = a.Tenant,
                                              LineNumber = a.LineNumber,
                                              ReceivableId = a.ReceivableId,
                                              MeasurementId = a.MeasurementId,
                                              Quantity = a.Quantity,
                                              UnitPrice = a.UnitPrice,
                                              IsExchangeRateFixed = a.IsExchangeRateFixed,                                              
                                              ProfitCurrencyAmount = a.ProfitCurrencyAmount,
                                              InvoiceCurrencyCode = a.ARInvoice == null ? "" : (a.ARInvoice.InvoiceCurrency == null ? "" : a.ARInvoice.InvoiceCurrency.Code),
                                              InvoiceLocalCurrencyCode = a.ARInvoice == null ? "" : (a.ARInvoice.LocalCurrency == null ? "" : a.ARInvoice.LocalCurrency.Code),
                                              MeasurementCode = a.Measurement == null ? "" : a.Measurement.Code,
                                              ExchangeRateDate = a.ExchangeRateDate,
                                              CreditAccount = a.CreditAccount,
                                              Description = a.Description,
                                              LocalDescription = a.LocalDescription,
                                              Notes = a.Notes,
                                              DateForInterest = a.DateForInterest,
                                              ValueDate = a.ValueDate,
                                              GLAccountId = a.GLAccountId,
                                              LineActionCode = a.LineActionCode,
                                              VatTypeId = a.VatTypeId,
                                              VatPercentage = a.VatPercentage,
                                              IsBackToBack = a.IsBackToBack,
                                              IsExpense = a.IsExpense,
                                              PrepaidCollectId = a.PrepaidCollectId
                                          }).ToList();

            ShipmentReceivableRepository receivableRepository = new ShipmentReceivableRepository(tenant);
            ARInvoiceEntityRepository arInvoiceEntityRepository = new ARInvoiceEntityRepository(tenant);
            ARInvoiceTotalVATRepository invoiceTotalVatRepository = new ARInvoiceTotalVATRepository(tenant);
            
            List<ARInvoiceTotalVAT> totalVats = invoiceTotalVatRepository.GetInvoiceTotalVatsForInvoice(invoiceId, tenant).ToList();

            foreach (ARInvoiceLinePM invoiceLinePM in list)
            {
                ARInvoiceTotalVAT singleTotalVat = totalVats.Where(d => d.ARInvoiceId == invoiceId && d.VatTypeId == invoiceLinePM.VatTypeId).FirstOrDefault();
                if (singleTotalVat != null)
                {
                    invoiceLinePM.ExternalVATCard = singleTotalVat.ExternalVATCard;
                    invoiceLinePM.ExternalTAXItemId = singleTotalVat.ExternalTAXItemId;                 
                }

                Currency currency = CurrencyRepository.GetSingleCurrency(invoiceLinePM.ForiegnCurrencyId, invoiceLinePM.Tenant, true);
                if (currency != null)
                {
                    invoiceLinePM.ForiegnCurrencyCode = currency.Code;
                }

                ShipmentReceivable receivable = receivableRepository.GetSingleShipmentReceivable(invoiceLinePM.ReceivableId, invoiceLinePM.Tenant);
                if (receivable != null)
                {
                    invoiceLinePM.PrepaidCollectId = receivable.PrepaidCollectId;
                }

                ChargesType charge = ChargesTypeRepository.GetSingleChargesType(invoiceLinePM.ChargesTypeId, invoiceLinePM.Tenant, true);
                if (charge != null)
                {
                    invoiceLinePM.ViewOrder = charge.ViewOrder;
                    invoiceLinePM.IsCustomsCharge = charge.IsCustoms;
                }

                ARInvoiceEntity invoiceEntity = arInvoiceEntityRepository.GetSingleInvoiceEntityByInvoiceAndEntity(invoiceLinePM.ARInvoiceId, invoiceLinePM.EntityId, invoiceLinePM.Tenant);
                if (invoiceEntity != null)
                {
                    invoiceLinePM.EntityReference = invoiceEntity.EntityReference;
                    //invoiceLinePM.ObjectTableId = invoiceEntity.ObjectTableId;
                }

                VatType vattype = VatTypeRepository.GetSingleVatType(invoiceLinePM.VatTypeId, invoiceLinePM.Tenant, true);
                if (vattype != null)
                {
                    invoiceLinePM.VatTypeName = vattype.EnglishName;
                    invoiceLinePM.VatIsMultiPercentage = vattype.IsMultiPercentage;
                }
            }

            return list.OrderBy(d => d.ViewOrder).ToList();
        }

        public ARInvoiceLinePM GetSinglePM(string id, int tenant)
        {
            ARInvoiceLinePM myResult = (from a in repository.context.ARInvoiceLines.Include("VatType")
                                        where a.Id == id
                                        select new ARInvoiceLinePM()
                                        {
                                            ARInvoiceId = a.ARInvoiceId,
                                            ForiegnCurrencyId = a.ForiegnCurrencyId,
                                            ForiegnCurrencyAmount = a.ForiegnCurrencyAmount,
                                            InvoiceCurrencyAmount = a.InvoiceCurrencyAmount,
                                            LocalCurrencyAmount = a.LocalCurrencyAmount,
                                            ChargesTypeId = a.ChargesTypeId,
                                            Id = a.Id,
                                            ForiegnExchangeRate = a.ForiegnExchangeRate,
                                            Tenant = a.Tenant,
                                            LineNumber = a.LineNumber,
                                            ReceivableId = a.ReceivableId,
                                            MeasurementId = a.MeasurementId,
                                            Quantity = a.Quantity,
                                            UnitPrice = a.UnitPrice,
                                            IsExchangeRateFixed = a.IsExchangeRateFixed,
                                            ProfitCurrencyAmount = a.ProfitCurrencyAmount,
                                            ExchangeRateDate = a.ExchangeRateDate,
                                            CreditAccount = a.CreditAccount,
                                            Description = a.Description,
                                            LocalDescription = a.LocalDescription,
                                            Notes = a.Notes,
                                            DateForInterest = a.DateForInterest,
                                            ValueDate = a.ValueDate,
                                            GLAccountId = a.GLAccountId,
                                            LineActionCode = a.LineActionCode,
                                            VatTypeId = a.VatTypeId,
                                            VatPercentage = a.VatPercentage,
                                            VatTypeName = a.VatType == null ? null : a.VatType.EnglishName,
                                            VatIsMultiPercentage = a.VatType == null ? false : a.VatType.IsMultiPercentage,
                                            IsExpense = a.IsExpense,
                                            PrepaidCollectId = a.PrepaidCollectId,
                                        }).FirstOrDefault();

            ShipmentReceivableRepository receivableRepository = new ShipmentReceivableRepository(tenant);
            ARInvoiceEntityRepository arInvoiceEntityRepository = new ARInvoiceEntityRepository(tenant);
            ARInvoiceTotalVATRepository invoiceTotalVatRepository = new ARInvoiceTotalVATRepository(tenant);

            IQueryable<ARInvoiceTotalVAT> totalVats = invoiceTotalVatRepository.GetInvoiceTotalVatsForInvoice(myResult.ARInvoiceId, tenant);

            ARInvoiceTotalVAT singleTotalVat = totalVats.Where(d => d.ARInvoiceId == myResult.ARInvoiceId && d.VatTypeId == myResult.VatTypeId).FirstOrDefault();
            if (singleTotalVat != null)
            {
                myResult.ExternalVATCard = singleTotalVat.ExternalVATCard;
                myResult.ExternalTAXItemId = singleTotalVat.ExternalTAXItemId;
            }

            Currency cur = CurrencyRepository.GetSingleCurrency(myResult.ForiegnCurrencyId, tenant, true);
            if (cur != null)
            {
                myResult.ForiegnCurrencyCode = cur.Code;
            }

            ShipmentReceivable receivable = receivableRepository.GetSingleShipmentReceivable(myResult.ReceivableId, tenant);
            if (receivable != null)
            {
                myResult.PrepaidCollectId = receivable.PrepaidCollectId;
            }

            ChargesType charge = ChargesTypeRepository.GetSingleChargesType(myResult.ChargesTypeId, tenant, true);
            if (charge != null)
            {
                myResult.ViewOrder = charge.ViewOrder;
            }

            ARInvoiceEntity invoiceEntity = arInvoiceEntityRepository.GetSingleInvoiceEntityByInvoiceAndEntity(myResult.ARInvoiceId, myResult.EntityId, tenant);
            if (invoiceEntity != null)
            {
                myResult.EntityReference = invoiceEntity.EntityReference;
                //myResult.ObjectTableId = invoiceEntity.ObjectTableId;
            }

            VatType vattype = VatTypeRepository.GetSingleVatType(myResult.VatTypeId, tenant, true);
            if (vattype != null)
            {
                myResult.VatTypeName = vattype.EnglishName;
            }

            return myResult;
        }

        public List<ARInvoiceLinePM> GetInvoiceLinePMsByInvoiceIds(List<string> invoiceIds, int tenant)
        {
            List<ARInvoiceLinePM> list = (from a in repository.context.ARInvoiceLines
                                          where a.Tenant == tenant && invoiceIds.Contains(a.ARInvoiceId)
                                          select new ARInvoiceLinePM()
                                          {
                                              ARInvoiceId = a.ARInvoiceId,
                                              ForiegnCurrencyAmount = a.ForiegnCurrencyAmount,
                                              InvoiceCurrencyAmount = a.InvoiceCurrencyAmount,
                                              LocalCurrencyAmount = a.LocalCurrencyAmount,
                                              ChargesTypeId = a.ChargesTypeId,
                                              ForiegnCurrencyId = a.ForiegnCurrencyId,
                                              Id = a.Id,
                                              EntityId = a.EntityId,
                                              ForiegnExchangeRate = a.ForiegnExchangeRate,
                                              Tenant = a.Tenant,
                                              LineNumber = a.LineNumber,
                                              ReceivableId = a.ReceivableId,
                                              MeasurementId = a.MeasurementId,
                                              Quantity = a.Quantity,
                                              UnitPrice = a.UnitPrice,
                                              IsExchangeRateFixed = a.IsExchangeRateFixed,
                                              ProfitCurrencyAmount = a.ProfitCurrencyAmount,
                                              InvoiceCurrencyCode = a.ARInvoice == null ? "" : (a.ARInvoice.InvoiceCurrency == null ? "" : a.ARInvoice.InvoiceCurrency.Code),
                                              InvoiceLocalCurrencyCode = a.ARInvoice == null ? "" : (a.ARInvoice.LocalCurrency == null ? "" : a.ARInvoice.LocalCurrency.Code),
                                              MeasurementCode = a.Measurement == null ? "" : a.Measurement.Code,
                                              ExchangeRateDate = a.ExchangeRateDate,
                                              CreditAccount = a.CreditAccount,
                                              Description = a.Description,
                                              LocalDescription = a.LocalDescription,
                                              Notes = a.Notes,
                                              DateForInterest = a.DateForInterest,
                                              ValueDate = a.ValueDate,
                                              GLAccountId = a.GLAccountId,
                                              LineActionCode = a.LineActionCode,
                                              VatTypeId = a.VatTypeId,
                                              VatPercentage = a.VatPercentage,
                                              IsBackToBack = a.IsBackToBack,
                                              IsExpense = a.IsExpense,
                                              PrepaidCollectId = a.PrepaidCollectId,
                                          }).ToList();
            return list;
        }
    }
}
