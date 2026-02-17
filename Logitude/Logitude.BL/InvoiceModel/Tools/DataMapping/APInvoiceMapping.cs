using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class APInvoiceMapping
    {
        public static void MapEntity(APInvoicePM entityPM, APInvoice entity, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            #region Not Approved yet
            if (string.IsNullOrEmpty(entity.StatusCode) || entity.StatusCode == "WA" || entity.StatusCode == "AC")
            {
                if (isNewState)
                {
                    entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    entityPM.CreatedByUserId = loggedContact.Id;
                    entity.Id = entityPM.Id;
                    entity.InternalNumber = entityPM.InternalNumber;
                    entity.Tenant = entityPM.Tenant;
                    entity.CreatedByUserId = entityPM.CreatedByUserId;
                    entity.CreateDate = entityPM.CreateDate;
                    entity.LocalCurrencyId = entityPM.LocalCurrencyId;
                    entity.ProfitCurrencyId = entityPM.ProfitCurrencyId;
                    entity.Description = entityPM.Description;
                    entity.IsMultipleEntities = entityPM.IsMultipleEntities;
                }

                entity.ProfitCurrencyExchangeRate = entityPM.ProfitCurrencyExchangeRate;
                entity.BranchId = entityPM.BranchId;
                entity.DueDate = entityPM.DueDate;
                entity.InvoiceDate = entityPM.InvoiceDate;
                entity.InvoiceCurrencyId = entityPM.InvoiceCurrencyId;
                entity.ExchangeRateDate = entityPM.ExchangeRateDate;
                entity.InvoiceCurrencyExchangeRate = entityPM.InvoiceCurrencyExchangeRate;
                entity.AmountInInvoiceCurrency = entityPM.AmountInInvoiceCurrency;
                entity.AmountInLocalCurrency = entityPM.AmountInLocalCurrency;
                entity.AmountInProfitCurrency = entityPM.AmountInProfitCurrency;
                entity.PaymentTermId = entityPM.PaymentTermId;
                entity.SubTotalInInvoiceCurrency = entityPM.SubTotalInInvoiceCurrency;
                entity.SubTotalInLocalCurrency = entityPM.SubTotalInLocalCurrency;
                entity.VendorId = entityPM.VendorId;
                entity.VATNumber = entityPM.VATNumber;
                entity.OperationalDate = entityPM.OperationalDate;
                entity.VendorGLAccountId = entityPM.VendorGLAccountId;
                entity.AccountingDate = entityPM.AccountingDate;
                entity.IsExternalEntity = entityPM.IsExternalEntity;

                if (entityPM.InvoiceNumber != null)
                {
                    entityPM.InvoiceNumber = entityPM.InvoiceNumber.Trim();
                }

                entity.InvoiceNumber = entityPM.InvoiceNumber;
            }
            #endregion

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

            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.UpdatedByUserId = loggedContact.Id;
            entity.UpdateDate = entityPM.UpdateDate;
            entity.UpdatedByUserId = entityPM.UpdatedByUserId;
            entity.MainEntityId = entityPM.MainEntityId;
            entity.MainEntityReference = entityPM.MainEntityReference;
            entity.RefundAmount = entityPM.RefundAmount;
            entity.AmountDue = entityPM.AmountDue;
            entity.AmountDueInLocalCurrency = entityPM.AmountDueInLocalCurrency;
            entity.AmountDueInProfitCurrency = entityPM.AmountDueInProfitCurrency;
            entity.StatusCode = entityPM.StatusCode;
            entity.IsClosed = entityPM.IsClosed;
            entity.InternalNotes = entityPM.InternalNotes;
            entity.HouseNumber = entityPM.HouseNumber;
            entity.MasterNumber = entityPM.MasterNumber;
            entity.CreditAccount = entityPM.CreditAccount;
            entity.TransferTries = entityPM.TransferTries;
            entity.TransferError = entityPM.TransferError;
            entity.IsTransferStarted = entityPM.IsTransferStarted;
            entity.TransferStatusCode = entityPM.TransferStatusCode;
            entity.AccountingExternalCode = entityPM.AccountingExternalCode;
            entity.PaymentTermExternalId = entityPM.PaymentTermExternalId;
            entity.ApprovedDate = entityPM.ApprovedDate;
            entity.ApprovedByUserId = entityPM.ApprovedByUserId;
            entity.IsGeneralInvoice = entityPM.IsGeneralInvoice;
            entity.ExternalAccountingEntityId = entityPM.ExternalAccountingEntityId;
            entity.CreatedByPartner = entityPM.CreatedByPartner;
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

        public static void MapInvoiceLine(APInvoiceLinePM entityPM, APInvoiceLine entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Tenant = entityPM.Tenant;
                entity.APInvoiceId = entityPM.APInvoiceId;
            }

            entity.LineNumber = entityPM.LineNumber;
            entity.ChargesTypeId = entityPM.ChargesTypeId;
            entity.InvoiceCurrencyAmount = entityPM.InvoiceCurrencyAmount;
            entity.VatTypeId = entityPM.VatTypeId;
            entity.LocalCurrencyAmount = entityPM.LocalCurrencyAmount;
            entity.Notes = entityPM.Notes;
            entity.EntityId = entityPM.EntityId;
            entity.EntityPayableId = entityPM.EntityPayableId;
            entity.RefundAmount = entityPM.RefundAmount;
            entity.ProfitCurrencyAmount = entityPM.ProfitCurrencyAmount;
            entity.VatPercentage = entityPM.VatPercentage;
            entity.ForiegnCurrencyId = entityPM.ForiegnCurrencyId;
            entity.ForiegnExchangeRate = entityPM.ForiegnExchangeRate;
            entity.ForiegnCurrencyAmount = entityPM.ForiegnCurrencyAmount;
            entity.DebitAccount = entityPM.DebitAccount;
            entity.Description = entityPM.Description;
            entity.LocalDescription = entityPM.LocalDescription;
            entity.ChargeTypeGLAccountId = entityPM.ChargeTypeGLAccountId;
            entity.AuthorizedSignatory = entityPM.AuthorizedSignatory;
            entity.PrepaidCollectId = entityPM.PrepaidCollectId;
            entity.ContainerTypeId = entityPM.ContainerTypeId;
            entity.Quantity = entityPM.Quantity;
        }

        public static void MapInvoicePayment(APInvoicePaymentPM entityPM, APInvoicePayment entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
            }

            entity.APInvoiceId = entityPM.APInvoiceId;
            entity.APPaymentId = entityPM.APPaymentId;
            entity.ForeignAmount = entityPM.ForeignAmount;
            entity.ForeignCurrencyId = entityPM.ForeignCurrencyId;
            entity.LocalAmount = entityPM.LocalAmount;
            entity.PaymentAmount = entityPM.PaymentAmount;
            entity.ExchangeRate = entityPM.ExchangeRate;
        }
    }
}
