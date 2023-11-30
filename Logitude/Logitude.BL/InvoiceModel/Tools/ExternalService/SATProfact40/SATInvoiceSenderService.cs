using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATInvoiceSenderService
    {
        private InvoiceComprobanteBuilderResultArgs invoiceComprobanteValidationResult;
        private SATInterfaceSetting sATInterfaceSetting;
        public InvoiceComprobanteBuilderResultArgs ValidateApprovalSendToSAT(ARInvoicePM aRInvoicePM)
        {
            invoiceComprobanteValidationResult = new InvoiceComprobanteBuilderResultArgs
            {
                IsValidToSendToSAT = true
            };

            if (!FeatureToggleHelper.HasFeatureToggle("AVC", aRInvoicePM.Tenant)) return invoiceComprobanteValidationResult;

            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(aRInvoicePM.Tenant);
            ARInvoice aRInvoice = aRInvoiceRepository.GetSingleARInvoice(aRInvoicePM.Id, aRInvoicePM.Tenant);
            bool isNewInvoice = aRInvoice == null;
            
            if (isNewInvoice)
            {
                ValidateSelectedSATSetting(aRInvoicePM);
            }
            else if (!isNewInvoice && !aRInvoicePM.IsAutoCredit)
            {
                ValidateSelectedSATSetting(aRInvoicePM);
            }

            ValidateComprobante(aRInvoicePM, aRInvoice);
            
            return invoiceComprobanteValidationResult;
        }

        private void ValidateSelectedSATSetting(ARInvoicePM aRInvoicePM)
        {
            SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(aRInvoicePM.Tenant);
            sATInterfaceSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(aRInvoicePM.Tenant);

            if (sATInterfaceSetting == null) return;
            if (sATInterfaceSetting.ActivationDate != null && aRInvoicePM.ApprovedDate < sATInterfaceSetting.ActivationDate) return;
            if (sATInterfaceSetting.SATInterfaceCode != "PROF40") return;
            if (!sATInterfaceSetting.IsARInvoiceTransferEnabled && FeatureToggleHelper.HasFeatureToggle("CPT", aRInvoicePM.Tenant)) return;
        }

        private void ValidateComprobante(ARInvoicePM aRInvoicePM, ARInvoice aRInvoice)
        {
            try
            {
                ValidateProfactoComprobante(aRInvoicePM, aRInvoice);
            }
            catch (Exception ex)
            {
                invoiceComprobanteValidationResult.IsValidToSendToSAT = false;
                invoiceComprobanteValidationResult.ValidationMessage = ex.Message;
            }
        }

        private void ValidateProfactoComprobante(ARInvoicePM aRInvoicePM, ARInvoice aRInvoice)
        {
            SATInvoiceProfact40Service sATInvoiceProfact40Service = new SATInvoiceProfact40Service(aRInvoicePM, aRInvoice, sATInterfaceSetting);
            InvoiceComprobanteBuilderResultArgs builderResultArgs = sATInvoiceProfact40Service.BuildProfactoXML(new InvoiceComprobanteBuilderArgs { DontBuildCommunicationLog = true, CorrectARInvoiceLinesVatAmount = true });
            invoiceComprobanteValidationResult.Comprobante = builderResultArgs.Comprobante;
            invoiceComprobanteValidationResult.CorrectedARInvoiceTrasladoLines = builderResultArgs.CorrectedARInvoiceTrasladoLines;
            invoiceComprobanteValidationResult.CorrectedARInvoiceRetencionLines = builderResultArgs.CorrectedARInvoiceRetencionLines;
            invoiceComprobanteValidationResult.CorrectedARInvoiceRetencionDRLines = builderResultArgs.CorrectedARInvoiceRetencionDRLines;
            invoiceComprobanteValidationResult.ValidationMessage = "";
            invoiceComprobanteValidationResult.IsValidToSendToSAT = true;
            if (builderResultArgs.Comprobante == null)
            {
                invoiceComprobanteValidationResult.IsValidToSendToSAT = false;
                invoiceComprobanteValidationResult.ValidationMessage += "No Comprobante,";
            }
            if (builderResultArgs.CorrectedARInvoiceTrasladoLines.Count() > 0)
            {
                invoiceComprobanteValidationResult.IsValidToSendToSAT = false;
                invoiceComprobanteValidationResult.ValidationMessage += "NeedToCorrectTrasladoLineVatAmount,";
            }
            if (builderResultArgs.CorrectedARInvoiceRetencionLines.Count() > 0)
            {
                invoiceComprobanteValidationResult.IsValidToSendToSAT = false;
                invoiceComprobanteValidationResult.ValidationMessage += "NeedToCorrectRetencionLineVatAmount,";
            }
            if (builderResultArgs.CorrectedARInvoiceRetencionDRLines.Count() > 0)
            {
                invoiceComprobanteValidationResult.IsValidToSendToSAT = false;
                invoiceComprobanteValidationResult.ValidationMessage += "NeedToCorrectRetencionDRLineVatAmount,";
            }
        }
    }
}
