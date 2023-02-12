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
			Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting satSetting = args.SatSetting;
			Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository arPaymentRep = args.ARPaymentRep;
			Simplog.Data.InvoiceModel.EntityPOCOs.ARPayment payment = args.ARPayment;
			ARInvoiceRepository arInvoiceRep = args.ARInvoiceRep;

			Profact.TimbraCFDI40.Conector conector = SATBaseService.ConnectToSAT(satSetting.Token);
			ComprobanteDetails comprobanteDetails = SATInterfaceWorkerRole.GetcomprobanteDetails(payment.SATXML);
			if (comprobanteDetails == null || comprobanteDetails.ComplementoAny == null) return;

			System.Xml.XmlElement sATXmlElement = SATBaseService.GetSATXmlElement(comprobanteDetails.ComplementoAny);
			if (sATXmlElement == null) return;

			Profact.TimbraCFDI.TimbreFiscalDigital taxStampDigital = SATBaseService.GetTaxStampDigital(sATXmlElement.OuterXml);

			string rfcEmisor = comprobanteDetails.RfcEmisor;
			string folioFiscal = taxStampDigital.UUID?.Trim();
			const string motivoCancelaOperation = "03";

			ResultadoCancelacion resultadoCancelacion = conector.CancelaCFDI40(rfcEmisor, folioFiscal, motivoCancelaOperation, "");

			if (resultadoCancelacion.Exitoso)
			{
				UpdateCommunicationLog(args, "D");
				UpdatePayment(args, SATData.SATTransferedStatusCode, null);
				new SATInterfaceHelper().UpdatePaymentInvoicesSATStatus(payment.Id, waitingCommLog.Tenant, comprobanteDetails.ComplementoAny[0], arInvoiceRep, arPaymentRep);
			}


			if (SATBaseService.IsWaitingToCancelledFromSAT(resultadoCancelacion))
			{
				UpdateCommunicationLog(args, "D");
				UpdatePayment(args, SATData.SATCancelledWaitingStatusCode, null);
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
			if (transError != payment.TransmissionError || payment.SATTransferStatusCode != SATData.SATTransferedWithErrorStatusCode)
			{
				UpdatePayment(args, SATData.SATTransferedWithErrorStatusCode, transError);
			}

			throw new Exception("Failed," + transError);
		}

		private static void UpdatePayment(SATPaymentProfact40CancellationServiceArgs args, string SATTransferStatusCode, string transmissionError)
		{
			bool isConcurrencyToggleEnabled = FeatureToggleHelper.HasFeatureToggle("INU", args.ARPayment.Tenant);

			if (isConcurrencyToggleEnabled)
			{

				UpdateARPaymentUsingPMService(args, SATTransferStatusCode, transmissionError);
				return;
			}

			args.ARPayment.SATTransferStatusCode = SATTransferStatusCode;
			args.ARPayment.TransmissionError = transmissionError;
			args.ARPaymentRep.Update(args.ARPayment);
			args.ARPaymentRep.SubmitChanges();
		}

		private static void UpdateARPaymentUsingPMService(SATPaymentProfact40CancellationServiceArgs args, string SATTransferStatusCode, string transmissionError)
		{
			args.ARPaymentPM.IsUpdatedBySAT = true;
			args.ARPaymentPM.SATTransferStatusCode = SATTransferStatusCode;
			args.ARPaymentPM.TransmissionError = transmissionError;

			IInvoiceContext invoiceContext = InvoiceContext.GetContext(args.ARPaymentPM.Tenant);
			ARPaymentService aRPaymentService = new ARPaymentService(invoiceContext, args.ARPaymentPM.Tenant);
			aRPaymentService.Update(args.ARPaymentPM);
		}

		private static void UpdateCommunicationLog(SATPaymentProfact40CancellationServiceArgs args, string communicationStatusType)
        {
			args.WaitingCommLog.CommunicationStatusTypeCode = communicationStatusType;
			args.WaitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(args.WaitingCommLog.Tenant);
			args.WaitingCommLog.DoneDateUTC = DateTime.UtcNow;
			args.WaitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(args.WaitingCommLog.Tenant);
			args.WaitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
			args.CommunicationLogRep.Update(args.WaitingCommLog);
			args.CommunicationLogRep.SubmitChanges();
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
