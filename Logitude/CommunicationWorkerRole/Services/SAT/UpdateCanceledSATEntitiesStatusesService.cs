using Logitude.BL.InvoiceModel.Tools;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CommunicationWorkerRole.Services.SAT
{
    public class UpdateCanceledSATEntitiesStatusesService
    {
        private readonly static int numberofSubmitEntitiesForEachUpdate = 10;
        private readonly static string sATCanceledStatusCode = "CS";

        public static void Update()
        {
            UpdateCanceledSATARInvoicesStatuses();
            UpdateCanceledSATARPaymentsStatuses();
        }

        private static void UpdateCanceledSATARInvoicesStatuses()
        {
            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(0);
            List<ARInvoice> aRInvoices = aRInvoiceRepository.GetInvoices().Where(aRInvoice => aRInvoice.SATTransferStatusCode == sATCanceledStatusCode && !string.IsNullOrEmpty(aRInvoice.SATXML)).ToList();
            IEnumerable<List<ARInvoice>> listOfARInvoices = SplitListIntoNList(aRInvoices, numberofSubmitEntitiesForEachUpdate);
            listOfARInvoices.ToList().ForEach(subARInvoices =>
            {
                HandleProcessOfARInvoiceUpdate(subARInvoices, aRInvoiceRepository);
            });
        }

        private static void HandleProcessOfARInvoiceUpdate(List<ARInvoice> subARInvoices, ARInvoiceRepository aRInvoiceRepository)
        {
            Parallel.ForEach(subARInvoices, (aRInvoice) =>
            {
                ARInvoiceService.TryUpdateCanceledARInvoiceStatus(aRInvoice, aRInvoice.Tenant, aRInvoiceRepository);
            });

            aRInvoiceRepository.SubmitChanges();
        }

        private static void UpdateCanceledSATARPaymentsStatuses()
        {
            ARPaymentRepository aRPaymentRepository = new ARPaymentRepository(0);
            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(0);
            List<ARPayment> aRPayments = aRPaymentRepository.GetARPayments().Where(aRPayment => aRPayment.SATTransferStatusCode == sATCanceledStatusCode && !string.IsNullOrEmpty(aRPayment.SATXML)).ToList();
            IEnumerable<List<ARPayment>> listOfARPayments = SplitListIntoNList(aRPayments, numberofSubmitEntitiesForEachUpdate);
            listOfARPayments.ToList().ForEach(subARPayments =>
            {
                HandleProcessOfARPaymentUpdate(subARPayments, aRPaymentRepository, aRInvoiceRepository);
            });
        }

        private static void HandleProcessOfARPaymentUpdate(List<ARPayment> subARPayments, ARPaymentRepository aRPaymentRepository, ARInvoiceRepository aRInvoiceRepository)
        {
            subARPayments.ForEach(arPayment =>
            {
                UpdateSingleARPaymentWithRelatedInvoices(arPayment, aRPaymentRepository, aRInvoiceRepository);
            });
            aRPaymentRepository.SubmitChanges();
        }

        private static void UpdateSingleARPaymentWithRelatedInvoices(ARPayment arPayment, ARPaymentRepository aRPaymentRepository, ARInvoiceRepository aRInvoiceRepository)
        {
            string oldARPaymentStatus = arPayment.SATTransferStatusCode;
            ARPaymentService.TryUpdateCanceledARPaymentStatus(arPayment, arPayment.Tenant, aRPaymentRepository);
            if (oldARPaymentStatus == arPayment.SATTransferStatusCode) return;
            XmlElement comprobanteComplementoXMLElement = ARPaymentService.GetcomprobanteComplementoXMLElement(arPayment.SATXML);
            SATInterfaceHelper sATInterfaceHelper = new SATInterfaceHelper();
            sATInterfaceHelper.UpdatePaymentInvoicesSATStatus(arPayment.Id, arPayment.Tenant, comprobanteComplementoXMLElement, aRInvoiceRepository, aRPaymentRepository);
        }

        private static IEnumerable<List<T>> SplitListIntoNList<T>(List<T> fullList, int nSize)
        {
            for (int i = 0; i < fullList.Count; i += nSize)
            {
                yield return fullList.GetRange(i, Math.Min(nSize, fullList.Count - i)).ToList();
            }
        }
    }
}
