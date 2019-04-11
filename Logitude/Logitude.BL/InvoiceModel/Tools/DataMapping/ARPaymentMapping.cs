using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Security;
using System.Linq;
using System.Collections.Generic;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class ARPaymentMapping
    {
        public static void MapEntity(ARPaymentPM entityPM, ARPayment entity, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            #region Not Approved yet
            if (string.IsNullOrEmpty(entity.StatusCode) || entity.StatusCode == "DR" || entity.StatusCode == "AC")
            {
                if (isNewState)
                {
                    entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant);
                    if (entityPM.CreatedByUserId == null)
                    {
                        entityPM.CreatedByUserId = loggedContact.Id;
                    }
                    entityPM.OpenAmount = entityPM.AmountInPaymentCurrency;

                    entity.Id = entityPM.Id;
                    entity.PaymentNo = entityPM.PaymentNo;
                    entity.Tenant = entityPM.Tenant;
                    entity.CreatedByUserId = entityPM.CreatedByUserId;
                    entity.CreateDate = entityPM.CreateDate;                    
                    entity.LocalCurrencyId = entityPM.LocalCurrencyId;
                    if (string.IsNullOrEmpty(entityPM.SATTransferStatusCode))
                    {
                        entity.SATTransferStatusCode = "NT";
                    }
                    else
                    {
                        entity.SATTransferStatusCode = entityPM.SATTransferStatusCode;
                    }
                }

                entity.ExternalAccountingEntityId = entityPM.ExternalAccountingEntityId;
                entity.BranchId = entityPM.BranchId;
                entity.ARAccountId = entityPM.ARAccountId;
                entity.AccountingPaymentMethodId = entityPM.AccountingPaymentMethodId;
                entity.BillToAddressId = entityPM.BillToAddressId;
                entity.BillToId = entityPM.BillToId;
                entity.DebitAccountId = entityPM.DebitAccountId;
                entity.ExchangeRateDate = entityPM.ExchangeRateDate;
                entity.AmountInLocalCurrency = entityPM.AmountInLocalCurrency;
                entity.AmountInPaymentCurrency = entityPM.AmountInPaymentCurrency;                
                entity.PaymentCurrencyId = entityPM.PaymentCurrencyId;
                entity.PaymentCurrencyExchangeRate = entityPM.PaymentCurrencyExchangeRate;
                entity.Account = entityPM.Account;
                entity.Bank = entityPM.Bank;
                entity.BankBranch = entityPM.BankBranch;
                entity.ChequeOrPaymentRef = entityPM.ChequeOrPaymentRef;
                entity.ValueDate = entityPM.ValueDate;
                entity.CreditCardTypeId = entityPM.CreditCardTypeId;
                entity.ChequeOrPaymentRef = entityPM.ChequeOrPaymentRef;
                entity.CreateDate = entityPM.CreateDate;
            }
            #endregion

            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant);
            entityPM.UpdatedByUserId = loggedContact.Id;
            entity.RegisterDate = entityPM.RegisterDate;
            entity.UpdateDate = entityPM.UpdateDate;
            entity.UpdatedByUserId = entityPM.UpdatedByUserId;
            entity.OpenAmount = entityPM.OpenAmount;
            entity.PrintByUserId = entityPM.PrintByUserId;
            entity.PrintDate = entityPM.PrintDate;
            entity.PrintNotes = entityPM.PrintNotes;
            entity.PaidBy = entityPM.PaidBy;
            entity.InternalNotes = entityPM.InternalNotes;
            entity.TransferTries = entityPM.TransferTries;
            entity.TransferError = entityPM.TransferError;
            entity.IsTransferStarted = entityPM.IsTransferStarted;
            entity.TransferStatusCode = entityPM.TransferStatusCode;
            entity.BankAccountId = entityPM.BankAccountId;
            entity.CashbookId = entityPM.CashbookId;
            entity.SATPaymentMethodCode = entityPM.SATPaymentMethodCode;
            entity.BankAccountLiteId = entityPM.BankAccountLiteId;
            entity.MetodoPagoCode = entityPM.MetodoPagoCode;
            entity.TipoCadenaPago = entityPM.TipoCadenaPago;
            entity.CadPago = entityPM.CadPago;
            entity.CertPago = entityPM.CertPago;
            entity.SelloPago = entityPM.SelloPago;
            entity.ApprovedDate = entityPM.ApprovedDate;
            entity.ApprovedByUserId = entityPM.ApprovedByUserId;
            entity.FirstApproveDate = entityPM.FirstApproveDate;
            entity.IsFullAccounting = entityPM.IsFullAccounting;
            entity.IsExternalEntity = entityPM.IsExternalEntity;

            if (entityPM.IsExternalEntity) {
                entity.ChequeOrPaymentRef = entityPM.ChequeOrPaymentRef;
                entity.CreateDate = entityPM.CreateDate;
                entity.PaymentNo = entityPM.PaymentNo;
                entity.AmountInLocalCurrency = entityPM.AmountInLocalCurrency;
                entity.AmountInPaymentCurrency = entityPM.AmountInPaymentCurrency;
                entity.LocalCurrencyId = entityPM.LocalCurrencyId;
            }





            if (entityPM.StatusCode == "AD" && entityPM.OpenAmount == 0)
            {
                entity.IsClosed = false;
                entity.StatusCode = "CL";
            }

            else
            {
                entity.StatusCode = entityPM.StatusCode;
                entity.IsClosed = entityPM.IsClosed;
            }

            if (entityPM.SetApproved)
            {
                if (entity.FirstApproveDate == null)
                {
                    if (entityPM.ApprovedDate != null)
                    {
                        entity.FirstApproveDate = entityPM.ApprovedDate;
                        entityPM.FirstApproveDate = entityPM.ApprovedDate;
                    }
                }
            }

            entityPM.SetVoided = false;
            entityPM.SetApproved = false;
            entityPM.SetCancelApproval = false;
            entityPM.SetReTransfer = false;
            entityPM.SetReSendQBO = false;

        }

        public static void MapEntityInvoicePyament(ARPaymentInvoicePM entityPM, ARInvoicePayment entity, bool isNewState)
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
