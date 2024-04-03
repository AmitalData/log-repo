using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Profact.TimbraCFDI;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.SAT
{
	public class SATInvoiceProfact40CancellationService
	{
        public static void SendRequest(SATInvoiceProfact40CancellationServiceArgs args)
        {
            SATInterfaceSetting satSetting = args.SatSetting;
            ARInvoice invoice = args.ARInvoice;

            Profact.TimbraCFDI40.Conector conector = SATBaseService.ConnectToSAT(satSetting.Token);
            ComprobanteDetails comprobanteDetails = SATInterfaceWorkerRole.GetcomprobanteDetails(invoice.SATXML);
            if (comprobanteDetails == null || comprobanteDetails.ComplementoAny == null) return;

            System.Xml.XmlElement sATXmlElement = SATBaseService.GetSATXmlElement(comprobanteDetails.ComplementoAny);
            if (sATXmlElement == null) return;

            Profact.TimbraCFDI.TimbreFiscalDigital taxStampDigital = SATBaseService.GetTaxStampDigital(sATXmlElement.OuterXml);

            string rfcEmisor = comprobanteDetails.RfcEmisor;
            string folioFiscal = taxStampDigital?.UUID?.Trim();
            string motivoCancelacion = invoice.SATCancelReasonCode?.Trim();
            string folioSustitucion = GetRelatedInvoiceUUID(invoice, motivoCancelacion);

            ResultadoCancelacion resultadoCancelacion = conector.CancelaCFDI40(rfcEmisor, folioFiscal, motivoCancelacion, folioSustitucion);

            if (resultadoCancelacion.Exitoso)
            {
                UpdateCommunicationLog(args, "D");
                UpdateInvoice(args, SATData.SATTransferedStatusCode, null);
                return;
            }

            if (SATBaseService.IsWaitingToCancelledFromSAT(resultadoCancelacion))
            {
                UpdateCommunicationLog(args, "D");
                UpdateInvoice(args, SATData.SATCancelledWaitingStatusCode, null);
                return;
            }

            if (args.WaitingCommLog.Retries != 4)
            {
                throw new Exception("Failed," + resultadoCancelacion.Descripcion);
            }

            if (string.IsNullOrEmpty(resultadoCancelacion.Descripcion))
            {
                throw new Exception("Failed," + resultadoCancelacion.Descripcion);
            }

            string transError = resultadoCancelacion.Descripcion.Replace("Error en la validación de estructura xsd:", "").ToString().Trim();
            if (!string.IsNullOrEmpty(resultadoCancelacion.TipoExcepcion))
            {
                transError += Environment.NewLine + resultadoCancelacion.TipoExcepcion;
            }
            if (transError != invoice.TransmissionError || invoice.SATTransferStatusCode != SATData.SATTransferedWithErrorStatusCode)
            {
                UpdateInvoice(args, SATData.SATTransferedWithErrorStatusCode, transError);
            }


            throw new Exception("Failed," + transError);
        }

        private static void UpdateCommunicationLog(SATInvoiceProfact40CancellationServiceArgs args, string communicationStatusType)
        {
            args.WaitingCommLog.CommunicationStatusTypeCode = communicationStatusType;
            args.WaitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(args.WaitingCommLog.Tenant);
            args.WaitingCommLog.DoneDateUTC = DateTime.UtcNow;
            args.WaitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(args.WaitingCommLog.Tenant);
            args.WaitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
            args.CommunicationLogRep.Update(args.WaitingCommLog);
            args.CommunicationLogRep.SubmitChanges();
        }

        private static void UpdateInvoice(SATInvoiceProfact40CancellationServiceArgs args, string SATTransferStatusCode, string transmissionError)
        {
            bool isConcurrencyToggleEnabled = FeatureToggleHelper.HasFeatureToggle("INU", args.ARInvoice.Tenant);

            if (isConcurrencyToggleEnabled)
            {
                UpdateARInvoiceUsingPMService(args, SATTransferStatusCode, transmissionError);
                return;
            }

            args.ARInvoice.SATTransferStatusCode = SATTransferStatusCode;
            args.ARInvoice.TransmissionError = transmissionError;
            args.ARInvoiceRep.Update(args.ARInvoice);
            args.ARInvoiceRep.SubmitChanges();
        }

        private static void UpdateARInvoiceUsingPMService(SATInvoiceProfact40CancellationServiceArgs args, string SATTransferStatusCode, string transmissionError)
        {
            args.ARInvoicePM.IsUpdatedBySAT = true;
            args.ARInvoicePM.SATTransferStatusCode = SATTransferStatusCode;
            args.ARInvoicePM.TransmissionError = transmissionError;

            IInvoiceContext invoiceContext = InvoiceContext.GetContext(args.ARInvoicePM.Tenant);
            ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, args.ARInvoicePM.Tenant);
            invoiceService.Update(args.ARInvoicePM);
        }

        

        private static string GetRelatedInvoiceUUID(ARInvoice invoice, string cancelacionReasonCode)
        {
            if (cancelacionReasonCode != "01") return "";
            if (string.IsNullOrEmpty(invoice.RelatedInvoice)) return "";
            ARInvoice relatedARInvoice = GetRelatedARInvoice(invoice);
            if (relatedARInvoice == null) return "";
            System.Xml.XmlElement[] relatedInvoiceComprobanteComplementoAny = GetProfactComprobanteComplementoAny(relatedARInvoice);
            if (relatedInvoiceComprobanteComplementoAny == null) return "";
            System.Xml.XmlElement sATXmlElement = SATBaseService.GetSATXmlElement(relatedInvoiceComprobanteComplementoAny);
            if (sATXmlElement == null) return "";

            Profact.TimbraCFDI.TimbreFiscalDigital taxStampDigital = SATBaseService.GetTaxStampDigital(sATXmlElement.OuterXml);
            return taxStampDigital?.UUID?.Trim();
        }

        private static ARInvoice GetRelatedARInvoice(ARInvoice invoice)
        {
            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(invoice.Tenant);
            ARInvoice relatedARInvoice = aRInvoiceRepository.GetARInvoiceByInvoiceNumber(invoice.Tenant, invoice.RelatedInvoice);
            return relatedARInvoice;
        }

        private static System.Xml.XmlElement[] GetProfactComprobanteComplementoAny(ARInvoice arInvoice)
		{
			Encoding uTF8Encoding = Encoding.UTF8;
			int satVersion = SATBaseProfact40Service.GetSATVersion(arInvoice.SATXML);
			byte[] profactoXMLData = uTF8Encoding.GetBytes(arInvoice.SATXML);
			if(satVersion == 4)
			{
				return LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(profactoXMLData).Complemento.Any;
			}
			else
			{
				return LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(profactoXMLData).Complemento.Any;
			}
		}
	}

	public class SATInvoiceProfact40CancellationServiceArgs
	{
		public CommunicationLog WaitingCommLog { get; set; }
		public CommunicationLogRepository CommunicationLogRep { get; set; }
		public Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting SatSetting { get; set; }
		public Simplog.Data.InvoiceModel.EntityPOCOs.ARInvoice ARInvoice { get; set; }
        public ARInvoicePM ARInvoicePM { get; set; }
        public ARInvoiceRepository ARInvoiceRep { get; set; }
	}
}
