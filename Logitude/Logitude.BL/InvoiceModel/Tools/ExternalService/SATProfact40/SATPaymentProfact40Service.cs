using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
//using Profact.TimbraCFDI;
//using Profact.TimbraCFDI;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Resolvers;
using Profact.TimbraCFDI40.Complementos.Pagos20;
using Profact.TimbraCFDI40;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATPaymentProfact40Service
    {
        private ARPaymentPM arPpayment;
        private XmlElement comprobanteXmlPagos;
        private ARInvoiceRepository arInvoiceRep;
        private int tenant;
        private string paymentId;
        private SATCommunicationLogBuilder sATCommunicationLogBuilder;

        public SATPaymentProfact40Service(ARPaymentPM arPpayment, XmlElement comprobanteXmlPagos, ARInvoiceRepository arInvoiceRep)
        {
            tenant = arPpayment.Tenant;
            paymentId = arPpayment.Id;
            this.arPpayment = arPpayment;
            this.comprobanteXmlPagos = comprobanteXmlPagos;
            this.arInvoiceRep = arInvoiceRep;
        }

        public SATPaymentProfact40Service(string paymentId, int tenant)
        {
            this.tenant = tenant;
            this.paymentId = paymentId;
            this.sATCommunicationLogBuilder = new SATCommunicationLogBuilder(tenant);
        }

        public void SendRequest(SATInterfaceSetting satSetting)
        {
            ARPaymentPM arPaymentPM = GetARPaymentPM();
            ARPaymentRepository arPaymentRepository = new ARPaymentRepository(arPaymentPM.Tenant);
            ARPayment arPayment = arPaymentRepository.GetSingleARPayment(paymentId, tenant);

            SATPaymentComprobanteValidator sATPaymentComprobanteValidator = new SATPaymentComprobanteValidator(arPaymentPM);
            sATPaymentComprobanteValidator.Validate();

            SATComprobante sATPaymentComprobante = new SATComprobante(arPaymentPM, satSetting);
            Comprobante comprobante = sATPaymentComprobante.Get();

            sATCommunicationLogBuilder.Build(new SATCommunicationLogArgs { Comprobante = comprobante, ARPaymentPM = arPaymentPM, IsCancellation = false, IsPayment = true });
            SetSATTransferStatus(arPayment, arPaymentPM, SATData.InTransferingSATTransferStatusCode);
            arPaymentRepository.Update(arPayment);
            arPaymentRepository.SubmitChanges();


        }

        private ARPaymentPM GetARPaymentPM()
        {
            ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
            ARPaymentPM arPaymentPM = paymentQuery.GetSinglePM(paymentId, tenant);
            return arPaymentPM;
        }

        private void SetSATTransferStatus(ARPayment arPayment, ARPaymentPM arPaymentPM, string satTransferStatusCode)
        {
            arPayment.SATTransferStatusCode = arPaymentPM.SATTransferStatusCode = satTransferStatusCode;
        }

        public void UpdateStatus()
        {
            ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
            ARPaymentPM entityPM = paymentQuery.GetSinglePM(arPpayment.Id, tenant);
            List<ARInvoice> currentPaymentARInvoices = GetCurrentPaymentARInvoices(entityPM);
            List<ARInvoiceSATDetails> currentPaymentInvoiceDetails = GetCurrentPaymentInvoicesDetails(currentPaymentARInvoices);
            List<PagosPagoDoctoRelacionado> doctos;
            try
            {
                doctos = GetPagosPagoDoctoRelacionados();
            }
            catch(Exception ex)
            {
                doctos = GetNewInstancePagosPago20DoctoRelacionados();
            }
            foreach (PagosPagoDoctoRelacionado doctoItem in doctos)
            {
                UpdateRelatedARInvoiceStatus(currentPaymentInvoiceDetails, doctoItem);
            }

            arInvoiceRep.SubmitChanges();
        }
        
        private List<PagosPagoDoctoRelacionado> GetNewInstancePagosPago20DoctoRelacionados()
        {
            XmlElement xmlPagos = comprobanteXmlPagos;
            Profact.TimbraCFDI33.Complementos.Pagos10.Pagos pagos = Profact.TimbraCFDI.XMLUtilerias.DeserializaObjeto<Profact.TimbraCFDI33.Complementos.Pagos10.Pagos>(xmlPagos.OuterXml);
            Profact.TimbraCFDI33.Complementos.Pagos10.PagosPago pagoItem = pagos.Pago[0];
            List<Profact.TimbraCFDI33.Complementos.Pagos10.PagosPagoDoctoRelacionado> doctos = pagoItem.DoctoRelacionado.ToList();
            List<PagosPagoDoctoRelacionado> PagosPagoDoctoRelacionados = new List<PagosPagoDoctoRelacionado>();
            doctos.ForEach(docto => {
                PagosPagoDoctoRelacionados.Add(new PagosPagoDoctoRelacionado { IdDocumento = docto.IdDocumento });
            });

            return PagosPagoDoctoRelacionados;
        }

        private List<ARInvoice> GetCurrentPaymentARInvoices(ARPaymentPM entityPM)
        {
            List<string> invoiceIds = entityPM.PaymentInvoices.Select(f => f.ARInvoiceId).ToList();

            List<ARInvoice> currentPaymentARInvoices = (from a in arInvoiceRep.context.ARInvoices
                                                        where a.Tenant == tenant && invoiceIds.Contains(a.Id) && a.SATXML != null
                                                        select a).ToList();
            return currentPaymentARInvoices;
        }

        private List<ARInvoiceSATDetails> GetCurrentPaymentInvoicesDetails(List<ARInvoice> currentPaymentARInvoices)
        {
            List<ARInvoiceSATDetails> paymentInvoicesDetails = new List<ARInvoiceSATDetails>();
            foreach (ARInvoice invoice in currentPaymentARInvoices)
            {
                AddARInvoiceToPaymentInvoicesDetails(paymentInvoicesDetails, invoice);
            }

            return paymentInvoicesDetails;
        }

        private static void AddARInvoiceToPaymentInvoicesDetails(List<ARInvoiceSATDetails> paymentInvoicesDetails, ARInvoice invoice)
        {
            XmlElement[] invoiceComprobanteComplemento = GetComplementoXmlElementFromRelatedInvoice(invoice.SATXML);
            if (invoiceComprobanteComplemento == null) return;
            
            List<System.Xml.XmlElement> myLXmlComplementos = invoiceComprobanteComplemento.ToList<System.Xml.XmlElement>();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            if (timbreFiscalDigitalElement == null) return;

            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
            paymentInvoicesDetails.Add(new ARInvoiceSATDetails() { UUID = digitalTi.UUID, Invoice = invoice });

        }

        private static XmlElement[] GetComplementoXmlElementFromRelatedInvoice(string relatedInvoiceSATXML)
        {
            try
            {
                return Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Comprobante>(relatedInvoiceSATXML).Complemento?.Any;
            }
            catch (Exception ex)
            {
                return Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(relatedInvoiceSATXML).Complemento?.Any;
            }
        }

        private List<PagosPagoDoctoRelacionado> GetPagosPagoDoctoRelacionados()
        {
            XmlElement xmlPagos = comprobanteXmlPagos;
            Pagos pagos = Profact.TimbraCFDI.XMLUtilerias.DeserializaObjeto<Pagos>(xmlPagos.OuterXml);
            PagosPago pagoItem = pagos.Pago[0];
            List<PagosPagoDoctoRelacionado> doctos = pagoItem.DoctoRelacionado.ToList();

            return doctos;
        }

        private void UpdateRelatedARInvoiceStatus(List<ARInvoiceSATDetails> currentPaymentInvoiceDetails, PagosPagoDoctoRelacionado doctoItem)
        {
            ARInvoiceSATDetails relatedInvoice = currentPaymentInvoiceDetails.FirstOrDefault(d => d.UUID == doctoItem.IdDocumento);
            if (relatedInvoice == null) return;

            double previouslySentPaymentsTotal = GetPreviouslySentPaymentsTotal(relatedInvoice);

            if (previouslySentPaymentsTotal != 0)
            {
                relatedInvoice.Invoice.SATInvoiceStatusCode = relatedInvoice.Invoice.AmountInInvoiceCurrency.Value == previouslySentPaymentsTotal ? "PD" : "PP";
            }
            else
            {
                relatedInvoice.Invoice.SATInvoiceStatusCode = "OP";
            }

            arInvoiceRep.Update(relatedInvoice.Invoice);
        }

        private double GetPreviouslySentPaymentsTotal(ARInvoiceSATDetails relatedInvoice)
        {
            List<ARInvoicePayment> sentApprovedInvoicePayments = (from a in arInvoiceRep.context.ARInvoicePayments
                                                                  where a.ARInvoiceId == relatedInvoice.Invoice.Id && a.Tenant == relatedInvoice.Invoice.Tenant && !string.IsNullOrEmpty(a.ARPayment.SATXML)
                                                                  && a.ARPayment.SATTransferStatusCode == "TD" && (a.ARPayment.StatusCode == "AD" || a.ARPayment.StatusCode == "CL")
                                                                  select a).ToList();


            double previouslySentPaymentsTotal = 0;

            sentApprovedInvoicePayments.ForEach(p =>
            {
                previouslySentPaymentsTotal += (p.ForeignAmount.Value);

            });
            return previouslySentPaymentsTotal;
        }
    }
}
