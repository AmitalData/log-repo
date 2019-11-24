using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;

using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Security;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class APPaymentMapping
    {
        public static void MapEntity(APPaymentPM entityPM, APPayment entity, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            #region Not Approved yet
            if (string.IsNullOrEmpty(entity.StatusCode) || entity.StatusCode == "DR" || entity.StatusCode == "AC")
            {
                if (isNewState)
                {
                    entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    entityPM.CreatedByUserId = loggedContact.Id;
                    entityPM.OpenAmount = entityPM.AmountInPaymentCurrency;

                    entity.Id = entityPM.Id;
                    entity.PaymentNo = entityPM.PaymentNo;
                    entity.Tenant = entityPM.Tenant;
                    entity.CreatedByUserId = entityPM.CreatedByUserId;
                    entity.CreateDate = entityPM.CreateDate;
                    entity.LocalCurrencyId = entityPM.LocalCurrencyId;
                }

                entity.BranchId = entityPM.BranchId;
                entity.PaymentMethodId = entityPM.AccountingPaymentMethodId;
                entity.AccountingPaymentMethodId = entityPM.AccountingPaymentMethodId;
                entity.VendorAddressId = entityPM.VendorAddressId;
                entity.VendorId = entityPM.VendorId;
                entity.AmountInLocalCurrency = entityPM.AmountInLocalCurrency;
                entity.AmountInPaymentCurrency = entityPM.AmountInPaymentCurrency;
                entity.PaymentCurrencyId = entityPM.PaymentCurrencyId;
                entity.PaymentCurrencyExchangeRate = entityPM.PaymentCurrencyExchangeRate;
                entity.PaymentCurrencyExchangeRateDate = entityPM.PaymentCurrencyExchangeRateDate;
                entity.Account = entityPM.Account;
                entity.Bank = entityPM.Bank;
                entity.BankBranch = entityPM.BankBranch;
                entity.ChequeOrPaymentRef = entityPM.ChequeOrPaymentRef;
                entity.ValueDate = entityPM.ValueDate;
                entity.CreditCardTypeId = entityPM.CreditCardTypeId;
            }
            #endregion

            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.UpdatedByUserId = loggedContact.Id;
            entity.RegisterDate = entityPM.RegisterDate;
            entity.UpdateDate = entityPM.UpdateDate;
            entity.UpdatedByUserId = entityPM.UpdatedByUserId;
            entity.OpenAmount = entityPM.OpenAmount;
            entity.PrintDate = entityPM.PrintDate;
            entity.PrintNotes = entityPM.PrintNotes;
            entity.PrintedByUserId = entityPM.PrintedByUserId;
            entity.InternalNotes = entityPM.InternalNotes;
            entity.ExternalAccountingEntityId = entity.ExternalAccountingEntityId;
            entity.TransferStatusCode = entityPM.TransferStatusCode;
            entity.TaxDeductionLocalAmount = entityPM.TaxDeductionLocalAmount;
            entity.TaxDeductionPercentage = entityPM.TaxDeductionPercentage;
            entity.VendorBankAddress = entityPM.VendorBankAddress;
            entity.VendorBankName = entityPM.VendorBankName;
            entity.VendorBankAccountNumber = entityPM.VendorBankAccountNumber;
            entity.VendorIBANNumber = entityPM.VendorIBANNumber;
            entity.VendorSwift = entityPM.VendorSwift;
            
            if (entityPM.SetApproved)
            {
                entityPM.ApprovedByUserId = loggedContact.Id;
                entityPM.ApprovedDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
            entity.ApprovedByUserId = entityPM.ApprovedByUserId;
            entity.ApprovedDateTime = entityPM.ApprovedDateTime;
            entity.BankAccountId = entityPM.BankAccountId;
            entity.PaymentMethodId = entityPM.AccountingPaymentMethodId;
            entity.AutomaticPaymentCheque = entityPM.AutomaticPaymentCheque;
            if (entityPM.StatusCode == "AD" && entityPM.OpenAmount == 0)
            {
                entity.IsClosed = true;
                entity.StatusCode = "CL";
            }

            else
            {
                entity.IsClosed = entityPM.IsClosed;
                entity.StatusCode = entityPM.StatusCode;
            }

            if (entityPM.SetApproved)
            {
                if (entity.FirstApproveDate == null)
                {
                    if (entityPM.ApprovedDateTime != null)
                    {
                        entity.FirstApproveDate = entityPM.ApprovedDateTime;
                        entityPM.FirstApproveDate = entityPM.ApprovedDateTime;
                    }
                }
            }

            entityPM.SetVoided = false;
            entityPM.SetApproved = false;
            entityPM.SetCancelApproval = false;
            
            if (IsFullAccountingActivated(entityPM.Tenant))
                MapJournalFields(entityPM);

        }

        private static void MapJournalFields(APPaymentPM entityPM)
        {
            JournalEntity journal = GetJournalOfAPPayment(entityPM);
            if (journal != null)
            {
                entityPM.JournalId = journal.JournalId;
                entityPM.JournalNumber = journal.JournalNumber;
            }
        }

        private static JournalEntity GetJournalOfAPPayment(APPaymentPM entityPM)
        {
            JournalRepository rep = new JournalRepository(entityPM.Tenant);
            JournalEntity journal = rep.GetJournalByAccountingEntityId(entityPM.Id, entityPM.Tenant);
            return journal;
        }

        private static bool IsFullAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPoco = tenantRepository.GetSingleTenant(tenant);
            return (tenantPoco != null && tenantPoco.AccountingActivated);
        }

        public static void MapEntityInvoicePyament(APPaymentInvoicePM entityPM, APInvoicePayment entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
            }

            // Ayman: See the inner exception for details. (Cannot insert the value NULL into column 'LocalAmount', table 'LogitudeMain.dbo.APInvoicePayments'; column does not allow nulls. INSERT fails. The statement has been terminated.)

            if (entityPM.ForeignAmount == null)
            {
                entityPM.ForeignAmount = 0;
            }

            if (entityPM.LocalAmount == null)
            {
                entityPM.LocalAmount = 0;
            }

            if (entityPM.PaymentAmount == null)
            {
                entityPM.PaymentAmount = 0;
            }

            entity.APInvoiceId = entityPM.APInvoiceId;
            entity.APPaymentId = entityPM.APPaymentId;
            entity.ForeignAmount = entityPM.ForeignAmount;
            entity.ForeignCurrencyId = entityPM.ForeignCurrencyId;
            entity.LocalAmount = entityPM.LocalAmount;
            entity.ExchangeRate = entityPM.ExchangeRate;
            entity.PaymentAmount = entityPM.PaymentAmount;
        }
    }
}
