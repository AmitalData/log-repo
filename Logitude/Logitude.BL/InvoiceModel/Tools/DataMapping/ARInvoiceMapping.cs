
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using System.Linq;
using Logitude.Accounting.Data.Repositories;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class ARInvoiceMapping
    {
        public static void MapEntity(ARInvoicePM entityPM, ARInvoice entity, bool isNewState, string loggedContactId)
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
                entityPM.CreateDate = todayDateTime;

                if (entityPM.IsExternalAPI )
                {
                    entity.IssuedByUserId = entityPM.IssuedByUserId;
                    entity.CreatedByUserId = entityPM.CreatedByUserId;
                }

                else
                {
                    entity.IssuedByUserId = loggedContactId;
                    entity.CreatedByUserId = loggedContactId;
                }

                entity.CreateDate = entityPM.CreateDate;
                entity.LocalCurrencyId = entityPM.LocalCurrencyId;
                entity.ProfitCurrencyId = entityPM.ProfitCurrencyId;
                entity.ARInvoiceTypeCode = entityPM.ARInvoiceTypeCode;
                entity.Description = entityPM.Description;
                entity.IsConstituentInvoice = entityPM.IsConstituentInvoice;
                entity.IsConsolidationInvoice = entityPM.IsConsolidationInvoice;

                if (string.IsNullOrEmpty(entityPM.SATTransferStatusCode))
                {
                    entity.SATTransferStatusCode = "NT";
                }
                else
                {
                    entity.SATTransferStatusCode = entityPM.SATTransferStatusCode;
                }

                if (string.IsNullOrEmpty(entityPM.SATInvoiceStatusCode))
                {
                    entity.SATInvoiceStatusCode = "NO";
                }
                else
                {
                    entity.SATInvoiceStatusCode = entityPM.SATInvoiceStatusCode;
                }
            }

            entity.ProfitCurrencyExchangeRate = entityPM.ProfitCurrencyExchangeRate;
            entity.BranchId = entityPM.BranchId;
            entity.DueDate = entityPM.DueDate;
            entity.InvoiceDate = entityPM.InvoiceDate;
            entity.InvoiceCurrencyId = entityPM.InvoiceCurrencyId;
            entity.ExchangeRateDate = entityPM.ExchangeRateDate;
            entity.InvoiceCurrencyExchangeRate = entityPM.InvoiceCurrencyExchangeRate;
            entity.BillToAddressId = entityPM.BillToAddressId;
            entity.BillToId = entityPM.BillToId;
            entity.VatNumber = entityPM.VatNumber;
            entity.InvoiceNumber = entityPM.InvoiceNumber;
            entity.PaymentTermId = entityPM.PaymentTermId;
            entity.PrepaidCollectId = entityPM.PrepaidCollectId;
            entity.DraftNumber = entityPM.DraftNumber;
            entity.IsInvoiceNumberManuallySet = entityPM.IsInvoiceNumberManuallySet;
            entity.AmountInInvoiceCurrency = entityPM.AmountInInvoiceCurrency;
            entity.AmountInLocalCurrency = entityPM.AmountInLocalCurrency;
            entity.AmountInProfitCurrency = entityPM.AmountInProfitCurrency;
            entity.SubTotalInInvoiceCurrency = entityPM.SubTotalInInvoiceCurrency;
            entity.SubTotalInLocalCurrency = entityPM.SubTotalInLocalCurrency;
            entity.CustomerRef = entityPM.CustomerRef;
            entity.OperationalDate = entityPM.OperationalDate;
            entity.DateForVATInterest = entityPM.DateForVATInterest;
            entity.SplitJournalByCurrency = entityPM.SplitJournalByCurrency;
            entity.IsExternalEntity = entityPM.IsExternalEntity;
            entity.IsGeneralInvoice = entityPM.IsGeneralInvoice;
            entity.SalesmanUserId = entityPM.SalesmanUserId;
            entity.IsCustomsChargesOnly = entityPM.IsCustomsChargesOnly;
            entity.ExternalAccountingEntityId = entityPM.ExternalAccountingEntityId;

            if (entityPM.HouseNumber != null)
            {
                entityPM.HouseNumber = entityPM.HouseNumber.Trim();

                if (string.IsNullOrEmpty(entityPM.HouseNumber))
                {
                    entityPM.HouseNumber = null;
                }
            }

            if (entityPM.MasterNumber != null)
            {
                entityPM.MasterNumber = entityPM.MasterNumber.Trim();

                if (string.IsNullOrEmpty(entityPM.MasterNumber))
                {
                    entityPM.MasterNumber = null;
                }
            }
            entity.IsExternalEntity = entityPM.IsExternalEntity;
            entity.HouseNumber = entityPM.HouseNumber;
            entity.MasterNumber = entityPM.MasterNumber;
            entityPM.UpdateDate = todayDateTime;
            entityPM.UpdatedByUserId = loggedContactId;
            entity.UpdateDate = entityPM.UpdateDate;
            entity.UpdatedByUserId = entityPM.UpdatedByUserId;
            entity.ExpectedPaymentDate = entityPM.ExpectedPaymentDate;
            entity.AmountDue = entityPM.AmountDue;
            entity.AmountDueInLocalCurrency = entityPM.AmountDueInLocalCurrency;
            entity.AmountDueInProfitCurrency = entityPM.AmountDueInProfitCurrency;
            entity.MainEntityId = entityPM.MainEntityId;
            entity.MainEntityReference = entityPM.MainEntityReference;
            entity.Sent = entityPM.Sent;
            entity.StatusCode = entityPM.StatusCode;
            entity.IsClosed = entityPM.IsClosed;
            entity.IsAutoCredit = entityPM.IsAutoCredit;
            entity.IsCancelled = entityPM.IsCancelled;
            entity.CancelledByARInvoiceId = entityPM.CancelledByARInvoiceId;
            entity.CreditedByARInvoiceId = entityPM.CreditedByARInvoiceId;
            entity.IsPrinted = entityPM.IsPrinted;
            entity.PrintNotes = entityPM.PrintNotes;
            entity.PrintByUserId = entityPM.PrintByUserId;
            entity.PrintDate = entityPM.PrintDate;            
            entity.InternalNotes = entityPM.InternalNotes;
            entity.PaymentTermExternalId = entityPM.PaymentTermExternalId;
            entity.Field1 = entityPM.Field1 != null ? entityPM.Field1.Value : null;
            entity.Field2 = entityPM.Field2 != null ? entityPM.Field2.Value : null;
            entity.Field3 = entityPM.Field3 != null ? entityPM.Field3.Value : null;
            entity.Field4 = entityPM.Field4 != null ? entityPM.Field4.Value : null;
            entity.Field5 = entityPM.Field5 != null ? entityPM.Field5.Value : null;
            entity.Field6 = entityPM.Field6 != null ? entityPM.Field6.Value : null;
            entity.Field7 = entityPM.Field7 != null ? entityPM.Field7.Value : null;
            entity.Field8 = entityPM.Field8 != null ? entityPM.Field8.Value : null;
            entity.Field9 = entityPM.Field9 != null ? entityPM.Field9.Value : null;
            entity.Field10 = entityPM.Field10 != null ? entityPM.Field10.Value : null;
            entity.DebitAccount = entityPM.DebitAccount;
            entity.TransferTries = entityPM.TransferTries;
            entity.TransferError = entityPM.TransferError;
            entity.IsTransferStarted = entityPM.IsTransferStarted;
            entity.TransferStatusCode = entityPM.TransferStatusCode;
            entity.AccountingExternalCode = entityPM.AccountingExternalCode;
            entity.ConsolidationInvoiceId = entityPM.ConsolidationInvoiceId;
            entity.ApprovedDate = entityPM.ApprovedDate;
            entity.ApprovedByUserId = entityPM.ApprovedByUserId;
            entity.SATPaymentMethodCode = entityPM.SATPaymentMethodCode;
            entity.TransmissionError = entityPM.TransmissionError;
            entity.MetodoPagoCode = entityPM.MetodoPagoCode;
            entity.RelatedInvoice = entityPM.RelatedInvoice;
            entity.Intercompany = entityPM.Intercompany;
            entity.BankAccountLiteId = entityPM.BankAccountLiteId;
            entity.IsMultiCurrency = entityPM.IsMultiCurrency;
            entity.TotalAmountForTaxReport = entityPM.TotalAmountForTaxReport;
            entity.TotalVAT = entityPM.TotalVAT;
            entity.TotaVatableAmountForTaxReport = entityPM.TotaVatableAmountForTaxReport;
            entity.IsFullAccounting = entityPM.IsFullAccounting;
            entity.ARInvoiceStockId = entityPM.ARInvoiceStockId;
            entity.IsInvoiceNumberFromStock = entityPM.IsInvoiceNumberFromStock;
            entity.UsoCFDICode = entityPM.UsoCFDICode;
            entity.RelatedInvoice  = entityPM.RelatedInvoice;

            entityPM.SetVoided = false;
            entityPM.SetAsSent = false;
            entityPM.SetApproved = false;
            entityPM.SetReTransfer = false;
            entityPM.SetCancelDraft = false;
            entityPM.SetReSendQBO = false;
            //Full Accounting 
            TenantRepository tenantRepository = new TenantRepository(entityPM.Tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(entityPM.Tenant);
            if (tenantPOCO != null && tenantPOCO.AccountingActivated)
            {
                JournalRepository rep = new JournalRepository(entityPM.Tenant);
                JournalEntity journal = rep.GetJournalByAccountingEntityId(entityPM.Id, entityPM.Tenant);
                if (journal != null)
                {
                    entityPM.JournalId = journal.JournalId;
                    entityPM.JournalNumber = journal.JournalNumber;
                }
            }

            if (isNewState)
            {
                if (entityPM.NewConcurrencyGUID == null)
                {
                    entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
                }
            }

            entity.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            entityPM.ConcurrencyGUID = entity.ConcurrencyGUID;
        }

        public static void MapInvoiceLine(ARInvoiceLinePM entityPM, ARInvoiceLine entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
                entity.ARInvoiceId = entityPM.ARInvoiceId;
            }

            entity.ForiegnCurrencyAmount = entityPM.ForiegnCurrencyAmount;
            entity.InvoiceCurrencyAmount = entityPM.InvoiceCurrencyAmount;
            entity.LocalCurrencyAmount = entityPM.LocalCurrencyAmount;
            entity.ProfitCurrencyAmount = entityPM.ProfitCurrencyAmount;
            entity.ChargesTypeId = entityPM.ChargesTypeId;
            entity.CreditAccount = entityPM.CreditAccount;
            entity.ForiegnCurrencyId = entityPM.ForiegnCurrencyId;
            entity.Description = entityPM.Description;
            entity.EntityId = entityPM.EntityId;
            entity.ForiegnExchangeRate = entityPM.ForiegnExchangeRate;
            entity.VatTypeId = entityPM.VatTypeId;
            entity.LineNumber = entityPM.LineNumber;
            entity.ReceivableId = entityPM.ReceivableId;
            entity.UnitPrice = entityPM.UnitPrice;
            entity.Quantity = entityPM.Quantity;
            entity.MeasurementId = entityPM.MeasurementId;
            entity.IsExchangeRateFixed = entityPM.IsExchangeRateFixed;
            entity.LocalDescription = entityPM.LocalDescription;
            entity.VatPercentage = entityPM.VatPercentage;
            entity.ExchangeRateDate = entityPM.ExchangeRateDate;
            entity.Notes = entityPM.Notes;
            entity.DateForInterest = entityPM.DateForInterest;
            entity.ValueDate = entityPM.ValueDate;
            entity.GLAccountId = entityPM.GLAccountId;
            entity.LineActionCode = entityPM.LineActionCode;
            entity.IsBackToBack = entityPM.IsBackToBack;
            entity.IsExpense = entityPM.IsExpense;
            entity.PrepaidCollectId = entityPM.PrepaidCollectId;

            Tenant myTenant = TenantRepository.GetSingleTenant(entityPM.Tenant, true);

            if (myTenant.CurrencyId == entity.ForiegnCurrencyId)
            {
                entity.ForiegnExchangeRate = 1;
                entity.LocalCurrencyAmount = entity.ForiegnCurrencyAmount;
            }
        }

        public static void MapInvoicePayment(ARInvoicePaymentPM entityPM, ARInvoicePayment entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
            }

            entity.ARInvoiceId = entityPM.ARInvoiceId;
            entity.ARPaymentId = entityPM.ARPaymentId;
            entity.LocalAmount = entityPM.LocalAmount;
            entity.ForeignAmount = entityPM.ForeignAmount;
            entity.ForeignCurrencyId = entityPM.ForeignCurrencyId;
            entity.ExchangeRate = entityPM.ExchangeRate;
            entity.PaymentAmount = entityPM.PaymentAmount;

        }
    }
}
