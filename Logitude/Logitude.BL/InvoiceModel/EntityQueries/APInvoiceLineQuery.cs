using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class APInvoiceLineQuery
    {       
        private APInvoiceLineRepository repository;
        
        public APInvoiceLineQuery()
        {
            repository = new APInvoiceLineRepository(); 
        }

        public APInvoiceLineQuery(int tenant)
        {
            repository = new APInvoiceLineRepository(tenant);
        }

        public APInvoiceLineQuery(APInvoiceLineRepository apInvoiceLineRepository)
        {
            repository = apInvoiceLineRepository;
        }

        public List<APInvoiceLinePM> GetInvoiceLinesByInvoiceId(string invoiceId, int tenant)
        {
            List<APInvoiceLine> myData = (from a in repository.context.APInvoiceLines.Include("Currency")
                                          where a.Tenant == tenant
                                          && a.APInvoiceId == invoiceId
                                          select a).ToList();

            List<APInvoiceLinePM> myResult = this.MapToPM(myData, invoiceId, tenant);

            return myResult;
        }

        public List<APInvoiceLinePM> GetAPInvoiceLinesByEntityId(string invoiceId, string entityId, int tenant)
        {
            List<APInvoiceLine> myData = (from a in repository.context.APInvoiceLines
                                          where a.Tenant == tenant
                                          && a.APInvoiceId == invoiceId
                                          && a.EntityId == entityId
                                          select a).ToList();

            List<APInvoiceLinePM> myResult = this.MapToPM(myData, invoiceId, tenant);
            return myResult;
        }

        public List<PayableInvoiceClass> GetPayableInvoices(string payableId, string parentPayableId, int tenant)
        {
            List<string> ids = new List<string>();
            List<PayableInvoiceClass> myResult = new List<PayableInvoiceClass>();

            if (!string.IsNullOrEmpty(payableId))
            {
                List<string> myIds = (from a in repository.context.APInvoiceLines
                                    where a.EntityPayableId == payableId && a.Tenant == tenant
                                    group a by a.APInvoiceId into d
                                    select d.Key).ToList();

                foreach (string item in myIds)
                {
                    if (!ids.Contains(item))
                    {
                        ids.Add(item);
                    }
                }
            }

            if (!string.IsNullOrEmpty(parentPayableId))
            {
                List<string> myIds = (from a in repository.context.APInvoiceLines
                                      where a.EntityPayableId == parentPayableId && a.Tenant == tenant
                                      group a by a.APInvoiceId into d
                                      select d.Key).ToList();

                foreach (string item in myIds)
                {
                    if (!ids.Contains(item))
                    {
                        ids.Add(item);
                    }
                }
            }

            if (ids.Count > 0)
            {
                myResult = (from a in repository.context.APInvoices.Include("Status").Include("LocalCurrency").Include("InvoiceCurrency")
                            where ids.Contains(a.Id)
                            select new PayableInvoiceClass()
                            {
                                Id = a.Id,
                                DueDate = a.DueDate,
                                InvoiceNumber = a.InvoiceNumber,
                                StatusName = a.Status == null ? null : a.Status.Name,
                                LocalCurrencyId = a.LocalCurrencyId,
                                InvoiceCurrencyId = a.InvoiceCurrencyId,
                                LocalCurrencyCode = a.LocalCurrency == null ? null : a.LocalCurrency.Code,
                                InvoiceCurrencyCode = a.InvoiceCurrency == null ? null : a.InvoiceCurrency.Code,
                                GrandTotalInInvoiceCurrency = a.AmountInInvoiceCurrency,
                                GrandTotalInLocalCurrency = a.AmountInLocalCurrency,
                            }).ToList();
            }

            return myResult;
        }
        public APInvoiceLinePM GetSingle(string apinvoiceId, int lineNumber)
        {
            List<APInvoiceLine> invoices
                = (from a in repository.context.APInvoiceLines
                   where a.APInvoiceId == apinvoiceId
                   && a.LineNumber == lineNumber
                   select a).ToList();

            if (invoices.Count > 1)
                throw new ApplicationException("GetSinglePMByAPInvoiceIdAndLineNumber has wrong data!");
            APInvoiceLine InvoiceLine = invoices.FirstOrDefault();
            int tenant = 0;
            if (InvoiceLine != null)
            {
                tenant = InvoiceLine.Tenant;
            }
            
            List<APInvoiceLinePM> invoicePMs = MapToPM(invoices, apinvoiceId, tenant);
            APInvoiceLinePM invoicePM = invoicePMs.FirstOrDefault();

            return invoicePM;
        }

        public APInvoiceLinePM GetSinglePMByAPInvoiceIdAndLineNumber(string apinvoiceId,int lineNumber, int tenant)
        {
            List<APInvoiceLine> invoices
                = ( from a in repository.context.APInvoiceLines
                    where a.Tenant == tenant
                    && a.APInvoiceId == apinvoiceId
                    && a.LineNumber == lineNumber
                    select a).ToList();

            if (invoices.Count > 1)
                throw new ApplicationException("GetSinglePMByAPInvoiceIdAndLineNumber has wrong data!");

            List<APInvoiceLinePM> invoicePMs = MapToPM(invoices, apinvoiceId, tenant);
            APInvoiceLinePM invoicePM = invoicePMs.FirstOrDefault();

            return invoicePM;
        }

        private List<APInvoiceLinePM> MapToPM(List<APInvoiceLine> myData, string invoiceId, int tenant)
        {
            List<APInvoiceLinePM> myResult = new List<APInvoiceLinePM>();

            if (myData.Count > 0)
            {
                myResult = (from a in myData
                            select new APInvoiceLinePM()
                            {
                                APInvoiceId = a.APInvoiceId,
                                ChargesTypeId = a.ChargesTypeId,
                                InvoiceCurrencyAmount = a.InvoiceCurrencyAmount,
                                LineNumber = a.LineNumber,
                                LocalCurrencyAmount = a.LocalCurrencyAmount,
                                Notes = a.Notes,
                                ProfitCurrencyAmount = a.ProfitCurrencyAmount,
                                Tenant = a.Tenant,
                                EntityId = a.EntityId,
                                EntityPayableId = a.EntityPayableId,
                                PayableDebitGLAcountId = a.PayableDebitGLAcountId,

                                RefundAmount = a.RefundAmount,
                                ForiegnCurrencyId = a.ForiegnCurrencyId,
                                ForiegnCurrencyCode = a.Currency?.Code,
                                ForiegnExchangeRate = a.ForiegnExchangeRate,
                                ForiegnCurrencyAmount = a.ForiegnCurrencyAmount,
                                DebitAccount = a.DebitAccount,
                                Description = a.Description,
                                LocalDescription = a.LocalDescription,
                                ChargeTypeGLAccountId = a.ChargeTypeGLAccountId,
                                AuthorizedSignatory = a.AuthorizedSignatory,
                                VatTypeId = a.VatTypeId,
                                VatPercentage = a.VatPercentage,
                                PrepaidCollectId = a.PrepaidCollectId,
                                ContainerTypeId = a.ContainerTypeId,
                                Quantity = a.Quantity,
                                ExcludeFromTaxReport = a.ExcludeFromTaxReport,
                                IsPrepaidExpenses = a.IsPrepaidExpenses,
                            }).ToList();

                ShipmentPayableRepository payableRepository = new ShipmentPayableRepository(tenant);
                APInvoiceTotalVATRepository invoiceTotalVatRepository = new APInvoiceTotalVATRepository(tenant);
                GLAccountRepository gLAccountRepository = new GLAccountRepository(tenant);

                List<APInvoiceTotalVAT> totalVats = invoiceTotalVatRepository.GetInvoiceTotalVatsByInvoiceId(invoiceId, tenant).ToList();

                foreach (APInvoiceLinePM item in myResult)
                {
                    APInvoiceTotalVAT singleTotalVat = totalVats.Where(d => d.APInvoiceId == item.APInvoiceId && d.VatTypeId == item.VatTypeId).FirstOrDefault();
                    if (singleTotalVat != null)
                    {
                        item.ExternalVATCard = singleTotalVat.ExternalVATCard;
                        item.ExternalTAXItemId = singleTotalVat.ExternalTAXItemId;
                    }

                    ShipmentPayable payable = payableRepository.GetSingleShipmentPayable(item.EntityPayableId);
                    if (payable != null)
                    {
                        item.ForiegnCurrencyCode = payable.Currency.Code;
                        item.ExpectedAmount = payable.ExpectedAmount;
                        item.OtherInvoicesAmounts = payable.AccountedAmount - item.ForiegnCurrencyAmount;
                        item.OpenAmount = payable.OpenAmount;
                        item.CorrectionByUserId = payable.CorrectionByUserId;
                        item.CorrectionAmount = payable.CorrectionAmount;
                        item.CorrectionDate = payable.CorrectionDate;
                        item.CorrectionNote = payable.CorrectionNote;
                        item.VendorId = payable.VendorId;
                        item.AmountTypeCode = payable.ShipmentPayableAmountTypeCode;

                        Card vendorCard = CardRepository.GetSingleCard(item.VendorId, item.Tenant, true);
                        if (vendorCard != null)
                        {
                            item.VendorName = vendorCard.EnglishName;
                        }
                    }
                    

                   
                    if (!string.IsNullOrEmpty(item.VatTypeId))
                    {
                        VatType vatType = VatTypeRepository.GetSingleVatType(item.VatTypeId, tenant, true);
                        if (vatType != null)
                        {
                            item.VatTypeName = vatType.EnglishName;
                            item.VatIsMultiPercentage = vatType.IsMultiPercentage;
                            item.VatRecognizedPercentage = (vatType.RecognizedPercentage != null  && vatType.RecognizedPercentage != 0 )? vatType.RecognizedPercentage / 100: vatType.RecognizedPercentage ;
                        }
                    }

                    if (!string.IsNullOrEmpty(item.ChargesTypeId))
                    {
                        ChargesType chargesType = ChargesTypeRepository.GetSingleChargesType(item.ChargesTypeId, tenant, true);
                        
                        if (chargesType != null)
                        {
                            item.ChargesTypeCode = chargesType.Code;
                            item.ChargesTypeName = chargesType.EnglishName;
                            if (!string.IsNullOrEmpty(chargesType.PayableDebitGLAcountId) && string.IsNullOrEmpty(item.PayableDebitGLAcountId))
                            {
                                item.PayableDebitGLAcountId = chargesType.PayableDebitGLAcountId;
                            }
                        }
                    }

                    GLAccount PayableDebitGLAcount = gLAccountRepository.GetSingle(item.PayableDebitGLAcountId, tenant);
                    if (PayableDebitGLAcount != null)
                    {
                        item.PayableDebitGLAcountName = PayableDebitGLAcount.LocalName;


                    }
                }
            }

            return myResult;
        }

        public List<APInvoiceLinePM> GetInvoiceLinesByInvoiceIds(List<string> invoiceIds, int tenant)
        {
            List<APInvoiceLinePM> list = new List<APInvoiceLinePM>();
            const int sqlLimit = 5000;
            int iterations = invoiceIds.Count() / sqlLimit;
            for (int i = 0; i <= iterations; i++)
            {
                var tempInvoiceIds = invoiceIds.Skip(i * sqlLimit).Take(sqlLimit).ToList();
                List<APInvoiceLinePM> tempList = (from a in repository.context.APInvoiceLines
                                                  where a.Tenant == tenant
                                                  && tempInvoiceIds.Contains(a.APInvoiceId)
                                                  select new APInvoiceLinePM()
                                                  {

                                                      APInvoiceId = a.APInvoiceId,
                                                      Description = a.Description,
                                                      VatPercentage = a.VatPercentage,
                                                      LineNumber = a.LineNumber,
                                                      LocalCurrencyAmount = a.LocalCurrencyAmount,
                                                  }).ToList();
                list.AddRange(tempList);
            }
            return list;

        }

    }
}