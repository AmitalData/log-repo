using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public partial class ARInvoiceMapping
    {
        private static void MapConcurrencyFields(ARInvoicePM entityPM, ARInvoice entity, bool isNewState)
        {
            bool isConcurrencyToggleEnabled = FeatureToggleHelper.HasFeatureToggle("INU", entityPM.Tenant);

            if (!isConcurrencyToggleEnabled)
            {
                MapConcurrencyFields_MapAllFields(entityPM, entity, isNewState);
            }

            else 
            {
                if (isNewState)
                {
                    MapConcurrencyFields_QBO(entityPM, entity);
                    MapConcurrencyFields_SAT(entityPM, entity, isNewState);
                    MapConcurrencyFields_Print(entityPM, entity);
                    MapConcurrencyFields_Client(entityPM, entity);
                }

                else
                {
                    if (entityPM.IsUpdatedByQBO)
                        MapConcurrencyFields_QBO(entityPM, entity);

                    else if (entityPM.IsUpdatedBySAT)
                        MapConcurrencyFields_SAT(entityPM, entity, isNewState);

                    else if (entityPM.IsUpdatedByPrint)
                        MapConcurrencyFields_Print(entityPM, entity);

                    else
                    {
                        MapConcurrencyFields_OnEdited(entityPM, entity);
                        MapConcurrencyFields_Client(entityPM, entity);
                    }
                }
            }
        }
        private static void MapConcurrencyFields_MapAllFields(ARInvoicePM entityPM, ARInvoice entity, bool isNewState)
        {
            if (isNewState)
                entity.SATInvoiceStatusCode = entityPM.SATInvoiceStatusCode;

            entity.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            entity.TransferStatusCode = entityPM.TransferStatusCode;
            entity.IsTransferStarted = entityPM.IsTransferStarted;
            entity.SATTransferStatusCode = entityPM.SATTransferStatusCode;
            entity.SATApprovalDate = entityPM.SATApprovalDate;
            entity.SATXML = entityPM.SATXML;
            entity.TransmissionError = entityPM.TransmissionError;
            entity.SATAdditionalFieldsXML = entityPM.SATAdditionalFieldsXML;
            entity.PrintByUserId = entityPM.PrintByUserId;
            entity.PrintDate = entityPM.PrintDate;
            entity.IsPrinted = entityPM.IsPrinted;

            if (!string.IsNullOrEmpty(entityPM.ExternalAccountingEntityId))
                entity.ExternalAccountingEntityId = entityPM.ExternalAccountingEntityId;

            string transferError = entityPM.TransferError;
            if (!string.IsNullOrEmpty(transferError) && transferError.Length > 250)
                transferError = transferError.Substring(0, 250);

            entityPM.TransferError = transferError;
            entity.TransferError = transferError;
        }
        private static void MapConcurrencyFields_QBO(ARInvoicePM entityPM, ARInvoice entity)
        {
            entity.TransferStatusCode = entityPM.TransferStatusCode;
            entity.IsTransferStarted = entityPM.IsTransferStarted;

            if (!string.IsNullOrEmpty(entityPM.ExternalAccountingEntityId))
                entity.ExternalAccountingEntityId = entityPM.ExternalAccountingEntityId;

            string transferError = entityPM.TransferError;
            if (!string.IsNullOrEmpty(transferError) && transferError.Length > 250)
                transferError = transferError.Substring(0, 250);

            entityPM.TransferError = transferError;
            entity.TransferError = transferError;
        }
        private static void MapConcurrencyFields_SAT(ARInvoicePM entityPM, ARInvoice entity, bool isNewState)
        {
            entity.SATInvoiceStatusCode = entityPM.SATInvoiceStatusCode;
            entity.SATTransferStatusCode = entityPM.SATTransferStatusCode;
            entity.SATApprovalDate = entityPM.SATApprovalDate;
            entity.SATXML = entityPM.SATXML;
            entity.TransmissionError = entityPM.TransmissionError;
            entity.SATAdditionalFieldsXML = entityPM.SATAdditionalFieldsXML;
        }
        private static void MapConcurrencyFields_Print(ARInvoicePM entityPM, ARInvoice entity)
        {
            entity.PrintByUserId = entityPM.PrintByUserId;
            entity.PrintDate = entityPM.PrintDate;
            entity.IsPrinted = entityPM.IsPrinted;
        }
        private static void MapConcurrencyFields_OnEdited(ARInvoicePM entityPM, ARInvoice entity)
        {
            if (entityPM.SetApproved || entityPM.SetVoided)
            {
                entity.SATInvoiceStatusCode = entityPM.SATInvoiceStatusCode;
                entity.SATTransferStatusCode = entityPM.SATTransferStatusCode;
            }

            var transferStatusCode = ConcurrencyFieldHelper.GetConcurrencyFieldValue_String(entityPM.TransferStatusCode_Original, entityPM.TransferStatusCode, entity.TransferStatusCode);
            entity.TransferStatusCode = transferStatusCode;

            var isTransferStarted = ConcurrencyFieldHelper.GetConcurrencyFieldValue_Bool(entityPM.IsTransferStarted_Original, entityPM.IsTransferStarted, entity.IsTransferStarted);
            entity.IsTransferStarted = isTransferStarted;

            var transferError = ConcurrencyFieldHelper.GetConcurrencyFieldValue_String(entityPM.TransferError_Original, entityPM.TransferError, entity.TransferError);
            if (!string.IsNullOrEmpty(transferError) && transferError.Length > 250)
                transferError = transferError.Substring(0, 250);

            entity.TransferError = transferError;
        }
        private static void MapConcurrencyFields_Client(ARInvoicePM entityPM, ARInvoice entity)
        {
            entityPM.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            entity.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
        }        
    }
}
