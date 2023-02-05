using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Profact.TimbraCFDI;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.SAT
{
	public class SATPaymentProfact40CancellationService
	{
		public static void SendRequest(SATPaymentProfact40CancellationServiceArgs args)
		{
			CommunicationLog waitingCommLog = args.WaitingCommLog;
			CommunicationLogRepository communicationLogRep = args.CommunicationLogRep;
			Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting satSetting = args.SatSetting;
			Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository arPaymentRep = args.ARPaymentRep;
			Simplog.Data.InvoiceModel.EntityPOCOs.ARPayment payment = args.ARPayment;
			ARInvoiceRepository arInvoiceRep = args.ARInvoiceRep;
			bool isConcurrencyToggleEnabled = FeatureToggleHelper.HasFeatureToggle("INU", payment.Tenant);

			SATInterfaceHelper sATInterfaceHelper = new SATInterfaceHelper();

			bool isProduction = satSetting.Token != "mvpNUXmQfK8=";
			Profact.TimbraCFDI40.Conector conector = new Profact.TimbraCFDI40.Conector(isProduction);
			conector.EstableceCredenciales(satSetting.Token);

			ComprobanteDetails comprobanteDetails = SATInterfaceWorkerRole.GetcomprobanteDetails(payment.SATXML);

			if (comprobanteDetails.ComplementoAny != null)
			{
				List<System.Xml.XmlElement> myLXmlComplementos = comprobanteDetails.ComplementoAny.ToList<System.Xml.XmlElement>();
				var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
				if (timbreFiscalDigitalElement != null)
				{
					Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);

					string rfcEmisor = comprobanteDetails.RfcEmisor;

					string folioFiscal = digitalTi.UUID.Trim();

					const string motivoCancelaOperation = "03";

					ResultadoCancelacion resultadoCancelacion = conector.CancelaCFDI40(rfcEmisor, folioFiscal, motivoCancelaOperation, "");

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

						sATInterfaceHelper.UpdatePaymentInvoicesSATStatus(payment.Id, waitingCommLog.Tenant, comprobanteDetails.ComplementoAny[0], arInvoiceRep, arPaymentRep);
					}
					else
					{
						string transError = resultadoCancelacion.Descripcion;
						if (transError.Contains("Comprobante ya está en proceso de cancelación") || transError == "El comprobante será cancelado")
						{
							waitingCommLog.CommunicationStatusTypeCode = "D";
							waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
							waitingCommLog.DoneDateUTC = DateTime.UtcNow;
							waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
							waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
							communicationLogRep.Update(waitingCommLog);
							communicationLogRep.SubmitChanges();

							SATTransferStatusCode = "CS";
							transmissionError = null;
						}

						else
						{
							if (waitingCommLog.Retries == 4)
							{
								if (!string.IsNullOrEmpty(resultadoCancelacion.Descripcion) && payment != null)
								{
									transError = resultadoCancelacion.Descripcion.Replace("Error en la validación de estructura xsd:", "").ToString().Trim();
									if (!string.IsNullOrEmpty(resultadoCancelacion.TipoExcepcion))
									{
										transError += Environment.NewLine + resultadoCancelacion.TipoExcepcion;
									}
									if (transError != payment.TransmissionError || payment.SATTransferStatusCode != "TE")
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
						args.ARPaymentPM.IsUpdatedBySAT = true;
						args.ARPaymentPM.SATTransferStatusCode = SATTransferStatusCode;
						args.ARPaymentPM.TransmissionError = transmissionError;

						IInvoiceContext invoiceContext = InvoiceContext.GetContext(payment.Tenant);
						ARPaymentService aRPaymentService = new ARPaymentService(invoiceContext, payment.Tenant);
						aRPaymentService.Update(args.ARPaymentPM);
					}

					else
					{
						payment.SATTransferStatusCode = SATTransferStatusCode;
						payment.TransmissionError = transmissionError;
						arPaymentRep.Update(payment);
						arPaymentRep.SubmitChanges();
					}
				}
			}
		}
	}

	public class SATPaymentProfact40CancellationServiceArgs
	{
		public CommunicationLog WaitingCommLog { get; set; }
		public CommunicationLogRepository CommunicationLogRep { get; set; }
		public Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting SatSetting { get; set; }
		public Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository ARPaymentRep { get; set; }
		public Simplog.Data.InvoiceModel.EntityPOCOs.ARPayment ARPayment { get; set; }
		public ARPaymentPM ARPaymentPM { get; set; }
		public ARInvoiceRepository ARInvoiceRep { get; set; }
	}
}
