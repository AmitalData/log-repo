using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
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
	// not used
    public class SATUpdateEntityHelper
    {
		private int tenant;
		private bool isConcurrencyToggleEnabled = false;
		private SATUpdateFields SATUpdateField;
		private IInvoiceContext invoiceContext;
		private ARInvoiceRepository arInvoiceRepository;
		private ARPaymentRepository arPaymentRepository;

		public SATUpdateEntityHelper(int tenant)
        {
			this.tenant = tenant;
			isConcurrencyToggleEnabled = FeatureToggleHelper.HasFeatureToggle("INU", tenant);
		}

		public void UpdatePayment(SATUpdateFields SATUpdateFields)
		{
			this.invoiceContext = SATUpdateFields.InvoiceContext;
			this.arPaymentRepository = SATUpdateFields.ARPaymentRepository;


			if (isConcurrencyToggleEnabled)
			{

				//this.SaveARPayment_UpdateService();
			}

			else
			{

				//this.SaveARPayment_Repository();
			}
		}

		public void UpdateInvoice(SATUpdateFields SATUpdateFields)
		{
			this.invoiceContext = SATUpdateFields.InvoiceContext;
			this.arInvoiceRepository = SATUpdateFields.ARInvoiceRepository;



			if (isConcurrencyToggleEnabled)
			{

				//this.SaveARInvoice_UpdateService();
			}

			else
			{

				//this.SaveARInvoice_Repository();
			}
		}

		private void SaveARInvoice_UpdateService(ARInvoicePM invoice)
		{
			ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, invoice.Tenant);
			invoiceService.Update(invoice);
		}
		private void SaveARPayment_UpdateService(ARPaymentPM payment)
		{
			ARPaymentService paymentService = new ARPaymentService(invoiceContext, payment.Tenant);
			paymentService.Update(payment);
		}

		private void SaveARInvoice_Repository(ARInvoice invoice)
		{
			arInvoiceRepository.Update(invoice);
			arInvoiceRepository.SubmitChanges();
		}
		private void SaveARPayment_Repository(ARPayment payment)
		{
			arPaymentRepository.Update(payment);
			arPaymentRepository.SubmitChanges();
		}
	}

	public class SATUpdateFields
    {
		public string SATApprovalDate;
		public string SATAdditionalFieldsXML;
		public string SATXML;
		public string SATTransferStatusCode;
		public string TransmissionError;
		public IInvoiceContext InvoiceContext;
		public ARInvoiceRepository ARInvoiceRepository;
		public ARPaymentRepository ARPaymentRepository;
	}
}
