using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;

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
        public List<ARInvoiceLinePM> GetGLAccountLocalNameAndDisplayNumber(List<ARInvoiceLinePM> lines, string invoiceId, int tenant)
        {
            IAccountingContext context = AccountingContext.GetContext(tenant);
            var glaAccountIds = from a in lines select a.GLAccountId;
            glaAccountIds = glaAccountIds.Distinct().ToList();

            List<ARInvoiceLinePM> gLAccountsInfo = (
            from gLAccount in context.GLAccounts
            where glaAccountIds.Contains(gLAccount.Id)
            select new ARInvoiceLinePM()
            {
                GLAccountId = gLAccount != null ? gLAccount.Id : "",
                GLAccountLocalName = gLAccount != null ? gLAccount.LocalName : "",
                GLAccountDisplayNumber = gLAccount != null ? gLAccount.DisplayNumber : "",
            }).ToList();

            lines = lines.Select(x =>
            {
                x.GLAccountLocalName = gLAccountsInfo.FirstOrDefault(y => y.GLAccountId == x.GLAccountId)?.GLAccountLocalName;
                x.GLAccountDisplayNumber = gLAccountsInfo.FirstOrDefault(y => y.GLAccountId == x.GLAccountId)?.GLAccountDisplayNumber;
                return x;
            }
            ).ToList();

            return lines;
        }
        public Contact GetLogContact(int tenant)
        {
            ContactRepository contactRep = new ContactRepository(tenant);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            else
            {
                email = "system@tenant" + tenant + ".com";
            }

            Contact contact = contactRep.GetSingleContactByEmail(email, tenant);


            return contact;

        }
        public bool IsFullAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            bool isFullAccountingActivated = tenantPOCO.AccountingActivated;
            return isFullAccountingActivated;
        }

        public List<ARInvoiceLinePM> GetInvoiceLinePMsByInvoiceId(string invoiceId, int tenant)
        {
            var isFullAccountingActivated = IsFullAccountingActivated(tenant);
            Contact loggedContact = GetLogContact(tenant);
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
                                              InvoiceCurrencyId =  a.ARInvoice == null ? "" : ( ( a.ARInvoice.InvoiceCurrencyId != null && isFullAccountingActivated ) ? a.ARInvoice.InvoiceCurrency.Id  : "" ),
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
                                              ReportedinTaxReport = loggedContact.DontShowLocalLabels ?( a.LineActionCode == "1" ? "Y":"N"): (a.LineActionCode == "1" ? "כן" : "לא"),
                                              VatTypeId = a.VatTypeId,
                                              VatPercentage = a.VatPercentage,
                                              IsBackToBack = a.IsBackToBack,
                                              IsExpense = a.IsExpense,
                                              PrepaidCollectId = a.PrepaidCollectId,
                                              IsRegionalTax = a.IsRegionalTax,
                                              ReceivableCreditGLAccountId = a.ReceivableCreditGLAccountId,
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

                GLAccountRepository gLAccountRepository = new GLAccountRepository(tenant);
                GLAccount receivableCreditGLAcount = gLAccountRepository.GetSingle(invoiceLinePM.ReceivableCreditGLAccountId, tenant);
                if (receivableCreditGLAcount != null)
                {
                    invoiceLinePM.ReceivableCreditGLAccountName = receivableCreditGLAcount.LocalName;
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
            Contact loggedContact = GetLogContact(tenant);
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
                                            ReportedinTaxReport = loggedContact.DontShowLocalLabels ? (a.LineActionCode == "1" ? "Y" : "N") : (a.LineActionCode == "1" ? "כן" : "לא"),
                                            VatTypeId = a.VatTypeId,
                                            VatPercentage = a.VatPercentage,
                                            VatTypeName = a.VatType == null ? null : a.VatType.EnglishName,
                                            VatIsMultiPercentage = a.VatType == null ? false : a.VatType.IsMultiPercentage,
                                            IsExpense = a.IsExpense,
                                            PrepaidCollectId = a.PrepaidCollectId,
                                            IsRegionalTax = a.IsRegionalTax,
                                            ReceivableCreditGLAccountId = a.ReceivableCreditGLAccountId,
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

            GLAccountRepository gLAccountRepository = new GLAccountRepository(tenant);
            GLAccount receivableCreditGLAcount = gLAccountRepository.GetSingle(myResult.ReceivableCreditGLAccountId, tenant);
            if (receivableCreditGLAcount != null)
            {
                myResult.ReceivableCreditGLAccountName = receivableCreditGLAcount.LocalName;
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
            var isFullAccountingActivated = IsFullAccountingActivated(tenant);
            Contact loggedContact = GetLogContact(tenant);
            List<ARInvoiceLinePM> list = new List<ARInvoiceLinePM>();
            const int sqlLimit = 5000;

            int iterations = invoiceIds.Count() / sqlLimit;

            for (int i = 0; i <= iterations; i++)
            {
                var tempInvoiceIds = invoiceIds.Skip(i * sqlLimit).Take(sqlLimit).ToList();
                List<ARInvoiceLinePM> tempList = (from a in repository.context.ARInvoiceLines
                                              where a.Tenant == tenant && tempInvoiceIds.Contains(a.ARInvoiceId)
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
                                                  InvoiceCurrencyId = a.ARInvoice == null ? "" : ((a.ARInvoice.InvoiceCurrencyId != null && isFullAccountingActivated) ? a.ARInvoice.InvoiceCurrency.Id : ""),
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
                                                  ReportedinTaxReport = loggedContact.DontShowLocalLabels ? (a.LineActionCode == "1" ? "Y" : "N") : (a.LineActionCode == "1" ? "כן" : "לא"),
                                                  VatTypeId = a.VatTypeId,
                                                  VatPercentage = a.VatPercentage,
                                                  IsBackToBack = a.IsBackToBack,
                                                  IsExpense = a.IsExpense,
                                                  PrepaidCollectId = a.PrepaidCollectId,
                                                  IsRegionalTax = a.IsRegionalTax,
                                              }).ToList();
                list.AddRange(tempList);
            }
            return list;
        }     
    }
}
