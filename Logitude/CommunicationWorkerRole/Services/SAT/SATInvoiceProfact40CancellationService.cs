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
			CommunicationLog waitingCommLog = args.WaitingCommLog;
			CommunicationLogRepository communicationLogRep = args.CommunicationLogRep;
			Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting satSetting = args.SatSetting;
			Simplog.Data.InvoiceModel.EntityPOCOs.ARInvoice invoice = args.ARInvoice;
			ARInvoiceRepository arinvoiceRep = args.ARInvoiceRep;

			bool isProduction = satSetting.Token != "mvpNUXmQfK8=";
			Profact.TimbraCFDI40.Conector conector = new Profact.TimbraCFDI40.Conector(isProduction);
			
			conector.EstableceCredenciales(satSetting.Token);

			ComprobanteDetails comprobanteDetails = SATInterfaceWorkerRole.GetcomprobanteDetails(invoice.SATXML);

			if (comprobanteDetails.ComplementoAny != null)
			{
				List<System.Xml.XmlElement> myLXmlComplementos = comprobanteDetails.ComplementoAny.ToList<System.Xml.XmlElement>();
				var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
				if (timbreFiscalDigitalElement != null)
				{
					Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);

					string rfcEmisor = comprobanteDetails.RfcEmisor;

					string folioFiscal = digitalTi.UUID?.Trim();
					
					string motivoCancelacion = invoice.SATCancelReasonCode?.Trim();

					string folioSustitucion = GetRelatedInvoiceUUID(invoice, motivoCancelacion);

					ResultadoCancelacion resultadoCancelacion = conector.CancelaCFDI40(rfcEmisor, folioFiscal, motivoCancelacion, folioSustitucion);
                    bool isConcurrencyToggleEnabled = FeatureToggleHelper.HasFeatureToggle("INU", invoice.Tenant);
                    string SATTransferStatusCode = null;
                    string transmissionError = null;

                    if (resultadoCancelacion.Exitoso)
					{
						waitingCommLog.CommunicationStatusTypeCode = "D";
						waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
						waitingCommLog.DoneDateUTC = DateTime.UtcNow;
						waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
						waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
						communicationLogRep.Update(waitingCommLog);
						communicationLogRep.SubmitChanges();

						SATTransferStatusCode = "TD";
						transmissionError = null;
					}
					else
                    {
                        string transError = resultadoCancelacion.Descripcion;
                        if (IsWaitingToCancelledFromSAT(resultadoCancelacion))
                        {
                            waitingCommLog.CommunicationStatusTypeCode = "D";
                            waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                            waitingCommLog.DoneDateUTC = DateTime.UtcNow;
                            waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                            waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
                            communicationLogRep.Update(waitingCommLog);
                            communicationLogRep.SubmitChanges();

                            SATTransferStatusCode = "CS";
                        }
                        else
                        {
                            if (waitingCommLog.Retries == 4)
                            {
                                if (!string.IsNullOrEmpty(resultadoCancelacion.Descripcion) && invoice != null)
                                {
                                    transError = resultadoCancelacion.Descripcion.Replace("Error en la validación de estructura xsd:", "").ToString().Trim();
                                    if (!string.IsNullOrEmpty(resultadoCancelacion.TipoExcepcion))
                                    {
                                        transError += Environment.NewLine + resultadoCancelacion.TipoExcepcion;
                                    }
                                    if (transError != invoice.TransmissionError || invoice.SATTransferStatusCode != "TE")
                                    {
                                        SATTransferStatusCode = "TE";
                                        transmissionError = transError;
                                    }
                                }
                            }

                            throw new Exception("Failed," + transError);
                        }
                    }

					if (isConcurrencyToggleEnabled)
					{
						args.ARInvoicePM.IsUpdatedBySAT = true;
						args.ARInvoicePM.SATTransferStatusCode = SATTransferStatusCode;
						args.ARInvoicePM.TransmissionError = transmissionError;

						IInvoiceContext invoiceContext = InvoiceContext.GetContext(invoice.Tenant);
						ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, invoice.Tenant);
						invoiceService.Update(args.ARInvoicePM);
					}

					else
					{
						invoice.SATTransferStatusCode = SATTransferStatusCode;
						invoice.TransmissionError = transmissionError;
						arinvoiceRep.Update(invoice);
						arinvoiceRep.SubmitChanges();
					}
				}
			}
		}

        private static bool IsWaitingToCancelledFromSAT(ResultadoCancelacion resultadoCancelacion)
        {
            string transferError = resultadoCancelacion.Descripcion;
            if(transferError == "Comprobante ya está en proceso de cancelación" && resultadoCancelacion.TipoExcepcion == "EstatusSat") return true;
            if (transferError == "El comprobante será cancelado") return true;
            if (transferError.Contains("Comprobante ya está en proceso de cancelación")) return true;

            return false;
        }

        private static string GetRelatedInvoiceUUID(ARInvoice invoice, string cancelacionReasonCode)
        {
            if (cancelacionReasonCode != "01") return "";
            if (string.IsNullOrEmpty(invoice.RelatedInvoice)) return "";
            ARInvoice relatedARInvoice = GetRelatedARInvoice(invoice);
            if (relatedARInvoice == null) return "";
            System.Xml.XmlElement[] relatedInvoiceComprobanteComplementoAny = GetProfactComprobanteComplementoAny(relatedARInvoice);
            if (relatedInvoiceComprobanteComplementoAny == null) return "";
            System.Xml.XmlElement timbreFiscalDigitalElement = GetTimbreFiscalDigitalElement(relatedInvoiceComprobanteComplementoAny);
            if (timbreFiscalDigitalElement == null) return "";

            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
            return digitalTi?.UUID?.Trim();
        }

        private static System.Xml.XmlElement GetTimbreFiscalDigitalElement(System.Xml.XmlElement[] relatedInvoiceComprobanteComplementoAny)
        {
            List<System.Xml.XmlElement> myLXmlComplementos = relatedInvoiceComprobanteComplementoAny.ToList<System.Xml.XmlElement>();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            return timbreFiscalDigitalElement;
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
