using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.Tools;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Profact.TimbraCFDI;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
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

			SATInterfaceHelper sATInterfaceHelper = new SATInterfaceHelper();

			Profact.TimbraCFDI40.Comprobante comprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(payment.SATXML);
			bool isProduction = satSetting.Token != "mvpNUXmQfK8=";
			Profact.TimbraCFDI40.Conector conector = new Profact.TimbraCFDI40.Conector(isProduction);
			conector.EstableceCredenciales(satSetting.Token);

			if (comprobante.Complemento.Any != null)
			{
				List<System.Xml.XmlElement> myLXmlComplementos = comprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
				var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
				if (timbreFiscalDigitalElement != null)
				{
					Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);

					string rfcEmisor = comprobante.Emisor.Rfc.Trim();

					string folioFiscal = digitalTi.UUID.Trim();

					ResultadoCancelacion resultadoCancelacion = conector.CancelaCFDI(rfcEmisor, folioFiscal);

					if (resultadoCancelacion.Exitoso)
					{
						waitingCommLog.CommunicationStatusTypeCode = "D";
						waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
						waitingCommLog.DoneDateUTC = DateTime.UtcNow;
						waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
						waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
						communicationLogRep.Update(waitingCommLog);
						communicationLogRep.SubmitChanges();

						payment.SATXML = null;
						payment.SATTransferStatusCode = "TD";
						payment.TransmissionError = null;
						arPaymentRep.Update(payment);
						arPaymentRep.SubmitChanges();

						sATInterfaceHelper.UpdatePaymentInvoicesSATStatus(payment, comprobante.Complemento.Any[0], arInvoiceRep, arPaymentRep);

					}
					else
					{
						string transError = resultadoCancelacion.Descripcion;
						if ((transError == "Comprobante ya está en proceso de cancelación" && resultadoCancelacion.TipoExcepcion == "EstatusSat") || transError == "El comprobante será cancelado")
						{
							waitingCommLog.CommunicationStatusTypeCode = "D";
							waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
							waitingCommLog.DoneDateUTC = DateTime.UtcNow;
							waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
							waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
							communicationLogRep.Update(waitingCommLog);
							communicationLogRep.SubmitChanges();

							payment.SATTransferStatusCode = "CS";
							arPaymentRep.Update(payment);
							arPaymentRep.SubmitChanges();
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
										payment.SATTransferStatusCode = "TE";
										payment.TransmissionError = transError;

										arPaymentRep.Update(payment);
										arPaymentRep.SubmitChanges();
									}
								}
							}

							throw new Exception("Failed," + transError);
						}
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
		public ARInvoiceRepository ARInvoiceRep { get; set; }
	}
}
