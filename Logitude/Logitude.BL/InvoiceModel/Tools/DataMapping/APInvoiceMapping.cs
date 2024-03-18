using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using System;

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
                    if (string.IsNullOrWhiteSpace(entityPM.CreatedByUserId))
                    {
                        entityPM.CreatedByUserId = loggedContact.Id;
                    }
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
                entity.TotalEquation = entityPM.TotalEquation;

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

            if (entityPM.HouseNumbers != null)
            {
                entityPM.HouseNumbers = entityPM.HouseNumbers.Trim();

                if (string.IsNullOrEmpty(entityPM.HouseNumbers))
                {
                    entityPM.HouseNumbers = null;
                }
            }

            if (entityPM.MasterNumbers != null)
            {
                entityPM.MasterNumbers = entityPM.MasterNumbers.Trim();

                if (string.IsNullOrEmpty(entityPM.MasterNumbers))
                {
                    entityPM.MasterNumbers = null;
                }
            }

            if (entityPM.MasterShipmentNumbers != null)
            {
                entityPM.MasterShipmentNumbers = entityPM.MasterShipmentNumbers.Trim();

                if (string.IsNullOrEmpty(entityPM.MasterShipmentNumbers))
                {
                    entityPM.MasterShipmentNumbers = null;
                }
            }

            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            if (string.IsNullOrWhiteSpace(entityPM.UpdatedByUserId))
            {
                entityPM.UpdatedByUserId = loggedContact.Id;
            }
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
            entity.IsTransferStarted = entityPM.IsTransferStarted;
            entity.TransferStatusCode = entity.TransferStatusCode == "TR" && entityPM.TransferStatusCode == "IP" ?
                                        entity.TransferStatusCode : entityPM.TransferStatusCode;
            entity.AccountingExternalCode = entityPM.AccountingExternalCode;
            entity.PaymentTermExternalId = entityPM.PaymentTermExternalId;
            entity.ApprovedDate = entityPM.ApprovedDate;
            entity.ApprovedByUserId = entityPM.ApprovedByUserId;
            entity.IsGeneralInvoice = entityPM.IsGeneralInvoice;
            if (!string.IsNullOrEmpty(entityPM.ExternalAccountingEntityId))
                entity.ExternalAccountingEntityId = entityPM.ExternalAccountingEntityId;
            entity.CreatedByPartner = entityPM.CreatedByPartner;
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
            entity.TotalVATOnly = entityPM.TotalVATOnly;
            entity.PaidDate = entityPM.PaidDate;
            entity.ShipmentsNumbers = entityPM.ShipmentsNumbers;
            entity.MasterNumbers = entityPM.MasterNumbers;
            entity.MasterShipmentNumbers = entityPM.MasterShipmentNumbers;
            entity.HouseNumbers = entityPM.HouseNumbers;
            entity.GlobalTaxCalculation = entityPM.GlobalTaxCalculation == "None" ? null : entityPM.GlobalTaxCalculation;
            entity.IsEquipment = entityPM.IsEquipment;
            entity.ConnectedPaymentsNumbers = entityPM.ConnectedPaymentsNumbers;

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

            string transferError = entityPM.TransferError;
            if (!string.IsNullOrEmpty(transferError))
            {
                if (transferError.Length > 250)
                {
                    transferError = transferError.Substring(0, 250);
                }
            }

            entityPM.TransferError = transferError;
            entity.TransferError = transferError;

            entityPM.SetVoided = false;
            entityPM.SetApproved = false;
            entityPM.SetCancelApproval = false;
            entityPM.SetReTransfer = false;
            entityPM.SetReSendQBO = false;

            if (isNewState)
            {
                if (entityPM.NewConcurrencyGUID == null)
                {
                    entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
                }
            }

            entity.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            entityPM.ConcurrencyGUID = entity.ConcurrencyGUID;
            entity.ConfirmationNumber = entityPM.ConfirmationNumber;
           

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
            MapJournalAmount(entityPM);

            //ForeignCurrencyAmount = localCurrencyAmount/ForeignExchangeRate;
        }


        private static void MapJournalAmount(APInvoiceLinePM entityPM)
        {
          
            if (entityPM.VatRecognizedPercentage == 0 || entityPM.VatRecognizedPercentage == null)
            {
                if (entityPM.VatPercentage == null || entityPM.VatRecognizedPercentage == null)
                {
                    entityPM.LocalAmountWithVatRecognized = entityPM.LocalCurrencyAmount;
                }
                else
                {
                    entityPM.LocalAmountWithVatRecognized = entityPM.LocalCurrencyAmount + ((entityPM.VatPercentage / 100) * entityPM.LocalCurrencyAmount);
                }

            }
            else
            {
                entityPM.LocalAmountWithVatRecognized = (entityPM.LocalCurrencyAmount + ((entityPM.VatPercentage / 100) * ((1 - entityPM.VatRecognizedPercentage) * entityPM.LocalCurrencyAmount)));
            }
            entityPM.ForiegnAmountWithRecognizedVat = entityPM.LocalAmountWithVatRecognized != null ? entityPM.LocalAmountWithVatRecognized / entityPM.ForiegnExchangeRate : entityPM.LocalAmountWithVatRecognized;


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
