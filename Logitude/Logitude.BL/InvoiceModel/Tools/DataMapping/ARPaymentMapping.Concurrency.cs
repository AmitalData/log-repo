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
    public partial class ARPaymentMapping
    {
        private const string sATSolvedManualStatusCode = "SM";
        private static void MapConcurrencyFields(ARPaymentPM entityPM, ARPayment entity, bool isNewState)
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
                    MapConcurrencyFields_SAT(entityPM, entity);
                    MapConcurrencyFields_Client(entityPM, entity);
                }

                else
                {
                    if (entityPM.IsUpdatedByQBO)
                        MapConcurrencyFields_QBO(entityPM, entity);

                    else if (entityPM.IsUpdatedBySAT)
                        MapConcurrencyFields_SAT(entityPM, entity);

                    else
                    {
                        MapConcurrencyFields_OnEdited(entityPM, entity);
                        MapConcurrencyFields_Client(entityPM, entity);
                    }
                }
            }
        }
        private static void MapConcurrencyFields_MapAllFields(ARPaymentPM entityPM, ARPayment entity, bool isNewState)
        {
            entity.TransferStatusCode = entityPM.TransferStatusCode;
            entity.IsTransferStarted = entityPM.IsTransferStarted;
            entity.SATApprovalDate = entityPM.SATApprovalDate;
            entity.SATXML = entityPM.SATXML;
            entity.TransmissionError = entityPM.TransmissionError;
            entity.SATAdditionalFieldsXML = entityPM.SATAdditionalFieldsXML;

            if (entityPM.SATTransferStatusCode == sATSolvedManualStatusCode)
                entity.SATTransferStatusCode = entityPM.SATTransferStatusCode;            

            if (!string.IsNullOrEmpty(entityPM.ExternalAccountingEntityId))
                entity.ExternalAccountingEntityId = entityPM.ExternalAccountingEntityId;

            string transferError = entityPM.TransferError;
            if (!string.IsNullOrEmpty(transferError) && transferError.Length > 250)
                transferError = transferError.Substring(0, 250);

            entityPM.TransferError = transferError;
            entity.TransferError = transferError;
        }
        private static void MapConcurrencyFields_QBO(ARPaymentPM entityPM, ARPayment entity)
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

        private static void MapConcurrencyFields_SAT(ARPaymentPM entityPM, ARPayment entity)
        {
            entity.SATTransferStatusCode = entityPM.SATTransferStatusCode;
            entity.SATApprovalDate = entityPM.SATApprovalDate;
            entity.SATXML = entityPM.SATXML;
            entity.TransmissionError = entityPM.TransmissionError;
            entity.SATAdditionalFieldsXML = entityPM.SATAdditionalFieldsXML;
        }

        private static void MapConcurrencyFields_OnEdited(ARPaymentPM entityPM, ARPayment entity)
        {
            var transferStatusCode = ConcurrencyFieldHelper.GetConcurrencyFieldValue_String(entityPM.TransferStatusCode_Original, entityPM.TransferStatusCode, entity.TransferStatusCode);
            entity.TransferStatusCode = transferStatusCode;

            var isTransferStarted = ConcurrencyFieldHelper.GetConcurrencyFieldValue_Bool(entityPM.IsTransferStarted_Original, entityPM.IsTransferStarted, entity.IsTransferStarted);
            entity.IsTransferStarted = isTransferStarted;

            var transferError = ConcurrencyFieldHelper.GetConcurrencyFieldValue_String(entityPM.TransferError_Original, entityPM.TransferError, entity.TransferError);
            if (!string.IsNullOrEmpty(transferError) && transferError.Length > 250)
                transferError = transferError.Substring(0, 250);

            entity.TransferError = transferError;
        }

        private static void MapConcurrencyFields_Client(ARPaymentPM entityPM, ARPayment entity)
        {
            //entityPM.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            //entity.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
        }
    }
}
