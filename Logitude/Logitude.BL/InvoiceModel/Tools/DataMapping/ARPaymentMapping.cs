using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Security;
using System.Linq;
using System.Collections.Generic;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public partial class ARPaymentMapping
    {
        
        public static void MapEntity(ARPaymentPM entityPM, ARPayment entity, bool isNewState)
        {
            ContactPM loggedContact = GetLoggedContactPM(entityPM.Tenant);

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
                    entityPM.OpenAmountInLocalCurrency = entityPM.AmountInLocalCurrency;

                    entity.Id = entityPM.Id;
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
                if (!string.IsNullOrEmpty(entityPM.ExternalAccountingEntityId))
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
                entity.PaymentNo = entityPM.PaymentNo;
            }
            #endregion

            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant);
            if (!entityPM.IsExternalEntity)
            {
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            entity.RegisterDate = entityPM.RegisterDate;
            entity.UpdateDate = entityPM.UpdateDate;
            entity.UpdatedByUserId = entityPM.UpdatedByUserId;
            entity.OpenAmount = entityPM.OpenAmount;
            entity.OpenAmountInLocalCurrency = entityPM.OpenAmountInLocalCurrency;
            entity.PrintByUserId = entityPM.PrintByUserId;
            entity.PrintDate = entityPM.PrintDate;
            entity.PrintNotes = string.IsNullOrEmpty(entityPM.PrintNotes) && isNewState ? TextCodesTranslator.TranslateText("ARPayment.S.ShortTitle", entity.Tenant, true) : entityPM.PrintNotes;
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
            entity.PartnerId = entityPM.PartnerId;
            entity.CancelationNotes = entityPM.CancelationNotes;
            entity.AccountingCancelationDate = entityPM.AccountingCancelationDate;

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
                entity.Bank = entityPM.Bank;
                entity.BankBranch = entityPM.BankBranch;
                entity.Account = entityPM.Account;
                entity.CashbookId = entityPM.CashbookId;

            }


            entity.FechaPago = entityPM.FechaPago;

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
            entity.CreatedByPartner = entityPM.CreatedByPartner;
            entity.IsPaymentNumberManuallySet = entityPM.IsPaymentNumberManuallySet;

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

            MapConcurrencyFields(entityPM, entity, isNewState);
        }
        public static ContactPM GetLoggedContactPM(int tenant)
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
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
