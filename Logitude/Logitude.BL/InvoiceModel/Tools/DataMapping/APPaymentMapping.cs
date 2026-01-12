using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;

using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Security;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
            entity.MasavInterfaceId = entityPM.MasavInterfaceId;
            if (entityPM.SetApproved)
            {
                entityPM.ApprovedByUserId = loggedContact.Id;
                entityPM.ApprovedDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
            entity.ApprovedByUserId = entityPM.ApprovedByUserId;
            entity.ApprovedDateTime = entityPM.ApprovedDateTime;
            entity.BankAccountId = entityPM.BankAccountId;
            entity.AutomaticPaymentCheque = entityPM.AutomaticPaymentCheque;
            entity.AccountingCancelationDate = entityPM.AccountingCancelationDate;
            entity.DontIncludeInDeductionReport = entityPM.DontIncludeInDeductionReport;
            entity.CancelationNotes = entityPM.CancelationNotes;
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
            entity.ExternalPaymentAmount = entityPM.ExternalPaymentAmount;
            entity.ExternalPaymentDate = entityPM.ExternalPaymentDate;
            entity.ExternalPaymentNotes = entityPM.ExternalPaymentNotes;
            entity.ConnectedInvoicesNumbers = entityPM.ConnectedInvoicesNumbers;

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
            JournalEntity journal = rep.GetJournalByAccountingEntityIdAndTypeCode(entityPM.Id,"5", entityPM.Tenant);
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
